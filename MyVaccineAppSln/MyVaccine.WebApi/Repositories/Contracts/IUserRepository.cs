using Microsoft.AspNetCore.Identity;
using MyVaccine.WebApi.Dtos;

namespace MyVaccine.WebApi.Repositories.Contracts;

public interface IUserRepository
{
    Task<IdentityResult> AddUser(RegisterRequetDto request);
}
