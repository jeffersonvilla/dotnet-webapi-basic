using MyVaccine.WebApi.Dtos.FamilyGroup;

namespace MyUser.WebApi.Services.Contracts;

public interface IFamilyGroupService
{
    Task<IEnumerable<FamilyGroupResponseDto>> GetAll();
    Task<FamilyGroupResponseDto> GetById(int id);
    Task<FamilyGroupResponseDto> Add(FamilyGroupRequestDto request);
    Task<FamilyGroupResponseDto> Update(FamilyGroupRequestDto request, int id);
    Task<FamilyGroupResponseDto> Delete(int id);
    Task<FamilyGroupResponseDto> SetUser(int familyGroupId, int userId);
    Task<FamilyGroupResponseDto> RemoveUser(int familyGroupId, int userId);
}
