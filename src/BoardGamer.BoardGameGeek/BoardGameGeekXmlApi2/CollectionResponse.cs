namespace BoardGamer.BoardGameGeek.BoardGameGeekXmlApi2
{
    public class CollectionResponse
    {
        public CollectionResponse(Collection collection)
        {
            this.Succeeded = true;
            this.Collection = collection;
        }

        public Collection Collection { get; }
        public bool Succeeded { get; }
    }
}