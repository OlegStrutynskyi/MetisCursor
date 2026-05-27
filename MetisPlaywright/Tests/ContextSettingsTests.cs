using FluentAssertions;
using MetisPlaywright.Pages;

namespace MetisPlaywright.Tests
{
    public class ContextSettingsTests : BaseTest
    {
        [Test]
        public async Task T01_ContextSettings_Configuration_DefaultView()
        {
            const string expectedTitle = "Context Settings";
            const string expectedMessage =
                "Set the title and description to define the purpose of your context.";

            var contextSettingsPage = new ContextSettingsPage(Fixture.Page);
            await contextSettingsPage.GetContextSettingsPageAutoTests1Async();

            var actualTitle = await contextSettingsPage.GetContextSettingsTitleAsync();
            actualTitle.Trim().Should().Be(expectedTitle, "Context Settings modal title is not correct.");

            var actualMessage = await contextSettingsPage.GetConfigurationTabMessageAsync();
            actualMessage.Trim().Should().Be(expectedMessage, "Configuration tab message is not correct.");

            await contextSettingsPage.ExpectConfigurationControlsVisibleAsync();
        }

        [Test]
        public async Task T02_ContextSettings_Privacy_DefaultView()
        {
            const string expectedMessage =
                "Control who can access this context and protect sensitive data with privacy settings.";

            var contextSettingsPage = new ContextSettingsPage(Fixture.Page);
            await contextSettingsPage.GetContextSettingsPageAutoTests1Async();
            await contextSettingsPage.ClickPrivacyTabAsync();

            var actualMessage = await contextSettingsPage.GetPrivacyTabMessageAsync();
            actualMessage.Trim().Should().Be(expectedMessage, "Privacy tab message is not correct.");

            await contextSettingsPage.ExpectPrivacyControlsVisibleAsync();
        }

        [Test]
        public async Task T03_ContextSettings_Labels_DefaultView()
        {
            const string expectedMessage =
                "Add labels to help categorize and optimize your context for search and organization.";

            var contextSettingsPage = new ContextSettingsPage(Fixture.Page);
            await contextSettingsPage.GetContextSettingsPageAutoTests1Async();
            await contextSettingsPage.ClickLabelsTabAsync();

            var actualMessage = await contextSettingsPage.GetLabelsTabMessageAsync();
            actualMessage.Trim().Should().Be(expectedMessage, "Labels tab message is not correct.");

            await contextSettingsPage.ExpectLabelsControlsVisibleAsync();
        }

        [Test]
        public async Task T04_ContextSettings_Notifications_DefaultView()
        {
            const string expectedMessage =
                "Configure how and when notifications are sent to keep your team updated on the progress of this context.";

            var contextSettingsPage = new ContextSettingsPage(Fixture.Page);
            await contextSettingsPage.GetContextSettingsPageAutoTests1Async();
            await contextSettingsPage.ClickNotificationsTabAsync();

            var actualMessage = await contextSettingsPage.GetNotificationsTabMessageAsync();
            actualMessage.Trim().Should().Be(expectedMessage, "Notifications tab message is not correct.");

            await contextSettingsPage.ExpectNotificationsControlsVisibleAsync();
        }

        [Test]
        public async Task T05_ContextSettings_Chat_DefaultView()
        {
            const string expectedMessage =
                "Enable or disable the chat feature to assist users in real-time during this context.";

            var contextSettingsPage = new ContextSettingsPage(Fixture.Page);
            await contextSettingsPage.GetContextSettingsPageAutoTests1Async();
            await contextSettingsPage.ClickChatTabAsync();

            var actualMessage = await contextSettingsPage.GetChatTabMessageAsync();
            actualMessage.Trim().Should().Be(expectedMessage, "Chat tab message is not correct.");

            await contextSettingsPage.ExpectChatControlsVisibleAsync();
        }

        [Test]
        public async Task T06_ContextSettings_EmptyFields()
        {
            const string expectedContextTitleError = "'Name' is required.";
            const string expectedCustomerAccountError = "'Customer Account' is required.";

            var contextSettingsPage = new ContextSettingsPage(Fixture.Page);
            await contextSettingsPage.GetContextSettingsPageAutoTests1Async();
            await contextSettingsPage.ClickCreateAsync();

            var actualContextTitleError = await contextSettingsPage.GetContextTitleErrorAsync();
            actualContextTitleError.Trim().Should().Be(expectedContextTitleError, "Context title error is not correct.");

            var actualCustomerAccountError = await contextSettingsPage.GetCustomerAccountErrorAsync();
            actualCustomerAccountError.Trim().Should().Be(expectedCustomerAccountError, "Customer account error is not correct.");

            var actualTopErrors = await contextSettingsPage.GetTopError();
            actualTopErrors.Should().Contain(expectedContextTitleError, "Top error should include context title validation message.");
            actualTopErrors.Should().Contain(expectedCustomerAccountError, "Top error should include customer account validation message.");
        }
    }
}
