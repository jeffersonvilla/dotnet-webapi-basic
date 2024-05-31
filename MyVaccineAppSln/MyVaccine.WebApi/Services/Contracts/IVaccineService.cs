using MyVaccine.WebApi.Dtos.Vaccine;

namespace MyVaccine.WebApi.Services.Contracts;

public interface IVaccineService
{
    Task<IEnumerable<VaccineResponseDto>> GetAll();
    Task<VaccineResponseDto> GetById(int id);
    Task<VaccineResponseDto> Add(VaccineRequestDto request);
    Task<VaccineResponseDto> Update(VaccineRequestDto request, int id);
    Task<VaccineResponseDto> Delete(int id);
    Task<VaccineResponseDto> SetVaccineCategory(int vaccineId, int categoryId);
    Task<VaccineResponseDto> RemoveVaccineCategory(int vaccineId, int categoryId);
}

