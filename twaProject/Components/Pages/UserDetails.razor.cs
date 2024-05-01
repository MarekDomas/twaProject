using Microsoft.AspNetCore.Components;

namespace twaProject.Components.Pages;

public partial class UserDetails
{
    [Parameter]
    public bool Succes { get; set; } = false;
    private void createProject()
    {
        navigationManager.NavigateTo("createProjPage");
    }
    private void editProject(int id)
    {
        navigationManager.NavigateTo($"createProjPage/{id}");
    }
}