using Api.Dto;
using Api.Models;

namespace Api.Services.Interfaces;

public interface IUserService
{
    public Task<User?> GetUserByAuth0Id(string auth0Id);
}
