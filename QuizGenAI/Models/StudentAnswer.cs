using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuizGenAI.Models
{
    [Table("student_answers")]
    public class StudentAnswer
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("attempt_id")]
        public int AttemptId { get; set; }

        [Column("question_id")]
        public int QuestionId { get; set; }

        [Column("selected_choice_id")]
        public int? SelectedChoiceId { get; set; }

        [Column("is_correct")]
        public bool IsCorrect { get; set; }

        [Column("answered_at")]
        public DateTime AnsweredAt { get; set; }

        public virtual StudentAttempt Attempt { get; set; }
        public virtual Question Question { get; set; }
        public virtual Choice SelectedChoice { get; set; }
    }
}
