using System.ComponentModel.DataAnnotations;

namespace Kursach.Models;

public class News
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Заголовок обязателен")]
    [StringLength(200, MinimumLength = 3, ErrorMessage = "Заголовок должен содержать от 3 до 200 символов")]
    [Display(Name = "Заголовок")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "Содержание обязательно")]
    [StringLength(10000, ErrorMessage = "Содержание не более 10000 символов")]
    [Display(Name = "Содержание")]
    public string Content { get; set; } = string.Empty;

    [Required(ErrorMessage = "Дата обязательна")]
    [Display(Name = "Дата")]
    public DateTime Date { get; set; }

    [Display(Name = "Изображение (URL)")]
    public string ImageUrl { get; set; } = string.Empty;

    [Required(ErrorMessage = "Категория обязательна")]
    [StringLength(50, ErrorMessage = "Категория не более 50 символов")]
    [Display(Name = "Категория")]
    public string Category { get; set; } = string.Empty;

    [Display(Name = "Избранная")]
    public bool IsFeatured { get; set; }

    [Display(Name = "Спортсмен")]
    public int? AthleteId { get; set; }
    public Athlete? Athlete { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}