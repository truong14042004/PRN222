using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Configuration;
using StudentManagementApp.BLL.DTOs;

namespace StudentManagementApp.BLL.Services;

public interface IQuizHintService
{
    Task<QuizHintResponseDto> GenerateHintAsync(QuizHintContextDto context, CancellationToken cancellationToken = default);
}

public class QuizHintService : IQuizHintService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;
    private readonly JsonSerializerOptions _jsonOptions = new(JsonSerializerDefaults.Web);

    public QuizHintService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
    }

    public async Task<QuizHintResponseDto> GenerateHintAsync(QuizHintContextDto context, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(context.StudentMessage))
        {
            context.StudentMessage = "Cho mình một gợi ý để làm câu này.";
        }

        var settings = ResolveModelSettings();
        var apiKey = settings.ApiKey;
        if (string.IsNullOrWhiteSpace(apiKey))
        {
            return GenerateRuleBasedHint(context);
        }

        try
        {
            var endpoint = settings.Endpoint;
            var model = settings.Model;

            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

            var body = new
            {
                model,
                temperature = 0.5,
                messages = BuildOpenAiMessages(context)
            };

            var content = new StringContent(JsonSerializer.Serialize(body, _jsonOptions), Encoding.UTF8, "application/json");
            using var response = await client.PostAsync(endpoint, content, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                var fallback = GenerateRuleBasedHint(context);
                fallback.Reason = $"LLM HTTP {(int)response.StatusCode}";
                return fallback;
            }

            var json = await response.Content.ReadAsStringAsync(cancellationToken);
            using var doc = JsonDocument.Parse(json);
            var reply = doc.RootElement
                .GetProperty("choices")[0]
                .GetProperty("message")
                .GetProperty("content")
                .GetString();

            if (string.IsNullOrWhiteSpace(reply))
            {
                return GenerateRuleBasedHint(context);
            }

            return new QuizHintResponseDto
            {
                Reply = reply.Trim(),
                Provider = settings.ProviderName
            };
        }
        catch (Exception ex)
        {
            var fallback = GenerateRuleBasedHint(context);
            fallback.Reason = $"LLM exception: {ex.GetType().Name}";
            return fallback;
        }
    }

    private (string ApiKey, string Model, string Endpoint, string ProviderName) ResolveModelSettings()
    {
        var geminiKey = _configuration["Gemini:ApiKey"];
        if (!string.IsNullOrWhiteSpace(geminiKey))
        {
            var geminiModel = _configuration["Gemini:Model"] ?? "gemini-2.5-flash";
            var geminiEndpoint = _configuration["Gemini:Endpoint"]
                ?? "https://generativelanguage.googleapis.com/v1beta/openai/chat/completions";
            return (geminiKey, geminiModel, geminiEndpoint, "gemini");
        }

        var openAiKey = _configuration["OpenAI:ApiKey"];
        if (!string.IsNullOrWhiteSpace(openAiKey))
        {
            var openAiModel = _configuration["OpenAI:Model"] ?? "gpt-4o-mini";
            var openAiEndpoint = _configuration["OpenAI:Endpoint"] ?? "https://api.openai.com/v1/chat/completions";
            return (openAiKey, openAiModel, openAiEndpoint, "openai");
        }

        return (string.Empty, string.Empty, string.Empty, "rules");
    }

    private static List<object> BuildOpenAiMessages(QuizHintContextDto context)
    {
        var options = context.Options.Any()
            ? string.Join("\n", context.Options.Select((o, i) => $"{i + 1}. {o.Text}"))
            : "(Không có lựa chọn)";

        var draftAnswer = context.QuestionType == "MultipleChoice"
            ? (context.SelectedOptionId.HasValue ? $"ID lựa chọn đã chọn: {context.SelectedOptionId}" : "Chưa chọn đáp án.")
            : $"Câu trả lời nhập: {context.FillInAnswer ?? "(trống)"}";

        var contextPrompt =
$"""
Môn học: {context.CourseName}
Quiz: {context.QuizTitle}
Loại câu hỏi: {context.QuestionType}
Nội dung câu hỏi: {context.QuestionText}
Lựa chọn:
{options}
Trạng thái làm bài: {draftAnswer}

Yêu cầu:
- Trả lời đúng trọng tâm câu người học vừa hỏi.
- Luôn dùng tiếng Việt có dấu.
- Không tiết lộ đáp án đúng/sai trực tiếp.
- Chỉ gợi ý cách nghĩ, cách loại trừ, cách kiểm tra.
- Trả lời ngắn gọn, rõ ràng, tự nhiên như hội thoại thật.
""";

        var messages = new List<object>
        {
            new
            {
                role = "system",
                content = "Bạn là trợ giảng quiz thông minh và thân thiện. Hãy trả lời linh hoạt theo đúng câu học viên hỏi."
            },
            new
            {
                role = "system",
                content = contextPrompt
            }
        };

        foreach (var turn in context.History.TakeLast(10))
        {
            messages.Add(new
            {
                role = NormalizeRole(turn.Role),
                content = turn.Content
            });
        }

        messages.Add(new
        {
            role = "user",
            content = context.StudentMessage
        });

        return messages;
    }

    private static QuizHintResponseDto GenerateRuleBasedHint(QuizHintContextDto context)
    {
        var rawMessage = (context.StudentMessage ?? string.Empty).Trim();
        var message = rawMessage.ToLowerInvariant();

        var directReply = TryBuildDirectReply(rawMessage, context);
        if (!string.IsNullOrWhiteSpace(directReply))
        {
            return new QuizHintResponseDto { Reply = directReply, Provider = "rules" };
        }

        var asksKeyword = message.Contains("từ khóa") || message.Contains("tu khoa");
        var asksStep = message.Contains("từng bước") || message.Contains("tung buoc");
        var asksElimination = message.Contains("loại trừ") || message.Contains("loai tru");

        string reply;
        if (context.QuestionType == "MultipleChoice")
        {
            if (asksKeyword)
            {
                var keywords = ExtractKeywords(context.QuestionText, 4);
                reply = keywords.Any()
                    ? $"Từ khóa cần bám: {string.Join(", ", keywords)}. Bạn dùng các từ khóa này để loại dần các lựa chọn lệch ngữ cảnh."
                    : "Bạn tập trung vào điều kiện chính trong đề, rồi loại các lựa chọn không bám sát điều kiện đó.";
            }
            else if (asksStep)
            {
                reply = "Làm theo 3 bước: đọc kỹ điều kiện chính, loại lựa chọn lệch ngữ cảnh, rồi so kỹ 2 lựa chọn cuối theo từng cụm từ trong đề.";
            }
            else if (asksElimination)
            {
                reply = "Bạn khoanh 2 lựa chọn khả thi nhất trước, sau đó tìm một chi tiết trong đề để tách 2 lựa chọn đó.";
            }
            else
            {
                reply = "Với câu này, bạn nên bám từ khóa chính của đề rồi đối chiếu từng lựa chọn theo ngữ cảnh thay vì chọn theo cảm giác.";
            }
        }
        else
        {
            reply = asksStep
                ? "Làm theo 3 bước: xác định dạng từ cần điền, thử đáp án ngắn gọn, rồi đọc lại cả câu để kiểm tra độ tự nhiên."
                : "Bạn kiểm tra theo thứ tự: thì, dạng từ, rồi mới đến nghĩa tổng thể của câu.";
        }

        var lastAssistantMessages = context.History
            .Where(h => string.Equals(h.Role, "assistant", StringComparison.OrdinalIgnoreCase))
            .Select(h => NormalizeText(h.Content))
            .TakeLast(2)
            .ToList();

        if (lastAssistantMessages.Contains(NormalizeText(reply)))
        {
            reply = context.QuestionType == "MultipleChoice"
                ? "Mình đổi cách tiếp cận nhé: bạn thử viết lý do ngắn cho từng lựa chọn, rồi bỏ lựa chọn có lý do yếu nhất."
                : "Mình đổi cách tiếp cận nhé: bạn thử 2 đáp án gần nhau, rồi đọc lại cả câu để xem đáp án nào tự nhiên hơn.";
        }

        return new QuizHintResponseDto
        {
            Reply = reply,
            Provider = "rules"
        };
    }

    private static string? TryBuildDirectReply(string rawMessage, QuizHintContextDto context)
    {
        if (string.IsNullOrWhiteSpace(rawMessage))
        {
            return null;
        }

        var m = rawMessage.ToLowerInvariant();
        var asksStructure = m.Contains("cấu trúc") || m.Contains("cau truc") || m.Contains("ngữ pháp") || m.Contains("ngu phap");
        var asksWhy = m.Contains("vì sao") || m.Contains("vi sao") || m.Contains("tại sao") || m.Contains("tai sao");
        var asksHow = m.Contains("như thế nào") || m.Contains("lam sao") || m.Contains("làm sao");
        var wantsChat = m.Contains("giao tiếp") || m.Contains("noi chuyen") || m.Contains("nói chuyện")
            || m.Contains("tôi muốn hỏi") || m.Contains("toi muon hoi")
            || m.Contains("bạn có thể") || m.Contains("ban co the")
            || m.Equals("hi") || m.Equals("hello") || m.Equals("xin chào") || m.Equals("xin chao");
        var asksQuizIntent = m.Contains("đáp án") || m.Contains("dap an")
            || m.Contains("lựa chọn") || m.Contains("lua chon")
            || m.Contains("từ khóa") || m.Contains("tu khoa")
            || m.Contains("loại trừ") || m.Contains("loai tru")
            || m.Contains("câu này") || m.Contains("cau nay")
            || m.Contains("câu hỏi") || m.Contains("cau hoi");

        if (m.Contains("đọc câu hỏi") || m.Contains("doc cau hoi"))
        {
            return "Có. Mình đọc được câu bạn vừa hỏi và sẽ trả lời đúng trọng tâm câu đó.";
        }

        if (wantsChat && !asksQuizIntent)
        {
            return "Có, mình giao tiếp được với bạn bình thường. Bạn cứ hỏi trực tiếp điều bạn muốn, mình sẽ trả lời đúng câu đó; khi bạn cần gợi ý quiz thì chỉ cần nói rõ câu đang làm.";
        }

        if (asksStructure)
        {
            var target = ExtractSentenceForStructure(rawMessage);
            if (!string.IsNullOrWhiteSpace(target))
            {
                return $"Mình đang phân tích đúng câu bạn đưa: \"{TrimText(target, 120)}\". Bạn tách theo Subject - Verb - Object/Complement trước, rồi kiểm tra thì và dạng từ của động từ chính.";
            }

            return "Với câu cấu trúc, bạn tách theo Subject - Verb - Object/Complement trước, sau đó kiểm tra thì và dạng từ.";
        }

        if (asksWhy)
        {
            return context.QuestionType == "MultipleChoice"
                ? "Vì lỗi thường nằm ở chỗ lựa chọn không khớp điều kiện chính trong đề. Bạn so từng lựa chọn với từ khóa điều kiện để loại dần."
                : "Vì câu điền đáp án thường sai ở thì hoặc dạng từ. Bạn kiểm tra ngữ pháp trước rồi mới chốt nghĩa.";
        }

        if (asksHow)
        {
            return context.QuestionType == "MultipleChoice"
                ? "Cách làm nhanh: xác định từ khóa, loại các lựa chọn lệch ngữ cảnh, rồi so kỹ 2 lựa chọn cuối."
                : "Cách làm nhanh: xác định dạng từ cần điền, thử đáp án, rồi đọc lại cả câu để kiểm tra độ tự nhiên.";
        }

        if (rawMessage.Contains('?'))
        {
            return $"Mình đã đọc câu bạn hỏi: \"{TrimText(rawMessage, 120)}\". Với câu hiện tại, bạn bám vào từ khóa chính của đề để kiểm tra độ khớp ngữ cảnh.";
        }

        return null;
    }

    private static string? ExtractSentenceForStructure(string rawMessage)
    {
        var quoteMatch = Regex.Match(rawMessage, "\"([^\"]{6,})\"");
        if (quoteMatch.Success)
        {
            return quoteMatch.Groups[1].Value.Trim();
        }

        var colonIndex = rawMessage.IndexOf(':');
        if (colonIndex >= 0 && colonIndex < rawMessage.Length - 1)
        {
            var afterColon = rawMessage[(colonIndex + 1)..].Trim();
            if (afterColon.Length >= 6)
            {
                return afterColon;
            }
        }

        return null;
    }

    private static List<string> ExtractKeywords(string text, int maxCount)
    {
        var stopWords = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "the","and","for","with","that","this","from","were","have","has","had","into","about","which","what","when","where",
            "một","những","của","cho","trong","với","không","nhiều","ít","đang","được","người","bài","câu","đề","này","kia",
            "mot","nhung","cua","cho","trong","voi","khong","nhieu","it","dang","duoc","nguoi","bai","cau","de","nay","kia"
        };

        return Regex.Matches(text ?? string.Empty, @"\b[\p{L}\p{N}]{3,}\b")
            .Select(m => m.Value)
            .Where(w => !stopWords.Contains(w))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .Take(Math.Max(1, maxCount))
            .ToList();
    }

    private static string TrimText(string text, int maxLength)
    {
        if (string.IsNullOrWhiteSpace(text)) return string.Empty;
        if (text.Length <= maxLength) return text;
        return text[..maxLength].TrimEnd() + "...";
    }

    private static string NormalizeText(string text)
    {
        return Regex.Replace((text ?? string.Empty).ToLowerInvariant(), @"\s+", " ").Trim();
    }

    private static string NormalizeRole(string? role)
    {
        return string.Equals(role, "assistant", StringComparison.OrdinalIgnoreCase) ? "assistant" : "user";
    }
}
