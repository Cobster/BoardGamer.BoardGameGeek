using System;
using System.Collections.Generic;

namespace BoardGamer.BoardGameGeek.BoardGameGeekXmlApi2
{
    /// <summary>
    /// Represents a request to the /collection endpoint of the BGG XML API2.
    /// </summary>
    public class CollectionRequest
    {
        public CollectionRequest(
            string username,
            bool? version = null,
            string subType = null,
            string excludeSubType = null,
            IEnumerable<int> ids = null,
            bool? brief = null,
            bool? stats = null,
            bool? own = null,
            bool? rated = null,
            bool? played = null,
            bool? comment = null,
            bool? trade = null,
            bool? want = null,
            bool? wishList = null,
            int? wishListPriority = null,
            bool? preordered = null,
            bool? wantToPlay = null,
            bool? wantToBuy = null,
            bool? previouslyOwned = null,
            bool? hasParts = null,
            bool? wantsParts = null,
            int? minRating = null,
            int? maxRating = null,
            int? minBggRating = null,
            int? maxBggRating = null,
            int? minPlays = null,
            int? maxPlays = null,
            int? collectionId = null,
            DateTime? modifiedSince = null)
        {
            UserName = username;
            Version = version;
            SubType = subType;
            ExcludeSubType = excludeSubType;
            Ids = ids;
            Brief = brief;
            Stats = stats;
            Own = own;
            Rated = rated;
            Played = played;
            Comment = comment;
            Trade = trade;
            Want = want;
            WishList = wishList;
            WishListPriority = wishListPriority;
            Preordered = preordered;
            WantToPlay = wantToPlay;
            WantToBuy = wantToBuy;
            PreviouslyOwned = previouslyOwned;
            HasParts = hasParts;
            WantsParts = wantsParts;
            MinRating = minRating;
            MaxRating = maxRating;
            MinBggRating = minBggRating;
            MaxBggRating = maxBggRating;
            MinPlays = minPlays;
            MaxPlays = maxPlays;
            CollectionId = collectionId;
            ModifiedSince = modifiedSince;
            RelativeUrl = BuildRelativeUrl();
        }

