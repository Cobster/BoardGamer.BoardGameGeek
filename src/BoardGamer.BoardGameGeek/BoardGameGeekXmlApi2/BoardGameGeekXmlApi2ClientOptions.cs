using System;

namespace BoardGamer.BoardGameGeek.BoardGameGeekXmlApi2
{
    public class BoardGameGeekXmlApi2ClientOptions
    {
        public readonly static BoardGameGeekXmlApi2ClientOptions Default = new BoardGameGeekXmlApi2ClientOptions
        {
            Delay = TimeSpan.FromMilliseconds(500),
            MaxRetries = 20
        };        

        /// <summary>
        /// The time to wait before retrying a request.
        /// </summary>
        public TimeSpan Delay { get; set; }

        /// <summary>
        /// The maximum number of times to try a request.
        /// </summary>
        public int MaxRetries { get; set; }        
    }
}
