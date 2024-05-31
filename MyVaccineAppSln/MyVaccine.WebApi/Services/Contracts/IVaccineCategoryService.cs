using MyVaccine.WebApi.Dtos.Vaccine;
using MyVaccine.WebApi.Dtos.VaccineCategory;

namespace MyVaccine.WebApi.Services.Contracts;

public interface IVaccineCategoryService
{
    Task<IEnumerable<VaccineCategoryResponseDto>> GetAll();
    Task<VaccineCategoryResponseDto> GetById(int id);
    Task<VaccineCategoryResponseDto> Add(VaccineCategoryRequestDto request);
    Task<VaccineCategoryResponseDto> Update(VaccineCategoryRequestDto request, int id);
    Task<VaccineCategoryResponseDto> Delete(int id);
    Task<VaccineCategoryResponseDto> SetVaccine(int categoryId, int vaccineId);
    Task<VaccineCategoryResponseDto> RemoveVaccine(int categoryId, int vaccineId);
}
