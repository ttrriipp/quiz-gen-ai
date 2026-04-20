using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuizGenAI.Models
{
    [Table("student_attempts")]
    public class StudentAttempt
    {
        public StudentAttempt()
        {
            Answers = new HashSet<StudentAnswer>();
        }

        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("quiz_id")]
        public int QuizId { get; set; }

        [Column("student_id")]
        public int StudentId { get; set; }

        [Column("attempt_number")]
        public int AttemptNumber { get; set; }

        [Column("started_at")]
        public DateTime StartedAt { get; set; }

        [Column("submitted_at")]
        public DateTime? SubmittedAt { get; set; }

        [Column("score_percentage")]
        public double? ScorePercentage { get; set; }

        [Column("time_spent_seconds")]
        public int TimeSpentSeconds { get; set; }

        public virtual Quiz Quiz { get; set; }
        public virtual User Student { get; set; }
        public virtual ICollection<StudentAnswer> Answers { get; set; }
    }
}
