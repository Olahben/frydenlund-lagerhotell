global using FluentValidation;

namespace LagerhotellAPI.Models.DomainModels.Validators;

public class LocationValidator : AbstractValidator<Location>
{
    public LocationValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Lokasjavn er obligatorisk");
        RuleFor(x => x.Name).MaximumLength(200).WithMessage("Lokasjonsnavn kan ikke være lengre enn 200 bokstaver");
        RuleFor(x => x.Name).MinimumLength(2).WithMessage("Lokasjonsnavn må være minst 2 bokstaver langt");
        RuleFor(x => x.IsActive).NotNull().WithMessage("Aktiv status er obligatorisk");
    }

    public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
    {
        var result = await ValidateAsync(ValidationContext<Location>.CreateWithOptions((Location)model, x => x.IncludeProperties(propertyName)));
        if (result.IsValid)
            return Array.Empty<string>();
        return result.Errors.Select(e => e.ErrorMessage);
    };
}
