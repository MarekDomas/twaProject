using Microsoft.AspNetCore.Components.Forms;
using twaProject.Classes;
using Task = System.Threading.Tasks.Task;

namespace twaProject.Components.Pages;

public partial class Login
{
    private WebUser WebUser = new();
    private bool succes = true;

    protected override void OnInitialized()
    {
        localStorage.DeleteAsync("currentUser");
        localStorage.DeleteAsync("isUserLogged");
        base.OnInitialized();
    }

    private async Task Submit(EditContext editContext)
    {
        var tempUser = (WebUser)editContext.Model;
        
        context.Database.EnsureCreated();
        succes = context.WebUser.Any(u => u.Name == tempUser.Name && u.Password == tempUser.Password);
        if (succes)
        {
            stateManager.isUserLogged = true;
            stateManager.CurrentUser = context.WebUser.FirstOrDefault( u => u.Name == tempUser.Name);
            
            await localStorage.SetAsync("isUserLogged", true);
            await localStorage.SetAsync("currentUser", stateManager.CurrentUser);
            
            navigationManager.NavigateTo("userDetails");
        }
    }

    private void navToRegister()
    {
        navigationManager.NavigateTo("registerPage");
    }
}