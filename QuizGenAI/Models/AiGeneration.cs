using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuizGenAI.Models
{
    [Table("ai_generations")]
    public class AiGeneration
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("quiz_id")]
        public int QuizId { get; set; }

        [Required]
        [Column("prompt")]
        public string Prompt { get; set; }

        [Column("raw_response_json")]
        public string RawResponseJson { get; set; }

        [MaxLength(100)]
        [Column("provider")]
        public string Provider { get; set; }

        [MaxLength(100)]
        [Column("model_name")]
        public string ModelName { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        public virtual Quiz Quiz { get; set; }
    }
}
