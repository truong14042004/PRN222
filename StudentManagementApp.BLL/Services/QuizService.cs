using Microsoft.EntityFrameworkCore;
using StudentManagementApp.BLL.DTOs;
using StudentManagementApp.DAL.Data;
using StudentManagementApp.DAL.Models;

namespace StudentManagementApp.BLL.Services;

public class QuizService : IQuizService
{
    private readonly AppDbContext _context;

    public QuizService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<QuizDto>> GetAllAsync()
    {
        return await _context.Quizzes
            .Include(q => q.Course)
            .Include(q => q.Questions)
            .Select(q => new QuizDto
            {
                Id = q.Id,
                CourseId = q.CourseId,
                CourseName = q.Course.Name,
                Title = q.Title,
                Description = q.Description,
                TimeLimitMinutes = q.TimeLimitMinutes,
                StartAt = q.StartAt,
                EndAt = q.EndAt,
                IsActive = q.IsActive,
                CreatedAt = q.CreatedAt,
                QuestionCount = q.Questions.Count,
                TotalPoints = q.Questions.Sum(qq => qq.Point)
            }).ToListAsync();
    }

    public async Task<IEnumerable<QuizDto>> GetByCourseIdAsync(int courseId)
    {
        return await _context.Quizzes
            .Include(q => q.Course)
            .Include(q => q.Questions)
            .Where(q => q.CourseId == courseId && q.IsActive)
            .Select(q => new QuizDto
            {
                Id = q.Id,
                CourseId = q.CourseId,
                CourseName = q.Course.Name,
                Title = q.Title,
                Description = q.Description,
                TimeLimitMinutes = q.TimeLimitMinutes,
                StartAt = q.StartAt,
                EndAt = q.EndAt,
                IsActive = q.IsActive,
                CreatedAt = q.CreatedAt,
                QuestionCount = q.Questions.Count,
                TotalPoints = q.Questions.Sum(qq => qq.Point)
            }).ToListAsync();
    }

    public async Task<QuizDto?> GetByIdAsync(int id)
    {
        var q = await _context.Quizzes
            .Include(q => q.Course)
            .Include(q => q.Questions)
            .FirstOrDefaultAsync(q => q.Id == id);

        if (q == null) return null;

        return new QuizDto
        {
            Id = q.Id,
            CourseId = q.CourseId,
            CourseName = q.Course.Name,
            Title = q.Title,
            Description = q.Description,
            TimeLimitMinutes = q.TimeLimitMinutes,
            StartAt = q.StartAt,
            EndAt = q.EndAt,
            IsActive = q.IsActive,
            CreatedAt = q.CreatedAt,
            QuestionCount = q.Questions.Count,
            TotalPoints = q.Questions.Sum(qq => qq.Point)
        };
    }

    public async Task<QuizDetailDto?> GetDetailAsync(int id)
    {
        var q = await _context.Quizzes
            .Include(q => q.Course)
            .Include(q => q.Questions.OrderBy(qq => qq.SortOrder))
                .ThenInclude(qq => qq.Options.OrderBy(o => o.SortOrder))
            .FirstOrDefaultAsync(q => q.Id == id);

        if (q == null) return null;

        return new QuizDetailDto
        {
            Id = q.Id,
            CourseId = q.CourseId,
            CourseName = q.Course.Name,
            Title = q.Title,
            Description = q.Description,
            TimeLimitMinutes = q.TimeLimitMinutes,
            StartAt = q.StartAt,
            EndAt = q.EndAt,
            IsActive = q.IsActive,
            CreatedAt = q.CreatedAt,
            QuestionCount = q.Questions.Count,
            TotalPoints = q.Questions.Sum(qq => qq.Point),
            Questions = q.Questions.Select(qq => new QuizQuestionDto
            {
                Id = qq.Id,
                QuestionText = qq.QuestionText,
                QuestionType = qq.QuestionType.ToString(),
                Point = qq.Point,
                SortOrder = qq.SortOrder,
                Options = qq.Options.Select(o => new QuizOptionDto
                {
                    Id = o.Id,
                    OptionText = o.OptionText,
                    IsCorrect = o.IsCorrect,
                    SortOrder = o.SortOrder
                }).ToList()
            }).ToList()
        };
    }

