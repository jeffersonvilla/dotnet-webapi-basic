using FluentValidation;
using MyVaccine.WebApi.Dtos.Allergy;

namespace MyVaccine.WebApi.Configurations.Validators;

public class AllergyDtoValidator : AbstractValidator<AllergyRequestDto>
{
    public AllergyDtoValidator()
    {
        RuleFor(dto => dto.Name).NotEmpty().MaximumLength(255);
        RuleFor(dto => dto.UserId).NotNull();
    }
}
