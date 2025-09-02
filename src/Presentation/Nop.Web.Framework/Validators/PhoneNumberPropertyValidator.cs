using System.Text.RegularExpressions;
using FluentValidation;
using FluentValidation.Validators;
using Nop.Core.Domain.Customers;

namespace Nop.Web.Framework.Validators;

/// <summary>
/// Phohe number validator
/// </summary>
public partial class PhoneNumberPropertyValidator<T, TProperty> : PropertyValidator<T, TProperty>
{
    protected readonly CustomerSettings _customerSettings;

    public override string Name => "PhoneNumberPropertyValidator";

    /// <summary>
    /// Ctor
    /// </summary>
    public PhoneNumberPropertyValidator(CustomerSettings customerSettings)
    {
        _customerSettings = customerSettings;
    }

    /// <summary>
    /// Is valid?
    /// </summary>
    /// <param name="context">Validation context</param>
    /// <returns>Result</returns>
    public override bool IsValid(ValidationContext<T> context, TProperty value)
    {
        return IsValid(value as string, _customerSettings);
    }

    /// <summary>
    /// Is valid?
    /// </summary>
    /// <param name="phoneNumber">Phone number</param>
    /// <param name="customerSettings">Customer settings</param>
    /// <returns>Result</returns>
    public static bool IsValid(string phoneNumber, CustomerSettings customerSettings)
    {
        if (!customerSettings.PhoneNumberValidationEnabled || string.IsNullOrEmpty(customerSettings.PhoneNumberValidationRule))
            return true;

        if (string.IsNullOrEmpty(phoneNumber))
        {
            return !customerSettings.PhoneRequired;
        }


        return Regex.IsMatch(phoneNumber,
    customerSettings.PhoneNumberValidationUseRegex
        ? customerSettings.PhoneNumberValidationRule
        : @"^(\+\d{1,3}[- ]?)?\d{6,14}$",
    RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);
        //return customerSettings.PhoneNumberValidationUseRegex
        //    ? Regex.IsMatch(phoneNumber, customerSettings.PhoneNumberValidationRule, RegexOptions.CultureInvariant | RegexOptions.IgnoreCase)
        //    : phoneNumber.All(l => customerSettings.PhoneNumberValidationRule.Contains(l));
    }

    protected override string GetDefaultMessageTemplate(string errorCode) => "Phone number is not valid";
}