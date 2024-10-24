using System.Text.RegularExpressions;

namespace LagerhotellAPI.Models.DomainModels.Validators;
public class WarehouseHotelValidator : AbstractValidator<DomainModels.WarehouseHotel>
{
    public WarehouseHotelValidator()
    {
        RuleFor(x => x.Coordinate).NotNull().NotEmpty().WithMessage("Koordinat kan ikke være tom");
        RuleFor(x => x.Coordinate.Longitude).NotNull().NotEmpty().WithMessage("Lengdegrad kan ikke være tom");
        RuleFor(x => x.Coordinate.Latitude).NotNull().NotEmpty().WithMessage("Breddegrad kan ikke være tom");
        RuleFor(x => x.Coordinate.Latitude).InclusiveBetween(-90, 90).WithMessage("Breddegrad må være mellom -90 og 90");
        RuleFor(x => x.Coordinate.Longitude).InclusiveBetween(-180, 180).WithMessage("Lengdegrad må være mellom -180 og 180");
        RuleFor(x => x.Address).NotNull().NotEmpty().WithMessage("Adresse kan ikke være tom");
        RuleFor(x => x.Address.StreetAddress).NotNull().NotEmpty().WithMessage("Gateadresse kan ikke være tom");
        RuleFor(x => x.Address.StreetAddress).MaximumLength(250).WithMessage("Gateadresse kan ikke være lengre enn 250 bokstaver");
        RuleFor(x => x.Address.PostalCode).NotNull().NotEmpty().WithMessage("Postnummer kan ikke være tom");
        RuleFor(x => x.Address.PostalCode).MaximumLength(4).MinimumLength(4).WithMessage("Postnummer kan ikke være lengre enn 4 siffer");
        RuleFor(x => x.Address.City).NotNull().NotEmpty().WithMessage("Poststed kan ikke være tom");
        RuleFor(x => x.Address.City).MaximumLength(250).WithMessage("Poststed kan ikke være lengre enn 250 bokstaver");
        RuleFor(x => x.Name).NotNull().NotEmpty().WithMessage("Navn kan ikke være tom");
        RuleFor(x => x.Name).MaximumLength(250).WithMessage("Navn kan ikke være lengre enn 250 bokstaver");
        RuleFor(x => x.OpeningHours).NotNull().NotEmpty().WithMessage("Åpningstider kan ikke være tom");
        RuleFor(x => x.OpeningHours.Closes).Must((x, y) => x.OpeningHours.Opens < y).WithMessage("Åpningstid må være før stengetid");
        RuleFor(x => x.PhoneNumber).NotNull().NotEmpty().WithMessage("Telefonnummer kan ikke være tom");
        RuleFor(x => x.PhoneNumber).NotEmpty().NotNull().WithMessage("Telefonnummer er obligatorisk").MinimumLength(8).WithMessage("Telefonnummer må være åtte siffer langt").MaximumLength(8).WithMessage("Telefonnummer må være åtte siffer langt").Matches(new Regex(@"^\d{8}$")).WithMessage("Telefonnummer er ikke gyldig");
        RuleFor(x => x.DetailedDescription).NotNull().NotEmpty().WithMessage("Detaljert beskrivelse kan ikke være tom");
        RuleFor(x => x.DetailedDescription).MaximumLength(5000).WithMessage("Detaljert beskrivelse kan ikke være lengre enn 5000 tegn");
        RuleFor(x => x.BulletPoints).NotNull().NotEmpty().WithMessage("Stikkord kan ikke være tom");
        RuleFor(x => x.BulletPoints).MaximumLength(500).WithMessage("Stikkord kan ikke være lengre enn 500 tegn");
        RuleFor(x => x.ContainsTemperatedStorageUnits).NotNull().NotEmpty().WithMessage("Lagerhotellet må inneholde tempererte lagerenheter");
        RuleFor(x => x.IsActive).NotNull().NotEmpty().WithMessage("Aktiv status kan ikke være tom");
        RuleFor(x => x.StorageUnitsSizes).NotNull().NotEmpty().WithMessage("Lagereenhetsstørrelser kan ikke være tom");
        RuleFor(x => x.StorageUnitsSizes.MinSize).Must((x, y) => x.StorageUnitsSizes.MinSize < x.StorageUnitsSizes.MaxSize).WithMessage("Minimumsstørrelse må være mindre enn maksimumsstørrelse");
    }
    public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
    {
        var result = await ValidateAsync(ValidationContext<DomainModels.WarehouseHotel>.CreateWithOptions((DomainModels.WarehouseHotel)model, x => x.IncludeProperties(propertyName)));
        if (result.IsValid)
            return Array.Empty<string>();
        return result.Errors.Select(e => e.ErrorMessage);
    };
}