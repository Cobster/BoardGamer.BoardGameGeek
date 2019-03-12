using System;
using System.Collections.Generic;
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

        public async Task<FamilyResponse> GetFamilyAsync(FamilyRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            XDocument xdoc = await GetXDocumentAsync(request.RelativeUrl).ConfigureAwait(false);
            FamilyResponse response = Map(xdoc);
            return response;

            #region Helpers 

            FamilyResponse Map(XDocument document)
            {
                return new FamilyResponse(MapItemCollection(document.Root));
            }

            FamilyResponse.ItemCollection MapItemCollection(XElement itemsEl)
            {
                return new FamilyResponse.ItemCollection(itemsEl.Elements("item").Select(MapItem))
                {
                    TermsOfUse = itemsEl.AttributeValue("termsofuse")
                };
            }

            FamilyResponse.Item MapItem(XElement itemEl)
            {
                return new FamilyResponse.Item
                {
                    Id = itemEl.AttributeValueAsInt32("id"),
                    Type = itemEl.AttributeValue("type"),
                    Thumbnail = itemEl.ElementValue("thumbnail"),
                    Image = itemEl.ElementValue("image"),
                    Name = itemEl.Element("name").AttributeValue(),
                    Description = itemEl.ElementValue("description"),
                    Links = itemEl.Elements("link").Select(MapLink).ToList()
                };
            }

            FamilyResponse.Link MapLink(XElement linkEl)
            {
                return new FamilyResponse.Link
                {
                    Type = linkEl.AttributeValue("type"),
                    Id = linkEl.AttributeValueAsInt32("id"),
                    Value = linkEl.AttributeValue(),
                    Inbound = linkEl.AttributeValueAsBoolean("inbound")
                };
            }

            #endregion
        }

        public async Task<ForumListResponse> GetForumListAsync(ForumListRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            XDocument xdoc = await GetXDocumentAsync(request.RelativeUrl).ConfigureAwait(false);
            ForumListResponse response = Map(xdoc);
            return response;

            #region Helpers 

            ForumListResponse Map(XDocument document)
            {
                return new ForumListResponse(MapForums(document.Root));
            }

            ForumListResponse.Forums MapForums(XElement root)
            {
                return new ForumListResponse.Forums(root.Elements("forum").Select(MapForum))
                {
                    Id = root.AttributeValueAsInt32("id"),
                    Type = root.AttributeValue("type"),
                    TermsOfUse = root.AttributeValue("termsofuse")
                };
            }

            ForumListResponse.Forum MapForum(XElement forumEl)
            {
                return new ForumListResponse.Forum
                {
                    Id = forumEl.AttributeValueAsInt32("id"),
                    GroupId = forumEl.AttributeValueAsInt32("groupid"),
                    Title = forumEl.AttributeValue("title"),
                    NoPosting = forumEl.AttributeValueAsBoolean("noposting"),
                    Description = forumEl.AttributeValue("description"),
                    NumThreads = forumEl.AttributeValueAsInt32("numthreads"),
                    NumPosts = forumEl.AttributeValueAsInt32("numposts"),
                    LastPostDate = forumEl.AttributeValueAsNullableDateTimeOffset("lastpostdate")
                };
            }

            #endregion
        }

        public async Task<ForumsResponse> GetForumsAsync(ForumsRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            XDocument xdoc = await GetXDocumentAsync(request.RelativeUrl).ConfigureAwait(false);
            ForumsResponse response = Map(xdoc);
            return response;

            #region Helpers 

            ForumsResponse Map(XDocument document)
            {
                return new ForumsResponse(MapForum(document.Root));
            }

            ForumsResponse.Forum MapForum(XElement forumEl)
            {
                return new ForumsResponse.Forum
                {
                    Id = forumEl.AttributeValueAsInt32("id"),
                    Title = forumEl.AttributeValue("title"),
                    NumPosts = forumEl.AttributeValueAsInt32("numposts"),
                    NumThreads = forumEl.AttributeValueAsInt32("numthreads"),
                    LastPostDate = forumEl.AttributeValueAsNullableDateTimeOffset("lastpostdate"),
                    NoPosting = forumEl.AttributeValueAsBoolean("noposting"),
                    TermsOfUse = forumEl.AttributeValue("termsofuse"),
                    Threads = forumEl.Descendants("thread").Select(MapThread).ToList()
                };
            }

            ForumsResponse.Thread MapThread(XElement threadEl)
            {
                return new ForumsResponse.Thread
                {
                    Id = threadEl.AttributeValueAsInt32("id"),
                    Subject = threadEl.AttributeValue("subject"),
                    Author = threadEl.AttributeValue("author"),
                    NumArticles = threadEl.AttributeValueAsInt32("numarticles"),
                    PostDate = threadEl.AttributeValueAsDateTimeOffset("postdate"),
                    LastPostDate = threadEl.AttributeValueAsDateTimeOffset("lastpostdate")
                };
            }

            #endregion
        }

        public async Task<GuildResponse> GetGuildAsync(GuildRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            XDocument xdoc = await GetXDocumentAsync(request.RelativeUrl).ConfigureAwait(false);
            GuildResponse response = Map(xdoc);
            return response;

            #region Helpers 

            GuildResponse Map(XDocument document)
            {
                return new GuildResponse(MapGuild(document.Root));
            }

            GuildResponse.Guild MapGuild(XElement guildEl)
            {
                return new GuildResponse.Guild
                {
                    Id = guildEl.AttributeValueAsInt32("id"),
                    Name = guildEl.AttributeValue("name"),
                    Created = guildEl.AttributeValueAsDateTimeOffset("created"),
                    TermsOfUse = guildEl.AttributeValue("termsofuse"),
                    Category = guildEl.ElementValue("category"),
                    Website = guildEl.ElementValue("website"),
                    Manager = guildEl.ElementValue("manager"),
                    Description = guildEl.ElementValue("description"),
                    Location = MapLocation(guildEl.Element("location")),
                    Members = MapMembers(guildEl.Element("members"))
                };
            }

            GuildResponse.Location MapLocation(XElement locEl)
            {
                return new GuildResponse.Location
                {
                    Addr1 = locEl.ElementValue("addr1"),
                    Addr2 = locEl.ElementValue("addr2"),
                    City = locEl.ElementValue("city"),
                    StateOrProvince = locEl.ElementValue("stateorprovince"),
                    PostalCode = locEl.ElementValue("postalcode"),
                    Country = locEl.ElementValue("country")
                };
            }

            GuildResponse.MemberCollection MapMembers(XElement membersEl)
            {
                return new GuildResponse.MemberCollection(membersEl.Elements("member").Select(MapMember))
                {
                    Total = membersEl.AttributeValueAsInt32("count"),
                    Page = membersEl.AttributeValueAsInt32("page")
                };
            }

            GuildResponse.Member MapMember(XElement memberEl)
            {
                return new GuildResponse.Member
                {
                    Name = memberEl.AttributeValue("name"),
                    Date = memberEl.AttributeValueAsDateTimeOffset("date")
                };
            }

            #endregion
        }

        public async Task<HotItemsResponse> GetHotItemsAsync(HotItemsRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            XDocument xdoc = await GetXDocumentAsync(request.RelativeUrl).ConfigureAwait(false);
            HotItemsResponse response = Map(xdoc);
            return response;

            #region Helpers 

            HotItemsResponse Map(XDocument document)
            {
                return new HotItemsResponse(MapItems(document.Root));
            }

            HotItemsResponse.ItemCollection MapItems(XElement itemsEl)
            {
                var items = itemsEl.Elements("item").Select(MapItem);

                return new HotItemsResponse.ItemCollection(items)
                {
                    TermsOfUse = itemsEl.AttributeValue("termsofuse")
                };
            }

            HotItemsResponse.Item MapItem(XElement itemEl)
            {
                return new HotItemsResponse.Item
                {
                    Id = itemEl.AttributeValueAsInt32("id"),
                    Rank = itemEl.AttributeValueAsInt32("rank"),
                    Thumbnail = itemEl.Element("thumbnail").AttributeValue(),
                    Name = itemEl.Element("name").AttributeValue(),
                    YearPublished = itemEl.Element("yearpublished").AttributeValueAsNullableInt32()
                };
            }

            #endregion
        }


        public async Task<PlaysResponse> GetPlaysAsync(PlaysRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            XDocument xdoc = await GetXDocumentAsync(request.RelativeUrl).ConfigureAwait(false);
            PlaysResponse response = Map(xdoc);
            return response;

            #region Helpers 

            PlaysResponse Map(XDocument document)
            {
                return new PlaysResponse(MapPlaysCollection(document.Root));
            }

            PlaysResponse.PlaysCollection MapPlaysCollection(XElement playsEl)
            {
                var collection = new PlaysResponse.PlaysCollection
                {
                    Username = playsEl.AttributeValue("username"),
                    UserId = playsEl.AttributeValueAsInt32("userid"),
                    Total = playsEl.AttributeValueAsInt32("total"),
                    Page = playsEl.AttributeValueAsInt32("page"),
                    TermsOfUse = playsEl.AttributeValue("termsofuse"),
                    Plays = playsEl.Elements("play").Select(MapPlay).ToList()
                };

                return collection;
            }

            PlaysResponse.Play MapPlay(XElement playEl)
            {
                return new PlaysResponse.Play
                {
                    Id = playEl.AttributeValueAsInt32("id"),
                    Date = playEl.AttributeValueAsDateTime("date"),
                    Quantity = playEl.AttributeValueAsInt32("quantity"),
                    Length = playEl.AttributeValueAsInt32("length"),
                    Incomplete = playEl.AttributeValueAsBoolean("incomplete"),
                    NowInStats = playEl.AttributeValueAsBoolean("nowinstats"),
                    Location = playEl.AttributeValue("location"),
                    Item = MapItem(playEl.Element("item")),
                    Comments = playEl.ElementValue("comments"),
                    Players = playEl.Descendants("player").Select(MapPlayer).ToList()
                };
            }

            PlaysResponse.Item MapItem(XElement itemEl)
            {
                return new PlaysResponse.Item
                {
                    Name = itemEl.AttributeValue("name"),
                    ObjectType = itemEl.AttributeValue("objecttype"),
                    ObjectId = itemEl.AttributeValueAsInt32("objectid"),
                    SubTypes = itemEl.Descendants("subtype").Select(el => el.AttributeValue()).ToList()
                };
            }

            PlaysResponse.Player MapPlayer(XElement playerEl)
            {
                return new PlaysResponse.Player
                {
                    Username = playerEl.AttributeValue("username"),
                    UserId = playerEl.AttributeValueAsInt32("userid"),
                    Name = playerEl.AttributeValue("name"),
                    StartPosition = playerEl.AttributeValue("startposition"),
                    Color = playerEl.AttributeValue("color"),
                    Score = playerEl.AttributeValue("score"),
                    New = playerEl.AttributeValueAsBoolean("new"),
                    Rating = playerEl.AttributeValueAsInt32("rating"),
                    Win = playerEl.AttributeValueAsBoolean("win")
                };
            }

            #endregion
        }

        public async Task<ThingResponse> GetThingAsync(ThingRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            bool comments = request.Comments.GetValueOrDefault();
            bool ratingComments = request.RatingComments.GetValueOrDefault();

            if (comments == true && ratingComments == true)
            {
                // If request is sent, and both are true, the comments will supercede the rating comments.
                // Figured it was best to just call this out as an error, rather than return unexpected information.
                throw new ArgumentException("The comments and ratingcomments properties are mutually exclusive.");
            }

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
                           Versions = MapVersions(versions),
                           Videos = MapVideoCollection(videos),
                           Statistics = MapStatistics(item.Element("statistics")),
                           Comments = comments ? MapComments(item.Element("comments")) : null,
                           RatingComments = ratingComments ? MapRatingComments(item.Element("comments")) : null,
                           Marketplace = MapMarketplace(item.Element("marketplacelistings"))
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

            List<ThingResponse.Version> MapVersions(XElement versionsEl)
            {
                if (versionsEl == null)
                {
                    return null;
                }

                var versions = new IEnumerable<ThingResponse.Version>[]
                {
        MapBoardGameVersions(versionsEl),
        MapRpgItemVersion(versionsEl),
        MapVideoGameVersion(versionsEl),
        MapVideoGameCharacterVersion(versionsEl)
                };

                return versions.SelectMany(x => x).ToList();
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

            ThingResponse.Comments MapComments(XElement commentsEl)
            {
                if (commentsEl == null)
                {
                    return null;
                }

                return new ThingResponse.Comments(
                        from comment in commentsEl.Elements("comment")
                        select new ThingResponse.Comment
                        {
                            Username = comment.AttributeValue("username"),
                            Rating = comment.AttributeValueAsNullableDouble("rating"),
                            Value = comment.AttributeValue()
                        },
                        commentsEl.AttributeValueAsInt32("page"),
                        commentsEl.AttributeValueAsInt32("totalitems")
                    );
            }

            ThingResponse.RatingComments MapRatingComments(XElement commentsEl)
            {
                if (commentsEl == null)
                {
                    return null;
                }

                return new ThingResponse.RatingComments(
                        from comment in commentsEl.Elements("comment")
                        select new ThingResponse.RatingComment
                        {
                            Username = comment.AttributeValue("username"),
                            Rating = comment.AttributeValueAsInt32("rating"),
                            Value = comment.AttributeValue()
                        },
                        commentsEl.AttributeValueAsInt32("page"),
                        commentsEl.AttributeValueAsInt32("totalitems")
                    );
            }

            List<ThingResponse.MarketplaceListing> MapMarketplace(XElement marketplaceEl)
            {
                if (marketplaceEl == null)
                {
                    return null;
                }

                return (from listing in marketplaceEl.Elements("listing")
                        select new ThingResponse.MarketplaceListing
                        {
                            ListDate = listing.Element("listdate").AttributeValueAsDateTimeOffset(),
                            Currency = listing.Element("price").AttributeValue("currency"),
                            Price = listing.Element("price").AttributeValueAsDouble(),
                            Condition = listing.Element("condition").AttributeValue(),
                            Notes = listing.Element("notes").AttributeValue(),
                            Link = listing.Element("link").AttributeValue("href")
                        }).ToList();
            }

            #endregion
        }

        public async Task<ThreadsResponse> GetThreadsAsync(ThreadsRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            XDocument xdoc = await GetXDocumentAsync(request.RelativeUrl).ConfigureAwait(false);
            ThreadsResponse response = Map(xdoc);
            return response;

            #region Helpers 

            ThreadsResponse Map(XDocument document)
            {
                return new ThreadsResponse(MapThread(document.Root));
            }

            ThreadsResponse.Thread MapThread(XElement threadEl)
            {
                return new ThreadsResponse.Thread
                {
                    Id = threadEl.AttributeValueAsInt32("id"),
                    NumArticles = threadEl.AttributeValueAsInt32("numarticles"),
                    Link = threadEl.AttributeValue("link"),
                    TermsOfUse = threadEl.AttributeValue("termsofuse"),
                    Subject = threadEl.ElementValue("subject"),
                    Articles = threadEl.Descendants("article").Select(MapArticle).ToList()
                };
            }

            ThreadsResponse.Article MapArticle(XElement articleEl)
            {
                return new ThreadsResponse.Article
                {
                    Id = articleEl.AttributeValueAsInt32("id"),
                    UserName = articleEl.AttributeValue("username"),
                    Link = articleEl.AttributeValue("link"),
                    PostDate = articleEl.AttributeValueAsDateTimeOffset("postdate"),
                    EditDate = articleEl.AttributeValueAsDateTimeOffset("editdate"),
                    NumEdits = articleEl.AttributeValueAsInt32("numedits"),
                    Subject = articleEl.ElementValue("subject"),
                    Body = articleEl.ElementValue("body")
                };
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

            UserResponse.User MapUser(XElement userEl)
            {
                return new UserResponse.User
                {
                    Id = userEl.AttributeValueAsInt32("id"),
                    Name = userEl.AttributeValue("name"),
                    TermsOfUse = userEl.AttributeValue("termsofuse"),
                    FirstName = userEl.Element("firstname").AttributeValue(),
                    LastName = userEl.Element("lastname").AttributeValue(),
                    AvatarLink = userEl.Element("avatarlink").AttributeValue(),
                    YearRegistered = userEl.Element("yearregistered").AttributeValueAsInt32(),
                    LastLogin = userEl.Element("lastlogin").AttributeValueAsDateTime(),
                    StateOrProvince = userEl.Element("stateorprovince").AttributeValue(),
                    Country = userEl.Element("country").AttributeValue(),
                    WebAddress = userEl.Element("webaddress").AttributeValue(),
                    XboxAccount = userEl.Element("xboxaccount").AttributeValue(),
                    WiiAccount = userEl.Element("wiiaccount").AttributeValue(),
                    PsnAccount = userEl.Element("psnaccount").AttributeValue(),
                    BattleNetAccount = userEl.Element("battlenetaccount").AttributeValue(),
                    SteamAccount = userEl.Element("steamaccount").AttributeValue(),
                    TradeRating = userEl.Element("traderating").AttributeValueAsInt32(),
                    MarketRating = userEl.Element("marketrating").AttributeValueAsInt32(),
                    Buddies = MapBuddies(userEl).ToList(),
                    Guilds = MapGuilds(userEl).ToList(),
                    Top = MapListItems(userEl, "top").ToList(),
                    Hot = MapListItems(userEl, "hot").ToList()
                };
            }

            IEnumerable<UserResponse.Buddy> MapBuddies(XElement userEl)
            {
                if (userEl == null)
                {
                    return Enumerable.Empty<UserResponse.Buddy>();
                }

                return from buddy in userEl.Descendants("buddy")
                       select new UserResponse.Buddy
                       {
                           Id = buddy.AttributeValueAsInt32("id"),
                           Name = buddy.AttributeValue("name")
                       };
            }

            IEnumerable<UserResponse.Guild> MapGuilds(XElement userEl)
            {
                if (userEl == null)
                {
                    return Enumerable.Empty<UserResponse.Guild>();
                }

                return from guild in userEl.Descendants("guild")
                       select new UserResponse.Guild
                       {
                           Id = guild.AttributeValueAsInt32("id"),
                           Name = guild.AttributeValue("name")
                       };
            }

            IEnumerable<UserResponse.Item> MapListItems(XElement userEl, string list)
            {
                if (userEl == null)
                {
                    return Enumerable.Empty<UserResponse.Item>();
                }

                return from item in xdoc.XPathSelectElements($"/user/{list}/item")
                       select new UserResponse.Item
                       {
                           Rank = item.AttributeValueAsInt32("rank"),
                           Type = item.AttributeValue("type"),
                           Id = item.AttributeValueAsInt32("id"),
                           Name = item.AttributeValue("name")
                       };
            }

            #endregion
        }

        public async Task<SearchResponse> SearchAsync(SearchRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            XDocument xdoc = await GetXDocumentAsync(request.RelativeUrl).ConfigureAwait(false);
            SearchResponse response = Map(xdoc);
            return response;

            #region Helpers

            SearchResponse Map(XDocument document)
            {
                return new SearchResponse(MapItemCollection(document.Root));
            }

            SearchResponse.ItemCollection MapItemCollection(XElement itemsEl)
            {
                return new SearchResponse.ItemCollection
                {
                    Total = itemsEl.AttributeValueAsInt32("total"),
                    TermsOfUse = itemsEl.AttributeValue("termsofuse"),
                    Items = itemsEl.Elements("item").Select(MapItem).ToList()
                };
            }

            SearchResponse.Item MapItem(XElement itemEl)
            {
                return new SearchResponse.Item
                {
                    Type = itemEl.AttributeValue("type"),
                    Id = itemEl.AttributeValueAsInt32("id"),
                    Name = itemEl.Element("name").AttributeValue(),
                    YearPublished = itemEl.Element("yearpublished").AttributeValueAsNullableInt32()
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
