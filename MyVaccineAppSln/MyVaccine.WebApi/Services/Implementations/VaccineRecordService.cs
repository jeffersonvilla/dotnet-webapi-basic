using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MyVaccine.WebApi.Dtos.VaccineRecord;
using MyVaccine.WebApi.Models;
using MyVaccine.WebApi.Repositories.Contracts;
using MyVaccine.WebApi.Services.Contracts;

namespace MyVaccine.WebApi.Services.Implementations;

public class VaccineRecordService : IVaccineRecordService
{

    private readonly IVaccineRecordRepository<VaccineRecord> _vaccineRecordRepository;
    private readonly IBaseRepository<Dependent> _dependentRepository;
    private readonly IUserRepository _userRepository;
    private readonly IVaccineRepository<Vaccine> _vaccineRepository;
    private readonly IMapper _mapper;

    public VaccineRecordService(IVaccineRecordRepository<VaccineRecord> vaccineRecordRepository
        , IBaseRepository<Dependent> dependentRepository, IMapper mapper, IUserRepository userRepository
        , IVaccineRepository<Vaccine> vaccineRepository)
    {
        _vaccineRecordRepository = vaccineRecordRepository;
        _dependentRepository = dependentRepository;
        _userRepository = userRepository;
        _mapper = mapper;
        _userRepository = userRepository;
        _vaccineRepository = vaccineRepository;
    }

    public async Task<VaccineRecordResponseDto> Add(VaccineRecordRequestDto request)
    {

        var user = await _userRepository
            .FindBy(x => x.UserId == request.UserId).FirstOrDefaultAsync();

        if (user == null)
        {
            throw new KeyNotFoundException($"User with ID {request.UserId} does not exist.");
        }

        var dependent = await _dependentRepository
            .FindBy(x => x.DependentId == request.DependentId).FirstOrDefaultAsync();

        if (dependent == null)
        {
            throw new KeyNotFoundException($"Dependent with ID {request.DependentId} does not exist.");
        }

        var vaccine = await _vaccineRepository
            .FindBy(x => x.VaccineId == request.VaccineId).FirstOrDefaultAsync();

        if (vaccine == null)
        {
            throw new KeyNotFoundException($"Vaccine with ID {request.VaccineId} does not exist.");
        }


        var vaccineRecord = _mapper.Map<VaccineRecord>(request);
        await _vaccineRecordRepository.Add(vaccineRecord);
        var response = _mapper.Map<VaccineRecordResponseDto>(vaccineRecord);


        return response;
    }

    public async Task<VaccineRecordResponseDto> Delete(int id)
    {
        var vaccineRecord = await _vaccineRecordRepository
            .FindBy(x => x.VaccineRecordId == id).FirstOrDefaultAsync();

        if (vaccineRecord == null)
        {
            throw new KeyNotFoundException($"VaccineRecord with id {id} not found.");
        }

        await _vaccineRecordRepository.Delete(vaccineRecord);
        var response = _mapper.Map<VaccineRecordResponseDto>(vaccineRecord);
        return response;
    }

    public async Task<IEnumerable<VaccineRecordResponseDto>> GetAll()
    {
        var vaccineRecordCategories = await _vaccineRecordRepository.GetAll();
        var response = _mapper.Map<IEnumerable<VaccineRecordResponseDto>>(vaccineRecordCategories);
        return response;
    }

    public async Task<VaccineRecordResponseDto> GetById(int id)
    {
        var vaccineRecord = await _vaccineRecordRepository.FindBy(x => x.VaccineRecordId == id).FirstOrDefaultAsync();

        if (vaccineRecord == null)
        {
            throw new KeyNotFoundException($"VaccineRecord with id {id} not found.");
        }

        var response = _mapper.Map<VaccineRecordResponseDto>(vaccineRecord);
        return response;
    }

    public async Task<VaccineRecordResponseDto> Update(VaccineRecordRequestDto request, int id)
    {
        var vaccineRecord = await _vaccineRecordRepository.FindBy(x => x.VaccineRecordId == id).FirstOrDefaultAsync();

        if (vaccineRecord == null)
        {
            throw new KeyNotFoundException($"VaccineRecord with id {id} not found.");
        }

        var user = await _userRepository
            .FindBy(x => x.UserId == request.UserId).FirstOrDefaultAsync();

        if (user == null)
        {
            throw new KeyNotFoundException($"User with ID {request.UserId} does not exist.");
        }

        var dependent = await _dependentRepository
            .FindBy(x => x.DependentId == request.DependentId).FirstOrDefaultAsync();

        if (dependent == null)
        {
            throw new KeyNotFoundException($"Dependent with ID {request.DependentId} does not exist.");
        }

        var vaccine = await _vaccineRepository
            .FindBy(x => x.VaccineId == request.VaccineId).FirstOrDefaultAsync();

        if (vaccine == null)
        {
            throw new KeyNotFoundException($"Vaccine with ID {request.VaccineId} does not exist.");
        }

        _mapper.Map(request, vaccineRecord);

        await _vaccineRecordRepository.Update(vaccineRecord);
        var response = _mapper.Map<VaccineRecordResponseDto>(vaccineRecord);
        return response;
    }

}