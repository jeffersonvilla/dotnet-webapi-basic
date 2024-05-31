using MyVaccine.WebApi.Dtos;
using MyVaccine.WebApi.Dtos.User;
using MyVaccine.WebApi.Dtos.Vaccine;
using MyVaccine.WebApi.Models;

namespace MyVaccine.WebApi.Services.Contracts;

public interface IUserService
{
    Task<AuthResponseDto> AddUserAsync(RegisterRequetDto request);
    Task<AuthResponseDto> Login(LoginRequestDto request);
    Task<AuthResponseDto> RefreshToken(string email);
    Task<UserResponseDto> GetUserInfo(string email);
    Task<UserResponseDto> SetFamilyGroup(int userId, int familyGroupId);
    Task<UserResponseDto> RemoveFamilyGroup(int userId, int familyGroupId);
}
