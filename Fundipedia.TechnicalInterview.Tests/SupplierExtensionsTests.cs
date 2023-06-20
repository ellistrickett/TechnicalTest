using Fundipedia.TechnicalInterview.Model.Supplier;
using Fundipedia.TechnicalInterview.Model.Extensions;
using System.ComponentModel.DataAnnotations;

namespace Fundipedia.TechnicalInterview.Test;
public class SupplierExtensionsTests
{
    [Fact]
    public void IsActive_ReturnsFalse_WhenActivationDateIsInThePast()
    {
        var supplier = new Supplier { ActivationDate = DateTime.Today.AddDays(-1) };

        var result = supplier.IsActive();

        Assert.False(result);
    }

    [Fact]
    public void IsActive_ReturnsTrue_WhenActivationDateIsTomorrow()
    {
        var supplier = new Supplier { ActivationDate = DateTime.Today.AddDays(1) };

        var result = supplier.IsActive();

        Assert.True(result);
    }

    [Fact]
    public void SupplierValidate_ReturnsValidationResult_WhenInvalidEmail()
    {
        var invalidSupplier = new Supplier();
        var invalidEmail = new Email();
        invalidEmail.EmailAddress = "invalidEmail";
        invalidSupplier.Emails.Add(invalidEmail);

        var validationContext = new ValidationContext(invalidSupplier);
        var validationResults = invalidSupplier.Validate(validationContext);

        Assert.NotEmpty(validationResults);
        Assert.Contains(validationResults, vr => vr.MemberNames.Contains(nameof(Supplier.Emails)));
    }

    [Fact]
    public void SupplierValidate_ReturnsValidationResult_WhenInvalidPhoneNumber()
    {
        var invalidSupplier = new Supplier();
        var invalidPhoneNumber = new Phone();
        invalidPhoneNumber.PhoneNumber = "01234567890";
        invalidSupplier.Phones.Add(invalidPhoneNumber);

        var validationContext = new ValidationContext(invalidSupplier);
        var validationResults = invalidSupplier.Validate(validationContext);

        Assert.NotEmpty(validationResults);
        Assert.Contains(validationResults, vr => vr.MemberNames.Contains(nameof(Supplier.Phones)));
    }

    [Fact]
    public void SupplierValidate_ReturnsEmpty_WhenValid()
    {
        var invalidSupplier = new Supplier();

        var validationContext = new ValidationContext(invalidSupplier);
        var validationResults = invalidSupplier.Validate(validationContext);

        Assert.Empty(validationResults);
    }
}