using System;
using System.Collections.Generic;
using System.ComponentModel;
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

        

        public async Task<CollectionResponse> GetCollectionAsync(CollectionRequest request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            XDocument xdoc = await GetXDocumentAsync(request.RelativeUrl).ConfigureAwait(false);

            Collection collection = new Collection();
            collection.PublishDate = xdoc.Root.AttributeValueAsDateTime("pubdate");
            collection.Items = (from item in xdoc.Descendants("item")
                                let stats = item.Element("stats")
                                let rating = stats?.Element("rating")
                                let ranks = rating.Descendants("rank")
                                let status = item.Element("status")
                                select new Collection.Item
                                {
                                    ObjectType = item.AttributeValue("objecttype"),
                                    ObjectId = item.AttributeValue("objectid"),
                                    SubType = item.AttributeValue("subtype"),
                                    CollectionId = item.AttributeValue("collid"),
                                    Name = item.ElementValue("name"),
                                    YearPublished = item.ElementValue("yearpublished"),
                                    Image = item.ElementValue("image"),
                                    Thumbnail = item.ElementValue("thumbnail"),
                                    Stats = new Collection.Stats
                                    {
                                        MinPlayers = stats.AttributeValueAsInt32("minplayers"),
                                        MaxPlayers = stats.AttributeValueAsInt32("maxplayers"),
                                        MinPlayTime = stats.AttributeValueAsInt32("minplaytime"),
                                        MaxPlayTime = stats.AttributeValueAsInt32("maxplaytime"),
                                        PlayingTime = stats.AttributeValueAsInt32("playingtime"),
                                        NumOwned = stats.AttributeValueAsInt32("numowned"),
                                        Rating = new Collection.Rating
                                        {
                                            Value = rating.AttributeValueAsNullableInt32("value"),
                                            UsersRated = rating.Element("usersrated").AttributeValueAsNullableInt32("value"),
                                            Average = rating.Element("average").AttributeValueAsDouble("value"),
                                            BayesAverage = rating.Element("bayesaverage").AttributeValueAsDouble("value"),
                                            StandardDeviation = rating.Element("stddev").AttributeValueAsNullableDouble("value"),
                                            Median = rating.Element("median").AttributeValueAsNullableInt32("value"),
                                            Ranks = (from rank in ranks
                                                     select new Collection.Rank
                                                     {
                                                         Type = rank.AttributeValue("type"),
                                                         Id = rank.AttributeValueAsInt32("id"),
                                                         Name = rank.AttributeValue("name"),
                                                         FriendlyName = rank.AttributeValue("friendlyname"),
                                                         Value = rank.AttributeValueAsNullableInt32("value"),
                                                         BayesAverage = rank.AttributeValueAsDouble("bayesaverage")
                                                     }).ToList()
                                        }
                                    },
                                    Status = new Collection.Status
                                    {
                                        Owned = status.AttributeValueAsBoolean("own"),
                                        PreviouslyOwned = status.AttributeValueAsBoolean("prevowned"),
                                        ForTrade = status.AttributeValueAsBoolean("fortrade"),
                                        Want = status.AttributeValueAsBoolean("want"),
                                        WantToPlay = status.AttributeValueAsBoolean("wanttoplay"),
                                        WantToBuy = status.AttributeValueAsBoolean("wanttobuy"),
                                        Wishlist = status.AttributeValueAsBoolean("wishlist"),
                                        Preordered = status.AttributeValueAsBoolean("preordered"),
                                        LastModified = status.AttributeValueAsDateTime("lastmodified")
                                    },
                                    NumPlays = item.ElementValueAsInt32("numplays")
                                }).ToList();
            

            CollectionResponse response = new CollectionResponse(collection);


            return response;
        }

        public async Task<UserResponse> GetUserAsync(UserRequest request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            XDocument xdoc = await GetXDocumentAsync(request.RelativeUrl).ConfigureAwait(false);

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

        private async Task<XDocument> GetXDocumentAsync(Uri relativeUri)
        {
            Uri requestUrl = new Uri(BaseUrl, relativeUri);

            for (int retry = 0; retry < 20; retry++)
            {
                HttpRequestMessage httpRequest = new HttpRequestMessage(HttpMethod.Get, requestUrl);
                HttpResponseMessage httpResponse = await this.http.SendAsync(httpRequest).ConfigureAwait(false);

                if (!httpResponse.IsSuccessStatusCode)
                {
                    // error occurred handle it
                    throw new Exception("An error occurred.");
                }

                if (httpResponse.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    await Task.Delay(500).ConfigureAwait(false);
                    continue;
                }

                return await httpResponse.Content.ReadAsXDocumentAsync().ConfigureAwait(false);
            }

            throw new Exception("Retries exhausted");
        }

        private string GetAttributeValue(XElement element, string attributeName = "value")
        {
            return element?.Attribute(attributeName)?.Value;
        }

        
    }
}
