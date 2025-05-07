using System.ComponentModel.DataAnnotations.Schema;

namespace Domain;

public class Project {
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    
    [ForeignKey("User")]
    public int UserId { get; set; }
    public User User { get; set; } = new User();
    
    public List<Task> Tasks { get; set; } = new List<Task>();
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}