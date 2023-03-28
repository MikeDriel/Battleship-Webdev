using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
    public class HighScore
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public int Score { get; set; }
    }
}