    public async Task<QuizDto> CreateAsync(CreateQuizDto dto)
    {
        if (dto.CourseId <= 0)
            throw new InvalidOperationException("Khóa học là bắt buộc.");
        if (string.IsNullOrWhiteSpace(dto.Title))
            throw new InvalidOperationException("Tiêu đề quiz là bắt buộc.");
        if (dto.TimeLimitMinutes <= 0)
            throw new InvalidOperationException("Thời gian làm bài phải lớn hơn 0.");
        if (dto.StartAt.HasValue && dto.EndAt.HasValue && dto.StartAt.Value > dto.EndAt.Value)
            throw new InvalidOperationException("Thời gian bắt đầu phải nhỏ hơn hoặc bằng thời gian kết thúc.");

        var quiz = new Quiz
        {
            CourseId = dto.CourseId,
            Title = dto.Title.Trim(),
            Description = string.IsNullOrWhiteSpace(dto.Description) ? null : dto.Description.Trim(),
            TimeLimitMinutes = dto.TimeLimitMinutes,
            StartAt = dto.StartAt,
            EndAt = dto.EndAt,
            IsActive = dto.IsActive
        };

        _context.Quizzes.Add(quiz);
        await _context.SaveChangesAsync();

        return await GetByIdAsync(quiz.Id) ?? throw new Exception("Không thể tạo quiz.");
    }

    public async Task<QuizDto> UpdateAsync(int id, CreateQuizDto dto)
    {
        var quiz = await _context.Quizzes.FindAsync(id) ?? throw new Exception("Không tìm thấy quiz.");

        if (dto.CourseId <= 0)
            throw new InvalidOperationException("Khóa học là bắt buộc.");
        if (string.IsNullOrWhiteSpace(dto.Title))
            throw new InvalidOperationException("Tiêu đề quiz là bắt buộc.");
        if (dto.TimeLimitMinutes <= 0)
            throw new InvalidOperationException("Thời gian làm bài phải lớn hơn 0.");
        if (dto.StartAt.HasValue && dto.EndAt.HasValue && dto.StartAt.Value > dto.EndAt.Value)
            throw new InvalidOperationException("Thời gian bắt đầu phải nhỏ hơn hoặc bằng thời gian kết thúc.");

        quiz.CourseId = dto.CourseId;
        quiz.Title = dto.Title.Trim();
        quiz.Description = string.IsNullOrWhiteSpace(dto.Description) ? null : dto.Description.Trim();
        quiz.TimeLimitMinutes = dto.TimeLimitMinutes;
        quiz.StartAt = dto.StartAt;
        quiz.EndAt = dto.EndAt;
        quiz.IsActive = dto.IsActive;
        
        await _context.SaveChangesAsync();
        return await GetByIdAsync(id) ?? throw new Exception("Không thể cập nhật quiz.");
    }

    public async Task DeleteAsync(int id)
    {
        var quiz = await _context.Quizzes.FindAsync(id) ?? throw new Exception("Không tìm thấy quiz.");
        _context.Quizzes.Remove(quiz);
        await _context.SaveChangesAsync();
    }
}

public class QuizQuestionService : IQuizQuestionService
{
    private readonly AppDbContext _context;

    public QuizQuestionService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<QuizQuestionDto>> GetByQuizIdAsync(int quizId)
    {
        return await _context.QuizQuestions
            .Include(q => q.Options)
            .Where(q => q.QuizId == quizId)
            .OrderBy(q => q.SortOrder)
            .Select(q => new QuizQuestionDto
            {
                Id = q.Id,
                QuestionText = q.QuestionText,
                QuestionType = q.QuestionType.ToString(),
                Point = q.Point,
                SortOrder = q.SortOrder,
                Options = q.Options.OrderBy(o => o.SortOrder).Select(o => new QuizOptionDto
                {
                    Id = o.Id,
                    OptionText = o.OptionText,
                    IsCorrect = o.IsCorrect,
                    SortOrder = o.SortOrder
                }).ToList()
            }).ToListAsync();
    }

