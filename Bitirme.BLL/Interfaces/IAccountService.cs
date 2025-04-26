using Bitirme.BLL.Services;

namespace Bitirme.BLL.Interfaces
{
    public interface IAccountService
    {
        AccountViewModel Login(string email, string password);
        bool SignUp(string password, string email,string name,UserType userType);
        bool VerifyEmail(string userId);
    }
}