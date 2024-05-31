using MyVaccine.WebApi.Dtos.Vaccine;

namespace MyVaccine.WebApi.Dtos.VaccineCategory;

public class VaccineCategoryResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<VaccineResponseDto> Vaccines { get; set; }
}
