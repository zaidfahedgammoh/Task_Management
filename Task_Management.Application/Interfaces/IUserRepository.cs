
using Task_Management.Domain;

namespace Task_Management.Application.Interfaces
{
    public interface IUserRepository
    {
        bool EmailExists(string email);
        User? GetByEmail(string email);
    }
}
