using System;

namespace BoardGamer.BoardGameGeek.BoardGameGeekXmlApi2
{
    /// <summary>
    /// Gets logged play information for a user and/or a game.
    /// </summary>
    public class PlaysRequest
    {
        /// <summary>
        /// Creates a new request to get information about logged plays.
        /// </summary>
        /// <param name="username">The username for the player.</param>
        /// <param name="id">The id of the game.</param>
        /// <param name="type">The type of item you want to request play information for, valid values are 'thing' or 'family'.</param>
        /// <param name="minDate">Used to filter out logged plays before this date.</param>
        /// <param name="maxDate">Used to filter out logged plays after this date.</param>
        /// <param name="subType">Filter the logged play results to a specified type. Valid values are 'boardgame', 'boardgameexpansion', 'boardgameaccessory', 'rpgitem', or 'videogame'.</param>
        /// <param name="page">The page of information to retrieve. Page size is 100 records.</param>
        public PlaysRequest(
            string username = null,
            int? id = null,
            string type = null,
            DateTime? minDate = null,
            DateTime? maxDate = null,
            string subType = null,
            int? page = null)
        {
            // Either the username, or the id, or both must be specified.
            if (username == null && id == null)
            {
                throw new ArgumentException("Must specify a value for username or id.");
            }

            this.Username = username;
            this.Id = id;
            this.Type = type;
            this.MinDate = minDate;
            this.MaxDate = maxDate;
            this.SubType = subType;
            this.Page = page;

            this.RelativeUrl = BuildRelativeUrl();
        }

        /// <summary>
        /// The username of the player you want get play information.
        /// </summary>
        public string Username { get; }

        /// <summary>
        /// The id of the game you want play information for.
        /// </summary>
        public int? Id { get; }

        /// <summary>
        /// The type of the item you want to request play information for.
        /// </summary>
        public string Type { get; }

        /// <summary>
        /// Returns only plays of the specified date or later.
        /// </summary>
        public DateTime? MinDate { get; }

        /// <summary>
        /// Returns only plays of the specified date or earlier.
        /// </summary>
        public DateTime? MaxDate { get; }

        /// <summary>
        /// Limits play results to the specified type. The valid types are 'boardgame', 'boardgameexpansion', 'boardgameaccessory', 'rpgitem', or 'videogame'.
        /// By default, 'boardgame' is used when not specified.
        /// </summary>
        public string SubType { get; }

        /// <summary>
        /// The page of information requested. Each page contains a maximum of 100 logged plays.
        /// </summary>
        public int? Page { get; }

        /// <summary>
        /// The relative url to the BGG XML API2 that will fulfill this request.
        /// </summary>
        public Uri RelativeUrl { get; }

        private Uri BuildRelativeUrl()
        {
            return new UrlBuilder()
                .Path("plays")
                .AddQueryArgument("username", Username)
                .AddQueryArgument("id", Id)
                .AddQueryArgument("type", Type)
                .AddQueryArgument("mindate", MinDate, "yyyy-MM-dd")
                .AddQueryArgument("maxdate", MaxDate, "yyyy-MM-dd")
                .AddQueryArgument("subtype", SubType)
                .AddQueryArgument("page", Page)
                .ToUrl();
        }
    }
}
