using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using QuizGenAI.Enums;
using QuizGenAI.Models;

namespace QuizGenAI.Data
{
    public static class QuizGenAIDatabaseInitializer
    {
        public static void SeedIfNeeded(QuizGenAIDbContext context)
        {
            context.Database.Initialize(false);

            var now = DateTime.UtcNow;

            var admin = EnsureUser(context, "admin@quizgenai.local", "Admin123!", "Demo Admin", now);
            var teacher = EnsureUser(context, "teacher@quizgenai.local", "Teacher123!", "Demo Teacher", now);
            var student = EnsureUser(context, "student@quizgenai.local", "Student123!", "Demo Student", now);

            EnsureRole(context, admin.Id, UserRole.Admin);
            EnsureRole(context, teacher.Id, UserRole.Teacher);
            EnsureRole(context, student.Id, UserRole.Student);

            var mathematics = EnsureSubject(context, "Mathematics", now);
            var science = EnsureSubject(context, "Science", now);
            var english = EnsureSubject(context, "English", now);
            EnsureSubject(context, "History", now);
            EnsureSubject(context, "Geography", now);
            EnsureSubject(context, "Computer Science", now);
            EnsureSubject(context, "Literature", now);
            EnsureSubject(context, "Social Studies", now);

            context.SaveChanges();

            EnsureQuizWithQuestions(
                context,
                teacher.Id,
                mathematics.Id,
                "Fractions Fundamentals",
                "Understanding equivalent fractions",
                QuizDifficulty.Easy,
                15,
                new[]
                {
                    NewQuestion(
                        "Which fraction is equivalent to 1/2?",
                        "Equivalent fractions describe the same portion of a whole.",
                        "2/4",
                        "2/4",
                        "3/4",
                        "1/3",
                        "4/5"),
                    NewQuestion(
                        "What is 3/6 written in simplest form?",
                        "Divide the numerator and denominator by their greatest common factor.",
                        "1/2",
                        "1/2",
                        "2/3",
                        "3/5",
                        "5/6"),
                    NewQuestion(
                        "Which fraction is larger than 1/2?",
                        "Compare how much of the whole each fraction represents.",
                        "5/8",
                        "5/8",
                        "1/4",
                        "2/5",
                        "3/10")
                },
                now.AddMonths(-4));

            EnsureQuizWithQuestions(
                context,
                teacher.Id,
                science.Id,
                "Cell Basics",
                "Plant and animal cell structures",
                QuizDifficulty.Medium,
                20,
                new[]
                {
                    NewQuestion(
                        "Which organelle is known as the control center of the cell?",
                        "The nucleus stores DNA and directs many cell activities.",
                        "Nucleus",
                        "Nucleus",
                        "Cell membrane",
                        "Cytoplasm",
                        "Ribosome"),
                    NewQuestion(
                        "Which cell part helps plants make food using sunlight?",
                        "Chloroplasts contain chlorophyll for photosynthesis.",
                        "Chloroplast",
                        "Chloroplast",
                        "Vacuole",
                        "Mitochondrion",
                        "Nucleus"),
                    NewQuestion(
                        "Which structure controls what enters and leaves the cell?",
                        "The cell membrane regulates transport.",
                        "Cell membrane",
                        "Cell membrane",
                        "Cell wall",
                        "Golgi body",
                        "Nucleolus")
                },
                now.AddMonths(-2));

            EnsureQuizWithQuestions(
                context,
                teacher.Id,
                english.Id,
                "Vocabulary Review",
                "Context clues and word meaning",
                QuizDifficulty.Medium,
                12,
                new[]
                {
                    NewQuestion(
                        "What is the best synonym for diligent?",
                        "Diligent describes someone who works carefully and steadily.",
                        "Hardworking",
                        "Hardworking",
                        "Careless",
                        "Silent",
                        "Angry"),
                    NewQuestion(
                        "Which word best completes the sentence: The river was so placid that it looked like glass.",
                        "Placid means calm or still.",
                        "Calm",
                        "Calm",
                        "Noisy",
                        "Dangerous",
                        "Cloudy"),
                    NewQuestion(
                        "Which option best describes reluctant?",
                        "Reluctant means unwilling or hesitant.",
                        "Unwilling",
                        "Excited",
                        "Unwilling",
                        "Confused",
                        "Prepared")
                },
                now.AddMonths(-1));

            context.SaveChanges();

            EnsureDemoAttempts(context, student.Id, now);
            context.SaveChanges();
        }

