namespace StudentManagementApp.BLL.DTOs;

public class QuizDto
{
    public int Id { get; set; }
    public int CourseId { get; set; }
    public string CourseName { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int TimeLimitMinutes { get; set; }
    public DateTime? StartAt { get; set; }
    public DateTime? EndAt { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public int QuestionCount { get; set; }
    public int TotalPoints { get; set; }
}

public class QuizDetailDto : QuizDto
{
    public List<QuizQuestionDto> Questions { get; set; } = new();
}

public class QuizQuestionDto
{
    public int Id { get; set; }
    public string QuestionText { get; set; } = string.Empty;
    public string QuestionType { get; set; } = string.Empty;
    public int Point { get; set; }
    public int SortOrder { get; set; }
    public List<QuizOptionDto> Options { get; set; } = new();
}

public class QuizOptionDto
{
    public int Id { get; set; }
    public string OptionText { get; set; } = string.Empty;
    public bool IsCorrect { get; set; }
    public int SortOrder { get; set; }
}

public class QuizResultDto
{
    public int Id { get; set; }
    public int StudentId { get; set; }
    public string StudentName { get; set; } = string.Empty;
    public int QuizId { get; set; }
    public string QuizTitle { get; set; } = string.Empty;
    public decimal Score { get; set; }
    public int TotalPoints { get; set; }
    public int CorrectCount { get; set; }
    public int TotalQuestions { get; set; }
    public TimeSpan TimeTaken { get; set; }
    public DateTime CompletedAt { get; set; }
    public bool Passed { get; set; }
    public string PercentageScore => TotalPoints > 0 ? $"{(Score / TotalPoints * 100):F1}%" : "0%";
    public List<QuizAnswerReviewDto> AnswerReviews { get; set; } = new();
}

public class QuizAnswerReviewDto
{
    public int QuestionId { get; set; }
    public int SortOrder { get; set; }
    public string QuestionText { get; set; } = string.Empty;
    public string QuestionType { get; set; } = string.Empty;
    public int Point { get; set; }
    public bool IsCorrect { get; set; }
    public int PointsEarned { get; set; }
    public string? StudentAnswer { get; set; }
    public string CorrectAnswer { get; set; } = string.Empty;
}

public class CreateQuizDto
{
    public int CourseId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int TimeLimitMinutes { get; set; } = 30;
    public DateTime? StartAt { get; set; }
    public DateTime? EndAt { get; set; }
    public bool IsActive { get; set; } = true;
}

public class CreateQuestionDto
{
    public int QuizId { get; set; }
    public string QuestionText { get; set; } = string.Empty;
    public string QuestionType { get; set; } = "MultipleChoice";
    public int Point { get; set; } = 10;
    public int SortOrder { get; set; }
    public List<CreateOptionDto> Options { get; set; } = new();
}

public class CreateOptionDto
{
    public string OptionText { get; set; } = string.Empty;
    public bool IsCorrect { get; set; }
    public int SortOrder { get; set; }
}

public class SubmitAnswerDto
{
    public int QuestionId { get; set; }
    public int? SelectedOptionId { get; set; }
    public string? FillInAnswer { get; set; }
}

public class CourseProgressDto
{
    public int Id { get; set; }
    public int StudentId { get; set; }
    public string StudentName { get; set; } = string.Empty;
    public int CourseId { get; set; }
    public string CourseName { get; set; } = string.Empty;
    public int CompletedLessons { get; set; }
    public int TotalLessons { get; set; }
    public decimal Percentage { get; set; }
    public DateTime UpdatedAt { get; set; }
    public int QuizzesTaken { get; set; }
    public decimal AverageScore { get; set; }
}
