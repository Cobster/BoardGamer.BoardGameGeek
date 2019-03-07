using System;
using System.Collections.Generic;
using System.Text;

namespace BoardGamer.BoardGameGeek.BoardGameGeekXmlApi2
{
    public class PlaysRequest
    {
        public PlaysRequest(string username)
        {
            this.Username = username;
            this.RelativeUrl = BuildRelativeUrl();
        }

        public string Username { get; }

        public Uri RelativeUrl { get; }

        private Uri BuildRelativeUrl()
        {
            return new UrlBuilder()
                .Path("plays")
                .AddQueryArgument("username", Username)
                .ToUrl();
        }
    }
}
