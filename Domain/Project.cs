using System.ComponentModel.DataAnnotations.Schema;

namespace Domain;

public class Project {
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    
    [ForeignKey("User")]
    public int UserId { get; set; }
    public User User { get; set; }
    
    public List<Task> Tasks { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}