using FluentAssertions;
using MetisPlaywright.Pages;
using MetisPlaywright.Utils;

namespace MetisPlaywright.Tests
{
    public class ContextExplorerTests : BaseTest
    {
        [Test]
        public async Task T01_ContextExplorer_DefaultView()
        {
            const string expectedTitle = "Contexts";
            const string expectedGridEmptyMessage = "No contexts found.";
            const string expectedDetailsEmptyMessage = "Select a context from the tree to view details";

            var contextExplorerPage = new ContextExplorerPage(Fixture.Page);
            await contextExplorerPage.OpenForEmptyTenantAsync();
            var actualTitle = await contextExplorerPage.GetContextExplorerPageTitleAsync();
            actualTitle.Should().Be(expectedTitle, "Context Explorer page title is not correct.");
            await contextExplorerPage.ExpectDefaultControlsVisibleAsync();

            await contextExplorerPage.GetGridRecordsCountAsync();
            var actualGridEmptyMessage = await contextExplorerPage.GetGridEmptyMessageTextAsync();
            actualGridEmptyMessage.Trim().Should().Be(expectedGridEmptyMessage, "Grid empty message is not correct.");

            var actualDetailsEmptyMessage = await contextExplorerPage.GetDetailsEmptyMessageTextAsync();
            actualDetailsEmptyMessage.Trim().Should().Be(expectedDetailsEmptyMessage, "Details empty message is not correct.");
        }

        [Test]
        public async Task T02_ContextExplorer_ClickCreateContext()
        {
            const string expectedTitle = "Builder";
            var contextExplorerPage = new ContextExplorerPage(Fixture.Page);
            await contextExplorerPage.OpenForAutoTests1Async();
            var builderPage = await contextExplorerPage.ClickCreateNewContextBtnAsync();
            var actualTitle = await builderPage.GetPageTitleTextAsync();
            actualTitle.Should().Be(expectedTitle, "Builder page title is not correct after clicking Create New Context button.");
        }

        [Test]
        public async Task T03_ContextExplorer_ClickCreateCustomerAccount()
        {
            const string expectedTitle = "Create New Customer Account";
            var contextExplorerPage = new ContextExplorerPage(Fixture.Page);
            await contextExplorerPage.OpenForAutoTests1Async();
            var customerAccountPage = await contextExplorerPage.ClickCreateNewCustomerAccountBtnAsync();
            var actualTitle = await customerAccountPage.GetPageTitleTextAsync();
            actualTitle.Should().Be(expectedTitle, "Create Customer Account page title is not correct after clicking Create New Customer Account button.");
        }

        [Test]
        public async Task T04_ContextExplorer_LabelFilter()
        {
            var expectedLabel = $"{Config.DefaultCoreLabel}";
            var contextExplorerPage = new ContextExplorerPage(Fixture.Page);
            await contextExplorerPage.OpenForAutoTests1Async();
            var labelOptions = await contextExplorerPage.GetLabelFilterOptionsAsync();
            labelOptions.Should().Contain(expectedLabel, "Label filter dropdown should contain the core label.");
        }

        [Test]
        public async Task T05_ContextExplorer_CreateContext()
        {
            const string expectedName = "AutoTests Context 1";
            var contextExplorerPage = new ContextExplorerPage(Fixture.Page);
            var contextRepository = new Neo4jRepository();
            await contextRepository.DeleteContextByNameAsync(expectedName); // Ensure the test Context does not already exist in the database before running the test

            try
            {
                await contextExplorerPage.OpenForAutoTests1Async();
                var numberBefore = await contextExplorerPage.GetGridRecordsCountAsync();
                var builderPage = await contextExplorerPage.ClickCreateNewContextBtnAsync();
                var contextSettingsPage = await builderPage.ClickCreateNewContextBtnAsync();
                await contextSettingsPage.FillContextTitleAsync(expectedName);
                await contextSettingsPage.FillContextDescriptionAsync(Config.AutoTestsContext1Description);
                await contextSettingsPage.SelectCustomerAccountRandomlyAsync();
                await contextSettingsPage.ClickLabelsTabAsync();
                await contextSettingsPage.SelectJobLabelAsync();
                await contextSettingsPage.ClickCreateAsync();

                var leftmenuPage = new LeftMenuPage(Fixture.Page);
                await leftmenuPage.ClickContextExplorerIconAsync();
                var contextExplorerPageAfter = new ContextExplorerPage(Fixture.Page);
                var numberAfter = await contextExplorerPageAfter.GetGridRecordsCountAsync();
                var contextNames = await contextExplorerPageAfter.GetContextNamesAsync();
                numberAfter.Should().Be(numberBefore + 1, "Total number of Contexts should increase by 1 after creating a new Context.");
                contextNames.Should().Contain(expectedName, "Newly created Context should be present in the grid.");
            }
            finally
            {
                await contextRepository.DeleteContextByNameAsync(expectedName);
            }
        }

