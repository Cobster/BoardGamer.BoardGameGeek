using System;
using System.Collections.Generic;
using System.Text;

namespace BoardGamer.BoardGameGeek.BoardGameGeekXmlApi2
{
    public class PlaysResponse
    {
        public PlaysResponse()
        {
            this.Succeeded = true;
        }

        public bool Succeeded { get; }
    }
}
