using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MyUser.WebApi.Services.Contracts;
using MyVaccine.WebApi.Dtos.Allergy;
using MyVaccine.WebApi.Models;
using MyVaccine.WebApi.Repositories.Contracts;
using MyVaccine.WebApi.Services.Contracts;

namespace MyVaccine.WebApi.Services.Implementations;

public class AllergyService : IAllergyService
{

    private readonly IAllergyRepository<Allergy> _allergyRepository;
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public AllergyService(IAllergyRepository<Allergy> allergyRepository
        , IMapper mapper, IUserRepository userRepository)
    {
        _allergyRepository = allergyRepository;
        _mapper = mapper;
        _userRepository = userRepository;
    }

    public async Task<AllergyResponseDto> Add(AllergyRequestDto request)
    {

        var user = await _userRepository
            .FindBy(x => x.UserId == request.UserId).FirstOrDefaultAsync();

        if (user == null)
        {
            throw new KeyNotFoundException($"User with ID {request.UserId} does not exist.");
        }

        var allergy = _mapper.Map<Allergy>(request);
        await _allergyRepository.Add(allergy);
        var response = _mapper.Map<AllergyResponseDto>(allergy);
        return response;
    }

    public async Task<AllergyResponseDto> Delete(int id)
    {
        var allergy = await _allergyRepository
            .FindBy(x => x.AllergyId == id).FirstOrDefaultAsync();

        if (allergy == null)
        {
            throw new KeyNotFoundException($"Allergy with id {id} not found.");
        }

        await _allergyRepository.Delete(allergy);
        var response = _mapper.Map<AllergyResponseDto>(allergy);
        return response;
    }

    public async Task<IEnumerable<AllergyResponseDto>> GetAll()
    {
        var allergyCategories = await _allergyRepository.GetAll();
        var response = _mapper.Map<IEnumerable<AllergyResponseDto>>(allergyCategories);
        return response;
    }

    public async Task<AllergyResponseDto> GetById(int id)
    {
        var allergy = await _allergyRepository.FindBy(x => x.AllergyId == id).FirstOrDefaultAsync();

        if (allergy == null)
        {
            throw new KeyNotFoundException($"Allergy with id {id} not found.");
        }

        var response = _mapper.Map<AllergyResponseDto>(allergy);
        return response;
    }

    public async Task<AllergyResponseDto> Update(AllergyRequestDto request, int id)
    {
        var allergy = await _allergyRepository.FindBy(x => x.AllergyId == id).FirstOrDefaultAsync();

        if (allergy == null)
        {
            throw new KeyNotFoundException($"Allergy with id {id} not found.");
        }

        var user = await _userRepository
            .FindBy(x => x.UserId == request.UserId).FirstOrDefaultAsync();

        if (user == null)
        {
            throw new KeyNotFoundException($"User with ID {request.UserId} does not exist.");
        }

        _mapper.Map(request, allergy);

        await _allergyRepository.Update(allergy);
        var response = _mapper.Map<AllergyResponseDto>(allergy);
        return response;
    }

}