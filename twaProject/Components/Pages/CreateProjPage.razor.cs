using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop;
using twaProject.Classes;
using twaProject.Components.Templates;
using Task = System.Threading.Tasks.Task;

namespace twaProject.Components.Pages;

public partial class CreateProjPage : ComponentBase
{
    [Parameter]
    public int? id { get; set; } = null;//id used for routing purposes
    
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
    private bool projectExists;
    List<Classes.Task> tasksInProjekt ;

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
        tasksInProjekt = context.Task
            .Where(task => task.ProjektId == _projekt.ProjektId)
            .Include(task => task.WebUser) // Include related WebUser
            .ToList();
        base.OnInitialized();
    }

    protected override async Task OnInitializedAsync()
    {
        var result =  await localStorage.GetAsync<WebUser>("currentUser");
        var userLogged= await localStorage.GetAsync<bool>("isUserLogged");
        stateManager.CurrentUser = result.Value;
        stateManager.isUserLogged = userLogged.Value;
    }

    private void Submit(EditContext editContext)
    {
        Projekt tempProj = (Projekt)editContext.Model;
        if (tempProj.StartDate > tempProj.EndDate)
        {
            JsRuntime.InvokeVoidAsync("alert","The end date cannot be less than start date!");
            return;
        }
        
        if (edit)
        {
            _projekt.MemberUsers = _projekt.MemberUsers.Distinct().ToList();
            _projekt.MemberUsers.AddRange(usersToAdd);
            context.SaveChanges();
            navigationManager.NavigateTo("userDetails");
        }
        else
        {

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

        if (edit)
        {
            WebUser userToRemove = context.WebUser.FirstOrDefault(u => u.WebUserId == i);
            _projekt.MemberUsers.Remove(userToRemove);    
        }
        else
        {
            _projekt.MemberUsers.RemoveAt(_projekt.MemberUsers.Count - 1);
        }
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