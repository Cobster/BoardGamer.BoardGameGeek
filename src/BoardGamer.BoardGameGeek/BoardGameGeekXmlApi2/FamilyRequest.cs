using System;
using System.Collections.Generic;

namespace BoardGamer.BoardGameGeek.BoardGameGeekXmlApi2
{
    public class FamilyRequest
    {
        public FamilyRequest(int id, string type)
            : this(new int[] { id }, type)
        { }

        public FamilyRequest(IEnumerable<int> ids, string type)
        {
            this.Ids = ids;
            this.Type = type;
            this.RelativeUrl = new UrlBuilder()
                .Path("family")
                .AddQueryArgument("id", Ids)
                .AddQueryArgument("type", Type)
                .ToUrl();
        }

        public IEnumerable<int> Ids { get; }
        public string Type { get; }
        public Uri RelativeUrl { get; }
    }
}