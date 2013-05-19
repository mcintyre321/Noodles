using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Noodles.Example.Domain;
using Noodles.Models;

namespace Noodles.Tests
{
    public class Tests
    {
        [Test] public void CanGetFieldsAsNodeMethods()
        {
            var application = new Application();
            var resource = ReflectionResource.CreateGeneric(application, null, "");
            var nodeMethodFromBehaviour = resource.NodeMethod("SignIn");
            Assert.NotNull(nodeMethodFromBehaviour);
        }
    }
}
