using System.Data.Entity;
using QuizGenAI.Models;

namespace QuizGenAI.Data
{
    public class QuizGenAIDbContext : DbContext
    {
        public QuizGenAIDbContext() : base("name=QuizGenAIDbContext")
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<UserRoleAssignment> UserRoles { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Quiz> Quizzes { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Choice> Choices { get; set; }
        public DbSet<StudentAttempt> StudentAttempts { get; set; }
        public DbSet<StudentAnswer> StudentAnswers { get; set; }
        public DbSet<AiGeneration> AiGenerations { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasMany(x => x.Roles)
                .WithRequired(x => x.User)
                .HasForeignKey(x => x.UserId)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<User>()
                .HasMany(x => x.CreatedQuizzes)
                .WithRequired(x => x.CreatedByUser)
                .HasForeignKey(x => x.CreatedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(x => x.StudentAttempts)
                .WithRequired(x => x.Student)
                .HasForeignKey(x => x.StudentId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Subject>()
                .HasMany(x => x.Quizzes)
                .WithRequired(x => x.Subject)
                .HasForeignKey(x => x.SubjectId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Quiz>()
                .HasMany(x => x.Questions)
                .WithRequired(x => x.Quiz)
                .HasForeignKey(x => x.QuizId)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<Quiz>()
                .HasMany(x => x.StudentAttempts)
                .WithRequired(x => x.Quiz)
                .HasForeignKey(x => x.QuizId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Quiz>()
                .HasMany(x => x.AiGenerations)
                .WithRequired(x => x.Quiz)
                .HasForeignKey(x => x.QuizId)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<Question>()
                .HasMany(x => x.Choices)
                .WithRequired(x => x.Question)
                .HasForeignKey(x => x.QuestionId)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<Question>()
                .HasMany(x => x.StudentAnswers)
                .WithRequired(x => x.Question)
                .HasForeignKey(x => x.QuestionId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<StudentAttempt>()
                .HasMany(x => x.Answers)
                .WithRequired(x => x.Attempt)
                .HasForeignKey(x => x.AttemptId)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<Choice>()
                .HasMany(x => x.SelectedByAnswers)
                .WithOptional(x => x.SelectedChoice)
                .HasForeignKey(x => x.SelectedChoiceId)
                .WillCascadeOnDelete(false);

            base.OnModelCreating(modelBuilder);
        }
    }
}
