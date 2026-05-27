using FluentAssertions;
using MetisPlaywright.Pages;

namespace MetisPlaywright.Tests
{
    public class SettingsTests : BaseTest
    {
        [Test]
        public async Task T01_Settings_DefaultView()
        {
            const string expectedTitle = "Settings";

            var settingsPage = new SettingsPage(Fixture.Page);
            await settingsPage.OpenForAutoTests1Async();

            var actualTitle = await settingsPage.GetPageTitleTextAsync();
            actualTitle.Should().Be(expectedTitle, "Settings page title is not correct.");

            await settingsPage.ExpectDefaultSectionsVisibleAsync();
        }

        [Test]
        public async Task T02_Settings_GlobalPermissionsSection()
        {
            const string expectedSectionTitle = "Global Permissions";
            const string expectedSectionMessage = "Control which users can perform administrative task as Scheduling.";
            const string expectedPageTitle = "Global Permissions";

            var settingsPage = new SettingsPage(Fixture.Page);
            await settingsPage.OpenForAutoTests1Async();

            var actualSectionTitle = await settingsPage.GetGlobalPermissionsTitleAsync();
            var actualSectionMessage = await settingsPage.GetGlobalPermissionsMessageAsync();
            actualSectionTitle.Should().Be(expectedSectionTitle, "Global Permissions section title is not correct.");
            actualSectionMessage.Should().Be(expectedSectionMessage, "Global Permissions section message is not correct.");

            var globalPermissionsPage = await settingsPage.ClickEditGlobalPermissionsBtnAsync();
            var actualPageTitle = await globalPermissionsPage.GetPageTitleTextAsync();
            actualPageTitle.Should().Be(expectedPageTitle, "Global Permissions page title is not correct after clicking Edit Global Permissions.");
        }

        [Test]
        public async Task T03_Settings_CompanySettingsSection()
        {
            const string expectedSectionTitle = "Company Settings";
            const string expectedSectionMessage = "You can edit company name, address and other details from Company section.";
            const string expectedPageTitle = "Company Settings";

            var settingsPage = new SettingsPage(Fixture.Page);
            await settingsPage.OpenForAutoTests1Async();

            var actualSectionTitle = await settingsPage.GetCompanySettingsTitleAsync();
            var actualSectionMessage = await settingsPage.GetCompanySettingsMessageAsync();
            actualSectionTitle.Should().Be(expectedSectionTitle, "Company Settings section title is not correct.");
            actualSectionMessage.Should().Be(expectedSectionMessage, "Company Settings section message is not correct.");

            var companySettingsPage = await settingsPage.ClickEditCompanySettingsBtnAsync();
            var actualPageTitle = await companySettingsPage.GetPageTitleTextAsync();
            actualPageTitle.Should().Be(expectedPageTitle, "Company Settings page title is not correct after clicking Edit Company Settings.");
        }

        [Test]
        public async Task T04_Settings_PersonalSettingsSection()
        {
            const string expectedSectionTitle = "Personal Settings";
            const string expectedSectionMessage = "You can edit your name, email and password and other details from People section.";
            const string expectedModalTitle = "Person Settings";

            var settingsPage = new SettingsPage(Fixture.Page);
            await settingsPage.OpenForAutoTests1Async();

            var actualSectionTitle = await settingsPage.GetPersonalSettingsTitleAsync();
            var actualSectionMessage = await settingsPage.GetPersonalSettingsMessageAsync();
            actualSectionTitle.Should().Be(expectedSectionTitle, "Personal Settings section title is not correct.");
            actualSectionMessage.Should().Be(expectedSectionMessage, "Personal Settings section message is not correct.");

            var personOverviewPage = await settingsPage.ClickEditMyPersonalSettingsBtnAsync();
            var actualModalTitle = await personOverviewPage.GetPersonSettingsTitleAsync();
            actualModalTitle.Should().Be(expectedModalTitle, "Person Settings modal title is not correct after clicking Edit My Personal Settings.");
        }

