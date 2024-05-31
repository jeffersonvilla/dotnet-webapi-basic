using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MyVaccine.WebApi.Dtos.Vaccine;
using MyVaccine.WebApi.Dtos.VaccineCategory;
using MyVaccine.WebApi.Models;
using MyVaccine.WebApi.Repositories.Contracts;
using MyVaccine.WebApi.Repositories.Implementations;
using MyVaccine.WebApi.Services.Contracts;
using System.Reflection.PortableExecutable;

namespace MyVaccine.WebApi.Services.Implementations;

public class VaccineCategoryService : IVaccineCategoryService
{

    private readonly IVaccineCategoryRepository<VaccineCategory> _vaccineCategoryRepository;
    private readonly IVaccineRepository<Vaccine> _vaccineRepository;
    private readonly IMapper _mapper;

    public VaccineCategoryService(IVaccineCategoryRepository<VaccineCategory> vaccineCategoryRepository,
        IVaccineRepository<Vaccine> vaccineRepository, IMapper mapper)
    {
        _vaccineCategoryRepository = vaccineCategoryRepository;
        _vaccineRepository = vaccineRepository;
        _mapper = mapper;
    }

    public async Task<VaccineCategoryResponseDto> Add(VaccineCategoryRequestDto request)
    {
        var vaccineCategory = _mapper.Map<VaccineCategory>(request);
        if (request.VaccineIds != null && request.VaccineIds.Any())
        {
            vaccineCategory.Vaccines = await _vaccineRepository.GetAllByIds(request.VaccineIds);
        }
        await _vaccineCategoryRepository.Add(vaccineCategory);
        var response = _mapper.Map<VaccineCategoryResponseDto>(vaccineCategory);
        return response;
    }

    public async Task<VaccineCategoryResponseDto> Delete(int id)
    {
        var vaccineCategory = await _vaccineCategoryRepository
            .FindBy(x => x.VaccineCategoryId == id).FirstOrDefaultAsync();

        if (vaccineCategory == null)
        {
            throw new KeyNotFoundException($"Vaccine category with id {id} not found.");
        }

        await _vaccineCategoryRepository.Delete(vaccineCategory);
        var response = _mapper.Map<VaccineCategoryResponseDto>(vaccineCategory);
        return response;
    }

    public async Task<IEnumerable<VaccineCategoryResponseDto>> GetAll()
    {
        var vaccineCategories = await _vaccineCategoryRepository.GetAll(query => query.Include(v => v.Vaccines));
        var response = _mapper.Map<IEnumerable<VaccineCategoryResponseDto>>(vaccineCategories);
        return response;
    }

    public async Task<VaccineCategoryResponseDto> GetById(int id)
    {
        var vaccineCategory = await _vaccineCategoryRepository
            .GetById(id, include: query => query.Include(v => v.Vaccines));

        if (vaccineCategory == null)
        {
            throw new KeyNotFoundException($"Vaccine category with id {id} not found.");
        }

        var response = _mapper.Map<VaccineCategoryResponseDto>(vaccineCategory);
        return response;
    }

    public async Task<VaccineCategoryResponseDto> Update(VaccineCategoryRequestDto request, int id)
    {
        var vaccineCategory = await _vaccineCategoryRepository.
            GetById(id, include: query => query.Include(v => v.Vaccines));

        if (vaccineCategory == null)
        {
            throw new KeyNotFoundException($"Vaccine category with id {id} not found.");
        }

        _mapper.Map(request, vaccineCategory);

        if (request.VaccineIds != null && request.VaccineIds.Any())
        {
            vaccineCategory.Vaccines = await _vaccineRepository.GetAllByIds(request.VaccineIds);
        }

        await _vaccineCategoryRepository.Update(vaccineCategory);
        var response = _mapper.Map<VaccineCategoryResponseDto>(vaccineCategory);
        return response;
    }

    public async Task<VaccineCategoryResponseDto> SetVaccine(int categoryId, int vaccineId)
    {
        var category = await _vaccineCategoryRepository.
            GetById(categoryId, include: query => query.Include(v => v.Vaccines));

        if (category == null)
        {
            throw new KeyNotFoundException($"Vaccine category with id {categoryId} not found.");
        }

        var vaccine = await _vaccineRepository
            .FindBy(x => x.VaccineId == vaccineId).FirstOrDefaultAsync();

        if (vaccine == null)
        {
            throw new KeyNotFoundException($"Vaccine with id {vaccineId} not found.");
        }

        if (category.Vaccines == null)
        {
            category.Vaccines = new List<Vaccine>();

        }

        if (category.Vaccines.Any(c => c.VaccineId == vaccineId))
        {
            return _mapper.Map<VaccineCategoryResponseDto>(category);
        }

        category.Vaccines.Add(vaccine);

        await _vaccineCategoryRepository.Update(category);

        return _mapper.Map<VaccineCategoryResponseDto>(category);
    }

    public async Task<VaccineCategoryResponseDto> RemoveVaccine(int categoryId, int vaccineId)
    {
        var category = await _vaccineCategoryRepository
            .GetById(categoryId, include: query => query.Include(v => v.Vaccines));

        if (category == null)
        {
            throw new KeyNotFoundException($"Vaccine category with id {categoryId} not found.");
        }

        var vaccineToRemove = category.Vaccines.FirstOrDefault(c => c.VaccineId == vaccineId);

        if (vaccineToRemove == null)
        {
            throw new KeyNotFoundException($"Vaccine with id {vaccineId} not found in vaccine category.");
        }

        category.Vaccines.Remove(vaccineToRemove);
        await _vaccineCategoryRepository.Update(category);

        return _mapper.Map<VaccineCategoryResponseDto>(category);
    }

    
}
