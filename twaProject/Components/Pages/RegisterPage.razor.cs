using Microsoft.AspNetCore.Components.Forms;
using twaProject.Classes;

namespace twaProject.Components.Pages;

public partial class RegisterPage
{
    private bool passwordsAlign = true;
    private bool succes = false;
    private bool exists = false;
    private WebUser _webUser = new();
    private string psswAgain = string.Empty;

    protected override void OnInitialized()
    {
        _webUser = new();
        base.OnInitialized();
    }

    private void Register(EditContext editContext)
    {
        if (_webUser.Password != psswAgain)
        {
            passwordsAlign = false;   
            return;
        }
        
        using (var context = new MainDbContext())
        {
            var tempUser = (WebUser)editContext.Model;
            context.Database.EnsureCreated();
            exists = context.WebUser.Any(u => u.Name == tempUser.Name);
            if (!exists)
            {
                context.Database.EnsureCreated();
                context.WebUser.Add(tempUser);
                context.SaveChanges();
                succes = true;
            }
        }
    }
}