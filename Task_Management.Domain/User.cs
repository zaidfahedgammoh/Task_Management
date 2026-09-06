using System.ComponentModel.DataAnnotations;

namespace Task_Management.Domain
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string username { get; set; } = string.Empty;

        [Required]

        public string email { get; set; } = string.Empty;

        [Required]
        public string password { get; set; } = string.Empty;

        [Phone]
        public string? mobile_number { get; set; }

        public UserRoles role { get; set; }

    }
}
