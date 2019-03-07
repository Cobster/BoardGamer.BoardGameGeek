﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
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
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            XDocument xdoc = await GetXDocumentAsync(request.RelativeUrl).ConfigureAwait(false);
            CollectionResponse response = Map(xdoc);
            return response;

            #region Helpers

            CollectionResponse Map(XDocument doc)
            {
                return new CollectionResponse(MapItemCollection(xdoc.Root));
            }

            CollectionResponse.ItemCollection MapItemCollection(XElement itemsEl)
            {
                return new CollectionResponse.ItemCollection(
                    MapItems(itemsEl),
                    itemsEl.AttributeValueAsDateTime("pubdate"));
            }

            IEnumerable<CollectionResponse.Item> MapItems(XElement itemsEl)
            {
                return from item in itemsEl.Elements("item")
                       let status = item.Element("status")
                       select new CollectionResponse.Item
                       {
                           ObjectType = item.AttributeValue("objecttype"),
                           ObjectId = item.AttributeValue("objectid"),
                           SubType = item.AttributeValue("subtype"),
                           CollectionId = item.AttributeValue("collid"),
                           Name = item.ElementValue("name"),
                           YearPublished = item.ElementValue("yearpublished"),
                           Image = item.ElementValue("image"),
                           Thumbnail = item.ElementValue("thumbnail"),
                           Stats = MapStats(item.Element("stats")),
                           Status = MapStatus(item.Element("status")),
                           NumPlays = item.ElementValueAsInt32("numplays")
                       };
            }

            CollectionResponse.Stats MapStats(XElement stats)
            {
                if (stats == null)
                {
                    return null;
                }

                XElement rating = stats?.Element("rating");
                IEnumerable<XElement> ranks = rating.Descendants("rank");

                return new CollectionResponse.Stats
                {
                    MinPlayers = stats.AttributeValueAsInt32("minplayers"),
                    MaxPlayers = stats.AttributeValueAsInt32("maxplayers"),
                    MinPlayTime = stats.AttributeValueAsInt32("minplaytime"),
                    MaxPlayTime = stats.AttributeValueAsInt32("maxplaytime"),
                    PlayingTime = stats.AttributeValueAsInt32("playingtime"),
                    NumOwned = stats.AttributeValueAsInt32("numowned"),
                    Rating = new CollectionResponse.Rating
                    {
                        Value = rating.AttributeValueAsNullableInt32(),
                        UsersRated = rating.Element("usersrated").AttributeValueAsNullableInt32(),
                        Average = rating.Element("average").AttributeValueAsDouble(),
                        BayesAverage = rating.Element("bayesaverage").AttributeValueAsDouble(),
                        StandardDeviation = rating.Element("stddev").AttributeValueAsNullableDouble(),
                        Median = rating.Element("median").AttributeValueAsNullableInt32(),
                        Ranks = (from rank in ranks
                                 select new CollectionResponse.Rank
                                 {
                                     Type = rank.AttributeValue("type"),
                                     Id = rank.AttributeValueAsInt32("id"),
                                     Name = rank.AttributeValue("name"),
                                     FriendlyName = rank.AttributeValue("friendlyname"),
                                     Value = rank.AttributeValueAsNullableInt32(),
                                     BayesAverage = rank.AttributeValueAsDouble("bayesaverage")
                                 }).ToList()
                    }
                };
            }

            CollectionResponse.Status MapStatus(XElement status)
            {
                if (status == null)
                {
                    return null;
                }

                return new CollectionResponse.Status
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
                };
            }

            #endregion
        }

        public async Task<ThingResponse> GetThingAsync(ThingRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            XDocument xdoc = await GetXDocumentAsync(request.RelativeUrl).ConfigureAwait(false);
            ThingResponse response = Map(xdoc);
            return response;

            #region Helpers

            ThingResponse Map(XDocument doc)
            {
                return new ThingResponse(MapItems(doc));
            }

            IEnumerable<ThingResponse.Item> MapItems(XDocument document)
            {
                return from item in xdoc.Descendants("item")
                       let versions = item.Element("versions")
                       let videos = item.Element("videos")
                       select new ThingResponse.Item
                       {
                           Id = item.AttributeValueAsInt32("id"),
                           Type = item.AttributeValue("type"),
                           Thumbnail = item.ElementValue("thumbnail"),
                           Image = item.ElementValue("image"),
                           Name = item.Elements("name").FirstOrDefault(x => x.AttributeValue("type") == "primary")?.AttributeValue(),
                           AlternateNames = (from n in item.Elements("name").Where(x => x.AttributeValue("type") != "primary")
                                             select n.AttributeValue()).ToList(),
                           Description = WebUtility.HtmlDecode(item.ElementValue("description")),
                           YearPublished = item.Element("yearpublished").AttributeValueAsNullableInt32(),
                           ReleaseDate = item.Element("releasedate").AttributeValueAsNullableDateTime(),
                           MinPlayers = item.Element("minplayers").AttributeValueAsInt32(),
                           MaxPlayers = item.Element("maxplayers").AttributeValueAsInt32(),
                           PlayingTime = item.Element("playingtime").AttributeValueAsNullableInt32(),
                           MinPlayingTime = item.Element("minplaytime").AttributeValueAsNullableInt32(),
                           MaxPlayingTime = item.Element("maxplaytime").AttributeValueAsNullableInt32(),
                           MinAge = item.Element("minage").AttributeValueAsNullableInt32(),
                           Polls = MapPolls(item).ToList(),
                           Links = MapLinks(item).ToList(),
                           Versions = MapVersions(versions).ToList(),
                           Videos = MapVideoCollection(videos),
                           Statistics = MapStatistics(item.Element("statistics")),
                       };
            }


            IEnumerable<ThingResponse.Poll> MapPolls(XElement thingEl)
            {
                return from poll in thingEl.Elements("poll")
                       let results = poll.Elements("results")
                       select new ThingResponse.Poll
                       {
                           Name = poll.AttributeValue("name"),
                           Title = poll.AttributeValue("title"),
                           TotalVotes = poll.AttributeValueAsInt32("totalvotes"),
                           Results = (from r in results
                                      let res = r.Elements("result")
                                      select new ThingResponse.PollResults
                                      {
                                          NumPlayers = r.AttributeValueAsNullableInt32("numplayers"), // only found on the suggested num players
                                          Results = (from x in res
                                                     select new ThingResponse.PollResult
                                                     {
                                                         Value = x.AttributeValue(),
                                                         NumVotes = x.AttributeValueAsInt32("numvotes")
                                                     }).ToList()
                                      }).ToList()
                       };
            }

            IEnumerable<ThingResponse.Link> MapLinks(XElement thingEl)
            {
                return from link in thingEl.Elements("link")
                       select new ThingResponse.Link
                       {
                           Type = link.AttributeValue("type"),
                           Id = link.AttributeValueAsInt32("id"),
                           Value = link.AttributeValue()
                       };
            }

            IEnumerable<ThingResponse.Version> MapVersions(XElement versionsEl)
            {
                if (versionsEl == null)
                {
                    return Enumerable.Empty<ThingResponse.Version>();
                }

                var versions = new IEnumerable<ThingResponse.Version>[]
                {
                    MapBoardGameVersions(versionsEl),
                    MapRpgItemVersion(versionsEl),
                    MapVideoGameVersion(versionsEl),
                    MapVideoGameCharacterVersion(versionsEl)
                };

                return versions.SelectMany(x => x);
            }

            IEnumerable<ThingResponse.BoardGameVersion> MapBoardGameVersions(XElement versionsEl)
            {
                return from v in versionsEl.Elements("item")
                       where v.Attribute("type").Value == "boardgameversion"
                       select new ThingResponse.BoardGameVersion
                       {
                           Type = v.AttributeValue("type"),
                           Id = v.AttributeValueAsInt32("id"),
                           Thumbnail = v.ElementValue("thumbnail"),
                           Image = v.ElementValue("image"),
                           Name = v.Element("name").AttributeValue(),
                           YearPublished = v.Element("yearpublished").AttributeValueAsInt32(),
                           ProductCode = v.Element("productcode").AttributeValue(),
                           Width = v.Element("width").AttributeValueAsNullableDouble(),
                           Length = v.Element("length").AttributeValueAsNullableDouble(),
                           Depth = v.Element("depth").AttributeValueAsNullableDouble(),
                           Weight = v.Element("weight").AttributeValueAsNullableDouble(),
                           Links = MapLinks(v).ToList()
                       };
            }

            IEnumerable<ThingResponse.RpgItemVersion> MapRpgItemVersion(XElement versionsEl)
            {
                return from v in versionsEl.XPathSelectElements("item[@type='rpgitemversion']")
                       select new ThingResponse.RpgItemVersion
                       {
                           Type = v.AttributeValue("type"),
                           Id = v.AttributeValueAsInt32("id"),
                           Thumbnail = v.ElementValue("thumbnail"),
                           Image = v.ElementValue("image"),
                           Name = v.Element("name").AttributeValue(),
                           YearPublished = v.Element("yearpublished").AttributeValueAsInt32(),
                           Format = v.Element("format").AttributeValue(),
                           ProductCode = v.Element("productcode").AttributeValue(),
                           PageCount = v.Element("pagecount").AttributeValueAsNullableInt32(),
                           Isbn10 = v.Element("isbn10").AttributeValue(),
                           Isbn13 = v.Element("isbn13").AttributeValue(),
                           Width = v.Element("width").AttributeValueAsNullableDouble(),
                           Height = v.Element("height").AttributeValueAsNullableDouble(),
                           Weight = v.Element("weight").AttributeValueAsNullableDouble(),
                           Description = WebUtility.HtmlDecode(v.ElementValue("description")),
                           Links = (from l in v.Elements("link")
                                    select new ThingResponse.Link
                                    {
                                        Type = l.AttributeValue("type"),
                                        Id = l.AttributeValueAsInt32("id"),
                                        Value = l.AttributeValue()
                                    }).ToList()
                       };
            }

            IEnumerable<ThingResponse.VideoGameVersion> MapVideoGameVersion(XElement versionsEl)
            {
                return from v in versionsEl.XPathSelectElements("item[@type='videogameversion']")
                       select new ThingResponse.VideoGameVersion
                       {
                           Type = v.AttributeValue("type"),
                           Id = v.AttributeValueAsInt32("id"),
                           Thumbnail = v.ElementValue("thumbnail"),
                           Image = v.ElementValue("image"),
                           Name = v.Element("name").AttributeValue(),
                           ReleaseDate = v.Element("releasedate").AttributeValueAsDateTime(),
                           Links = (from l in v.Elements("link")
                                    select new ThingResponse.Link
                                    {
                                        Type = l.AttributeValue("type"),
                                        Id = l.AttributeValueAsInt32("id"),
                                        Value = l.AttributeValue()
                                    }).ToList()
                       };
            }

            IEnumerable<ThingResponse.VideoGameCharacterVersion> MapVideoGameCharacterVersion(XElement versionsEl)
            {
                return from v in versionsEl.XPathSelectElements("item[@type='vgcharacterversion']")
                       select new ThingResponse.VideoGameCharacterVersion
                       {
                           Type = v.AttributeValue("type"),
                           Id = v.AttributeValueAsInt32("id"),
                           Thumbnail = v.ElementValue("thumbnail"),
                           Image = v.ElementValue("image")
                       };
            }

            ThingResponse.VideoCollection MapVideoCollection(XElement videosEl)
            {
                if (videosEl == null)
                {
                    return null;
                }

                var videos = from v in videosEl.Elements("video")
                             select new ThingResponse.Video
                             {
                                 Id = v.AttributeValueAsInt32("id"),
                                 Title = v.AttributeValue("title"),
                                 Category = v.AttributeValue("category"),
                                 Language = v.AttributeValue("language"),
                                 Link = v.AttributeValue("link"),
                                 Username = v.AttributeValue("username"),
                                 UserId = v.AttributeValueAsInt32("userid"),
                                 PostDate = v.AttributeValueAsDateTime("postdate")
                             };

                int? total = videosEl.AttributeValueAsNullableInt32("total");

                return new ThingResponse.VideoCollection(videos, total);
            }

            ThingResponse.Statistics MapStatistics(XElement statisticsEl)
            {
                if (statisticsEl == null)
                {
                    return null;
                }

                var ratingEl = statisticsEl.Element("ratings");

                var statistics = new ThingResponse.Statistics
                {
                    Page = statisticsEl.AttributeValueAsInt32("page"),
                    Ratings = new ThingResponse.Ratings
                    {
                        UsersRated = ratingEl.Element("usersrated").AttributeValueAsNullableInt32(),
                        Average = ratingEl.Element("average").AttributeValueAsDouble(),
                        BayesAverage = ratingEl.Element("bayesaverage").AttributeValueAsDouble(),
                        StandardDeviation = ratingEl.Element("stddev").AttributeValueAsNullableDouble(),
                        Median = ratingEl.Element("median").AttributeValueAsNullableInt32(),
                        Ranks = (from rank in ratingEl.Descendants("rank")
                                 select new ThingResponse.Rank
                                 {
                                     Type = rank.AttributeValue("type"),
                                     Id = rank.AttributeValueAsInt32("id"),
                                     Name = rank.AttributeValue("name"),
                                     FriendlyName = rank.AttributeValue("friendlyname"),
                                     Value = rank.AttributeValueAsNullableInt32(),
                                     BayesAverage = rank.AttributeValueAsDouble("bayesaverage")
                                 }).ToList(),
                        Owned = ratingEl.Element("owned").AttributeValueAsNullableInt32(),
                        Trading = ratingEl.Element("trading").AttributeValueAsNullableInt32(),
                        Wanting = ratingEl.Element("wanting").AttributeValueAsNullableInt32(),
                        Wishing = ratingEl.Element("wishing").AttributeValueAsNullableInt32(),
                        NumComments = ratingEl.Element("numcomments").AttributeValueAsNullableInt32(),
                        NumWeights = ratingEl.Element("numweights").AttributeValueAsNullableInt32(),
                        AverageWeight = ratingEl.Element("averageweight").AttributeValueAsNullableDouble()
                    }
                };

                return statistics;
            }

            #endregion
        }

        public async Task<UserResponse> GetUserAsync(UserRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            XDocument xdoc = await GetXDocumentAsync(request.RelativeUrl).ConfigureAwait(false);
            UserResponse response = Map(xdoc);
            return response;

            #region Helpers 

            UserResponse Map(XDocument doc)
            {
                return new UserResponse(MapUser(doc.Root));
            }

            User MapUser(XElement userEl)
            {
                return new User
                {
                    Id = userEl.Attribute("id")?.Value,
                    Name = userEl.Attribute("name").Value,
                    FirstName = userEl.Element("firstname").AttributeValue(),
                    LastName = userEl.Element("lastname").AttributeValue(),
                    AvatarLink = userEl.Element("avatarlink").AttributeValue(),
                    YearRegistered = userEl.Element("yearregistered").AttributeValue(),
                    LastLogin = userEl.Element("lastlogin").AttributeValue(),
                    StateOrProvince = userEl.Element("stateorprovince").AttributeValue(),
                    Country = userEl.Element("country").AttributeValue(),
                    WebAddress = userEl.Element("webaddress").AttributeValue(),
                    XboxAccount = userEl.Element("xboxaccount").AttributeValue(),
                    WiiAccount = userEl.Element("wiiaccount").AttributeValue(),
                    PsnAccount = userEl.Element("psnaccount").AttributeValue(),
                    BattleNetAccount = userEl.Element("battlenetaccount").AttributeValue(),
                    SteamAccount = userEl.Element("steamaccount").AttributeValue(),
                    TradeRating = userEl.Element("traderating").AttributeValue(),
                    MarketRating = userEl.Element("marketrating").AttributeValue(),
                    Buddies = MapBuddies(userEl).ToList(),
                    Guilds = MapGuilds(userEl).ToList(),
                    Top = MapListItems(userEl, "top").ToList(),
                    Hot = MapListItems(userEl, "hot").ToList()
                };
            }

            IEnumerable<Buddy> MapBuddies(XElement userEl)
            {
                if (userEl == null)
                {
                    return Enumerable.Empty<Buddy>();
                }

                return from buddy in userEl.Descendants("buddy")
                       select new Buddy
                       {
                           Id = buddy.AttributeValue("id"),
                           Name = buddy.AttributeValue("name")
                       };
            }

            IEnumerable<Guild> MapGuilds(XElement userEl)
            {
                if (userEl == null)
                {
                    return Enumerable.Empty<Guild>();
                }

                return from guild in userEl.Descendants("guild")
                       select new Guild
                       {
                           Id = guild.AttributeValue("id"),
                           Name = guild.AttributeValue("name")
                       };
            }

            IEnumerable<Item> MapListItems(XElement userEl, string list)
            {
                if (userEl == null)
                {
                    return Enumerable.Empty<Item>();
                }

                return from item in xdoc.XPathSelectElements($"/user/{list}/item")
                       select new Item
                       {
                           Rank = item.AttributeValue("rank"),
                           Type = item.AttributeValue("type"),
                           Id = item.AttributeValue("id"),
                           Name = item.AttributeValue("name")
                       };
            }

            #endregion
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
    }
}
