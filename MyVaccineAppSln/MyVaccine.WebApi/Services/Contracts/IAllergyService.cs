using MyVaccine.WebApi.Dtos.Allergy;

namespace MyVaccine.WebApi.Services.Contracts
{
    public interface IAllergyService
    {
        Task<IEnumerable<AllergyResponseDto>> GetAll();
        Task<AllergyResponseDto> GetById(int id);
        Task<AllergyResponseDto> Add(AllergyRequestDto request);
        Task<AllergyResponseDto> Update(AllergyRequestDto request, int id);
        Task<AllergyResponseDto> Delete(int id);
    }
}
