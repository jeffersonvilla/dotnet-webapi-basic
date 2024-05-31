using MyVaccine.WebApi.Dtos.Dependent;
using MyVaccine.WebApi.Dtos.User;
using MyVaccine.WebApi.Dtos.Vaccine;

namespace MyVaccine.WebApi.Dtos.VaccineRecord;

public class VaccineRecordResponseDto: VaccineRecordRequestDto
{
    public int Id { get; set; }

}
