using System.ComponentModel.DataAnnotations;

namespace Kursach.Models;

public class College
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Название колледжа обязательно")]
    [StringLength(200, MinimumLength = 3, ErrorMessage = "Название должно содержать от 3 до 200 символов")]
    [Display(Name = "Название колледжа")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Город обязателен")]
    [StringLength(100, ErrorMessage = "Город не более 100 символов")]
    [Display(Name = "Город")]
    public string City { get; set; } = string.Empty;

    public ICollection<Team> Teams { get; set; } = new List<Team>();

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}