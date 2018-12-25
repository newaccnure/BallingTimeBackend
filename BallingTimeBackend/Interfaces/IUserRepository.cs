using System.Collections.Generic;
using BallingTimeBackend.Models;
using BallingTimeBackend.Data_for_frontend;

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
        User_shortened_model GetUserById(int userId);
        int GetUserIdByEmail(string email);
        User_shortened_model GetUserByEmail(string email);
        List<User> GetAllUsers();
    }
}
