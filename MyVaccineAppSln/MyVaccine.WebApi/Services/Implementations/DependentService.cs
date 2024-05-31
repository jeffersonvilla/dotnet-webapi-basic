using AutoMapper;
using Azure.Core;
using Microsoft.EntityFrameworkCore;
using MyVaccine.WebApi.Dtos.Dependent;
using MyVaccine.WebApi.Models;
using MyVaccine.WebApi.Repositories.Contracts;
using MyVaccine.WebApi.Repositories.Implementations;
using MyVaccine.WebApi.Services.Contracts;

namespace MyVaccine.WebApi.Services.Implementations;

public class DependentService : IDependentService
{
    private readonly IBaseRepository<Dependent> _dependentRepository;
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    public DependentService(IBaseRepository<Dependent> dependentRepository, IMapper mapper
        , IUserRepository userRepository)
    {
        _dependentRepository = dependentRepository;
        _mapper = mapper;
        _userRepository = userRepository;
    }
    public async Task<DependentResponseDto> Add(DependentRequestDto request)
    {

        var user = await _userRepository
            .FindBy(x => x.UserId == request.UserId).FirstOrDefaultAsync();

        if (user == null)
        {
            throw new KeyNotFoundException($"User with ID {request.UserId} does not exist.");
        }

        var dependents = new Dependent();
        dependents.Name = request.Name;
        dependents.DateOfBirth = request.DateOfBirth;
        dependents.UserId = request.UserId;

        await _dependentRepository.Add(dependents);
        var response = _mapper.Map<DependentResponseDto>(dependents);
        return response;
    }

    public async Task<DependentResponseDto> Delete(int id)
    {
        var dependents = await _dependentRepository.FindBy(x => x.DependentId == id).FirstOrDefaultAsync();
    
        await _dependentRepository.Delete(dependents);
        var response = _mapper.Map<DependentResponseDto>(dependents);
        return response;
    }

    public async Task<IEnumerable<DependentResponseDto>> GetAll()
    {
        var dependents = await _dependentRepository.GetAll().AsNoTracking().ToListAsync();
        var response = _mapper.Map<IEnumerable<DependentResponseDto>>(dependents);
        return response;
    }

    public async Task<DependentResponseDto> GetById(int id)
    {
        var dependents = await _dependentRepository.FindByAsNoTracking(x=>x.DependentId==id).FirstOrDefaultAsync();
        var response = _mapper.Map<DependentResponseDto>(dependents);
        return response;
    }

    public async Task<DependentResponseDto> Update(DependentRequestDto request, int id)
    {

        var user = await _userRepository
            .FindBy(x => x.UserId == request.UserId).FirstOrDefaultAsync();

        if (user == null)
        {
            throw new KeyNotFoundException($"User with ID {request.UserId} does not exist.");
        }

        var dependents = await _dependentRepository.FindBy(x => x.DependentId == id).FirstOrDefaultAsync();
        dependents.Name = request.Name;
        dependents.DateOfBirth= request.DateOfBirth;
        dependents.UserId = request.UserId;

        await _dependentRepository.Update(dependents);
        var response = _mapper.Map<DependentResponseDto>(dependents);
        return response;
    }
}
