using MyVaccine.WebApi.Dtos.Allergy;
using MyVaccine.WebApi.Dtos.Dependent;
using MyVaccine.WebApi.Dtos.FamilyGroup;
using MyVaccine.WebApi.Dtos.VaccineRecord;

namespace MyVaccine.WebApi.Dtos.User;

public class UserResponseDto
{
    public int UserId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }

    public List<DependentResponseDto> Dependents { get; set; }
    public List<FamilyGroupResponseDto> FamilyGroups { get; set; }
    public List<VaccineRecordResponseDto> VaccineRecords { get; set; }
    public List<AllergyResponseDto> Allergies { get; set; }
}
