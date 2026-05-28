using FluentAssertions;
using MetisPlaywright.Pages;
using MetisPlaywright.Utils;

namespace MetisPlaywright.Tests
{
    public class DashboardTests : BaseTest
    {
        [Test]
        public async Task T01_Dashboard_DefaultView()
        {
            string expectedTitle = $"{Config.DefaultCoreLabel}s Dashboard";
            const string expectedMessage = "No context records";

            var dashboardPage = new DashboardPage(Fixture.Page);
            await dashboardPage.OpenForEmptyTenantAsync();

            var actualTitle = await dashboardPage.GetDashboardTitleAsync();
            await dashboardPage.ExpectDefaultControlsVisibleAsync();
            var actualMessage = await dashboardPage.GetGridEmptyMessageAsync();

            actualTitle.Should().Be(expectedTitle, "The Dashboard title is not correct.");
            actualMessage.Should().Be(expectedMessage, "The message for empty grid is not correct.");
        }

        [Test]
        public async Task T02_Dashboard_CheckRecordsCount_EmptyGrid()
        {
            const int expectedCount = 0;

            var dashboardPage = new DashboardPage(Fixture.Page);
            await dashboardPage.OpenForEmptyTenantAsync();

            var totalValue = await dashboardPage.GetTotalCounterValueAsync();
            var inProgressValue = await dashboardPage.GetInProgressCounterValueAsync();
            var delayedValue = await dashboardPage.GetDelayedCounterValueAsync();
            var completedValue = await dashboardPage.GetCompletedCounterValueAsync();
            var contextNamesCount = await dashboardPage.GetAllContextNamesCountAsync();
            var totalItems = await dashboardPage.GetGridTotalRecordsAsync();

            totalValue.Should().Be(expectedCount, "Total counter value should be 0 for an empty grid.");
            inProgressValue.Should().Be(expectedCount, "In Progress counter value should be 0 for an empty grid.");
            delayedValue.Should().Be(expectedCount, "Delayed counter value should be 0 for an empty grid.");
            completedValue.Should().Be(expectedCount, "Completed counter value should be 0 for an empty grid.");
            totalItems.Should().Be(contextNamesCount, "Total number of records should match the count of context names.");
            totalItems.Should().Be(0, "Total number of records should be 0 for an empty grid.");
        }

        [Test]
        public async Task T03_Dashboard_CheckRecordsCount_NonEmptyGrid()
        {
            var dashboardPage = new DashboardPage(Fixture.Page);
            await dashboardPage.OpenForAutoTests1Async();

            var totalValue = await dashboardPage.GetTotalCounterValueAsync();
            var contextNamesCount = await dashboardPage.GetAllContextNamesCountAsync();
            var totalItems = await dashboardPage.GetGridTotalRecordsAsync();

            totalValue.Should().BeGreaterThan(0, "Total counter value should be greater than 0 for a non-empty grid.");
            totalItems.Should().Be(contextNamesCount, "Total number of records should match the count of context names.");
            totalItems.Should().BeGreaterThan(0, "Total number of records should be greater than 0 for a non-empty grid.");
        }

        [Test]
        public async Task T04_Dashboard_ClickShowChildren()
        {
            var dashboardPage = new DashboardPage(Fixture.Page);
            await dashboardPage.OpenForAutoTests1Async();

            await dashboardPage.ExpectContextHiddenInGridAsync(Config.AutoTestsContext1Child1);

            await dashboardPage.ClickShowChildrenBtnAsync();
            await dashboardPage.ExpectContextVisibleInGridAsync(Config.AutoTestsContext1Child1);

            await dashboardPage.ClickShowChildrenBtnAsync();
            await dashboardPage.ExpectContextHiddenInGridAsync(Config.AutoTestsContext1Child1);
        }

        [Test]
        public async Task T05_Dashboard_ClickContext1()
        {
            const string expectedOverviewTitle = "Context Overview";

            var dashboardPage = new DashboardPage(Fixture.Page);
            await dashboardPage.OpenForAutoTests1Async();

            var contextOverviewPage = await dashboardPage.ClickGridContextAsync(Config.AutoTestsContext1);

            var actualOverviewTitle = await contextOverviewPage.GetContextOverviewTitleAsync();
            actualOverviewTitle.Should().Be(expectedOverviewTitle, "Context Overview page title is not correct.");

            var actualContextName = (await contextOverviewPage.GetContextNameAsync()).Trim();
            actualContextName.Should().Be(Config.AutoTestsContext1, "Context Overview name is not correct.");
        }
    }
}
