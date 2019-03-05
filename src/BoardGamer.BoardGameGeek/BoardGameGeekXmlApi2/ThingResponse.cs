using System.Collections.Generic;

namespace BoardGamer.BoardGameGeek.BoardGameGeekXmlApi2
{
    public class ThingResponse
    {
        public ThingResponse(IEnumerable<Thing> things)
        {
            this.Succeeded = true;
            this.Things = new List<Thing>(things);
        }

        public bool Succeeded { get; }
        public List<Thing> Things { get; }
    }
}
