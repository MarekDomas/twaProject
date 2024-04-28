using Microsoft.AspNetCore.Components;

namespace twaProject.Components.Templates;

public partial class Alert : ComponentBase
{
    [Parameter]
    public string Title { get; set; }
    [Parameter]
    public string Message { get; set; }

    private Alert _alert = new()
    {
        Title = "test",
        Message = "Message"
    };
}