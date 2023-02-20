using System.Linq;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace SimpleAudioSystem.Editor
{
    public class DefinitionTree<T>
    {
        public Dictionary<string, T> leaves;
        public Dictionary<string, DefinitionTree<T>> branches;

        public DefinitionTree()
        {
            leaves = new Dictionary<string, T>();
            branches = new Dictionary<string, DefinitionTree<T>>();
        }

        private void AddLeafInternal(string[] branchPath, T value, int startingIndex = 0)
        {
            if (branchPath.Length > 0 && startingIndex < branchPath.Length)
            {
                var currentPath = branchPath[startingIndex];
                if (startingIndex == branchPath.Length - 1)
                {
                    if (!leaves.ContainsKey(currentPath))
                    {
                        leaves[currentPath] = value;
                    }
                }
                else
                {
                    DefinitionTree<T> nextBranch;
                    if (!branches.TryGetValue(currentPath, out nextBranch))
                    {
                        nextBranch = new DefinitionTree<T>();
                        branches.Add(currentPath, nextBranch);
                    }
                    nextBranch.AddLeafInternal(branchPath, value, startingIndex + 1);
                }
            }
        }

        private T GetLeafInternal(string[] branchPath, int startingIndex = 0)
        {
            if (branchPath.Length > 0 && startingIndex < branchPath.Length)
            {
                var currentPath = branchPath[startingIndex];
                if (startingIndex == branchPath.Length - 1)
                {
                    if (leaves.TryGetValue(currentPath, out var value))
                    {
                        return value;
                    }
                }
                else
                {
                    if (branches.TryGetValue(currentPath, out var tree))
                    {
                        return tree.GetLeafInternal(branchPath, startingIndex + 1);
                    }
                }
            }
            return default;
        }

        private List<string> GetPathInternal(List<string> rootPath, T value)
        {
            foreach (var leaf in leaves)
            {
                if (leaf.Value.Equals(value))
                {
                    rootPath.Add(leaf.Key);
                    return rootPath;
                }
            }
            foreach (var branch in branches)
            {
                rootPath.Add(branch.Key);
                var result = branch.Value.GetPathInternal(rootPath, value);
                if (result != null)
                {
                    return result;
                }
                rootPath.RemoveAt(rootPath.Count - 1);
            }
            return null;
        }

        public void AddLeaf(string[] branchPath, T value)
        {
            if (branchPath != null)
            {
                AddLeafInternal(branchPath, value, 0);
            }
        }

        public T GetLeaf(string[] branchPath)
        {
            return branchPath != null ? GetLeafInternal(branchPath, 0) : default;
        }

        public string[] GetPath(T value)
        {
            return GetPathInternal(new List<string>(), value)?.ToArray();
        }
    }

    public static class DefinitionTreeExtensions
    {
        private static string ConvertToIdentifier(string value)
        {
            value = Regex.Replace(value, "[^A-Za-z0-9_]", "_");
            if (!string.IsNullOrEmpty(value))
            {
                if (Regex.IsMatch(value, "^[0-9]"))
                {
                    value = "_" + value;
                }
            }
            return value;
        }

        public static void AddTreeLeaf<T>(this DefinitionTree<T> tree, string fullpath, T value)
        {
            if (tree != null && !string.IsNullOrEmpty(fullpath))
            {
                var branchPath = fullpath.Split("/", System.StringSplitOptions.RemoveEmptyEntries).Select(b => ConvertToIdentifier(b)).ToArray();
                tree.AddLeaf(branchPath, value);
            }
        }

        public static T GetTreeLeaf<T>(this DefinitionTree<T> tree, string fullpath)
        {
            if (tree != null && !string.IsNullOrEmpty(fullpath))
            {
                var branchPath = fullpath.Split("/", System.StringSplitOptions.RemoveEmptyEntries).Select(b => ConvertToIdentifier(b)).ToArray();
                return tree.GetLeaf(fullpath.Split("/"));
            }
            return default;
        }

        public static string GetLeafPath<T>(this DefinitionTree<T> tree, T value)
        {
            var result = tree?.GetPath(value);
            if (result != null)
            {
                return string.Join("/", result);
            }
            return null;
        }
    }
}
