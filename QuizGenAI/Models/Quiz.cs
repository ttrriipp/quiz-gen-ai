using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using QuizGenAI.Enums;

namespace QuizGenAI.Models
{
    [Table("quizzes")]
    public class Quiz
    {
        public Quiz()
        {
            Questions = new HashSet<Question>();
            StudentAttempts = new HashSet<StudentAttempt>();
            AiGenerations = new HashSet<AiGeneration>();
        }

        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        [Column("title")]
        public string Title { get; set; }

        [Column("subject_id")]
        public int SubjectId { get; set; }

        [Required]
        [MaxLength(200)]
        [Column("topic")]
        public string Topic { get; set; }

        [Column("difficulty")]
        public QuizDifficulty Difficulty { get; set; }

        [Column("duration_minutes")]
        public int DurationMinutes { get; set; }

        [Column("status")]
        public QuizStatus Status { get; set; }

        [Column("created_by")]
        public int CreatedBy { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; }

        [Column("ai_generated")]
        public bool AiGenerated { get; set; }

        [Column("available_from")]
        public DateTime? AvailableFrom { get; set; }

        [Column("available_until")]
        public DateTime? AvailableUntil { get; set; }

        [Column("is_deleted")]
        public bool IsDeleted { get; set; }

        public virtual Subject Subject { get; set; }
        public virtual User CreatedByUser { get; set; }
        public virtual ICollection<Question> Questions { get; set; }
        public virtual ICollection<StudentAttempt> StudentAttempts { get; set; }
        public virtual ICollection<AiGeneration> AiGenerations { get; set; }
    }
}
