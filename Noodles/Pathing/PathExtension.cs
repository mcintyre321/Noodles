using System;
using System.Collections.Generic;
using System.Linq;

namespace Noodles
{

    public interface IHasPath
    {
        string Path { get; }
    }

    public delegate string ResolvePathForNode(object o);

    public static class ResolvePathForNodeExtensions
    {
        static ResolvePathForNodeExtensions()
        {
            PathRules = new List<ResolvePathForNode>()
            {
                GetPathFromIHasPath,
                ParentChildPathRules.WalkParents
            };
        }

        public static List<ResolvePathForNode> PathRules;

        public static readonly ResolvePathForNode GetPathFromIHasPath = obj => obj is IHasPath ? ((IHasPath)obj).Path : null;


        public static string Path(this object obj)
        {
            var pathString = PathRules.Select(p => p(obj)).FirstOrDefault(path => path != null);
            if (string.IsNullOrWhiteSpace(pathString))
            {
                throw new Exception("Path should never be null/whitespace!");
            }
            return pathString;
        }

    }
}