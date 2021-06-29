using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace MultiplayerSnake.Server.Entities
{
    [Index(nameof(Username), IsUnique = true)]
    public class User
    {
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Username { get; set; }

        [Required]
        [StringLength(255)]
        public string Password { get; set; }
    }
}
