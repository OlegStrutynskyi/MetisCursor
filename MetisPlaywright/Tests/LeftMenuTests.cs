using FluentAssertions;
using MetisPlaywright.Pages;
using MetisPlaywright.Utils;

namespace MetisPlaywright.Tests
{
    public class LeftMenuTests : BaseTest
    {
        [Test]
        public async Task T01_LeftMenu_NavigationItemsVisible()
        {
            var leftMenu = new LeftMenuPage(Fixture.Page);
            await leftMenu.OpenForEmptyTenantAsync();
            await leftMenu.ExpectNavigationItemsVisibleAsync();
        }

        [Test]
        public async Task T02_LeftMenu_NavigateToContextExplorer()
        {
            const string expectedUrl = "contexts";

            var leftMenu = new LeftMenuPage(Fixture.Page);
            await leftMenu.OpenForEmptyTenantAsync();
            await leftMenu.ClickContextExplorerIconAsync();

            Fixture.Page.Url.Should().Contain(expectedUrl, "Clicking on Context Explorer icon should navigate to the Context Explorer page.");
        }

        [Test]
        public async Task T03_LeftMenu_NavigateToLabels()
        {
            const string expectedUrl = "labels";

            var leftMenu = new LeftMenuPage(Fixture.Page);
            await leftMenu.OpenForEmptyTenantAsync();
            await leftMenu.ClickLabelsIconAsync();

            Fixture.Page.Url.Should().Contain(expectedUrl, "Clicking on Labels icon should navigate to the Labels page.");
        }

        [Test]
        public async Task T04_LeftMenu_NavigateToTemplates()
        {
            const string expectedUrl = "templates";

            var leftMenu = new LeftMenuPage(Fixture.Page);
            await leftMenu.OpenForEmptyTenantAsync();
            await leftMenu.ClickTemplatesIconAsync();

            Fixture.Page.Url.Should().Contain(expectedUrl, "Clicking on Templates icon should navigate to the Templates page.");
        }

        [Test]
        public async Task T05_LeftMenu_NavigateToResourceManager()
        {
            const string expectedUrl = "resources";

            var leftMenu = new LeftMenuPage(Fixture.Page);
            await leftMenu.OpenForEmptyTenantAsync();
            await leftMenu.ClickResourceManagerIconAsync();

            Fixture.Page.Url.Should().Contain(expectedUrl, "Clicking on Resource Manager icon should navigate to the Resource Manager page.");
        }

        [Test]
        public async Task T06_LeftMenu_NavigateToPeople()
        {
            const string expectedUrl = "people";

            var leftMenu = new LeftMenuPage(Fixture.Page);
            await leftMenu.OpenForEmptyTenantAsync();
            await leftMenu.ClickPeopleIconAsync();

            Fixture.Page.Url.Should().Contain(expectedUrl, "Clicking on People icon should navigate to the People page.");
        }

        [Test]
        public async Task T07_LeftMenu_NavigateToCustomerAccounts()
        {
            const string expectedUrl = "customers";

            var leftMenu = new LeftMenuPage(Fixture.Page);
            await leftMenu.OpenForEmptyTenantAsync();
            await leftMenu.ClickCustomerAccountsIconAsync();

            Fixture.Page.Url.Should().Contain(expectedUrl, "Clicking on Customer Accounts icon should navigate to the Customer Accounts page.");
        }

        [Test]
        public async Task T08_LeftMenu_NavigateToSettings()
        {
            const string expectedUrl = "settings";

            var leftMenu = new LeftMenuPage(Fixture.Page);
            await leftMenu.OpenForEmptyTenantAsync();
            await leftMenu.ClickSettingsIconAsync();

            Fixture.Page.Url.Should().Contain(expectedUrl, "Clicking on Settings icon should navigate to the Settings page.");
        }

        [Test]
        public async Task T09_LeftMenu_ClickDashboardFromContextExplorer()
        {
            var expectedUrl = Config.BaseUrl + "?tab=all";

            var leftMenu = new LeftMenuPage(Fixture.Page);
            await leftMenu.OpenForEmptyTenantAsync();
            await leftMenu.ClickContextExplorerIconAsync();
            await leftMenu.ClickDashboardIconAsync();

            Fixture.Page.Url.Should().Be(expectedUrl, "Clicking Dashboard from Context Explorer should navigate to the Dashboard with '?tab=all'.");
        }

        [Test]
        public async Task T10_LeftMenu_ClickUserIcon()
        {
            const string expectedUrl1 = "people/";
            const string expectedUrl2 = "?tab=contexts&contextsTab=all";

            var leftMenu = new LeftMenuPage(Fixture.Page);
            await leftMenu.OpenForEmptyTenantAsync();
            await leftMenu.ClickUserIconAsync();

            var actualUrl = Fixture.Page.Url;
            actualUrl.Should().Contain(expectedUrl1, "Clicking the user icon should navigate to a people URL.");
            actualUrl.Should().Contain(expectedUrl2, "Clicking the user icon should open contexts tab with all contexts selected.");
        }

        [Test]
        public async Task T11_LeftMenu_ClickLogOut()
        {
            const string expectedUrl = "login";
            const string expectedTitle = "Login";

            var leftMenu = new LeftMenuPage(Fixture.Page);
            await leftMenu.OpenForEmptyTenantAsync();
            await leftMenu.ClickLogOutIconAsync();

            Fixture.Page.Url.Should().Contain(expectedUrl, "Clicking on Log out icon should navigate to the Login page.");

            var loginPage = new LoginPage(Fixture.Page);
            var actualTitle = await loginPage.GetTitleAsync();
            actualTitle.Should().Be(expectedTitle, "Login page title should be displayed after logging out.");
        }
    }
}
