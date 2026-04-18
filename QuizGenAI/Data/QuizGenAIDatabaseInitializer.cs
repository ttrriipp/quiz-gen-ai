using System;
using System.Linq;
using QuizGenAI.Enums;
using QuizGenAI.Models;

namespace QuizGenAI.Data
{
    public static class QuizGenAIDatabaseInitializer
    {
        public static void SeedIfNeeded(QuizGenAIDbContext context)
        {
            if (context.Users.Any())
            {
                return;
            }

            var now = DateTime.UtcNow;

            var adminUser = new User
            {
                Email = "admin@quizgenai.local",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin123!"),
                Name = "System Admin",
                CreatedAt = now,
                UpdatedAt = now
            };

            var teacherUser = new User
            {
                Email = "teacher@quizgenai.local",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Teacher123!"),
                Name = "Demo Teacher",
                CreatedAt = now,
                UpdatedAt = now
            };

            var studentUser = new User
            {
                Email = "student@quizgenai.local",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Student123!"),
                Name = "Demo Student",
                CreatedAt = now,
                UpdatedAt = now
            };

            context.Users.Add(adminUser);
            context.Users.Add(teacherUser);
            context.Users.Add(studentUser);
            context.SaveChanges();

            context.UserRoles.Add(new UserRoleAssignment { UserId = adminUser.Id, Role = UserRole.Admin });
            context.UserRoles.Add(new UserRoleAssignment { UserId = teacherUser.Id, Role = UserRole.Teacher });
            context.UserRoles.Add(new UserRoleAssignment { UserId = studentUser.Id, Role = UserRole.Student });

            context.Subjects.Add(new Subject { Name = "Mathematics", CreatedAt = now });
            context.Subjects.Add(new Subject { Name = "Science", CreatedAt = now });
            context.Subjects.Add(new Subject { Name = "English", CreatedAt = now });

            context.SaveChanges();
        }
    }
}
