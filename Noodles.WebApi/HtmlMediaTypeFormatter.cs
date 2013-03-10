using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using RazorEngine;

namespace Noodles.WebApi
{
    public class HtmlMediaTypeFormatter : MediaTypeFormatter
    {
        public static ConcurrentDictionary<string, string> Templates = new ConcurrentDictionary<string, string>();

        public HtmlMediaTypeFormatter()
        {
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));
        }

        public override bool CanReadType(Type type)
        {
            return false;
        }

        public override bool CanWriteType(Type type)
        {
            return true;
        }

        public override Task<object> ReadFromStreamAsync(Type type, Stream readStream, System.Net.Http.HttpContent content, IFormatterLogger formatterLogger)
        {
            return base.ReadFromStreamAsync(type, readStream, content, formatterLogger);
        }

        public override Task WriteToStreamAsync(Type type, object value, Stream writeStream, System.Net.Http.HttpContent content, TransportContext transportContext)
        {
            
            var task = Task.Factory.StartNew(() =>
            {
                var assembly = typeof (HtmlMediaTypeFormatter).Assembly;
                var resourceName = assembly.GetManifestResourceNames().Single();
                using (var stream = assembly.GetManifestResourceStream(resourceName))
                using (var reader = new StreamReader(stream))
                {
                    var streamWriter = new StreamWriter(writeStream);
                    var template = reader.ReadToEnd();
                    string result = Razor.Parse(template, value );
                    streamWriter.Write(result);
                    streamWriter.Flush();
                }
            });
            return task;
        }

    }
}