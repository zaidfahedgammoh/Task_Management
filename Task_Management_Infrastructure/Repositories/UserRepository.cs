using System;
using System.Collections.Generic;
using System.Text;
using Task_Management.Application.Interfaces;

namespace Task_Management_Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly TaskDbContext _context;

    public UserRepository(TaskDbContext context)
    {
        _context = context;
    }
    public bool EmailExists(string email)
    {
        return _context.Users.Any(u => u.email == email); 
    }
}