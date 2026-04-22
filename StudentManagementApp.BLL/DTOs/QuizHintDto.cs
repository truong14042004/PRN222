namespace StudentManagementApp.BLL.DTOs;

public class QuizHintRequestDto
{
    public int QuizId { get; set; }
    public int QuestionId { get; set; }
    public string Message { get; set; } = string.Empty;
    public int? SelectedOptionId { get; set; }
    public string? FillInAnswer { get; set; }
    public List<QuizChatMessageDto> History { get; set; } = new();
}

public class QuizHintContextDto
{
    public string QuizTitle { get; set; } = string.Empty;
    public string CourseName { get; set; } = string.Empty;
    public string QuestionText { get; set; } = string.Empty;
    public string QuestionType { get; set; } = string.Empty;
    public List<QuizOptionHintDto> Options { get; set; } = new();
    public int? SelectedOptionId { get; set; }
    public string? FillInAnswer { get; set; }
    public string StudentMessage { get; set; } = string.Empty;
    public List<QuizChatMessageDto> History { get; set; } = new();
}

public class QuizOptionHintDto
{
    public int Id { get; set; }
    public string Text { get; set; } = string.Empty;
    public bool IsCorrect { get; set; }
}

public class QuizHintResponseDto
{
    public string Reply { get; set; } = string.Empty;
    public string Provider { get; set; } = "rules";
    public string? Reason { get; set; }
}

public class QuizChatMessageDto
{
    public string Role { get; set; } = "user";
    public string Content { get; set; } = string.Empty;
}
