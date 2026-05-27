using MetisPlaywright.Utils;
using Microsoft.Playwright;
using System.Threading.Tasks;

namespace MetisPlaywright.Pages
{
    public class OptimizationsPage : BasePage
    {
        public OptimizationsPage(IPage page) : base(page) { }

        private ILocator PageTitle => Page.Locator("//h1").First;

        public async Task GetOptimizationsPage()
        {
            await Page.GotoAsync(Config.BaseUrl + "/optimizations");
            await PageTitle.WaitForAsync();
        }

        public async Task<string> GetPageTitle()
        {
            await PageTitle.WaitForAsync();
            return (await PageTitle.TextContentAsync()) ?? string.Empty;
        }
    }
}
