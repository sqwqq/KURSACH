using System.ComponentModel.DataAnnotations;

namespace Kursach.Models;

public class Competition
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Название обязательно")]
    [StringLength(200, ErrorMessage = "Название не более 200 символов")]
    [Display(Name = "Название")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "Дата обязательна")]
    [Display(Name = "Дата")]
    public DateTime Date { get; set; }

    [StringLength(200, ErrorMessage = "Место не более 200 символов")]
    [Display(Name = "Место проведения")]
    public string Location { get; set; } = string.Empty;

    [Required(ErrorMessage = "Вид спорта обязателен")]
    [StringLength(100, ErrorMessage = "Вид спорта не более 100 символов")]
    [Display(Name = "Вид спорта")]
    public string SportType { get; set; } = string.Empty;

    [StringLength(1000, ErrorMessage = "Описание не более 1000 символов")]
    [Display(Name = "Описание")]
    public string Description { get; set; } = string.Empty;

    [Display(Name = "Завершено")]
    public bool IsCompleted { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public int? TeamId { get; set; }
    public Team? Team { get; set; }
}
