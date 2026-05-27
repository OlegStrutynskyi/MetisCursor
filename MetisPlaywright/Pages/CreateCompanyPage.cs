using MetisPlaywright.Utils;
using Microsoft.Playwright;

namespace MetisPlaywright.Pages
{
    public class CreateCompanyPage : BasePage
    {
        public CreateCompanyPage(IPage page) : base(page) { }

        //endtest.io mailbox renders emails newest-first (proven empirically: when the count
        //grew from 13 to 14 after POST /setup, .Last kept returning the same stale token, so
        //.First is the freshly-arrived invitation).
        private ILocator AllFinishLinks => Page.Locator("//div[contains(text(),'Finish Setting Up Your Account!')]");
        private ILocator AllFinishSetupBtns => Page.Locator("//a[normalize-space()='Finish Setup']");
        private ILocator FinishLink => AllFinishLinks.First;
        private ILocator FinishSetupBtn => AllFinishSetupBtns.First;

        public Task ClickFinishLinkAsync() => FinishLink.ClickAsync();

        public async Task<string> GetFinishSetupLinkAsync()
        {
            var href = await FinishSetupBtn.GetAttributeAsync("href");
            return href ?? throw new InvalidOperationException(
                "Finish Setup link is missing the 'href' attribute on the mailbox page.");
        }

        public Task<int> GetSetupEmailCountAsync() => AllFinishLinks.CountAsync();

        /// <summary>
        /// Polls the mailbox until a new "Finish Setting Up Your Account!" email appears (i.e.
        /// the email count grows above <paramref name="baselineCount"/>). This is the timestamp
        /// filter: endtest.io renders emails oldest-first, so a count increment guarantees that
        /// .Last now points to the freshly-arrived invitation rather than a stale one from a
        /// previous run.
        /// </summary>
        public async Task WaitForNewSetupEmailAsync(string mailboxUrl, int baselineCount, TimeSpan? timeout = null, TimeSpan? pollInterval = null)
        {
            var timeoutValue = timeout ?? TimeSpan.FromMinutes(2);
            var pollDelay = pollInterval ?? TimeSpan.FromSeconds(3);
            var deadline = DateTime.UtcNow + timeoutValue;
            int currentCount = baselineCount;

            while (DateTime.UtcNow < deadline)
            {
                await Page.GotoAsync(mailboxUrl);
                await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
                currentCount = await GetSetupEmailCountAsync();
                if (currentCount > baselineCount)
                    return;

                await Page.WaitForTimeoutAsync((float)pollDelay.TotalMilliseconds);
            }

            throw new TimeoutException(
                $"Mailbox did not receive a new 'Finish Setting Up Your Account!' email within {timeoutValue.TotalSeconds:0}s " +
                $"(baseline: {baselineCount}, last observed: {currentCount}).");
        }

        public async Task<IAPIRequestContext> CreateRequestContextAsync()
        {
            //NOTE: spawns its own IPlaywright instance because the API request context is
            //returned to the caller and must outlive this method. The handle is intentionally
            //not disposed here; the caller owns the lifetime of the returned context.
            var playwright = await Playwright.CreateAsync();
            return await playwright.APIRequest.NewContextAsync(new APIRequestNewContextOptions
            {
                BaseURL = Config.BaseUrlAPI,
                IgnoreHTTPSErrors = true,
            });
        }
    }
}
