using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Noodles.Models;
using Noodles.WebApi.Models;
using Noodles.WebApi.Views;
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

        public override Task WriteToStreamAsync(Type type, object value, Stream writeStream, System.Net.Http.HttpContent content, TransportContext transportContext)
        {
            var task = Task.Factory.StartNew(() =>
            {
                var s = Renderer.RenderView("Api", value);
                using(var sw = new StreamWriter(writeStream))
                {
                    sw.Write(s);
                    sw.Flush();
                }
            });
            return task;
        }

    }
}