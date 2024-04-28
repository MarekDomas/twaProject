namespace twaProject.Classes;

public class Task
{
    public int TaskId { get; set; }
    public string Name { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public string Description { get; set; }
    public bool IsCompleted { get; set; }
    
    //Foreign key
    public int? WebUserId { get; set; }
    public WebUser? WebUser { get; set; } 
}