        private static User EnsureUser(QuizGenAIDbContext context, string email, string password, string name, DateTime now)
        {
            var user = context.Users.SingleOrDefault(x => x.Email == email);
            if (user != null)
            {
                return user;
            }

            user = new User
            {
                Email = email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
                Name = name,
                CreatedAt = now,
                UpdatedAt = now
            };

            context.Users.Add(user);
            context.SaveChanges();
            return user;
        }

        private static void EnsureRole(QuizGenAIDbContext context, int userId, UserRole role)
        {
            var exists = context.UserRoles.Any(x => x.UserId == userId && x.Role == role);
            if (exists)
            {
                return;
            }

            context.UserRoles.Add(new UserRoleAssignment
            {
                UserId = userId,
                Role = role
            });
        }

        private static Subject EnsureSubject(QuizGenAIDbContext context, string name, DateTime now)
        {
            var subject = context.Subjects.SingleOrDefault(x => x.Name == name);
            if (subject != null)
            {
                return subject;
            }

            subject = new Subject
            {
                Name = name,
                CreatedAt = now
            };

            context.Subjects.Add(subject);
            context.SaveChanges();
            return subject;
        }

        private static void EnsureQuizWithQuestions(
            QuizGenAIDbContext context,
            int teacherId,
            int subjectId,
            string title,
            string topic,
            QuizDifficulty difficulty,
            int durationMinutes,
            IReadOnlyList<SeedQuestion> questions,
            DateTime createdAt)
        {
            var quiz = context.Quizzes
                .Include(x => x.Questions.Select(q => q.Choices))
                .SingleOrDefault(x => x.Title == title && x.SubjectId == subjectId);

            if (quiz == null)
            {
                quiz = new Quiz
                {
                    Title = title,
                    SubjectId = subjectId,
                    Topic = topic,
                    Difficulty = difficulty,
                    DurationMinutes = durationMinutes,
                    Status = QuizStatus.Published,
                    CreatedBy = teacherId,
                    CreatedAt = createdAt,
                    UpdatedAt = createdAt,
                    AiGenerated = false,
                    AvailableFrom = createdAt.Date,
                    AvailableUntil = null
                };

                context.Quizzes.Add(quiz);
                context.SaveChanges();
            }

            if (quiz.Questions.Any())
            {
                return;
            }

            for (var i = 0; i < questions.Count; i++)
            {
                var seedQuestion = questions[i];
                var question = new Question
                {
                    QuizId = quiz.Id,
                    Text = seedQuestion.Text,
                    QuestionType = QuestionType.MultipleChoice,
                    Explanation = seedQuestion.Explanation,
                    OrderIndex = i + 1,
                    CreatedAt = createdAt.AddMinutes(i),
                    UpdatedAt = createdAt.AddMinutes(i)
                };

                context.Questions.Add(question);
                context.SaveChanges();

                for (var choiceIndex = 0; choiceIndex < seedQuestion.Choices.Count; choiceIndex++)
                {
                    var choiceText = seedQuestion.Choices[choiceIndex];
                    context.Choices.Add(new Choice
                    {
                        QuestionId = question.Id,
                        Text = choiceText,
                        IsCorrect = string.Equals(choiceText, seedQuestion.CorrectChoice, StringComparison.Ordinal),
                        OrderIndex = choiceIndex + 1
                    });
                }
            }
        }