        /// <summary>
        /// The name of the user.
        /// </summary>
        public string UserName { get; }
        /// <summary>
        /// Includes version information in the response for each item in the collection.
        /// </summary>
        public bool? Version { get; }
        /// <summary>
        /// Used to specify the collection you want to retrieve. Use boardgame, boardgameexpansion, boardgameaccessory, rpgitem, rpgissue, or videogame.
        /// </summary>
        public string SubType { get; }
        /// <summary>
        /// Used to specify which <c>SubType</c> you want to exclude from the collection results.
        /// </summary>
        public string ExcludeSubType { get; }
        /// <summary>
        /// Used to filter the collection to the specifically listed items.
        /// </summary>
        public IEnumerable<int> Ids { get; }
        /// <summary>
        /// Used to return abbreviated results.
        /// </summary>
        public bool? Brief { get; }
        /// <summary>
        /// Used to return expanded rating and ranking information for each item in the collection.
        /// </summary>
        public bool? Stats { get; }
        /// <summary>
        /// Used to filter by owned items.
        /// </summary>
        public bool? Own { get; }
        /// <summary>
        /// Used to filter by items that have been rated.
        /// </summary>
        public bool? Rated { get; }
        /// <summary>
        /// Used to filter by items that have been played.
        /// </summary>
        public bool? Played { get; }
        /// <summary>
        /// Used to filter by items that have been commented upon.
        /// </summary>
        public bool? Comment { get; }
        /// <summary>
        /// Used to filter by items marked for trade.
        /// </summary>
        public bool? Trade { get; }
        /// <summary>
        /// Used to filter by items marked as wanted in trade.
        /// </summary>
        public bool? Want { get; }
        /// <summary>
        /// Used to filter by items on the wish list.
        /// </summary>
        public bool? WishList { get; }
        /// <summary>
        /// Used to filter by items with a specific wish list priority.
        /// </summary>
        public int? WishListPriority { get; }
        /// <summary>
        /// Used to filter by items marked as preorder.
        /// </summary>
        public bool? Preordered { get; }
        /// <summary>
        /// Used to filter by items marked as wanting to play.
        /// </summary>
        public bool? WantToPlay { get; }
        /// <summary>
        /// Used to filter by items marked as wanting to buy.
        /// </summary>
        public bool? WantToBuy { get; }
        /// <summary>
        /// Used to filter by items marked as previously owned.
        /// </summary>
        public bool? PreviouslyOwned { get; }
        /// <summary>
        /// Used to filter by items where there is a comment in the has parts field.
        /// </summary>
        public bool? HasParts { get; }
        /// <summary>
        /// Used to filter by items where ther is a comment in the wants parts field.
        /// </summary>
        public bool? WantsParts { get; }
        /// <summary>
        /// A value from 1 to 10 used to filter on the minimum personal rating assigned to items in the collection.
        /// </summary>
        public int? MinRating { get; }
        /// <summary>
        /// A value from 1 to 10 used to filter on the maximum personal rating assigned to items in the collection.
        /// </summary>
        public int? MaxRating { get; }
        /// <summary>
        /// A value from 1 to 10 used to filter on the minimum boardgamegeek.com rating assigned to items in the collection.
        /// </summary>
        public int? MinBggRating { get; }
        /// <summary>
        /// A value from 1 to 10 used to filter on the maximum boardgamegeek.com rating assigned to items in the collection.
        /// </summary>
        public int? MaxBggRating { get; }
        /// <summary>
        /// A value used to filter by the minimum number of recorded plays.
        /// </summary>
        public int? MinPlays { get; }
        /// <summary>
        /// A value used to filter by the maximum number of recorded plays.
        /// </summary>
        public int? MaxPlays { get; }
        /// <summary>
        /// Restricts the collection results to a single specified collection id.
        /// </summary>
        public int? CollectionId { get; }
        /// <summary>
        /// Restricts the collection results to only those items which has had a status change or has been added since the specified date.
        /// </summary>
        public DateTime? ModifiedSince { get; }
        /// <summary>
        /// The relative url to the BGG XML API2 that represents this request.
        /// </summary>
        public Uri RelativeUrl { get; }

        private Uri BuildRelativeUrl()
        {
            UrlBuilder builder = new UrlBuilder()
                .Path("collection")
                .AddQueryArgument("username", UserName)
                .AddQueryArgument("version", Version)
                .AddQueryArgument("subtype", SubType)
                .AddQueryArgument("excludesubtype", ExcludeSubType)
                .AddQueryArgument("id", Ids)
                .AddQueryArgument("brief", Brief)
                .AddQueryArgument("stats", Stats)
                .AddQueryArgument("own", Own)
                .AddQueryArgument("rated", Rated)
                .AddQueryArgument("played", Played)
                .AddQueryArgument("comment", Comment)
                .AddQueryArgument("trade", Trade)
                .AddQueryArgument("want", Want)
                .AddQueryArgument("wishlist", WishList)
                .AddQueryArgument("wishlistpriority", WishListPriority)
                .AddQueryArgument("preordered", Preordered)
                .AddQueryArgument("wanttoplay", WantToPlay)
                .AddQueryArgument("wanttobuy", WantToBuy)
                .AddQueryArgument("prevowned", PreviouslyOwned)
                .AddQueryArgument("hasparts", HasParts)
                .AddQueryArgument("wantparts", WantsParts)
                .AddQueryArgument("minrating", MinRating)
                .AddQueryArgument("rating", MaxRating)
                .AddQueryArgument("minbggrating", MinBggRating)
                .AddQueryArgument("bggrating", MaxBggRating)
                .AddQueryArgument("minplays", MinPlays)
                .AddQueryArgument("maxplays", MaxPlays)
                .AddQueryArgument("collid", CollectionId)
                .AddQueryArgument("modifiedsince", ModifiedSince, "yy-MM-dd");

            return builder.ToUrl();
        }
    }
}