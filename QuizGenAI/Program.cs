using System;
using System.Data.Entity;
using System.IO;
using System.Windows.Forms;
using QuizGenAI.Data;
using QuizGenAI.Forms.Auth;

namespace QuizGenAI
{
    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            var dataDirectory = Path.Combine(Application.StartupPath, "App_Data");
            Directory.CreateDirectory(dataDirectory);
            AppDomain.CurrentDomain.SetData("DataDirectory", dataDirectory);

            Database.SetInitializer(new QuizGenAIDatabaseInitializer());

            using (var context = new QuizGenAIDbContext())
            {
                context.Database.Initialize(false);
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new LoginForm());
        }
    }
}
