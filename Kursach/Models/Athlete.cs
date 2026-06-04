using System.ComponentModel.DataAnnotations;

namespace Kursach.Models;

public class Athlete
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Имя обязательно")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Имя должно содержать от 2 до 100 символов")]
    [Display(Name = "Имя")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Фамилия обязательна")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Фамилия должна содержать от 2 до 100 символов")]
    [Display(Name = "Фамилия")]
    public string LastName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Группа обязательна")]
    [StringLength(20, ErrorMessage = "Группа не более 20 символов")]
    [Display(Name = "Группа")]
    public string Group { get; set; } = string.Empty;

    [Display(Name = "Фото (URL)")]
    public string PhotoUrl { get; set; } = string.Empty;

    [StringLength(1000, ErrorMessage = "Биография не более 1000 символов")]
    [Display(Name = "Биография")]
    public string Bio { get; set; } = string.Empty;

    [Range(100, 250, ErrorMessage = "Рост должен быть от 100 до 250 см")]
    [Display(Name = "Рост (см)")]
    public double Height { get; set; }

    [Range(30, 200, ErrorMessage = "Вес должен быть от 30 до 200 кг")]
    [Display(Name = "Вес (кг)")]
    public double Weight { get; set; }

    [Required(ErrorMessage = "Команда обязательна")]
    [Display(Name = "Команда")]
    public int TeamId { get; set; }
    public Team Team { get; set; } = null!;
    public ICollection<Achievement> Achievements { get; set; } = new List<Achievement>();
    public ICollection<News> News { get; set; } = new List<News>();

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}