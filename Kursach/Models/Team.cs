namespace Kursach.Models;

public class Team
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string SportType { get; set; } = string.Empty;
    public string CoachName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int CollegeId { get; set; }
    public College College { get; set; } = null!;
    public ICollection<Athlete> Athletes { get; set; } = new List<Athlete>();
    public ICollection<Achievement> TeamAchievements { get; set; } = new List<Achievement>();
}