using FluentValidation;
using MyVaccine.WebApi.Dtos.Vaccine;

namespace MyVaccine.WebApi.Configurations.Validators;

public class VaccineDtoValidator : AbstractValidator<VaccineRequestDto>
{
    public VaccineDtoValidator()
    {
        RuleFor(dto => dto.Name).NotEmpty().MaximumLength(255);
        RuleFor(dto => dto.RequiresBooster).NotNull();
    }
}
