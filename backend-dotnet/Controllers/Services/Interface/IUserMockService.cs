using REST_project.Controllers.Models;

namespace REST_project.Controllers.Services.Interface
{
    public interface IUserMockService
    {
        string? Authinticate(UserDTO user);
        UserDTO? getCurrentUser();
        bool Register(UserDTO user);
        bool PutUser(UserDTO updatedUser);
        bool DeleteUser(string username);
    }
}