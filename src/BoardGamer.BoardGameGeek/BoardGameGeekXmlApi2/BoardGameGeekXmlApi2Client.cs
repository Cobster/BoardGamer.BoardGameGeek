using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.XPath;

namespace BoardGamer.BoardGameGeek.BoardGameGeekXmlApi2
{

    public class BoardGameGeekXmlApi2Client : IBoardGameGeekXmlApi2Client
    {
        public static readonly Uri BaseUrl = new Uri("https://www.boardgamegeek.com/xmlapi2/");

        private readonly HttpClient http;

        public BoardGameGeekXmlApi2Client(HttpClient http)
        {
            this.http = http;
        }

        public async Task<UserResponse> GetUserAsync(UserRequest request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            Uri requestUri = new Uri(BaseUrl, request.Url); //Api.User(username, buddies, guilds, hot, top, domain, page);

            HttpRequestMessage httpRequest = new HttpRequestMessage(HttpMethod.Get, requestUri);
            HttpResponseMessage httpResponse = await this.http.SendAsync(httpRequest).ConfigureAwait(false);

            if (!httpResponse.IsSuccessStatusCode)
            {
                // error occured
                // handle it
            }

            XDocument xdoc;
            using (var contentStream = await httpResponse.Content.ReadAsStreamAsync())
            {
                xdoc = XDocument.Load(contentStream);
            }

            IEnumerable<User> users = from xuser in xdoc.Descendants("user")
                                      select new User
                                      {
                                          Id = xuser.Attribute("id")?.Value,
                                          Name = xuser.Attribute("name").Value,
                                          FirstName = GetAttributeValue(xuser.Element("firstname")),
                                          LastName = GetAttributeValue(xuser.Element("lastname")),
                                          AvatarLink = GetAttributeValue(xuser.Element("avatarlink")),
                                          YearRegistered = GetAttributeValue(xuser.Element("yearregistered")),
                                          LastLogin = GetAttributeValue(xuser.Element("lastlogin")),
                                          StateOrProvince = GetAttributeValue(xuser.Element("stateorprovince")),
                                          Country = GetAttributeValue(xuser.Element("country")),
                                          WebAddress = GetAttributeValue(xuser.Element("webaddress")),
                                          XboxAccount = GetAttributeValue(xuser.Element("xboxaccount")),
                                          WiiAccount = GetAttributeValue(xuser.Element("wiiaccount")),
                                          PsnAccount = GetAttributeValue(xuser.Element("psnaccount")),
                                          BattleNetAccount = GetAttributeValue(xuser.Element("battlenetaccount")),
                                          SteamAccount = GetAttributeValue(xuser.Element("steamaccount")),
                                          TradeRating = GetAttributeValue(xuser.Element("traderating")),
                                          MarketRating = GetAttributeValue(xuser.Element("marketrating")),
                                          Buddies = (from buddy in xuser.Descendants("buddy")
                                                     select new Buddy
                                                     {
                                                        Id = GetAttributeValue(buddy, "id"),
                                                        Name = GetAttributeValue(buddy, "name")
                                                     }).ToList(),

                                          Guilds = (from guild in xuser.Descendants("guild")
                                                    select new Guild
                                                    {
                                                        Id = GetAttributeValue(guild, "id"),
                                                        Name = GetAttributeValue(guild, "name")
                                                    }).ToList(),

                                          Top = (from item in xdoc.XPathSelectElements("/user/top/item")
                                                 select new Item
                                                 {
                                                     Rank = GetAttributeValue(item,"rank"),
                                                     Type = GetAttributeValue(item,"type"),
                                                     Id = GetAttributeValue(item, "id"),
                                                     Name = GetAttributeValue(item, "name")
                                                 }).ToList(),
                                          Hot = (from item in xdoc.XPathSelectElements("/user/hot/item")
                                                 select new Item
                                                 {
                                                     Rank = GetAttributeValue(item, "rank"),
                                                     Type = GetAttributeValue(item, "type"),
                                                     Id = GetAttributeValue(item, "id"),
                                                     Name = GetAttributeValue(item, "name")
                                                 }).ToList()
                                      };

            User user = users.FirstOrDefault();

            UserResponse response = new UserResponse(user);

            return response;
        }
        
        private string GetAttributeValue(XElement element, string attributeName = "value")
        {
            return element.Attribute(attributeName).Value;
        }
    }
}
