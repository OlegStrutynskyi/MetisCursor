using FluentAssertions;
using MetisPlaywright.Pages;
using MetisPlaywright.Utils;

namespace MetisPlaywright.Tests
{
    public class ContextOverviewTests : BaseTest
    {
        [Test]
        public async Task T01_ContextOverview_DefaultView()
        {
            const string expectedTitle = "Context Overview";
            var expectedContextName = $"{Config.AutoTestsContext1}";
            var expectedCustomerName = $"{Config.AutoTestsCustomer1}";

            var contextOverviewPage = new ContextOverviewPage(Fixture.Page);
            await contextOverviewPage.OpenForContextAsync(expectedContextName);

            var actualTitle = await contextOverviewPage.GetContextOverviewTitleAsync();
            var actualContextName = await contextOverviewPage.GetContextNameAsync();
            var actualCustomerName = await contextOverviewPage.GetCustomerNameAsync();

            actualTitle.Should().Be(expectedTitle, "The page title is not correct.");
            actualContextName.Should().Be(expectedContextName, "The context name in the overview is not correct.");
            actualCustomerName.Should().Be(expectedCustomerName, "The customer name in the overview is not correct.");

            await contextOverviewPage.ExpectDefaultControlsVisibleAsync();
        }

        [Test]
        public async Task T02_ContextOverview_ClickOpenContextBuilder()
        {
            const string expectedBuilderTitle = "Builder";

            var contextOverviewPage = new ContextOverviewPage(Fixture.Page);
            await contextOverviewPage.OpenForContextAsync(Config.AutoTestsContext1);

            var contextName = (await contextOverviewPage.GetContextNameAsync()).Trim();
            await contextOverviewPage.ClickOpenContextBuilderAsync();

            var builderPage = new BuilderPage(Fixture.Page);
            await builderPage.ExpectOpenedAsync();

            var actualBuilderTitle = await builderPage.GetPageTitleTextAsync();
            actualBuilderTitle.Should().Be(expectedBuilderTitle, "Builder page title is not correct.");

            var actualContextName = (await builderPage.GetNameTextAsync()).Trim();
            actualContextName.Should().Be(contextName, "Builder context name should match Context Overview name.");
        }

        [Test]
        public async Task T03_ContextOverview_CreateChildContext_Flow()
        {
            const string childContextName = "Context 1 Child 1";
            const string expectedOverviewTitle = "Context Overview";
            var contextRepository = new Neo4jRepository();

            await contextRepository.DeleteContextByNameAsync(childContextName);

            try
            {
                var contextOverviewPage = new ContextOverviewPage(Fixture.Page);
                await contextOverviewPage.OpenForContextAsync(Config.AutoTestsContext1);
                var contextSettingsPage = await contextOverviewPage.ClickCreateChildContextAndOpenContextSettingsAsync();

                await contextSettingsPage.FillContextTitleAsync(childContextName);
                await contextSettingsPage.ClickCreateAsync();
                await contextOverviewPage.ExpectContextNameAsync(childContextName);

                var leftMenuPage = new LeftMenuPage(Fixture.Page);
                await leftMenuPage.ClickContextExplorerIconAsync();
                var contextExplorerPage = new ContextExplorerPage(Fixture.Page);

                await leftMenuPage.ClickContextExplorerIconAsync();
                await contextExplorerPage.ClickGridAutoTestContext1NameAsync();
                var parentOverviewPage = await contextExplorerPage.ClickDetailsOpenBtnAndOpenContextOverviewAsync();
                var actualParentOverviewTitle = (await parentOverviewPage.GetContextOverviewTitleAsync()).Trim();
                actualParentOverviewTitle.Should().Be(expectedOverviewTitle, "Parent Context Overview page title is not correct.");

                var actualParentContextName = (await parentOverviewPage.GetContextNameAsync()).Trim();
                actualParentContextName.Should().Be(Config.AutoTestsContext1, "Parent Context Overview name is not correct.");

                await parentOverviewPage.ExpectChildContextVisibleInGridAsync(childContextName);
            }
            finally
            {
                await contextRepository.DeleteContextByNameAsync(childContextName);
            }
        }
    }
}
