using System;
using System.Linq;
using QuizGenAI.Data;
using QuizGenAI.DTOs;
using QuizGenAI.Enums;

namespace QuizGenAI.Services
{
    public class AuthService
    {
        public LoginResultDto Login(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                LoggingService.Warning("Login failed because required credentials were missing.");
                return new LoginResultDto
                {
                    IsSuccess = false,
                    ErrorMessage = "Email and password are required."
                };
            }

            var normalizedEmail = email.Trim();

            using (var context = new QuizGenAIDbContext())
            {
                var user = context.Users.SingleOrDefault(x => x.Email == normalizedEmail);
                if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
                {
                    LoggingService.Warning("Login failed for {Email}", normalizedEmail);
                    return new LoginResultDto
                    {
                        IsSuccess = false,
                        ErrorMessage = "Invalid email or password."
                    };
                }

                var role = context.UserRoles
                    .Where(x => x.UserId == user.Id)
                    .OrderBy(x => x.Role)
                    .Select(x => x.Role)
                    .FirstOrDefault();

                if (!Enum.IsDefined(typeof(UserRole), role))
                {
                    LoggingService.Warning("Login failed for {Email} because no valid role is assigned.", normalizedEmail);
                    return new LoginResultDto
                    {
                        IsSuccess = false,
                        ErrorMessage = "No valid role is assigned to this user."
                    };
                }

                LoggingService.Information("Login succeeded for {Email} with role {Role}", normalizedEmail, role);
                return new LoginResultDto
                {
                    IsSuccess = true,
                    UserId = user.Id,
                    DisplayName = user.Name,
                    Role = role
                };
            }
        }
    }
}