        [Test]
        public async Task T06_ContextExplorer_ShowChildContext1()
        {
            var contextExplorerPage = new ContextExplorerPage(Fixture.Page);
            await contextExplorerPage.OpenForAutoTests1Async();
            await contextExplorerPage.GetGridRecordsCountAsync();

            await contextExplorerPage.ExpectContextHiddenInGridAsync(Config.AutoTestsContext1Child1);

            await contextExplorerPage.ClickGridAutoTestContext1ArrowAsync();
            await contextExplorerPage.ExpectContextVisibleInGridAsync(Config.AutoTestsContext1Child1);

            await contextExplorerPage.ClickGridAutoTestContext1ArrowAsync();
            await contextExplorerPage.ExpectContextHiddenInGridAsync(Config.AutoTestsContext1Child1);
        }

        [Test]
        public async Task T07_ContextExplorer_Grid_AutoTestContext1_View()
        {
            const string expectedStatus = "Draft";

            var contextExplorerPage = new ContextExplorerPage(Fixture.Page);
            await contextExplorerPage.OpenForAutoTests1Async();
            await contextExplorerPage.GetGridRecordsCountAsync();
            await contextExplorerPage.ExpectGridAutoTestContext1RowVisibleAsync(expectedStatus);
        }

        [Test]
        public async Task T08_ContextExplorer_Details_Context1()
        {
            var contextExplorerPage = new ContextExplorerPage(Fixture.Page);
            await contextExplorerPage.OpenForAutoTests1Async();
            await contextExplorerPage.GetGridRecordsCountAsync();

            var gridStatus = (await contextExplorerPage.GetGridAutoTestContext1StatusTextAsync()).Trim();
            await contextExplorerPage.ClickGridAutoTestContext1NameAsync();

            var actualName = await contextExplorerPage.GetDetailsContextNameTextAsync();
            actualName.Trim().Should().Be(Config.AutoTestsContext1, "Details context name is not correct.");

            await contextExplorerPage.ExpectDetailsActionButtonsVisibleAsync();

            var actualStatus = await contextExplorerPage.GetDetailsContextStatusTextAsync();
            actualStatus.Trim().Should().Be(gridStatus, "Details status should match the grid status.");

            var actualLabel = await contextExplorerPage.GetDetailsContextLabelTextAsync();
            actualLabel.Trim().Should().Be(Config.DefaultCoreLabel, "Details label is not correct.");
        }

        [Test]
        public async Task T09_ContextExplorer_Details_ClickOpen()
        {
            const string expectedOverviewTitle = "Context Overview";

            var contextExplorerPage = new ContextExplorerPage(Fixture.Page);
            await contextExplorerPage.OpenForAutoTests1Async();
            await contextExplorerPage.GetGridRecordsCountAsync();

            var gridContextName = (await contextExplorerPage.GetGridAutoTestContext1NameTextAsync()).Trim();
            await contextExplorerPage.ClickGridAutoTestContext1NameAsync();

            var contextOverviewPage = await contextExplorerPage.ClickDetailsOpenBtnAndOpenContextOverviewAsync();

            var actualOverviewTitle = await contextOverviewPage.GetContextOverviewTitleAsync();
            actualOverviewTitle.Should().Be(expectedOverviewTitle, "Context Overview page title is not correct.");

            var actualContextName = (await contextOverviewPage.GetContextNameAsync()).Trim();
            actualContextName.Should().Be(gridContextName, "Context Overview name should match the grid name.");
        }

        [Test]
        public async Task T10_ContextExplorer_Details_ClickAddChild()
        {
            const string expectedBuilderTitle = "Builder";

            var contextExplorerPage = new ContextExplorerPage(Fixture.Page);
            await contextExplorerPage.OpenForAutoTests1Async();
            await contextExplorerPage.GetGridRecordsCountAsync();

            var gridContextName = (await contextExplorerPage.GetGridAutoTestContext1NameTextAsync()).Trim();
            await contextExplorerPage.ClickGridAutoTestContext1NameAsync();

            var builderPage = await contextExplorerPage.ClickDetailsAddChildBtnAndOpenBuilderAsync();

            var actualBuilderTitle = await builderPage.GetPageTitleTextAsync();
            actualBuilderTitle.Should().Be(expectedBuilderTitle, "Builder page title is not correct.");

            var actualContextName = (await builderPage.GetNameTextAsync()).Trim();
            actualContextName.Should().Be(gridContextName, "Builder context name should match the grid name.");
        }

        [Test]
        public async Task T11_ContextExplorer_Details_ClickAddDependency()
        {
            const string expectedModalTitle = "Add Dependency";

            var contextExplorerPage = new ContextExplorerPage(Fixture.Page);
            await contextExplorerPage.OpenForAutoTests1Async();
            await contextExplorerPage.GetGridRecordsCountAsync();
            await contextExplorerPage.ClickGridAutoTestContext1NameAsync();

            await contextExplorerPage.ClickDetailsAddDependencyBtnAsync();

            var actualModalTitle = await contextExplorerPage.GetDetailsAddDependencyModalTitleTextAsync();
            actualModalTitle.Trim().Should().Be(expectedModalTitle, "Add Dependency modal title is not correct.");
        }
    }
}
