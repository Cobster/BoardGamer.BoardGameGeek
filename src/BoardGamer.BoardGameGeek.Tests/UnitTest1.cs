using BoardGamer.BoardGameGeek.BoardGameGeekXmlApi2;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace BoardGamer.BoardGameGeek.Tests
{
    public class BoardGameGeekClientTests
    {
        [Fact]
        public async Task Should_retrieve_user_by_boardgamegeek_username()
        {
            HttpClient http = new HttpClient();

            IBoardGameGeekXmlApi2Client bgg = new BoardGameGeekXmlApi2Client(http);

            User user = await bgg.GetUserAsync("jakefromstatefarm", buddies: true);

            Assert.NotNull(user);
            Assert.Equal("1266617", user.Id);
            Assert.Equal("jakefromstatefarm", user.Name);
            Assert.Equal("Jake", user.FirstName);
            Assert.Equal("Bruun", user.LastName);
            Assert.Equal("2016", user.YearRegistered);
            Assert.Equal("Oregon", user.StateOrProvince);
            Assert.Equal("United States", user.Country);
            Assert.Equal(4, user.Buddies.Count);

        }
    }
}
