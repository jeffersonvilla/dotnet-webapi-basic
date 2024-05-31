namespace MyVaccine.WebApi.Dtos.Vaccine;

public class VaccineRequestDto
{
    public string Name { get; set; }
    public bool RequiresBooster { get; set; }
    public List<int> CategoryIds { get; set; }
}
