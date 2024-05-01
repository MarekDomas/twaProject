using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using twaProject.Classes;
using twaProject.Components.Templates;

namespace twaProject.Components.Pages;

public partial class CreateProjPage : ComponentBase
{
    [Parameter]
    public int? id { get; set; } = null;//Id used for routing purposes
    
    private Projekt _projekt = new();
    private List<WebUser> memberUsers { get; set; }= new();
    private List<Classes.Task> _tasks { get; set; } = new();
    private string trashImage = "Images/trash.png";
    private bool succes = false;
    private int index = 0;
    private bool edit = false;
    private int newUsersCount = 0;
    private int existingUsersCount = 0;
    private List<WebUser> usersToAdd = new();
    private bool projectExists = false;
    
    protected override void OnInitialized()
    {
        if (id is not null)
        {
            _projekt = context.Projekt.FirstOrDefault(p => p.ProjektId == id);
            edit = true;
        }
        else
        {
            _projekt = new();
            _projekt.MemberUsers = new();
            _projekt.StartDate = DateOnly.FromDateTime(DateTime.Now.Date);
            _projekt.EndDate = DateOnly.FromDateTime(DateTime.Now.Date);
            SelectUser.Users = _projekt.MemberUsers;
        }
 
        base.OnInitialized();
    }

    private void Submit(EditContext editContext)
    {
        if (edit)
        {
            _projekt.MemberUsers = _projekt.MemberUsers.Distinct().ToList();
            _projekt.MemberUsers.AddRange(usersToAdd);
            context.SaveChanges();
            navigationManager.NavigateTo("userDetails");
        }
        else
        {
            Projekt tempProj = (Projekt)editContext.Model;

            context.Database.EnsureCreated();

            tempProj.MemberUsers = _projekt.MemberUsers;
            
            tempProj.MemberUsers.Add(stateManager.CurrentUser);
            tempProj.MemberUsers = tempProj.MemberUsers.Distinct().ToList();

            context.Projekt.Add(tempProj);
            context.SaveChanges();
            succes = true;
            StateHasChanged();
            navigationManager.NavigateTo($"userDetails/{true}");        
        }
    }
    
    private void addMember(ChangeEventArgs e)
    {
        string tempUserName = (string)e.Value ;
        if (tempUserName is "" or "Choose member")
        {
            return;
        }
        WebUser userToAdd = context.WebUser.FirstOrDefault(u => u.Name == tempUserName);
        if (edit)
        {
            usersToAdd.Add(userToAdd);
        }
        else
        {
            _projekt.MemberUsers.Add(userToAdd);
        }
    }

    private void addSelect()
    {
        if (index + 1 < context.WebUser.Count())
        {
            if (edit)
            {
                newUsersCount++;
                
            }
            memberUsers.Add(new());
            index++;
            StateHasChanged();
        }
    }

    private void removeSelect(int i)
    {
        if (!edit)
        {
            i--;
        }
        if (newUsersCount > 0 || !edit)
        {
            memberUsers.RemoveAt(memberUsers.Count() - 1);
            newUsersCount--;
            index--;
        }
        else
        {
            existingUsersCount = 0;
        }
        
        _projekt.MemberUsers.RemoveAt(_projekt.MemberUsers.Count - 1);
    }

    
    private void addTask()
    {
        projectExists = context.Projekt.Any(p => p.ProjektId == _projekt.ProjektId);
        if (projectExists)
        {
            navigationManager.NavigateTo($"createTask/{_projekt.ProjektId}");
        }
    }

    private void editTask(MouseEventArgs e, int taskId)
    {
        navigationManager.NavigateTo($"createTask/{_projekt.ProjektId}/{taskId}");
    }
}