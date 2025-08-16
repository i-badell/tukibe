using Api.Context;
using Api.Models;
using Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Api.Services;

public class UserService : IUserService
{
    private readonly ClientContext _context;

    public UserService(ClientContext context)
    {
        _context = context;
    }
    
    public async Task<User?> GetUserByAuth0Id(string? auth0Id)
    {
        return auth0Id is null ? null : await _context.Users
            .Where(u => u.Auth0Id == auth0Id)            
            .FirstOrDefaultAsync();          
    }
}
