using FluentValidation;
using MyVaccine.WebApi.Dtos.FamilyGroup;
using MyVaccine.WebApi.Dtos.Vaccine;

namespace MyVaccine.WebApi.Configurations.Validators;

public class FamilyGroupDtoValidator : AbstractValidator<FamilyGroupRequestDto>
{
    public FamilyGroupDtoValidator()
    {
        RuleFor(dto => dto.Name).NotEmpty().MaximumLength(255);
    }
}
