
namespace twaProject.Classes;

public class StateManager
{
    public bool isUserLogged = false;

    public WebUser CurrentUser = new()
    {
        WebUserId = -50
    };
}