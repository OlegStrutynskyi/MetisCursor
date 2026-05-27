using FluentAssertions;
using MetisPlaywright.Pages;

namespace MetisPlaywright.Tests
{
    public class CreatePasswordTests : BaseTest
    {
        [Test]
        public async Task T01_CreatePassword_DefaultView()
        {
            const string expectedTitle = "Create Your Password";
            const string expectedMessage = "To finish setting up your account, create a strong password with at least 8 characters, including uppercase, lowercase, a number and a special character (e.g.! @ # $ %).";
            const string expectedPasswordPlaceholder = "Enter a password";
            const string expectedConfirmPasswordPlaceholder = "Confirm your password";

            var createPasswordPage = new CreatePasswordPage(Fixture.Page);
            await createPasswordPage.OpenAsync();

            var actualTitle = await createPasswordPage.GetTitleAsync();
            var actualMessage = await createPasswordPage.GetMessageAsync();
            var actualPasswordPlaceholder = await createPasswordPage.GetPasswordPlaceholderAsync();
            var actualConfirmPasswordPlaceholder = await createPasswordPage.GetConfirmPasswordPlaceholderAsync();

            actualTitle.Should().Be(expectedTitle, "Create Password page title is not correct.");
            actualMessage.Should().Be(expectedMessage, "Create Password page message is not correct.");
            actualPasswordPlaceholder.Should().Be(expectedPasswordPlaceholder, "Password input placeholder is not correct.");
            actualConfirmPasswordPlaceholder.Should().Be(expectedConfirmPasswordPlaceholder, "Confirm Password input placeholder is not correct.");

            await createPasswordPage.ExpectDefaultControlsVisibleAsync();
        }

        [Test]
        public async Task T02_CreatePassword_EmptyFields_ShowsErrors()
        {
            const string expectedPasswordError = "'Password' is required.";
            const string expectedConfirmPasswordError = "'Confirm Password' is required.";

            var createPasswordPage = new CreatePasswordPage(Fixture.Page);
            await createPasswordPage.OpenAsync();
            await createPasswordPage.ClickSetPasswordAsync();

            var actualPasswordError = await createPasswordPage.GetPasswordErrorMessageAsync();
            var actualConfirmPasswordError = await createPasswordPage.GetConfirmPasswordErrorMessageAsync();

            actualPasswordError.Should().Be(expectedPasswordError, "An error message for the empty Password field should be shown.");
            actualConfirmPasswordError.Should().Be(expectedConfirmPasswordError, "An error message for the empty Confirm Password field should be shown.");
        }

        [Test]
        public async Task T03_CreatePassword_NoUppercase()
        {
            const string incorrectPassword = "test123!";
            const string expectedUppercaseError = "Password must include UPPERCASE letters.";

            var createPasswordPage = new CreatePasswordPage(Fixture.Page);
            await createPasswordPage.OpenAsync();
            await createPasswordPage.FillPasswordAsync(incorrectPassword);
            await createPasswordPage.ClickSetPasswordAsync();

            var actualPasswordError = await createPasswordPage.GetPasswordErrorMessageAsync();
            actualPasswordError.Should().Be(expectedUppercaseError, "An error message for missing uppercase letters should be shown.");
        }

        [Test]
        public async Task T04_CreatePassword_NoLowercase()
        {
            const string incorrectPassword = "TEST123!";
            const string expectedLowercaseError = "Password must include lowercase letters.";

            var createPasswordPage = new CreatePasswordPage(Fixture.Page);
            await createPasswordPage.OpenAsync();
            await createPasswordPage.FillPasswordAsync(incorrectPassword);
            await createPasswordPage.ClickSetPasswordAsync();

            var actualPasswordError = await createPasswordPage.GetPasswordErrorMessageAsync();
            actualPasswordError.Should().Be(expectedLowercaseError, "An error message for missing lowercase letters should be shown.");
        }

        [Test]
        public async Task T05_CreatePassword_NoDigit()
        {
            const string incorrectPassword = "TESTtest!";
            const string expectedDigitError = "Password must include digits.";

            var createPasswordPage = new CreatePasswordPage(Fixture.Page);
            await createPasswordPage.OpenAsync();
            await createPasswordPage.FillPasswordAsync(incorrectPassword);
            await createPasswordPage.ClickSetPasswordAsync();

            var actualPasswordError = await createPasswordPage.GetPasswordErrorMessageAsync();
            actualPasswordError.Should().Be(expectedDigitError, "An error message for missing digits should be shown.");
        }

        [Test]
        public async Task T06_CreatePassword_NoSpecialCharacter()
        {
            const string incorrectPassword = "Test1234";
            const string expectedSpecialCharError = "Password must include special chararacters.";

            var createPasswordPage = new CreatePasswordPage(Fixture.Page);
            await createPasswordPage.OpenAsync();
            await createPasswordPage.FillPasswordAsync(incorrectPassword);
            await createPasswordPage.ClickSetPasswordAsync();

            var actualPasswordError = await createPasswordPage.GetPasswordErrorMessageAsync();
            actualPasswordError.Should().Be(expectedSpecialCharError, "An error message for missing special characters should be shown.");
        }

        [Test]
        public async Task T07_CreatePassword_TooSmall()
        {
            const string incorrectPassword = "Te1!";
            const string expectedSizeError = "'Password' must be between 6 and 100 characters. You entered 4 characters.";

            var createPasswordPage = new CreatePasswordPage(Fixture.Page);
            await createPasswordPage.OpenAsync();
            await createPasswordPage.FillPasswordAsync(incorrectPassword);
            await createPasswordPage.ClickSetPasswordAsync();

            var actualPasswordError = await createPasswordPage.GetPasswordErrorMessageAsync();
            actualPasswordError.Should().Be(expectedSizeError, "An error message for too short passwords should be shown.");
        }

        [Test]
        public async Task T08_CreatePassword_TooLarge()
        {
            const string incorrectPassword = "Te12345678912345678912345678912345678901234567890!Te12345678912345678912345678912345678901234567890!00";
            const string expectedSizeError = "'Password' must be between 6 and 100 characters. You entered 102 characters.";

            var createPasswordPage = new CreatePasswordPage(Fixture.Page);
            await createPasswordPage.OpenAsync();
            await createPasswordPage.FillPasswordAsync(incorrectPassword);
            await createPasswordPage.ClickSetPasswordAsync();

            var actualPasswordError = await createPasswordPage.GetPasswordErrorMessageAsync();
            actualPasswordError.Should().Be(expectedSizeError, "An error message for too long passwords should be shown.");
        }

        [Test]
        public async Task T09_CreatePassword_IncorrectConfirmPassword()
        {
            const string password = "Test1!";
            const string confirmPassword = "Test1?";
            const string expectedConfirmError = "The password and confirmation password do not match.";

            var createPasswordPage = new CreatePasswordPage(Fixture.Page);
            await createPasswordPage.OpenAsync();
            await createPasswordPage.FillPasswordAsync(password);
            await createPasswordPage.FillConfirmPasswordAsync(confirmPassword);
            await createPasswordPage.ClickSetPasswordAsync();

            var actualConfirmError = await createPasswordPage.GetConfirmPasswordErrorMessageAsync();
            actualConfirmError.Should().Be(expectedConfirmError, "An error message for mismatching confirmation password should be shown.");
        }

        //TODO: add a test for successfully creating a password and being redirected to the Dashboard page.
    }
}
