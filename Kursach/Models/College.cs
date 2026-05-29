namespace Kursach.Models;

public class College
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public ICollection<Team> Teams { get; set; } = new List<Team>();
}