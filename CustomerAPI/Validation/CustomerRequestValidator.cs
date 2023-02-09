using System.Text.RegularExpressions;
using ConsumerAPI.Contracts.Requests;
using FluentValidation;

namespace ConsumerAPI.Validation;

public class CustomerRequestValidator : AbstractValidator<CustomerRequest>
{
    public CustomerRequestValidator()
    {
        RuleFor(x => x.FullName)
            .Matches(FullNameRegex());

        RuleFor(x => x.Email)
            .EmailAddress();

        RuleFor(x => x.GitHubUsername)
            .Matches(UsernameRegex());

        RuleFor(x => x.DateOfBirth)
            .LessThan(DateTime.Now)
            .WithMessage("Your date of birth cannot be in the future");
    }

    private static Regex FullNameRegex()
    {
        return new Regex("^[a-z ,.'-]+$", RegexOptions.IgnoreCase | RegexOptions.Compiled);
    }

    private static Regex UsernameRegex()
    {
        return new Regex("^[a-z\\d](?:[a-z\\d]|-(?=[a-z\\d])){0,38}$", RegexOptions.IgnoreCase | RegexOptions.Compiled);
    }

}