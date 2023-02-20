using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace SimpleAudioSystem.Editor
{
    public partial class SimpleAudioSystemEditor
    {
        private static class DefinitionTreeBuilder
        {
            public static DefinitionTree<int> CompleteTree(DefinitionTree<int> initialTree, int startingValue, IEnumerable<ExtendedAudioDescriptor> descriptorList)
            {
                foreach (var descriptor in descriptorList)
                {
                    initialTree.AddTreeLeaf(descriptor.descriptiveName, startingValue);
                    ++startingValue;
                }
                return initialTree;
            }
        }
    }
}
