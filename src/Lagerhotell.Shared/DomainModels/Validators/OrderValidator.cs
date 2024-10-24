namespace LagerhotellAPI.Models.DomainModels.Validators;

public class OrderValidator : AbstractValidator<Order>
{
    public OrderValidator()
    {
        RuleFor(x => x.UserId).NotEmpty().WithMessage("BrukerId er obligatorisk");
        RuleFor(x => x.OrderPeriod.OrderDate).NotEmpty().WithMessage("Startdato er obligatorisk");
        RuleFor(x => x.OrderPeriod.OrderDate).LessThan(DateTime.Today.AddMonths(1)).WithMessage("Du kan kun bestille en måned frem i tid");
        RuleFor(x => x.OrderPeriod.OrderDate).GreaterThan(x => DateTime.Today.AddDays(-1)).WithMessage("Du kan kun bestille fra den dag i dag");
        RuleFor(x => x.Status).NotNull().WithMessage("Status er obligatorisk");
        RuleFor(x => x.CustomInstructions).MaximumLength(500).WithMessage("Tilleggsinformasjon kan ikke være lengre enn 500 bokstaver");
        RuleFor(x => x.Insurance).NotNull().WithMessage("Forsikring er obligatorisk");
    }
    public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
    {
        var result = await ValidateAsync(ValidationContext<DomainModels.Order>.CreateWithOptions((DomainModels.Order)model, x => x.IncludeProperties(propertyName)));
        if (result.IsValid)
            return Array.Empty<string>();
        return result.Errors.Select(e => e.ErrorMessage);
    };
}