    public async Task<QuizQuestionDto> CreateAsync(CreateQuestionDto dto)
    {
        var quiz = await _context.Quizzes.FirstOrDefaultAsync(q => q.Id == dto.QuizId)
            ?? throw new InvalidOperationException("Không tìm thấy quiz.");
        if (dto.Point <= 0)
            throw new InvalidOperationException("Điểm câu hỏi phải lớn hơn 0.");

        var questionType = ParseQuestionType(dto.QuestionType);
        var normalizedOptions = NormalizeAndValidateOptions(questionType, dto.Options);

        var question = new QuizQuestion
        {
            QuizId = dto.QuizId,
            QuestionText = NormalizeQuestionText(dto.QuestionText),
            QuestionType = questionType,
            Point = dto.Point,
            SortOrder = dto.SortOrder
        };
        foreach (var option in normalizedOptions)
        {
            question.Options.Add(option);
        }

        _context.QuizQuestions.Add(question);
        await _context.SaveChangesAsync();
        
        var result = await GetByQuizIdAsync(quiz.Id);
        return result.First(q => q.Id == question.Id);
    }

    public async Task<QuizQuestionDto> UpdateAsync(int id, CreateQuestionDto dto)
    {
        var question = await _context.QuizQuestions
            .Include(q => q.Options)
            .FirstOrDefaultAsync(q => q.Id == id)
            ?? throw new InvalidOperationException("Không tìm thấy câu hỏi.");
        if (dto.Point <= 0)
            throw new InvalidOperationException("Điểm câu hỏi phải lớn hơn 0.");

        var questionType = ParseQuestionType(dto.QuestionType);
        var normalizedOptions = NormalizeAndValidateOptions(questionType, dto.Options);

        question.QuestionText = NormalizeQuestionText(dto.QuestionText);
        question.QuestionType = questionType;
        question.Point = dto.Point;
        question.SortOrder = dto.SortOrder;

        _context.QuizOptions.RemoveRange(question.Options);
        foreach (var option in normalizedOptions)
        {
            question.Options.Add(option);
        }

        await _context.SaveChangesAsync();
        
        var result = await GetByQuizIdAsync(question.QuizId);
        return result.First(q => q.Id == id);
    }

    public async Task DeleteAsync(int id)
    {
        var question = await _context.QuizQuestions.FindAsync(id) ?? throw new InvalidOperationException("Không tìm thấy câu hỏi.");
        _context.QuizQuestions.Remove(question);
        await _context.SaveChangesAsync();
    }

    private static QuestionType ParseQuestionType(string questionType)
    {
        if (!Enum.TryParse<QuestionType>(questionType, true, out var parsedType))
            throw new InvalidOperationException("Loại câu hỏi không hợp lệ.");

        return parsedType;
    }

    private static string NormalizeQuestionText(string questionText)
    {
        var normalized = questionText?.Trim() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(normalized))
            throw new InvalidOperationException("Nội dung câu hỏi là bắt buộc.");

        return normalized;
    }

    private static List<QuizOption> NormalizeAndValidateOptions(QuestionType questionType, List<CreateOptionDto>? inputOptions)
    {
        var options = (inputOptions ?? new List<CreateOptionDto>())
            .Select((o, index) => new OptionCandidate
            {
                Text = (o.OptionText ?? string.Empty).Trim(),
                IsCorrect = o.IsCorrect,
                SortOrder = o.SortOrder <= 0 ? index + 1 : o.SortOrder
            })
            .Where(o => !string.IsNullOrWhiteSpace(o.Text))
            .ToList();

        if (questionType == QuestionType.MultipleChoice)
        {
            if (options.Count < 2)
                throw new InvalidOperationException("Câu trắc nghiệm phải có ít nhất 2 lựa chọn.");

            var correctCount = options.Count(o => o.IsCorrect);
            if (correctCount != 1)
                throw new InvalidOperationException("Câu trắc nghiệm phải có đúng 1 đáp án.");
        }
        else if (questionType == QuestionType.FillInBlank)
        {
            if (!options.Any())
                throw new InvalidOperationException("Câu điền đáp án phải có ít nhất 1 đáp án.");

            var correctOptions = options.Where(o => o.IsCorrect).ToList();
            if (correctOptions.Count != 1)
                throw new InvalidOperationException("Câu điền đáp án phải có đúng 1 đáp án.");

            options = new List<OptionCandidate>
            {
                new OptionCandidate
                {
                    Text = correctOptions[0].Text,
                    IsCorrect = true,
                    SortOrder = 1
                }
            };
        }

        return options
            .Select(o => new QuizOption
            {
                OptionText = o.Text,
                IsCorrect = o.IsCorrect,
                SortOrder = o.SortOrder
            })
            .ToList();
    }

    private sealed class OptionCandidate
    {
        public string Text { get; set; } = string.Empty;
        public bool IsCorrect { get; set; }
        public int SortOrder { get; set; }
    }
}

