using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using twaProject.Classes;
using twaProject.Components.Templates;

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

    protected override void OnInitialized()
    {
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
        base.OnInitialized();
    }

    private void Submit(EditContext editContext)
    {
        if (_task.WebUser is null)
        {
            _task.WebUser = stateManager.CurrentUser;
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