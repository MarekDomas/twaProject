using System.ComponentModel.DataAnnotations;

namespace twaProject.Classes;

public class Task
{
    public int TaskId { get; set; }
    [Required]
    public string Name { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    [Required]
    public string Description { get; set; }

    public bool IsCompleted { get; set; }
    
    //Navigation properties
    public int? WebUserId { get; set; }
    public WebUser? WebUser { get; set; } 
    public int ProjektId { get; set; }
    public Projekt Projekt { get; set; }
    
}