using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using QuizGenAI.Enums;

namespace QuizGenAI.Models
{
    [Table("questions")]
    public class Question
    {
        public Question()
        {
            Choices = new HashSet<Choice>();
            StudentAnswers = new HashSet<StudentAnswer>();
        }

        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("quiz_id")]
        public int QuizId { get; set; }

        [Required]
        [MaxLength(2000)]
        [Column("text")]
        public string Text { get; set; }

        [Column("question_type")]
        public QuestionType QuestionType { get; set; }

        [MaxLength(4000)]
        [Column("explanation")]
        public string Explanation { get; set; }

        [Column("order_index")]
        public int OrderIndex { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; }

        public virtual Quiz Quiz { get; set; }
        public virtual ICollection<Choice> Choices { get; set; }
        public virtual ICollection<StudentAnswer> StudentAnswers { get; set; }
    }
}
