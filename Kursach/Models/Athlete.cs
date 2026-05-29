namespace Kursach.Models;

public class Athlete
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Group { get; set; } = string.Empty;
    public string PhotoUrl { get; set; } = string.Empty;
    public string Bio { get; set; } = string.Empty;
    public double Height { get; set; }
    public double Weight { get; set; }
    public int TeamId { get; set; }
    public Team Team { get; set; } = null!;
    public ICollection<Achievement> Achievements { get; set; } = new List<Achievement>();
    public ICollection<News> News { get; set; } = new List<News>();
}