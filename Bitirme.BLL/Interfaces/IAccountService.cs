using Bitirme.BLL.Models;
using Bitirme.BLL.Services;

namespace Bitirme.BLL.Interfaces
{
    public interface IAccountService
    {
        AccountViewModel Login(string email, string password);
        bool SignUp(string password, string email,string name,UserType userType);
        bool VerifyEmail(string userId,string code);
        IEnumerable<ClassViewModel> GetStudentInfo(string studentId);

        bool ResetPassword(string studentId, string oldPassword, string newPassword);
        string ForgotPasswordMail(string mail);
        bool ForgotPasswordCodeControl(string userId, string code);
        bool ForgotPasswordChange(string userId, string newPassword);



    }
}