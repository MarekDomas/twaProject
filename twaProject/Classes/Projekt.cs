namespace twaProject.Classes;

public class Projekt
{
    public int ProjektId { get; set; }
    //Foreign keys
    public List<WebUser> MemberUsers { get; set; }= new List<WebUser>();
    public IList<Task> Tasks { get; set; } = new List<Task>();
    
    
    public string Name { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public string Description { get; set; }
}