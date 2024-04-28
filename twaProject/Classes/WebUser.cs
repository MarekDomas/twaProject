using System.ComponentModel.DataAnnotations;

namespace twaProject.Classes;

public class WebUser
{
    public int WebUserId { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    public string Password { get; set; }
    
    //Foreign key
    public List<Projekt>? Projekts { get; set; } = [];
    public IList<Task>? Tasks { get; set; }= new List<Task>();
    
}