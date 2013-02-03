﻿using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Nustache.Core;

namespace Noodles.Example.WebApi
{
    internal class HtmlMediaTypeFormatter : MediaTypeFormatter
    {
        public static ConcurrentDictionary<string, Func<object, string>> Templates =
            new ConcurrentDictionary<string, Func<object, string>>();

        public HtmlMediaTypeFormatter()
        {
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));
        }

        public override bool CanReadType(Type type)
        {
            throw new NotImplementedException();
        }

        public override bool CanWriteType(Type type)
        {
            return true;
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
                    Render.Template(reader, value, streamWriter);

                    streamWriter.Flush();
                }
            });
            return task;
        }

    }
}