using FluentValidation;
using MyVaccine.WebApi.Dtos.Allergy;
using MyVaccine.WebApi.Dtos.VaccineRecord;

namespace MyVaccine.WebApi.Configurations.Validators;

public class VaccineRecordDtoValidator : AbstractValidator<VaccineRecordRequestDto>
{
    public VaccineRecordDtoValidator()
    {
        RuleFor(dto => dto.UserId).NotNull().GreaterThan(0); ;
        RuleFor(dto => dto.DependentId).NotNull().GreaterThan(0); ;
        RuleFor(dto => dto.VaccineId).NotNull().GreaterThan(0); ;
        RuleFor(dto => dto.DateAdministered).NotNull();
        RuleFor(dto => dto.AdministeredLocation).NotEmpty().MaximumLength(255);
        RuleFor(dto => dto.AdministeredBy).NotEmpty().MaximumLength(255);
    }
}