        [Test]
        public async Task T05_Settings_NotificationSettingsSection()
        {
            const string expectedSectionTitle = "Notification Settings";
            const string expectedSectionMessage = "Update your notification settings for in-app and email notifications.";
            const string expectedPageTitle = "NOTIFICATION SETTINGS";

            var settingsPage = new SettingsPage(Fixture.Page);
            await settingsPage.OpenForAutoTests1Async();

            var actualSectionTitle = await settingsPage.GetNotificationSettingsTitleAsync();
            var actualSectionMessage = await settingsPage.GetNotificationSettingsMessageAsync();
            actualSectionTitle.Should().Be(expectedSectionTitle, "Notification Settings section title is not correct.");
            actualSectionMessage.Should().Be(expectedSectionMessage, "Notification Settings section message is not correct.");

            var notificationSettingsPage = await settingsPage.ClickNotificationSettingsBtnAsync();
            var actualPageTitle = await notificationSettingsPage.GetPageTitleTextAsync();
            actualPageTitle.Should().Be(expectedPageTitle, "Notification Settings page title is not correct after clicking Notification Settings.");
        }

        [Test]
        public async Task T06_Settings_OptimizationSettingsSection()
        {
            const string expectedSectionTitle = "Optimization Settings";
            const string expectedSectionMessage = "Configure how long the company stays locked after the solver finishes, giving users time to review and apply.";
            const string expectedPageTitle = "Optimization Settings";

            var settingsPage = new SettingsPage(Fixture.Page);
            await settingsPage.OpenForAutoTests1Async();

            var actualSectionTitle = await settingsPage.GetOptimizationSettingsTitleAsync();
            var actualSectionMessage = await settingsPage.GetOptimizationSettingsMessageAsync();
            actualSectionTitle.Should().Be(expectedSectionTitle, "Optimization Settings section title is not correct.");
            actualSectionMessage.Should().Be(expectedSectionMessage, "Optimization Settings section message is not correct.");

            var optimizationSettingsPage = await settingsPage.ClickEditOptimizationSettingsBtnAsync();
            var actualPageTitle = await optimizationSettingsPage.GetPageTitleTextAsync();
            actualPageTitle.Should().Be(expectedPageTitle, "Optimization Settings page title is not correct after clicking Edit Optimization Settings.");
        }

        [Test]
        public async Task T07_Settings_CustomersSection()
        {
            const string expectedSectionTitle = "Customers";
            const string expectedSectionMessage = "Customers are the client companies that buy from your company. They are managed in the Customers section.";
            const string expectedPageTitle = "Customer Accounts";

            var settingsPage = new SettingsPage(Fixture.Page);
            await settingsPage.OpenForAutoTests1Async();

            var actualSectionTitle = await settingsPage.GetCustomersTitleAsync();
            var actualSectionMessage = await settingsPage.GetCustomersMessageAsync();
            actualSectionTitle.Should().Be(expectedSectionTitle, "Customers section title is not correct.");
            actualSectionMessage.Should().Be(expectedSectionMessage, "Customers section message is not correct.");

            var customerAccountsPage = await settingsPage.ClickManageCustomersBtnAsync();
            var actualPageTitle = await customerAccountsPage.GetPageTitleTextAsync();
            actualPageTitle.Should().Be(expectedPageTitle, "Customer Accounts page title is not correct after clicking Manage Customers.");
        }

        [Test]
        public async Task T08_Settings_TeamsSection()
        {
            const string expectedSectionTitle = "Teams";
            const string expectedSectionMessage = "People can be assigned to teams. They are managed in Team section.";
            const string expectedPageTitle = "Teams Overview";

            var settingsPage = new SettingsPage(Fixture.Page);
            await settingsPage.OpenForAutoTests1Async();

            var actualSectionTitle = await settingsPage.GetTeamsTitleAsync();
            var actualSectionMessage = await settingsPage.GetTeamsMessageAsync();
            actualSectionTitle.Should().Be(expectedSectionTitle, "Teams section title is not correct.");
            actualSectionMessage.Should().Be(expectedSectionMessage, "Teams section message is not correct.");

            var teamsOverviewPage = await settingsPage.ClickManageTeamsBtnAsync();
            var actualPageTitle = await teamsOverviewPage.GetPageTitleTextAsync();
            actualPageTitle.Should().Be(expectedPageTitle, "Teams Overview page title is not correct after clicking Manage Teams.");
        }
    }
}
