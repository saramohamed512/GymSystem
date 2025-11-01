using GymSystemBLL.ViewModels.AccountViewModel;
using GymSystemDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystemBLL.Services
{
    public interface IAccountService
    {
        ApplicationUser? ValidateUser(LoginViewModel loginVM);

    }
}
