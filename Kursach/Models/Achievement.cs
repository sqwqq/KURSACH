using System.ComponentModel.DataAnnotations;

namespace Kursach.Models;

public class Achievement
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Название обязательно")]
    [StringLength(200, ErrorMessage = "Название не более 200 символов")]
    [Display(Name = "Название")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "Дата обязательна")]
    [Display(Name = "Дата")]
    public DateTime Date { get; set; }

    [Required(ErrorMessage = "Вид спорта обязателен")]
    [StringLength(100, ErrorMessage = "Вид спорта не более 100 символов")]
    [Display(Name = "Вид спорта")]
    public string SportType { get; set; } = string.Empty;

    [Required(ErrorMessage = "Место обязательно")]
    [StringLength(50, ErrorMessage = "Место не более 50 символов")]
    [Display(Name = "Место")]
    public string Place { get; set; } = string.Empty;

    [Required(ErrorMessage = "Спортсмен обязателен")]
    [Display(Name = "Спортсмен")]
    public int AthleteId { get; set; }
    public Athlete Athlete { get; set; } = null!;

    [Display(Name = "Команда")]
    public int? TeamId { get; set; }
    public Team? Team { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}