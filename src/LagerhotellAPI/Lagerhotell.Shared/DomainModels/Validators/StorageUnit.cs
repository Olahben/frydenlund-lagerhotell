namespace LagerhotellAPI.Models.DomainModels.Validators;

public class StorageUnitValidator : AbstractValidator<DomainModels.StorageUnit>
{
    public StorageUnitValidator()
    {
        RuleFor(x => x.StorageUnitId).MaximumLength(250).WithMessage("LagerenhetId kan ikke være lengre enn 250 bokstaver");
        RuleFor(x => x.Dimensions).NotNull().WithMessage("Dimensjoner er obligatorisk");
        RuleFor(x => x.Dimensions.Length).GreaterThan(0).WithMessage("Lengde må være større enn 0");
        RuleFor(x => x.Dimensions.Width).GreaterThan(0).WithMessage("Lengde må være større enn 0");
        RuleFor(x => x.Dimensions.Height).GreaterThan(0).WithMessage("Lengde må være større enn 0");
        RuleFor(x => x.Temperated).NotNull().WithMessage("Temperatur er obligatorisk");
        RuleFor(x => x.LockCode).MaximumLength(6).WithMessage("Låskode kan ikke være lengre enn 6 bokstaver");
        RuleFor(x => x.Name).NotEmpty().WithMessage("Navn er obligatorisk");
        RuleFor(x => x.Name).MaximumLength(250).WithMessage("Navn kan ikke være lengre enn 250 bokstaver");
        RuleFor(x => x.Occupied).NotNull().WithMessage("Opptatt status er obligatorisk");
        RuleFor(x => x.Coordinate).NotNull().WithMessage("Koordinater er obligatorisk");
        RuleFor(x => x.Coordinate.Longitude).Must(x => x >= -180 && x <= 180).WithMessage("Longitude må være mellom -180 og 180");
        RuleFor(x => x.Coordinate.Latitude).Must(x => x >= -90 && x <= 90).WithMessage("Latitude må være mellom -90 og 90");
        RuleFor(x => x.PricePerMonth).NotNull().WithMessage("Pris per måned er obligatorisk");
    }
    public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
    {
        var result = await ValidateAsync(ValidationContext<DomainModels.StorageUnit>.CreateWithOptions((DomainModels.StorageUnit)model, x => x.IncludeProperties(propertyName)));
        if (result.IsValid)
            return Array.Empty<string>();
        return result.Errors.Select(e => e.ErrorMessage);
    };
}
