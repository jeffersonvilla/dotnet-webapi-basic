using MyVaccine.WebApi.Dtos.User;

namespace MyVaccine.WebApi.Dtos.FamilyGroup;

public class FamilyGroupResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<UserResponseDto> Users { get; set; }
}
