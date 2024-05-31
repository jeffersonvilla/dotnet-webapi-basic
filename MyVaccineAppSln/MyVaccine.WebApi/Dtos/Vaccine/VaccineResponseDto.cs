using MyVaccine.WebApi.Dtos.VaccineCategory;

namespace MyVaccine.WebApi.Dtos.Vaccine;

public class VaccineResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public bool RequiresBooster { get; set; }

    public List<VaccineCategoryResponseDto> Categories { get; set; }
}
