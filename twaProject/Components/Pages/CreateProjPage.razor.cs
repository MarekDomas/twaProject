using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using twaProject.Classes;
using twaProject.Components.Templates;

namespace twaProject.Components.Pages;

public partial class CreateProjPage : ComponentBase
{
    private Projekt _projekt = new();
    private WebUser selectedUser { get; set; } 
    private List<WebUser> memberUsers { get; set; }= new();
    private bool succes = false;
    
    protected override void OnInitialized()
    {
        _projekt = new();
        _projekt.MemberUsers = new();
        _projekt.StartDate = DateOnly.FromDateTime(DateTime.Now.Date);
        _projekt.EndDate = DateOnly.FromDateTime(DateTime.Now.Date);
        SelectUser.Users = _projekt.MemberUsers;
        base.OnInitialized();
    }

    private void Submit(EditContext editContext)
    {
        Projekt tempProj = (Projekt)editContext.Model;

      
        context.Database.EnsureCreated();

        // Fetch the user from the database within the same context
        var existingUser = context.WebUser.Find(stateManager.CurrentUser.WebUserId);

        if (existingUser != null)
        {
            // Then add the user to the project
            tempProj.MemberUsers.Add(existingUser);

            context.Projekt.Add(tempProj);
            context.SaveChanges();
            succes = true;
            StateHasChanged();
            navigationManager.NavigateTo($"userDetails/{true}");
        }
        
    }
    private List<SelectUser> selectUserComponents = new List<SelectUser>();
    private void addMember(ChangeEventArgs e)
    {
        string tempUser = (string)e.Value ;
        WebUser userToAdd = context.WebUser.FirstOrDefault(u => u.Name == tempUser);

        _projekt.MemberUsers.Add(userToAdd);
        //StateHasChanged();
    }

    private void addSelect()
    {
        if (memberUsers.Count() < context.WebUser.Count())
        {
            memberUsers.Add(new());
            StateHasChanged();
        }
    }

}