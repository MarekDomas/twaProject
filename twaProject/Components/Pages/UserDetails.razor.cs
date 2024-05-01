using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using twaProject.Classes;
using Task = System.Threading.Tasks.Task;

namespace twaProject.Components.Pages;

public partial class UserDetails
{
    [Parameter]
    public bool Succes { get; set; } = false;
    
    protected override async Task OnInitializedAsync()
    {
        var result =  await localStorage.GetAsync<WebUser>("currentUser");
        var userLogged= await localStorage.GetAsync<bool>("isUserLogged");
        stateManager.CurrentUser = result.Value;
        stateManager.isUserLogged = userLogged.Value;
    }

    private void createProject()
    {
        navigationManager.NavigateTo("createProjPage");
    }
    private void editProject(int id)
    {
        navigationManager.NavigateTo($"createProjPage/{id}");
    }
}