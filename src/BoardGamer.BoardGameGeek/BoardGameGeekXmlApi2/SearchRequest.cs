using System;

namespace BoardGamer.BoardGameGeek.BoardGameGeekXmlApi2
{
    public class SearchRequest
    {
        /// <summary>
        /// Creates a new <c>SearchRequest</c> instance that can be used to find information within the boardgamegeek.com database.
        /// </summary>
        /// <param name="query">The search term(s).</param>
        /// <param name="type">Used to filter the result set by type. Valid values include 'boardgame', 'boardgameexpansion', 'boardgameaccessory', 'videogame', and 'rpgitem'.</param>
        /// <param name="exact">Used to limit the result to items with match the search term exactly.</param>
        public SearchRequest(
            string query = null,
            string type = null,
            bool? exact = null)
        {
            Query = query;
            Type = type;
            Exact = exact;
            RelativeUrl = BuildRelativeUrl();
        }

        public string Query { get; }
        public string Type { get; }
        public bool? Exact { get; }
        public Uri RelativeUrl { get; }

        private Uri BuildRelativeUrl()
        {
            UrlBuilder builder = new UrlBuilder()
                .Path("search")
                .AddQueryArgument("query", Query)
                .AddQueryArgument("type", Type)
                .AddQueryArgument("exact", Exact);

            return builder.ToUrl();
        }
    }
}
