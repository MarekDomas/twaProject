using System.ComponentModel.DataAnnotations;

namespace twaProject.Classes;

public class Projekt
{
    public int ProjektId { get; set; }
    //Foreign keys
    public List<WebUser>? MemberUsers { get; set; }= [];
    public List<Task>? Tasks { get; set; } = new();
    
    [Required]
    public string Name { get; set; }
    [Required]
    public DateOnly StartDate { get; set; }
    [Required]
    public DateOnly EndDate { get; set; }
    public string Description { get; set; }
}