public class QuizResultService : IQuizResultService
{
    private readonly AppDbContext _context;

    public QuizResultService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<QuizResultDto> SubmitQuizAsync(int studentId, int quizId, List<SubmitAnswerDto> answers, TimeSpan timeTaken)
    {
        if (studentId <= 0)
            throw new InvalidOperationException("Học viên là bắt buộc.");

        var quiz = await _context.Quizzes
            .Include(q => q.Questions)
                .ThenInclude(q => q.Options)
            .FirstOrDefaultAsync(q => q.Id == quizId) ?? throw new InvalidOperationException("Không tìm thấy quiz.");

        if (!quiz.IsActive)
            throw new InvalidOperationException("Quiz đang tạm khóa.");

        var now = DateTime.Now;
        if (quiz.StartAt.HasValue && now < quiz.StartAt.Value)
            throw new InvalidOperationException($"Quiz chưa mở. Bắt đầu lúc {quiz.StartAt.Value:dd/MM/yyyy HH:mm}.");
        if (quiz.EndAt.HasValue && now > quiz.EndAt.Value)
            throw new InvalidOperationException($"Quiz đã đóng lúc {quiz.EndAt.Value:dd/MM/yyyy HH:mm}.");

        var safeTimeTaken = timeTaken < TimeSpan.Zero ? TimeSpan.Zero : timeTaken;
        var submittedAnswers = answers ?? new List<SubmitAnswerDto>();
        var duplicatedQuestionId = submittedAnswers
            .GroupBy(a => a.QuestionId)
            .FirstOrDefault(g => g.Count() > 1)?.Key;
        if (duplicatedQuestionId.HasValue)
            throw new InvalidOperationException($"Câu hỏi {duplicatedQuestionId.Value} bị gửi lặp đáp án.");

        var quizQuestions = quiz.Questions.ToDictionary(q => q.Id);
        var invalidQuestionId = submittedAnswers
            .Select(a => a.QuestionId)
            .Where(id => !quizQuestions.ContainsKey(id))
            .Cast<int?>()
            .FirstOrDefault();
        if (invalidQuestionId.HasValue)
            throw new InvalidOperationException($"Câu hỏi {invalidQuestionId.Value} không thuộc quiz này.");

        var answerMap = submittedAnswers.ToDictionary(a => a.QuestionId, a => a);

        var result = new QuizResult
        {
            StudentId = studentId,
            QuizId = quizId,
            TimeTaken = safeTimeTaken,
            CompletedAt = DateTime.UtcNow
        };

        var totalPoints = quiz.Questions.Sum(q => q.Point);
        int correctCount = 0;
        int totalQuestions = quiz.Questions.Count;

        foreach (var question in quiz.Questions)
        {
            answerMap.TryGetValue(question.Id, out var answer);
            var studentAnswer = new StudentAnswer
            {
                QuizResult = result,
                QuestionId = question.Id
            };

            bool isCorrect = false;

            if (answer is not null && question.QuestionType == QuestionType.MultipleChoice && answer.SelectedOptionId.HasValue)
            {
                var selectedOption = question.Options.FirstOrDefault(o => o.Id == answer.SelectedOptionId);
                if (selectedOption is null)
                    throw new InvalidOperationException($"Lựa chọn trả lời không thuộc câu hỏi {question.Id}.");

                studentAnswer.SelectedOptionId = selectedOption.Id;
                isCorrect = selectedOption.IsCorrect;
            }
            else if (answer is not null && question.QuestionType == QuestionType.FillInBlank && !string.IsNullOrWhiteSpace(answer.FillInAnswer))
            {
                var correctOption = question.Options.FirstOrDefault(o => o.IsCorrect);
                var normalizedAnswer = answer.FillInAnswer.Trim();
                studentAnswer.FillInAnswer = normalizedAnswer;
                isCorrect = correctOption != null &&
                    string.Equals(correctOption.OptionText.Trim(), normalizedAnswer, StringComparison.OrdinalIgnoreCase);
            }

            studentAnswer.IsCorrect = isCorrect;
            studentAnswer.PointsEarned = isCorrect ? question.Point : 0;
            result.StudentAnswers.Add(studentAnswer);

            if (isCorrect) correctCount++;
        }

        result.TotalPoints = totalPoints;
        result.Score = result.StudentAnswers.Sum(a => a.PointsEarned);
        result.CorrectCount = correctCount;
        result.TotalQuestions = totalQuestions;

        _context.QuizResults.Add(result);
        await _context.SaveChangesAsync();

        return await GetByIdAsync(result.Id) ?? throw new Exception("Không thể lấy kết quả bài làm.");
    }

