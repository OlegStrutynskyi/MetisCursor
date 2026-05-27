using MetisPlaywright.Utils;
using Microsoft.Playwright;
using System.Threading.Tasks;

namespace MetisPlaywright.Pages
{
    public class SupportPage : BasePage
    {
        public SupportPage(IPage page) : base(page) { }

        private ILocator PageTitle => Page.Locator("//h1").First;

        public async Task GetSupportPage()
        {
            await Page.GotoAsync(Config.BaseUrl + "/support");
            await PageTitle.WaitForAsync();
        }

        public async Task<string> GetPageTitle()
        {
            await PageTitle.WaitForAsync();
            return (await PageTitle.TextContentAsync()) ?? string.Empty;
        }
    }
}
