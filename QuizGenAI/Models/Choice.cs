using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuizGenAI.Models
{
    [Table("choices")]
    public class Choice
    {
        public Choice()
        {
            SelectedByAnswers = new HashSet<StudentAnswer>();
        }

        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("question_id")]
        public int QuestionId { get; set; }

        [Required]
        [MaxLength(500)]
        [Column("text")]
        public string Text { get; set; }

        [Column("is_correct")]
        public bool IsCorrect { get; set; }

        [Column("order_index")]
        public int OrderIndex { get; set; }

        public virtual Question Question { get; set; }
        public virtual ICollection<StudentAnswer> SelectedByAnswers { get; set; }
    }
}
