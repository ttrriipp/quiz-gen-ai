using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuizGenAI.Models
{
    [Table("users")]
    public class User
    {
        public User()
        {
            Roles = new HashSet<UserRoleAssignment>();
            CreatedQuizzes = new HashSet<Quiz>();
            StudentAttempts = new HashSet<StudentAttempt>();
        }

        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [MaxLength(150)]
        [Index("IX_Users_Email", IsUnique = true)]
        [Column("email")]
        public string Email { get; set; }

        [Required]
        [MaxLength(255)]
        [Column("password_hash")]
        public string PasswordHash { get; set; }

        [Required]
        [MaxLength(150)]
        [Column("name")]
        public string Name { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; }

        public virtual ICollection<UserRoleAssignment> Roles { get; set; }
        public virtual ICollection<Quiz> CreatedQuizzes { get; set; }
        public virtual ICollection<StudentAttempt> StudentAttempts { get; set; }
    }
}
