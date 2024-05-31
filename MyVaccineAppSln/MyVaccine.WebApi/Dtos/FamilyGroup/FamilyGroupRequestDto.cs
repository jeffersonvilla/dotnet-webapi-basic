namespace MyVaccine.WebApi.Dtos.FamilyGroup;

public class FamilyGroupRequestDto
{
    public string Name { get; set; }
    public List<int> UserIds { get; set; }
}
