using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BoardGamer.BoardGameGeek
{
    class UrlBuilder
    {
        private string path;
        private Dictionary<string, string> args = new Dictionary<string, string>();

        public UrlBuilder Path(string path)
        {
            this.path = path;
            return this;
        }

        public UrlBuilder AddQueryArgument(string name, string value)
        {
            if (value != null)
            {
                args.Add(name, Uri.EscapeDataString(value));
            }

            return this;
        }

        public UrlBuilder AddQueryArgument(string name, bool? value)
        {
            if (value.HasValue)
            {
                AddQueryArgument(name, value.Value ? "1" : "0");
            }
            return this;
        }

        public UrlBuilder AddQueryArgument(string name, int? value)
        {
            if (value.HasValue)
            {
                AddQueryArgument(name, value.Value.ToString());
            }
            return this;
        }

        public UrlBuilder AddQueryArgument(string name, DateTime? value, string format)
        {
            if (value.HasValue)
            {
                AddQueryArgument(name, value.Value.ToString(format));
            }
            return this;
        }

        public UrlBuilder AddQueryArgument<T>(string name, IEnumerable<T> collecton)
        {
            if (collecton != null)
            {
                AddQueryArgument(name, String.Join(",", collecton.Select(i => i.ToString())));
            }
            return this;
        }

        public Uri ToUrl()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(path);
            if (args.Count > 0)
            {
                sb.Append("?").Append(String.Join("&", args.Select((x) => $"{x.Key}={x.Value}")));
                //sb.Append(args.Aggregate("?", (qs, arg) => $"{qs}&{arg.Key}={arg.Value}"));
            }
            return new Uri(sb.ToString(), UriKind.Relative);
        }
    }
}