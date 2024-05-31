using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MyVaccine.WebApi.Dtos;
using MyVaccine.WebApi.Dtos.User;
using MyVaccine.WebApi.Dtos.Vaccine;
using MyVaccine.WebApi.Literals;
using MyVaccine.WebApi.Models;
using MyVaccine.WebApi.Repositories.Contracts;
using MyVaccine.WebApi.Services.Contracts;

namespace MyVaccine.WebApi.Services.Implementations;

public class UserService : IUserService
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IUserRepository _userRepository;
    private readonly IFamilyGroupRepository<FamilyGroup> _familyGroupRepository;
    private readonly IMapper _mapper;
    public UserService(UserManager<IdentityUser> userManager, IUserRepository userRepository,
        IMapper mapper, IFamilyGroupRepository<FamilyGroup> familyGroupRepository)
    {
        _userManager = userManager;
        _userRepository = userRepository;
        _familyGroupRepository = familyGroupRepository;
        _mapper = mapper;
    }
    public async Task<AuthResponseDto> AddUserAsync(RegisterRequetDto request)
    {
        var response = new AuthResponseDto();
        try
        {
            var result = await _userRepository.AddUser(request);

            if (result != null)
            {
                response.IsSuccess = result.Succeeded;
                response.Errors = result?.Errors?.Select(x => x.Description).ToArray() ?? new string[] { };
            }

        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.Errors = new string[] { ex.Message };
        }

        return response;
    }

    public async Task<AuthResponseDto> Login(LoginRequestDto request)
    {
        var response = new AuthResponseDto();
        try
        {
            var user = await _userManager.FindByNameAsync(request.Username);

            if (user != null && await _userManager.CheckPasswordAsync(user, request.Password))
            {
                var claims = new[]
                {
                    new Claim(ClaimTypes.Name, user.UserName)
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable(MyVaccineLiterals.JWT_KEY)));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    //issuer: _configuration["JwtIssuer"],
                    //audience: _configuration["JwtAudience"],
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(15),
                    signingCredentials: creds
                );

                var tokenresult = new JwtSecurityTokenHandler().WriteToken(token);
                response.Token = tokenresult;
                response.Expiration = token.ValidTo;
                response.IsSuccess = true;
            }
            else
            {
                response.IsSuccess = false;
            }
        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.Errors = new string[] { ex.Message };
        }

        return response;

    }
    public async Task<AuthResponseDto> RefreshToken(string email)
    {
        var response = new AuthResponseDto();
        try
        {
            var user = await _userManager.FindByNameAsync(email);

            if (user != null)
            {
                var claims = new[]
                {
                    new Claim(ClaimTypes.Name, user.UserName)
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable(MyVaccineLiterals.JWT_KEY)));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    //issuer: _configuration["JwtIssuer"],
                    //audience: _configuration["JwtAudience"],
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(15),
                    signingCredentials: creds
                );

                var tokenresult = new JwtSecurityTokenHandler().WriteToken(token);
                response.Token = tokenresult;
                response.Expiration = token.ValidTo;
                response.IsSuccess = true;
            }
            else
            {
                response.IsSuccess = false;
            }
        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.Errors = new string[] { ex.Message };
        }

        return response;

    }

    public async Task<UserResponseDto> GetUserInfo(string email)
    {
        var identityUser = await _userManager.FindByNameAsync(email);

        var user = await _userRepository
            .FindBy(x => x.AspNetUserId == identityUser.Id)
            .Include(u => u.FamilyGroups)
            .Include(u => u.Allergies)
            .Include(u => u.VaccineRecords)
            .Include(u => u.Dependents)
            .FirstOrDefaultAsync();


        return _mapper.Map<UserResponseDto>(user);
    }

    public async Task<UserResponseDto> SetFamilyGroup(int userId, int familyGroupId)
    {
        var user = await _userRepository.
            GetById(userId, include: query => query.Include(v => v.FamilyGroups));

        if (user == null)
        {
            throw new KeyNotFoundException($"User with id {userId} not found.");
        }

        var category = await _familyGroupRepository
            .FindBy(x => x.FamilyGroupId == familyGroupId).FirstOrDefaultAsync();

        if (category == null)
        {
            throw new KeyNotFoundException($"Family group with id {familyGroupId} not found.");
        }

        if (user.FamilyGroups == null)
        {
            user.FamilyGroups= new List<FamilyGroup>();

        }

        if (user.FamilyGroups.Any(c => c.FamilyGroupId == familyGroupId))
        {
            return _mapper.Map<UserResponseDto>(user);
        }

        user.FamilyGroups.Add(category);

        await _userRepository.Update(user);

        return _mapper.Map<UserResponseDto>(user);
    }

    public async Task<UserResponseDto> RemoveFamilyGroup(int userId, int familyGroupId)
    {
        var user = await _userRepository
            .GetById(userId, include: query => query.Include(v => v.FamilyGroups));

        if (user == null)
        {
            throw new KeyNotFoundException($"User with id {userId} not found.");
        }

        var familyGroupToRemove = user.FamilyGroups.FirstOrDefault(c => c.FamilyGroupId == familyGroupId);

        if (familyGroupToRemove == null)
        {
            throw new KeyNotFoundException($"Family group with id {familyGroupId} not found in user.");
        }

        user.FamilyGroups.Remove(familyGroupToRemove);
        await _userRepository.Update(user);

        return _mapper.Map<UserResponseDto>(user);
    }
}
