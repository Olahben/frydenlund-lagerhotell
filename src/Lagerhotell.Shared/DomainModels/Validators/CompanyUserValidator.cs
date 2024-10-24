namespace LagerhotellAPI.Models.DomainModels.Validators;

public class CompanyUserValidator : AbstractValidator<CompanyUser>
{
    public CompanyUserValidator()
    {
        RuleFor(x => x.FirstName).NotEmpty().WithMessage("Fornavn er obligatorisk");
        RuleFor(x => x.LastName).NotEmpty().WithMessage("Etternavn er obligatorisk");
        RuleFor(x => x.Name).NotEmpty().WithMessage("Navn er obligatorisk");
        RuleFor(x => x.CompanyNumber).NotEmpty().WithMessage("Organisasjonsnummer er obligatorisk");
        RuleFor(x => x.CompanyNumber).Matches(@"^\d{9}$").WithMessage("Organisasjonsnummer er ikke gyldig");
        RuleFor(x => x.Email).NotEmpty().WithMessage("Epost er obligatorisk");
        RuleFor(x => x.Email).EmailAddress().WithMessage("Epost er ikke gyldig");
        RuleFor(x => x.PhoneNumber).NotEmpty().WithMessage("Telefonnummer er obligatorisk");
        RuleFor(x => x.PhoneNumber).Matches(@"^(\+47|0047|47)?[1-9]\d{7}$").WithMessage("Telefonnummer er ikke gyldig, husk +47");
        RuleFor(x => x.Address).NotNull().WithMessage("Adresse er obligatorisk");
        RuleFor(x => x.Address.StreetAddress).NotEmpty().WithMessage("Gate er obligatorisk");
        RuleFor(x => x.Address.PostalCode).NotEmpty().WithMessage("Postnummer er obligatorisk");
        RuleFor(x => x.Address.PostalCode).Matches(@"^\d{4}$").WithMessage("Postnummer er ikke gyldig");
        RuleFor(x => x.Address.City).NotEmpty().WithMessage("By er obligatorisk");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Obligatorisk felt")
            .MinimumLength(8).WithMessage("Passord må være minst 8 tegn")
            .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).+$").WithMessage("Passord må inneholde minst én stor bokstav, én liten bokstav og ett tall");
    }

    public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
    {
        var result = await ValidateAsync(ValidationContext<CompanyUser>.CreateWithOptions((CompanyUser)model, x => x.IncludeProperties(propertyName)));
        if (result.IsValid)
            return Array.Empty<string>();
        return result.Errors.Select(e => e.ErrorMessage);
    };
}