        private static void EnsureDemoAttempts(QuizGenAIDbContext context, int studentId, DateTime now)
        {
            var existingSubmittedAttempts = context.StudentAttempts.Count(x => x.StudentId == studentId && x.SubmittedAt.HasValue);
            if (existingSubmittedAttempts >= 3)
            {
                return;
            }

            var quizzes = context.Quizzes
                .Include(x => x.Subject)
                .Include(x => x.Questions.Select(q => q.Choices))
                .Where(x => x.Status == QuizStatus.Published)
                .OrderBy(x => x.Id)
                .ToList();

            if (quizzes.Count == 0)
            {
                return;
            }

            var attemptPlans = new[]
            {
                new { QuizTitle = "Fractions Fundamentals", SubmittedAt = now.AddMonths(-4).AddDays(6), CorrectCount = 2, TimeSpentSeconds = 610 },
                new { QuizTitle = "Cell Basics", SubmittedAt = now.AddMonths(-2).AddDays(10), CorrectCount = 1, TimeSpentSeconds = 940 },
                new { QuizTitle = "Vocabulary Review", SubmittedAt = now.AddDays(-8), CorrectCount = 3, TimeSpentSeconds = 540 }
            };

            var nextAttemptNumber = context.StudentAttempts
                .Where(x => x.StudentId == studentId)
                .Select(x => (int?)x.AttemptNumber)
                .DefaultIfEmpty(0)
                .Max() ?? 0;

            foreach (var plan in attemptPlans)
            {
                var quiz = quizzes.FirstOrDefault(x => x.Title == plan.QuizTitle);
                if (quiz == null)
                {
                    continue;
                }

                var alreadyExists = context.StudentAttempts.Any(x =>
                    x.StudentId == studentId &&
                    x.QuizId == quiz.Id &&
                    x.SubmittedAt.HasValue);

                if (alreadyExists)
                {
                    continue;
                }

                nextAttemptNumber++;
                var orderedQuestions = quiz.Questions.OrderBy(x => x.OrderIndex).ToList();
                if (orderedQuestions.Count == 0)
                {
                    continue;
                }

                var attempt = new StudentAttempt
                {
                    QuizId = quiz.Id,
                    StudentId = studentId,
                    AttemptNumber = nextAttemptNumber,
                    StartedAt = plan.SubmittedAt.AddSeconds(-plan.TimeSpentSeconds),
                    SubmittedAt = plan.SubmittedAt,
                    ScorePercentage = Math.Round((double)plan.CorrectCount / orderedQuestions.Count * 100D, 1),
                    TimeSpentSeconds = plan.TimeSpentSeconds
                };

                context.StudentAttempts.Add(attempt);
                context.SaveChanges();

                for (var i = 0; i < orderedQuestions.Count; i++)
                {
                    var question = orderedQuestions[i];
                    var correctChoice = question.Choices.Single(x => x.IsCorrect);
                    var selectedChoice = i < plan.CorrectCount
                        ? correctChoice
                        : question.Choices.First(x => !x.IsCorrect);

                    context.StudentAnswers.Add(new StudentAnswer
                    {
                        AttemptId = attempt.Id,
                        QuestionId = question.Id,
                        SelectedChoiceId = selectedChoice.Id,
                        IsCorrect = selectedChoice.Id == correctChoice.Id,
                        AnsweredAt = attempt.StartedAt.AddMinutes(i + 1)
                    });
                }
            }
        }

        private static SeedQuestion NewQuestion(
            string text,
            string explanation,
            string correctChoice,
            params string[] choices)
        {
            return new SeedQuestion
            {
                Text = text,
                Explanation = explanation,
                CorrectChoice = correctChoice,
                Choices = choices.ToList()
            };
        }

        private sealed class SeedQuestion
        {
            public string Text { get; set; }
            public string Explanation { get; set; }
            public string CorrectChoice { get; set; }
            public List<string> Choices { get; set; }
        }
    }
}
