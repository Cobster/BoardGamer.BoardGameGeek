using System;
using System.Collections.Generic;
using System.Text;

namespace BoardGamer.BoardGameGeek.BoardGameGeekXmlApi2
{
    public class SearchResponse
    {
        public SearchResponse(ItemCollection collection)
        {
            Succeeded = true;
            Result = collection;
        }

        public ItemCollection Result { get; }
        public bool Succeeded { get; }

        public class ItemCollection
        {
            public List<Item> Items { get; set; }
            public int Total { get; set; }
            public string TermsOfUse { get; set; }
        }

        public class Item
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Type { get; set; }
            public int? YearPublished { get; set; }
        }
    }
}
