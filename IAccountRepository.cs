using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SchoolManagement.AdminProfile;

namespace SchoolManagement
{
    public interface IAccountRepository
    {
        Task<Account> GetAccountByIdAsync(string id);
        Task<bool> UpdatePasswordAsync(string id, string hashedPassword);
    }
}
