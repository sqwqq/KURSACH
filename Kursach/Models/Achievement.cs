namespace Kursach.Models;

public class Achievement
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public string SportType { get; set; } = string.Empty;
    public string Place { get; set; } = string.Empty;
    public int AthleteId { get; set; }
    public Athlete Athlete { get; set; } = null!;
    public int? TeamId { get; set; }
    public Team? Team { get; set; }
}