using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MyVaccine.WebApi.Dtos.Vaccine;
using MyVaccine.WebApi.Models;
using MyVaccine.WebApi.Repositories.Contracts;
using MyVaccine.WebApi.Services.Contracts;

namespace MyVaccine.WebApi.Services.Implementations;

public class VaccineService : IVaccineService
{

    private readonly IVaccineRepository<Vaccine> _vaccineRepository;
    private readonly IVaccineCategoryRepository<VaccineCategory> _vaccineCategoryRepository;
    private readonly IMapper _mapper;

    public VaccineService(IVaccineRepository<Vaccine> vaccineRepository
        , IVaccineCategoryRepository<VaccineCategory> vaccineCategoryRepository, IMapper mapper)
    {
        _vaccineRepository = vaccineRepository;
        _vaccineCategoryRepository = vaccineCategoryRepository;
        _mapper = mapper;
    }

    public async Task<VaccineResponseDto> Add(VaccineRequestDto request)
    {
        var vaccine = _mapper.Map<Vaccine>(request);
        if (request.CategoryIds != null && request.CategoryIds.Any())
        {
            vaccine.Categories = await _vaccineCategoryRepository.GetAllByIds(request.CategoryIds);
        }
        await _vaccineRepository.Add(vaccine);
        var response = _mapper.Map<VaccineResponseDto>(vaccine);
        return response;
    }

    public async Task<VaccineResponseDto> Delete(int id)
    {
        var vaccine = await _vaccineRepository
            .FindBy(x => x.VaccineId == id).FirstOrDefaultAsync();

        if (vaccine == null)
        {
            throw new KeyNotFoundException($"Vaccine with id {id} not found.");
        }

        await _vaccineRepository.Delete(vaccine);
        var response = _mapper.Map<VaccineResponseDto>(vaccine);
        return response;
    }

    public async Task<IEnumerable<VaccineResponseDto>> GetAll()
    {
        var vaccineCategories = await _vaccineRepository.GetAll(query => query.Include(v => v.Categories));
        var response = _mapper.Map<IEnumerable<VaccineResponseDto>>(vaccineCategories);
        return response;
    }

    public async Task<VaccineResponseDto> GetById(int id)
    {
        var vaccine = await _vaccineRepository
            .GetById(id, include: query => query.Include(v => v.Categories));

        if (vaccine == null)
        {
            throw new KeyNotFoundException($"Vaccine with id {id} not found.");
        }

        var response = _mapper.Map<VaccineResponseDto>(vaccine);
        return response;
    }

    public async Task<VaccineResponseDto> Update(VaccineRequestDto request, int id)
    {
        var vaccine = await _vaccineRepository.
            GetById(id, include: query => query.Include(v => v.Categories));

        if (vaccine == null)
        {
            throw new KeyNotFoundException($"Vaccine with id {id} not found.");
        }

        _mapper.Map(request, vaccine);

        if (request.CategoryIds != null && request.CategoryIds.Any())
        {
            vaccine.Categories = await _vaccineCategoryRepository.GetAllByIds(request.CategoryIds);
        }

        await _vaccineRepository.Update(vaccine);
        var response = _mapper.Map<VaccineResponseDto>(vaccine);
        return response;
    }

    public async Task<VaccineResponseDto> SetVaccineCategory(int vaccineId, int categoryId)
    {
        var vaccine = await _vaccineRepository.
            GetById(vaccineId, include: query => query.Include(v => v.Categories));

        if (vaccine == null)
        {
            throw new KeyNotFoundException($"Vaccine with id {vaccineId} not found.");
        }

        var category = await _vaccineCategoryRepository
            .FindBy(x => x.VaccineCategoryId == categoryId).FirstOrDefaultAsync();

        if (category == null)
        {
            throw new KeyNotFoundException($"Vaccine category with id {categoryId} not found.");
        }

        if (vaccine.Categories == null)
        {
            vaccine.Categories = new List<VaccineCategory>();

        }

        if (vaccine.Categories.Any(c => c.VaccineCategoryId == categoryId))
        {
            return _mapper.Map<VaccineResponseDto>(vaccine);
        }

        vaccine.Categories.Add(category);

        await _vaccineRepository.Update(vaccine);

        return _mapper.Map<VaccineResponseDto>(vaccine);
    }

    public async Task<VaccineResponseDto> RemoveVaccineCategory(int vaccineId, int categoryId)
    {
        var vaccine = await _vaccineRepository
            .GetById(vaccineId, include: query => query.Include(v => v.Categories));

        if (vaccine == null)
        {
            throw new KeyNotFoundException($"Vaccine with id {vaccineId} not found.");
        }

        var categoryToRemove = vaccine.Categories.FirstOrDefault(c => c.VaccineCategoryId == categoryId);

        if (categoryToRemove == null)
        {
            throw new KeyNotFoundException($"Vaccine category with id {categoryId} not found in vaccine.");
        }

        vaccine.Categories.Remove(categoryToRemove);
        await _vaccineRepository.Update(vaccine);

        return _mapper.Map<VaccineResponseDto>(vaccine);
    }
}
