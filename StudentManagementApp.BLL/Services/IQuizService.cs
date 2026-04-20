using StudentManagementApp.BLL.DTOs;

namespace StudentManagementApp.BLL.Services;

public interface IQuizService
{
    Task<IEnumerable<QuizDto>> GetAllAsync();
    Task<IEnumerable<QuizDto>> GetByCourseIdAsync(int courseId);
    Task<QuizDto?> GetByIdAsync(int id);
    Task<QuizDetailDto?> GetDetailAsync(int id);
    Task<QuizDto> CreateAsync(CreateQuizDto dto);
    Task<QuizDto> UpdateAsync(int id, CreateQuizDto dto);
    Task DeleteAsync(int id);
}

public interface IQuizQuestionService
{
    Task<IEnumerable<QuizQuestionDto>> GetByQuizIdAsync(int quizId);
    Task<QuizQuestionDto> CreateAsync(CreateQuestionDto dto);
    Task<QuizQuestionDto> UpdateAsync(int id, CreateQuestionDto dto);
    Task DeleteAsync(int id);
}

public interface IQuizResultService
{
    Task<QuizResultDto> SubmitQuizAsync(int studentId, int quizId, List<SubmitAnswerDto> answers, TimeSpan timeTaken);
    Task<IEnumerable<QuizResultDto>> GetByStudentIdAsync(int studentId);
    Task<IEnumerable<QuizResultDto>> GetByQuizIdAsync(int quizId);
    Task<QuizResultDto?> GetByIdAsync(int id);
    Task<decimal> GetAverageScoreAsync(int studentId);
}

public interface ICourseProgressService
{
    Task<CourseProgressDto?> GetByStudentAndCourseAsync(int studentId, int courseId);
    Task<IEnumerable<CourseProgressDto>> GetByStudentIdAsync(int studentId);
    Task<CourseProgressDto> UpdateProgressAsync(int studentId, int courseId, int completedLessons, int totalLessons = 100);
}
