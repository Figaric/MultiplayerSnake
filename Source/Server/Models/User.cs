using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace MultiplayerSnake.Server
{
    [Index(nameof(Username), IsUnique = true)]
    public class User
    {
        public int Id { get; set; }

        [StringLength(255)]
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        [Range(1, 14)]
        public int Color { get; set; }
    }
}
