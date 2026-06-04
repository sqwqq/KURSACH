using System.ComponentModel.DataAnnotations;

namespace Kursach.Models;

public class Team
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Название команды обязательно")]
    [StringLength(200, MinimumLength = 3, ErrorMessage = "Название должно содержать от 3 до 200 символов")]
    [Display(Name = "Название команды")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Вид спорта обязателен")]
    [StringLength(100, ErrorMessage = "Вид спорта не более 100 символов")]
    [Display(Name = "Вид спорта")]
    public string SportType { get; set; } = string.Empty;

    [Required(ErrorMessage = "Имя тренера обязательно")]
    [StringLength(200, ErrorMessage = "Имя тренера не более 200 символов")]
    [Display(Name = "Тренер")]
    public string CoachName { get; set; } = string.Empty;

    [StringLength(2000, ErrorMessage = "Описание не более 2000 символов")]
    [Display(Name = "Описание")]
    public string Description { get; set; } = string.Empty;

    [Display(Name = "Колледж")]
    public int CollegeId { get; set; }
    public College College { get; set; } = null!;
    public ICollection<Athlete> Athletes { get; set; } = new List<Athlete>();
    public ICollection<Achievement> TeamAchievements { get; set; } = new List<Achievement>();
    public ICollection<Competition> Competitions { get; set; } = new List<Competition>();

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}