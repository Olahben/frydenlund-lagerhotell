using System.Text.RegularExpressions;

namespace LagerhotellAPI.Models.DomainModels.Validators;

public class UserValidator : AbstractValidator<DomainModels.User>
{
    public UserValidator()
    {
        RuleFor(x => x.Id).NotNull().NotEmpty().WithMessage("Id kan ikke være tom");
        RuleFor(x => x.FirstName).NotNull().NotEmpty().WithMessage("Fornavn kan ikke være tom");
        RuleFor(x => x.FirstName).MaximumLength(250).WithMessage("Fornavn kan ikke være lengre enn 250 bokstaver");
        RuleFor(x => x.LastName).NotNull().NotEmpty().WithMessage("Etternavn(ene) din(e) kan ikke være tomme");
        RuleFor(x => x.LastName).MaximumLength(250).WithMessage("Etternavn kan ikke være lengre enn 250 bokstaver");
        RuleFor(x => x.PhoneNumber).NotNull().NotEmpty().WithMessage("Telefonnummer kan ikke være tom");
        RuleFor(x => x.PhoneNumber).NotEmpty().NotNull().WithMessage("Mobilnummer er obligatorisk").MinimumLength(8).WithMessage("Mobilnummer må være åtte siffer langt").MaximumLength(8).WithMessage("Mobilnummer må være åtte siffer langt").Matches(new Regex(@"((\(\d{3}\) ?)|(\d{3}-))?\d{3}-\d{4}")).WithMessage("Mobilnummer er ikke gyldig");
        RuleFor(x => x.BirthDate).NotNull().NotEmpty().WithMessage("Fødselsdato kan ikke være tom");
        RuleFor(x => x.BirthDate).Matches(new Regex(@"^(\d{4})-(\d{2})-(\d{2})$")).WithMessage("Fødselsdato må være på formatet yyyy-MM-dd");
        RuleFor(x => x.Address).NotNull().NotEmpty().WithMessage("Adresse kan ikke være tom");
        RuleFor(x => x.Address.StreetAddress).NotNull().NotEmpty().WithMessage("Gateadresse kan ikke være tom");
        RuleFor(x => x.Address.StreetAddress).MaximumLength(250).WithMessage("Gateadresse kan ikke være lengre enn 250 bokstaver");
        RuleFor(x => x.Address.PostalCode).NotNull().NotEmpty().WithMessage("Postnummer kan ikke være tom");
        RuleFor(x => x.Address.PostalCode).MaximumLength(4).MinimumLength(4).WithMessage("Postnummer kan ikke være lengre enn 4 siffer");
        RuleFor(x => x.Address.City).NotNull().NotEmpty().WithMessage("Poststed kan ikke være tom");
        RuleFor(x => x.Address.City).MaximumLength(250).WithMessage("Poststed kan ikke være lengre enn 250 bokstaver");
        RuleFor(x => x.Password).NotNull().NotEmpty().WithMessage("Passord kan ikke være tom");
        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Obligatorisk felt")
            .MinimumLength(8).WithMessage("Passord må være minst 8 tegn")
            .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).+$").WithMessage("Passord må inneholde minst én stor bokstav, én liten bokstav og ett tall");
        RuleFor(x => x.IsAdministrator).NotNull().NotEmpty().WithMessage("Administratorstatus kan ikke være tom");
    }
    public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
    {
        var result = await ValidateAsync(ValidationContext<DomainModels.User>.CreateWithOptions((DomainModels.User)model, x => x.IncludeProperties(propertyName)));
        if (result.IsValid)
            return Array.Empty<string>();
        return result.Errors.Select(e => e.ErrorMessage);
    };
}
