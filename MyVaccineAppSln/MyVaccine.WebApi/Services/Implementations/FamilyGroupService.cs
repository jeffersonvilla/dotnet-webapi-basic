using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MyUser.WebApi.Services.Contracts;
using MyVaccine.WebApi.Dtos.FamilyGroup;
using MyVaccine.WebApi.Models;
using MyVaccine.WebApi.Repositories.Contracts;


namespace MyFamilyGroup.WebApi.Services.Implementations;

public class FamilyGroupService : IFamilyGroupService
{

    private readonly IFamilyGroupRepository<FamilyGroup> _familyGroupRepository;
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public FamilyGroupService(IFamilyGroupRepository<FamilyGroup> familyGroupRepository
        , IUserRepository userRepository, IMapper mapper)
    {
        _familyGroupRepository = familyGroupRepository;
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<FamilyGroupResponseDto> Add(FamilyGroupRequestDto request)
    {
        var familyGroup = _mapper.Map<FamilyGroup>(request);
        if (request.UserIds != null && request.UserIds.Any())
        {
            familyGroup.Users = await _userRepository.GetAllByIds(request.UserIds);
        }
        await _familyGroupRepository.Add(familyGroup);
        var response = _mapper.Map<FamilyGroupResponseDto>(familyGroup);
        return response;
    }

    public async Task<FamilyGroupResponseDto> Delete(int id)
    {
        var familyGroup = await _familyGroupRepository
            .FindBy(x => x.FamilyGroupId == id).FirstOrDefaultAsync();

        if (familyGroup == null)
        {
            throw new KeyNotFoundException($"FamilyGroup with id {id} not found.");
        }

        await _familyGroupRepository.Delete(familyGroup);
        var response = _mapper.Map<FamilyGroupResponseDto>(familyGroup);
        return response;
    }

    public async Task<IEnumerable<FamilyGroupResponseDto>> GetAll()
    {
        var familyGroupCategories = 
            await _familyGroupRepository.GetAll(query => query.Include(v => v.Users));
        var response = _mapper.Map<IEnumerable<FamilyGroupResponseDto>>(familyGroupCategories);
        return response;
    }

    public async Task<FamilyGroupResponseDto> GetById(int id)
    {
        var familyGroup = await _familyGroupRepository
            .GetById(id, include: query => query.Include(v => v.Users));

        if (familyGroup == null)
        {
            throw new KeyNotFoundException($"FamilyGroup with id {id} not found.");
        }

        var response = _mapper.Map<FamilyGroupResponseDto>(familyGroup);
        return response;
    }

    public async Task<FamilyGroupResponseDto> Update(FamilyGroupRequestDto request, int id)
    {
        var familyGroup = await _familyGroupRepository.
            GetById(id, include: query => query.Include(v => v.Users));

        if (familyGroup == null)
        {
            throw new KeyNotFoundException($"FamilyGroup with id {id} not found.");
        }

        _mapper.Map(request, familyGroup);

        if (request.UserIds != null && request.UserIds.Any())
        {
            familyGroup.Users = await _userRepository.GetAllByIds(request.UserIds);
        }

        await _familyGroupRepository.Update(familyGroup);
        var response = _mapper.Map<FamilyGroupResponseDto>(familyGroup);
        return response;
    }

    public async Task<FamilyGroupResponseDto> SetUser(int familyGroupId, int userId)
    {
        var familyGroup = await _familyGroupRepository.
            GetById(familyGroupId, include: query => query.Include(v => v.Users));

        if (familyGroup == null)
        {
            throw new KeyNotFoundException($"FamilyGroup with id {familyGroupId} not found.");
        }

        var user = await _userRepository
            .FindBy(x => x.UserId == userId).FirstOrDefaultAsync();

        if (user == null)
        {
            throw new KeyNotFoundException($"User with id {userId} not found.");
        }

        if (familyGroup.Users == null)
        {
            familyGroup.Users = new List<User>();

        }

        if (familyGroup.Users.Any(c => c.UserId == userId))
        {
            return _mapper.Map<FamilyGroupResponseDto>(familyGroup);
        }

        familyGroup.Users.Add(user);

        await _familyGroupRepository.Update(familyGroup);

        return _mapper.Map<FamilyGroupResponseDto>(familyGroup);
    }

    public async Task<FamilyGroupResponseDto> RemoveUser(int familyGroupId, int userId)
    {
        var familyGroup = await _familyGroupRepository
            .GetById(familyGroupId, include: query => query.Include(v => v.Users));

        if (familyGroup == null)
        {
            throw new KeyNotFoundException($"FamilyGroup with id {familyGroupId} not found.");
        }

        var userToRemove = familyGroup.Users.FirstOrDefault(c => c.UserId == userId);

        if (userToRemove == null)
        {
            throw new KeyNotFoundException($"User with id {userId} not found in familyGroup.");
        }

        familyGroup.Users.Remove(userToRemove);
        await _familyGroupRepository.Update(familyGroup);

        return _mapper.Map<FamilyGroupResponseDto>(familyGroup);
    }
}