    public async Task<IEnumerable<QuizResultDto>> GetByStudentIdAsync(int studentId)
    {
        return await _context.QuizResults
            .Include(r => r.Student)
            .Include(r => r.Quiz)
            .Where(r => r.StudentId == studentId)
            .OrderByDescending(r => r.CompletedAt)
            .Select(r => new QuizResultDto
            {
                Id = r.Id,
                StudentId = r.StudentId,
                StudentName = r.Student.FullName,
                QuizId = r.QuizId,
                QuizTitle = r.Quiz.Title,
                Score = r.Score,
                TotalPoints = r.TotalPoints,
                CorrectCount = r.CorrectCount,
                TotalQuestions = r.TotalQuestions,
                TimeTaken = r.TimeTaken,
                CompletedAt = r.CompletedAt,
                Passed = r.Passed
            }).ToListAsync();
    }

    public async Task<IEnumerable<QuizResultDto>> GetByQuizIdAsync(int quizId)
    {
        return await _context.QuizResults
            .Include(r => r.Student)
            .Include(r => r.Quiz)
            .Where(r => r.QuizId == quizId)
            .OrderByDescending(r => r.CompletedAt)
            .Select(r => new QuizResultDto
            {
                Id = r.Id,
                StudentId = r.StudentId,
                StudentName = r.Student.FullName,
                QuizId = r.QuizId,
                QuizTitle = r.Quiz.Title,
                Score = r.Score,
                TotalPoints = r.TotalPoints,
                CorrectCount = r.CorrectCount,
                TotalQuestions = r.TotalQuestions,
                TimeTaken = r.TimeTaken,
                CompletedAt = r.CompletedAt,
                Passed = r.Passed
            }).ToListAsync();
    }

    public async Task<QuizResultDto?> GetByIdAsync(int id)
    {
        var r = await _context.QuizResults
            .Include(r => r.Student)
            .Include(r => r.Quiz)
            .Include(r => r.StudentAnswers)
                .ThenInclude(a => a.SelectedOption)
            .Include(r => r.StudentAnswers)
                .ThenInclude(a => a.Question)
                    .ThenInclude(q => q.Options)
            .FirstOrDefaultAsync(r => r.Id == id);

        if (r == null) return null;

        return new QuizResultDto
        {
            Id = r.Id,
            StudentId = r.StudentId,
            StudentName = r.Student.FullName,
            QuizId = r.QuizId,
            QuizTitle = r.Quiz.Title,
            Score = r.Score,
            TotalPoints = r.TotalPoints,
            CorrectCount = r.CorrectCount,
            TotalQuestions = r.TotalQuestions,
            TimeTaken = r.TimeTaken,
            CompletedAt = r.CompletedAt,
            Passed = r.Passed,
            AnswerReviews = r.StudentAnswers
                .OrderBy(a => a.Question.SortOrder)
                .Select(a => new QuizAnswerReviewDto
                {
                    QuestionId = a.QuestionId,
                    SortOrder = a.Question.SortOrder,
                    QuestionText = a.Question.QuestionText,
                    QuestionType = a.Question.QuestionType.ToString(),
                    Point = a.Question.Point,
                    IsCorrect = a.IsCorrect,
                    PointsEarned = a.PointsEarned,
                    StudentAnswer = a.Question.QuestionType == QuestionType.MultipleChoice
                        ? a.SelectedOption?.OptionText
                        : a.FillInAnswer,
                    CorrectAnswer = a.Question.QuestionType == QuestionType.MultipleChoice
                        ? string.Join(", ", a.Question.Options.Where(o => o.IsCorrect).OrderBy(o => o.SortOrder).Select(o => o.OptionText))
                        : (a.Question.Options.FirstOrDefault(o => o.IsCorrect)?.OptionText ?? string.Empty)
                })
                .ToList()
        };
    }

