using System.Net;
using System.Net.Http;

namespace BoardGamer.BoardGameGeek.BoardGameGeekXmlApi2
{
    public class BoardGameGeekHttpException : HttpRequestException
    {
        public HttpStatusCode StatusCode { get; }

        public BoardGameGeekHttpException(string message, HttpStatusCode statusCode)
            : base(message)
        {
            StatusCode = statusCode;
        }
    }
}