using System;
using System.Collections.Generic;
using System.Text;

namespace Task_Management.Application.Interfaces
{
    public interface IUserRepository
    {
        bool EmailExists(string email);
    }
}
