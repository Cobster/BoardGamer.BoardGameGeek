using System.Collections.Generic;

namespace BoardGamer.BoardGameGeek.BoardGameGeekXmlApi2
{
    public class FamilyResponse
    {
        public FamilyResponse(ItemCollection items)
        {
            this.Result = items;
            this.Succeeded = true;
        }

        public ItemCollection Result { get; }
        public bool Succeeded { get; }

        public class ItemCollection : List<Item>
        {
            public ItemCollection(IEnumerable<Item> collection) 
                : base(collection)
            {}

            public string TermsOfUse { get; set; }
        }

        public class Item
        {
            public int Id { get; set; }
            public string Type { get; set; }
            public string Thumbnail { get; set; }
            public string Image { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public List<Link> Links { get; set; }
        }

        public class Link
        {
            public int Id { get; set; }
            public string Type { get; set; }
            public string Value { get; set; }
            public bool Inbound { get; set; }
        }
    }
}