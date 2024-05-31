using MyVaccine.WebApi.Dtos.Dependent;
using MyVaccine.WebApi.Dtos.User;
using MyVaccine.WebApi.Dtos.Vaccine;

namespace MyVaccine.WebApi.Dtos.VaccineRecord;

public class VaccineRecordResponseDto: VaccineRecordRequestDto
{
    public int Id { get; set; }
    public UserResponseDto User { get; set; }
    public DependentResponseDto Dependent { get; set; }
    public VaccineResponseDto Vaccine { get; set; }

}
