using QuizGenAI.Enums;

namespace QuizGenAI.DTOs
{
    public class LoginResultDto
    {
        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; }
        public int UserId { get; set; }
        public string DisplayName { get; set; }
        public UserRole Role { get; set; }
    }
}
