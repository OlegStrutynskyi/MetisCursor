using FluentAssertions;
using MetisPlaywright.Pages;
using MetisPlaywright.Utils;

namespace MetisPlaywright.Tests
{
    public class AssetTypesTests : BaseTest
    {
        [Test]
        public async Task T01_AssetTypes_DefaultView()
        {
            const string expectedTitle = "Asset Types";
            const string expectedMessage = "No asset type records";
            var assetTypesPage = new AssetTypesPage(Fixture.Page);
            await assetTypesPage.OpenForEmptyTenantAsync();
            var actualTitle = await assetTypesPage.GetPageTitleTextAsync();
            var actualMessage = await assetTypesPage.GetGridEmptyMessageAsync();
            actualTitle.Should().Be(expectedTitle, "Asset Types page title is not correct.");
            actualMessage.Should().Be(expectedMessage, "Grid empty message is not correct when there are no Asset Types.");
        }

        [Test]
        public async Task T02_AssetTypes_OpensSettingsModal_WhenCreateClicked()
        {
            var assetTypesPage = new AssetTypesPage(Fixture.Page);
            await assetTypesPage.OpenForAutoTests1Async();
            await assetTypesPage.OpenCreateAssetTypeModalAsync();
            await assetTypesPage.ExpectAssetTypeSettingsModalVisibleAsync();
        }

        [Test]
        public async Task T03_AssetTypes_AssetTypeSettingsModal_View()
        {
            const string expectedTitle = "Asset Type Settings";
            var assetTypesPage = new AssetTypesPage(Fixture.Page);
            await assetTypesPage.OpenForAutoTests1Async();
            await assetTypesPage.OpenCreateAssetTypeModalAsync();
            var actualTitle = await assetTypesPage.GetAssetTypeSettingsTitleAsync();
            actualTitle.Should().Be(expectedTitle, "Asset Type Settings modal title is not correct.");
            await assetTypesPage.ExpectAssetTypeSettingsControlsVisibleAsync();
        }

        [Test]
        public async Task T04_AssetTypes_AssetTypeSettingsModal_EmptyField()
        {
            const string expectedNameError = "'Name' is required";
            var assetTypesPage = new AssetTypesPage(Fixture.Page);
            await assetTypesPage.OpenForAutoTests1Async();
            await assetTypesPage.OpenCreateAssetTypeModalAsync();
            await assetTypesPage.ClickAssetTypeSettingsSaveAsync();
            await assetTypesPage.ExpectAssetTypeSettingsNameErrorContainsAsync(expectedNameError);
        }

        [Test]
        public async Task T05_AssetTypes_CreateAssetType()
        {
            const string expectedName = "AutoTests Asset Type 1";
            var assetTypesPage = new AssetTypesPage(Fixture.Page);
            var assetTypeRepository = new Neo4jRepository();
            await assetTypeRepository.DeleteAssetTypeByNameAsync(expectedName); // Ensure the test Asset Type does not already exist in the database before running the test

            try
            {
                await assetTypesPage.OpenForAutoTests1Async();
                var numberBefore = await assetTypesPage.GetGridTotalRecordsAsync();
                await assetTypesPage.OpenCreateAssetTypeModalAsync();
                await assetTypesPage.FillAssetTypeSettingsNameAsync(expectedName);
                await assetTypesPage.ClickAssetTypeSettingsSaveAsync();
                var numberAfter = await assetTypesPage.GetGridTotalRecordsAsync();
                var assetTypeNames = await assetTypesPage.GetAssetTypeNamesAsync();
                numberAfter.Should().Be(numberBefore + 1, "Total number of Asset Types should increase by 1 after creating a new Asset Type.");
                assetTypeNames.Should().Contain(expectedName, "Newly created Asset Type should be present in the grid.");
            }
            finally
            {
                await assetTypeRepository.DeleteAssetTypeByNameAsync(expectedName);
            }
        }
    }
}
