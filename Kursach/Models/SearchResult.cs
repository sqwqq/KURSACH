namespace Kursach.Models;

public class SearchResult
{
    public string Query { get; set; } = "";
    public List<Athlete> Athletes { get; set; } = new();
    public List<News> News { get; set; } = new();
    public List<Achievement> Achievements { get; set; } = new();
    public List<Team> Teams { get; set; } = new();
}
