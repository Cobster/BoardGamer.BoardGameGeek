using System.Collections.Generic;

namespace BoardGamer.BoardGameGeek.BoardGameGeekXmlApi2
{
    public class HotItemsResponse
    {
        public HotItemsResponse(ItemCollection items)
        {
            Result = items;
        }

        public ItemCollection Result { get; }
        public bool Succeeded { get; }

        public class ItemCollection : List<Item>
        {
            public ItemCollection(IEnumerable<Item> items)
                : base(items)
            {

            }

            public string TermsOfUse { get; set; }
        }

        public class Item
        {
            public int Id { get; set; }
            public int Rank { get; set; }
            public string Thumbnail { get; set; }
            public string Name { get; set; }
            public int? YearPublished { get; set; }
        }
    }
}