using Microsoft.AspNetCore.Components;
using twaProject.Classes;

namespace twaProject.Components.Templates;

public partial class SelectUser : ComponentBase
{
    public static List<WebUser> Users { get; set; } = new();

}