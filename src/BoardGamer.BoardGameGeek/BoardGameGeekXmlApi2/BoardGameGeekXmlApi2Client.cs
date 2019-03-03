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
        private readonly HttpClient http;

        public BoardGameGeekXmlApi2Client(HttpClient http)
        {
            this.http = http;
        }

        public async Task<User> GetUserAsync(string username, 
            bool buddies = false, 
            bool guilds = false, 
            bool hot = false, 
            bool top = false, 
            string domain = null, 
            int page = 1)
        {
            if (String.IsNullOrWhiteSpace(username))
            {
                throw new ArgumentException(nameof(username));
            }

            Uri requestUri = Api.User(username, buddies, guilds, hot, top, domain, page);

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, requestUri);
            HttpResponseMessage response = await this.http.SendAsync(request).ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
            {
                // error occured
                // handle it
            }

            XDocument xdoc;
            using (var contentStream = await response.Content.ReadAsStreamAsync())
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
                                          Buddies = (from xbuddy in xuser.Descendants("buddy")
                                                     select new Buddy
                                                     {
                                                        Id = GetAttributeValue(xbuddy, "id"),
                                                        Name = GetAttributeValue(xbuddy, "name")
                                                     }).ToList(),

                                          Guilds = (from xguild in xuser.Descendants("guild")
                                                    select new Guild
                                                    {
                                                        Id = GetAttributeValue(xguild, "id"),
                                                        Name = GetAttributeValue(xguild, "name")
                                                    }).ToList(),

                                          Top = (from xtop in xdoc.XPathSelectElements("/user/top/item")
                                                 select new TopItem
                                                 {
                                                     Rank = GetAttributeValue(xtop,"rank"),
                                                     Type = GetAttributeValue(xtop,"type"),
                                                     Id = GetAttributeValue(xtop, "id"),
                                                     Name = GetAttributeValue(xtop, "name")
                                                 }).ToList()
                                      };

            User user = users.FirstOrDefault();
            
            return user;
        }

        private string GetAttributeValue(XElement element, string attributeName = "value")
        {
            return element.Attribute(attributeName).Value;
        }
    }

    internal class Api
    {
        public static readonly Uri BaseUrl = new Uri("https://www.boardgamegeek.com/xmlapi2/");

        public static Uri User(string username, bool buddies, bool guilds, bool hot, bool top, string domain, int page)
        {
            List<string> args = new List<string>();

            args.Add($"name={username}");

            if (buddies) args.Add("buddies=1");
            if (guilds) args.Add("guilds=1");
            if (hot) args.Add("hot=1");
            if (top) args.Add("top=1");
            if (!String.IsNullOrWhiteSpace(domain)) args.Add($"domain={domain}");
            if (page > 1) args.Add($"page={page}");

            string queryArgs = String.Join("&", args);

            return new Uri(BaseUrl, $"user?{queryArgs}");
        }
    }
}
