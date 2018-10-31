using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BallingTimeBackend.Models;

namespace BallingTimeBackend.Interfaces
{
    public interface IUserRepository
    {
        bool AddUser(string name, string email, string password, 
            string checkPassword, List<int> practiceDays);
        bool CheckUser(string email, string password);
        bool ChangeName(string email, string newName);
        bool ChangePassword(string email, string oldPassword, string newPassword, string checkPassword);
        bool DeleteAccount(string email);
        User GetUserById(int userId);
    }
}
