using Bitirme.BLL.Services;

namespace Bitirme.BLL.Interfaces
{
    public interface IAccountService
    {
        AccountViewModel Login(string username, string password);
        bool SignUp(string username, string password, string email,UserType userType);
        bool VerifyEmail(string userId);
    }
}