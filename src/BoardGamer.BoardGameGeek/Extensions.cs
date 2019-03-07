using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BoardGamer.BoardGameGeek
{
    static class Extensions
    {
        public static async Task<XDocument> ReadAsXDocumentAsync(this HttpContent content)
        {
            using (Stream stream = await content.ReadAsStreamAsync().ConfigureAwait(false))
            {
                return XDocument.Load(stream);
            }
        }

        public static string AttributeValue(this XElement element, string attributeName = "value")
        {
            return element?.Attribute(attributeName)?.Value;
        }

        public static bool AttributeValueAsBoolean(this XElement element, string attributeName = "value")
        {
            return element.AttributeValueAsNullableInt32(attributeName).GetValueOrDefault() == 1;
        }

        public static DateTime AttributeValueAsDateTime(this XElement element, string attributeName = "value")
        {
            return DateTime.TryParse(element.AttributeValue(attributeName), out DateTime v) ? v : default(DateTime);
        }

        public static DateTime? AttributeValueAsNullableDateTime(this XElement element, string attributeName = "value")
        {
            return DateTime.TryParse(element.AttributeValue(attributeName), out DateTime v) ? v : (DateTime?)null;
        }

        public static double AttributeValueAsDouble(this XElement element, string attributeName = "value")
        {
            return Double.TryParse(element.AttributeValue(attributeName), out double v) ? v : default(double);
        }

        public static double? AttributeValueAsNullableDouble(this XElement element, string attributeName = "value")
        {
            return Double.TryParse(element.AttributeValue(attributeName), out double v) ? v : (double?)null;
        }

        public static int AttributeValueAsInt32(this XElement element, string attributeName = "value")
        {
            return Int32.TryParse(element.AttributeValue(attributeName), out int v) ? v : default(int);
        }

        public static int? AttributeValueAsNullableInt32(this XElement element, string attributeName = "value")
        {
            return Int32.TryParse(element.AttributeValue(attributeName), out int v) ? v : (int?)null;
        }

        public static string ElementValue(this XElement element, string elementName)
        {
            return element?.Element(elementName)?.Value;
        }

        public static int ElementValueAsInt32(this XElement element, string elementName)
        {
            return Int32.TryParse(element.ElementValue(elementName), out int v) ? v : default(int);
        }

        public static int? ElementValueAsNullableInt32(this XElement element, string elementName)
        {
            return Int32.TryParse(element.ElementValue(elementName), out int v) ? v : (int?)null;
        }
    }
}
