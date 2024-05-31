namespace MyVaccine.WebApi.Dtos.VaccineCategory;

public class VaccineCategoryRequestDto
{
    public string Name { get; set; }

    public List<int> VaccineIds { get; set; }

}
