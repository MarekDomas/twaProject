using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using twaProject.Classes;
using twaProject.Components.Templates;
using Task = System.Threading.Tasks.Task;

namespace twaProject.Components.Pages;

public partial class CreateTask : ComponentBase
{
    [Parameter] 
    public int projektId { get; set; } = 0;
    [Parameter] 
    public int? taskId { get; set; }
    
    private Classes.Task _task = new();
    private string trashImage = "Images/trash.png";
    private bool edit = false;
    private int newUsersCount = 0;
    private List<WebUser> memberUsers = new();
    private int index = 0;
    private int existingUsersCount = 0;
    private bool isEdit = false;
    private List<WebUser> usersInProjekt = new();

    protected override void OnInitialized()
    {
        usersInProjekt = context.WebUser
            .Where(user => user.Projekts.Any(p => p.ProjektId == projektId))
            .ToList();
      
        if (taskId is not null)
        {
            _task = context.Task.FirstOrDefault(t => t.TaskId == taskId);
            isEdit = true;
        }
        else
        {
            _task.ProjektId = projektId;
            _task.Projekt = context.Projekt.FirstOrDefault(p => p.ProjektId == projektId);
            _task.StartDate = DateOnly.FromDateTime(DateTime.Now.Date);
            _task.EndDate = DateOnly.FromDateTime(DateTime.Now.Date);
        }
        if (usersInProjekt.Count == 1)
        {
            _task.WebUser = usersInProjekt[0];
        }
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
        Classes.Task tempTask = (Classes.Task)editContext.Model;

        if (tempTask.StartDate > tempTask.EndDate)
        {
            JsRuntime.InvokeVoidAsync("alert","The end date cannot be less than start date!");
            return;
        }
        
        if (_task.WebUser is null)
        {
            _task.WebUser = context.WebUser.FirstOrDefault(u => u.WebUserId == stateManager.CurrentUser.WebUserId);
        }

        if (!isEdit)
        {
            context.Task.Add(_task);
        }
        
        context.SaveChanges();
        navigationManager.NavigateTo($"createProjPage/{_task.ProjektId}");
    }
    
    private void addMember(ChangeEventArgs e)
    {
        
        WebUser userToAdd = context.WebUser.FirstOrDefault(u => u.Name == e.Value);
        _task.WebUser = userToAdd;
    }
}