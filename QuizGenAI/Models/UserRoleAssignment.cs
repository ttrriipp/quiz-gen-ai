using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using QuizGenAI.Enums;

namespace QuizGenAI.Models
{
    [Table("user_roles")]
    public class UserRoleAssignment
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Index("IX_UserRoles_UserRole", 1, IsUnique = true)]
        [Column("user_id")]
        public int UserId { get; set; }

        [Index("IX_UserRoles_UserRole", 2, IsUnique = true)]
        [Column("role")]
        public UserRole Role { get; set; }

        public virtual User User { get; set; }
    }
}
