using System.ComponentModel.DataAnnotations.Schema;

namespace Domain;

public class Project {
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty; 
    public List<Task> Tasks { get; set; } = new List<Task>();
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}