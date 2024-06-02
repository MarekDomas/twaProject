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
    public int? Id { get; set; } //id used for routing purposes
    
    private Projekt _projekt = new();
    private List<WebUser> memberUsers { get; set; }= [];
    private List<Classes.Task> _tasks { get; set; } = [];
    private string trashImage = "Images/trash.png";
    private bool succes = false;
    private int index;
    private bool edit = false;
    private int newUsersCount;
    private int existingUsersCount;
    private List<WebUser> usersToAdd = [];
    private bool projectExists;
    private List<Classes.Task> tasksInProjekt ;
    private int selectsCount;

    protected override void OnInitialized()
    {
        if (Id is not null)
        {
            _projekt = context.Projekt.FirstOrDefault(p => p.ProjektId == Id);
            edit = true;
        }
        else
        {
            _projekt = new();
            _projekt.MemberUsers = [];
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
        var result = await localStorage.GetAsync<WebUser>("currentUser");
        var userLogged= await localStorage.GetAsync<bool>("isUserLogged");
        stateManager.CurrentUser = result.Value;
        stateManager.isUserLogged = userLogged.Value;

        if (edit)
        {
            var user = context.WebUser.Include(webUser => webUser.Projekts).ToList().Find(u => u.WebUserId == stateManager.CurrentUser.WebUserId);
            
            if (user.Projekts.ToList().TrueForAll(p => p.ProjektId != Id))
            {
                navigationManager.NavigateTo("/Unauthorized");
            }
            
        }
    }

    private void Submit(EditContext editContext)
    {
        var tempProj = (Projekt)editContext.Model;
        if (tempProj.StartDate > tempProj.EndDate)
        {
            JsRuntime.InvokeVoidAsync("alert","The end date cannot be less than start date!");
            return;
        }
        
        if (edit)
        {
            _projekt.MemberUsers.AddRange(usersToAdd);
            _projekt.MemberUsers = _projekt.MemberUsers.DistinctBy(u => u.WebUserId).ToList();

            if (_projekt.MemberUsers.Count < 1)
            {
                _projekt.MemberUsers.Add( context.WebUser.FirstOrDefault(u=> u.WebUserId == stateManager.CurrentUser.WebUserId) );
            }
            
            context.SaveChanges();
            navigationManager.NavigateTo("userDetails");
        }
        else
        {
            context.Database.EnsureCreated();
            
            tempProj.MemberUsers = _projekt.MemberUsers;
            
            tempProj.MemberUsers.Add(context.WebUser.FirstOrDefault(u => u.WebUserId == stateManager.CurrentUser.WebUserId));
            tempProj.MemberUsers = tempProj.MemberUsers.Distinct().ToList();
            if (tempProj.MemberUsers.Count < 1)
            {
                tempProj.MemberUsers.Add( context.WebUser.FirstOrDefault(u=> u.WebUserId == stateManager.CurrentUser.WebUserId) );
            }
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
        var userToAdd = context.WebUser.FirstOrDefault(u => u.Name == tempUserName);
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
        if (index + 1 >= context.WebUser.Count() || _projekt.MemberUsers.Count == context.WebUser.Count() || selectsCount >= context.WebUser.Count())
            return;
        
        if (edit)
        {
            newUsersCount++;
        }
        
        memberUsers.Add(new());
        index++;
        selectsCount = 0;
    }

    private void removeSelect(int i)
    {
        selectsCount = 0;
        if (!edit)
        {
            i--;
        }
        if (newUsersCount > 0 || !edit)
        {
            memberUsers.RemoveAt(memberUsers.Count - 1);
            newUsersCount--;
            index--;
        }
        else
        {
            existingUsersCount = 0;
        }

        if (edit)
        {
            var userToRemove = context.WebUser.FirstOrDefault(u => u.WebUserId == i);
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