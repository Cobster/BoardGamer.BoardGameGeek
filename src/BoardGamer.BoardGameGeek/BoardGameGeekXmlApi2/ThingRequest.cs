using System;
using System.Collections.Generic;
using System.Linq;

namespace BoardGamer.BoardGameGeek.BoardGameGeekXmlApi2
{
    /// <summary>
    /// Represents a request to the Thing Items endpoint for the boardgamegeek.com XML API2.
    /// </summary>
    public class ThingRequest
    {
        /// <summary>
        /// Creates a new request to get information about a thing.
        /// </summary>
        /// <param name="ids">Specifies the id of the thing or things to retrieve.</param>
        /// <param name="types">Filters the results by type.</param>
        /// <param name="versions"></param>
        /// <param name="videos"></param>
        /// <param name="stats"></param>
        /// <param name="historical"></param>
        /// <param name="marketplace"></param>
        /// <param name="comments"></param>
        /// <param name="ratingComments"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        public ThingRequest(
            IEnumerable<int> ids,
            IEnumerable<string> types = null,
            bool? versions = null,
            bool? videos = null,
            bool? stats = null,
            bool? historical = null,
            bool? marketplace = null,
            bool? comments = null,
            bool? ratingComments = null,
            int? page = null,
            int? pageSize = null
        )
        {
            if (ids == null)
            {
                throw new ArgumentNullException(nameof(ids));
            }
            if (!ids.Any())
            {
                throw new ArgumentException("Must supply at least 1 id value in a thing request", nameof(ids));
            }

            this.Ids = ids;
            this.Types = types;
            this.Versions = versions;
            this.Videos = videos;
            this.Stats = stats;
            this.Historical = historical;
            this.Marketplace = marketplace;
            this.Comments = comments;
            this.RatingComments = comments;
            this.Page = page;
            this.PageSize = pageSize;
            this.RelativeUrl = this.BuildRelativeUrl();
        }

        public IEnumerable<int> Ids { get; }
        public IEnumerable<string> Types { get; }
        public bool? Versions { get; }
        public bool? Videos { get; }
        public bool? Stats { get; }
        public bool? Historical { get; }
        public bool? Marketplace { get; }
        public bool? Comments { get; }
        public bool? RatingComments { get; }
        public int? Page { get; }
        public int? PageSize { get; }
        public Uri RelativeUrl { get; }

        private Uri BuildRelativeUrl()
        {
            return new UrlBuilder()
                .Path("thing")
                .AddQueryArgument("id", Ids)
                .AddQueryArgument("types", Types)
                .AddQueryArgument("versions", Versions)
                .AddQueryArgument("videos", Videos)
                .AddQueryArgument("stats", Stats)
                .AddQueryArgument("historical", Historical)
                .AddQueryArgument("marketplace", Marketplace)
                .AddQueryArgument("comments", Comments)
                .AddQueryArgument("ratingcomments", RatingComments)
                .AddQueryArgument("page", Page)
                .AddQueryArgument("pagesize", PageSize)
                .ToUrl();
        }
    }
}