using System.Diagnostics;
using Microsoft.AspNetCore.Components.Forms;
using twaProject.Classes;

namespace twaProject.Components.Pages;

public partial class Login
{
    public WebUser WebUser = new();
    private bool succes = true;

    protected override void OnInitialized()
    {
        WebUser = new();
        base.OnInitialized();
    }

    private void Submit(EditContext editContext)
    {
        var tempUser = (WebUser)editContext.Model;

        using (var context = new MainDbContext())
        {
            context.Database.EnsureCreated();
            succes = context.WebUsers.Any(u => u.Name == tempUser.Name && u.Password == tempUser.Password);
            if (succes)
            {
                stateManager.isUserLogged = true;
                navigationManager.NavigateTo("userDetails");
                StateHasChanged();
            }
        }
    }

    private void navToRegister()
    {
        navigationManager.NavigateTo("registerPage");
    }
}