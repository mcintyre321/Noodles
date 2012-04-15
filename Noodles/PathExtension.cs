using System;
using System.Collections.Generic;
using System.Linq;

namespace Noodles
{

    public interface IHasPath
    {
        string Path { get; }
    }

    public delegate string ResolvePath(object o);

    public static class PathExtensions
    {
        static PathExtensions()
        {
            PathRules = new List<ResolvePath>()
            {
                GetPathFromIHasPath,
                GetPathFromWalkingParent
            };
        }

        public static List<ResolvePath> PathRules;

        public static readonly ResolvePath GetPathFromIHasPath = obj => obj is IHasPath ? ((IHasPath)obj).Path : null;
        public static readonly ResolvePath GetPathFromWalkingParent = obj =>
        {
            var name = obj.Name();
            if (name == null) return "/";
            var parent = obj.Parent();
            if (parent != null)
            {
                var parentPath = obj.Parent().Path();
                if (parentPath == null)
                {
                    throw new Exception("Object '" + name + "' parent '" + (parent.Name() ?? "<no name>") +  "' has no path");
                }else
                {
                    return parentPath + "/" + name;
                }
            }
            return name;
        };


        public static string Path(this object obj)
        {
            return PathRules.Select(p => p(obj)).FirstOrDefault(path => path != null);
        }


    }
}