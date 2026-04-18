using System;
using System.IO;
using System.Data.SQLite;
using System.Windows.Forms;
using QuizGenAI.Data;
using QuizGenAI.Forms.Auth;
using QuizGenAI.Services;

namespace QuizGenAI
{
    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            try
            {
                var localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                var dataDirectory = Path.Combine(localAppData, "QuizGenAI", "App_Data");
                var logDirectory = Path.Combine(localAppData, "QuizGenAI", "Logs");
                Directory.CreateDirectory(dataDirectory);
                AppDomain.CurrentDomain.SetData("DataDirectory", dataDirectory);

                LoggingService.Initialize(logDirectory);
                Application.ThreadException += Application_ThreadException;
                AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
                Application.ApplicationExit += delegate { LoggingService.CloseAndFlush(); };

                var databasePath = Path.Combine(dataDirectory, "quizgenai.db");
                if (File.Exists(databasePath) && !HasRequiredSchema(databasePath))
                {
                    File.Delete(databasePath);
                }

                EnsureDatabaseSchema(databasePath);

                using (var context = new QuizGenAIDbContext())
                {
                    QuizGenAIDatabaseInitializer.SeedIfNeeded(context);
                }

                LoggingService.Information("Application startup completed successfully.");
                Application.Run(new LoginForm());
            }
            catch (Exception ex)
            {
                LoggingService.Fatal(ex, "Application startup failed.");
                MessageBox.Show(
                    ex.ToString(),
                    "Startup Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                LoggingService.CloseAndFlush();
            }
        }

        private static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            LoggingService.Error(e.Exception, "Unhandled UI thread exception.");
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var exception = e.ExceptionObject as Exception ?? new Exception("Unknown unhandled exception.");
            LoggingService.Fatal(exception, "Unhandled domain exception. IsTerminating={IsTerminating}", e.IsTerminating);
        }

        private static bool HasRequiredSchema(string databasePath)
        {
            var connectionString = string.Format("Data Source={0};Version=3;", databasePath);

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT COUNT(*) FROM sqlite_master WHERE type = 'table' AND name = 'users';";
                    var usersTableCount = Convert.ToInt32(command.ExecuteScalar());
                    return usersTableCount > 0;
                }
            }
        }

        private static void EnsureDatabaseSchema(string databasePath)
        {
            if (File.Exists(databasePath) && HasRequiredSchema(databasePath))
            {
                return;
            }

            var connectionString = string.Format("Data Source={0};Version=3;Foreign Keys=True;", databasePath);
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = @"
PRAGMA foreign_keys = ON;

CREATE TABLE IF NOT EXISTS users (
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    email TEXT NOT NULL,
    password_hash TEXT NOT NULL,
    name TEXT NOT NULL,
    created_at DATETIME NOT NULL,
    updated_at DATETIME NOT NULL
);

CREATE UNIQUE INDEX IF NOT EXISTS IX_Users_Email ON users (email);

CREATE TABLE IF NOT EXISTS subjects (
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    name TEXT NOT NULL,
    created_at DATETIME NOT NULL
);

CREATE TABLE IF NOT EXISTS user_roles (
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    user_id INTEGER NOT NULL,
    role INTEGER NOT NULL,
    FOREIGN KEY(user_id) REFERENCES users(id) ON DELETE CASCADE
);

CREATE UNIQUE INDEX IF NOT EXISTS IX_UserRoles_UserRole ON user_roles (user_id, role);

CREATE TABLE IF NOT EXISTS quizzes (
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    title TEXT NOT NULL,
    subject_id INTEGER NOT NULL,
    topic TEXT NOT NULL,
    difficulty INTEGER NOT NULL,
    duration_minutes INTEGER NOT NULL,
    status INTEGER NOT NULL,
    created_by INTEGER NOT NULL,
    created_at DATETIME NOT NULL,
    updated_at DATETIME NOT NULL,
    ai_generated INTEGER NOT NULL,
    available_from DATETIME NULL,
    available_until DATETIME NULL,
    FOREIGN KEY(subject_id) REFERENCES subjects(id),
    FOREIGN KEY(created_by) REFERENCES users(id)
);

CREATE TABLE IF NOT EXISTS questions (
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    quiz_id INTEGER NOT NULL,
    text TEXT NOT NULL,
    question_type INTEGER NOT NULL,
    explanation TEXT NULL,
    order_index INTEGER NOT NULL,
    created_at DATETIME NOT NULL,
    updated_at DATETIME NOT NULL,
    FOREIGN KEY(quiz_id) REFERENCES quizzes(id) ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS choices (
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    question_id INTEGER NOT NULL,
    text TEXT NOT NULL,
    is_correct INTEGER NOT NULL,
    order_index INTEGER NOT NULL,
    FOREIGN KEY(question_id) REFERENCES questions(id) ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS student_attempts (
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    quiz_id INTEGER NOT NULL,
    student_id INTEGER NOT NULL,
    attempt_number INTEGER NOT NULL,
    started_at DATETIME NOT NULL,
    submitted_at DATETIME NULL,
    score_percentage REAL NULL,
    time_spent_seconds INTEGER NOT NULL,
    FOREIGN KEY(quiz_id) REFERENCES quizzes(id),
    FOREIGN KEY(student_id) REFERENCES users(id)
);

CREATE TABLE IF NOT EXISTS student_answers (
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    attempt_id INTEGER NOT NULL,
    question_id INTEGER NOT NULL,
    selected_choice_id INTEGER NULL,
    is_correct INTEGER NOT NULL,
    answered_at DATETIME NOT NULL,
    FOREIGN KEY(attempt_id) REFERENCES student_attempts(id) ON DELETE CASCADE,
    FOREIGN KEY(question_id) REFERENCES questions(id),
    FOREIGN KEY(selected_choice_id) REFERENCES choices(id)
);

CREATE TABLE IF NOT EXISTS ai_generations (
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    quiz_id INTEGER NOT NULL,
    prompt TEXT NOT NULL,
    raw_response_json TEXT NULL,
    provider TEXT NULL,
    model_name TEXT NULL,
    created_at DATETIME NOT NULL,
    FOREIGN KEY(quiz_id) REFERENCES quizzes(id) ON DELETE CASCADE
);";
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
