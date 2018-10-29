namespace FoxKit.Modules.DataSet.Editor
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using FoxKit.Modules.Archive;
    using FoxKit.Modules.DataSet.FoxCore;

    using UnityEditor.IMGUI.Controls;

    using UnityEngine;
    using UnityEngine.Assertions;

    public class SelectEntityWindowTreeView : TreeView
    {
        private readonly Dictionary<PackageDefinition, List<DataSetAsset>> packages;

        private readonly Action<Data> onEntitySelected;
        
        private readonly List<Data> idToData = new List<Data>();
        
        /// <summary>
        /// IDs of root (i.e., non-selectable) items.
        /// </summary>
        private readonly HashSet<int> rootIds = new HashSet<int>();

        //private string searchString = string.Empty;

        public SelectEntityWindowTreeView(TreeViewState state, IReadOnlyCollection<PackageDefinition> packages, Action<Data> onEntitySelectedCallback)
            : base(state)
        {
            this.onEntitySelected = onEntitySelectedCallback;
            this.showAlternatingRowBackgrounds = true;
            this.useScrollView = true;
            
            this.packages = packages.ToDictionary(package => package, package => (from asset in package.Entries
                                                                                  where asset is DataSetAsset
                                                                                  select asset as DataSetAsset).ToList());

            this.Reload();
        }

        protected override bool CanMultiSelect(TreeViewItem item)
        {
            return false;
        }

        public void SetSearchString(string newSearchString)
        {
            var isChanged = newSearchString != this.searchString;
            this.searchString = newSearchString;

            if (isChanged)
            {
                this.Reload();
            }
        }

        protected override TreeViewItem BuildRoot()
        {
            var root = new TreeViewItem { id = 0, depth = -1, displayName = "Root" };

            this.idToData.Clear();
            this.rootIds.Clear();

            // Add placeholder for the root.
            this.idToData.Add(null);

            // Add placeholder for "None."
            this.idToData.Add(null);

            // Add placeholder for package.
            this.idToData.Add(null);

            // Add placeholder for DataSet.
            this.idToData.Add(null);

            this.rootIds.Add(0);    // Root
            this.rootIds.Add(1);    // None

            var allItems = new List<TreeViewItem>
                               {
                                   new TreeViewItem { id = 1, depth = 0, displayName = "None" }
                               };

            var id = 2;
            foreach (var package in this.packages)
            {
                // Add package node.
                allItems.Add(new TreeViewItem { id = id, depth = 0, displayName = package.Key.name });
                this.rootIds.Add(id);
                id++;

                foreach (var dataSet in package.Value)
                {
                    // Add DataSet node.
                    allItems.Add(new TreeViewItem { id = id, depth = 1, displayName = dataSet.name });
                    this.rootIds.Add(id);
                    id++;

                    foreach (var entity in dataSet.GetDataSet().dataList.Values)
                    {
                        // Filter out results that don't contain search string.
                        if (entity.Name.IndexOf(this.searchString, 0, StringComparison.CurrentCultureIgnoreCase) == -1)
                        {
                            continue;
                        }

                        allItems.Add(new TreeViewItem { id = id, depth = 2, displayName = entity.Name });
                        this.idToData.Add(entity);
                        id++;
                    }
                }
            }

            // Utility method that initializes the TreeViewItem.children and .parent for all items.
            SetupParentsAndChildrenFromDepths(root, allItems);

            // Return root of the tree
            return root;
        }

        protected override void SelectionChanged(IList<int> selectedIds)
        {
            Assert.IsTrue(selectedIds.Count == 1);

            var selectedId = selectedIds[0];

            if (this.rootIds.Contains(selectedId))
            {
                this.onEntitySelected(null);
                return;
            }

            var selectedData = this.idToData[selectedId];
            Assert.IsNotNull(selectedData);

            this.onEntitySelected(selectedData);
        }
    }
}