    public async Task<decimal> GetAverageScoreAsync(int studentId)
    {
        var results = await _context.QuizResults.Where(r => r.StudentId == studentId).ToListAsync();
        if (!results.Any()) return 0;
        
        return results.Average(r => r.TotalPoints > 0 ? r.Score / r.TotalPoints * 100 : 0);
    }
}

public class CourseProgressService : ICourseProgressService
{
    private readonly AppDbContext _context;

    public CourseProgressService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<CourseProgressDto?> GetByStudentAndCourseAsync(int studentId, int courseId)
    {
        var cp = await _context.CourseProgresses
            .Include(cp => cp.Student)
            .Include(cp => cp.Course)
            .FirstOrDefaultAsync(cp => cp.StudentId == studentId && cp.CourseId == courseId);

        if (cp == null) return null;

        var quizzesTaken = await _context.QuizResults
            .Where(r => r.StudentId == studentId && r.Quiz.CourseId == courseId)
            .CountAsync();

        var avgScore = await _context.QuizResults
            .Where(r => r.StudentId == studentId && r.Quiz.CourseId == courseId)
            .ToListAsync();

        return new CourseProgressDto
        {
            Id = cp.Id,
            StudentId = cp.StudentId,
            StudentName = cp.Student.FullName,
            CourseId = cp.CourseId,
            CourseName = cp.Course.Name,
            CompletedLessons = cp.CompletedLessons,
            TotalLessons = cp.TotalLessons,
            Percentage = cp.Percentage,
            UpdatedAt = cp.UpdatedAt,
            QuizzesTaken = quizzesTaken,
            AverageScore = avgScore.Any() ? avgScore.Average(r => r.TotalPoints > 0 ? (decimal)r.Score / r.TotalPoints * 100 : 0) : 0
        };
    }

    public async Task<IEnumerable<CourseProgressDto>> GetByStudentIdAsync(int studentId)
    {
        var progresses = await _context.CourseProgresses
            .Include(cp => cp.Student)
            .Include(cp => cp.Course)
            .Where(cp => cp.StudentId == studentId)
            .ToListAsync();

        var result = new List<CourseProgressDto>();
        foreach (var cp in progresses)
        {
            var quizzesTaken = await _context.QuizResults
                .Where(r => r.StudentId == studentId && r.Quiz.CourseId == cp.CourseId)
                .CountAsync();

            var avgScore = await _context.QuizResults
                .Where(r => r.StudentId == studentId && r.Quiz.CourseId == cp.CourseId)
                .ToListAsync();

            result.Add(new CourseProgressDto
            {
                Id = cp.Id,
                StudentId = cp.StudentId,
                StudentName = cp.Student.FullName,
                CourseId = cp.CourseId,
                CourseName = cp.Course.Name,
                CompletedLessons = cp.CompletedLessons,
                TotalLessons = cp.TotalLessons,
                Percentage = cp.Percentage,
                UpdatedAt = cp.UpdatedAt,
                QuizzesTaken = quizzesTaken,
                AverageScore = avgScore.Any() ? avgScore.Average(r => r.TotalPoints > 0 ? (decimal)r.Score / r.TotalPoints * 100 : 0) : 0
            });
        }

        return result;
    }

    public async Task<CourseProgressDto> UpdateProgressAsync(int studentId, int courseId, int completedLessons, int totalLessons = 100)
    {
        var cp = await _context.CourseProgresses
            .FirstOrDefaultAsync(cp => cp.StudentId == studentId && cp.CourseId == courseId);

        if (cp == null)
        {
            cp = new CourseProgress
            {
                StudentId = studentId,
                CourseId = courseId,
                CompletedLessons = completedLessons,
                TotalLessons = totalLessons
            };
            _context.CourseProgresses.Add(cp);
        }
        else
        {
            cp.CompletedLessons = Math.Max(cp.CompletedLessons, completedLessons);
            cp.TotalLessons = totalLessons;
            cp.UpdatedAt = DateTime.UtcNow;
        }

        await _context.SaveChangesAsync();
        return await GetByStudentAndCourseAsync(studentId, courseId) ?? throw new Exception("Failed to update progress");
    }
}
