using Bunit;
using notifyme.server.Pages;
using Xunit;

namespace notifyme.server.tests.Pages_Tests
{
    public class CounterTests : TestContext
    {
        [Fact]
        public void CounterShouldIncrementWhenSelected()
        {
            // Arrange
            var cut = RenderComponent<Counter>();
            var paraElm = cut.Find("p");

            // Act
            cut.Find("button").Click();
            var paraElmText = paraElm.TextContent;

            // Assert
            paraElmText.MarkupMatches("Current count: 1");
        }
    }
}