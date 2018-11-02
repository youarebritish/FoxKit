namespace FoxKit.Modules.DataSet.Editor
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using FoxKit.Modules.Archive;
    using FoxKit.Modules.DataSet.Fox.FoxCore;
    using FoxKit.Modules.DataSet.FoxCore;

    using UnityEditor.IMGUI.Controls;

    using UnityEngine;
    using UnityEngine.Assertions;

    public class SelectEntityWindowTreeView : TreeView
    {
        private readonly Dictionary<PackageDefinition, List<DataSetAsset>> packages;

        private readonly List<DataIdentifier> dataIdentifiers;

        private readonly Action<Data> onEntitySelected;

        private readonly Action<DataIdentifier, string> onDataIdentifierEntitySelected;

        private readonly List<Data> idToData = new List<Data>();

        private readonly List<string> idToDataIdentifierLinkKeys = new List<string>();
        
        /// <summary>
        /// IDs of root (i.e., non-selectable) items.
        /// </summary>
        private readonly HashSet<int> rootIds = new HashSet<int>();
        
        public SelectEntityWindowTreeView(TreeViewState state, IReadOnlyCollection<PackageDefinition> packages, Action<Data> onEntitySelectedCallback, Action<DataIdentifier, string> onDataIdentifierEntitySelectedCallback)
            : base(state)
        {
            this.onEntitySelected = onEntitySelectedCallback;
            this.onDataIdentifierEntitySelected = onDataIdentifierEntitySelectedCallback;
            this.showAlternatingRowBackgrounds = true;
            this.useScrollView = true;
            this.searchString = string.Empty;
            
            this.packages = packages.ToDictionary(package => package, package => (from asset in package.Entries
                                                                                  where asset is DataSetAsset
                                                                                  select asset as DataSetAsset).ToList());

            this.dataIdentifiers = (from entry in this.packages.Values
                                   from dataSet in entry
                                   from entity in dataSet.GetDataSet().GetDataList().Values
                                   where entity is DataIdentifier
                                   select entity as DataIdentifier).ToList();

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
            this.idToDataIdentifierLinkKeys.Add(null);

            // Add placeholder for "None."
            this.idToData.Add(null);
            this.idToDataIdentifierLinkKeys.Add(null);

            // Add placeholder for "DataIdentifiers".
            this.idToData.Add(null);
            this.idToDataIdentifierLinkKeys.Add(null);

            this.rootIds.Add(0);    // Root
            this.rootIds.Add(1);    // None
            this.rootIds.Add(2);    // DataIdentifiers

            var allItems = new List<TreeViewItem>
                               {
                                   new TreeViewItem { id = 1, depth = 0, displayName = "None" },
                                   new TreeViewItem { id = 2, depth = 0, displayName = "DataIdentifiers" }
                               };

            // Show DataIdentifiers.
            var id = 3;
            foreach (var dataIdentifier in this.dataIdentifiers)
            {
                allItems.Add(new TreeViewItem { id = id, depth = 1, displayName = dataIdentifier.Identifier });
                this.idToData.Add(null);
                this.idToDataIdentifierLinkKeys.Add(null);
                this.rootIds.Add(id);
                id++;

                foreach (var link in dataIdentifier.Links)
                {
                    allItems.Add(new TreeViewItem { id = id, depth = 2, displayName = link.Key });
                    this.idToData.Add(dataIdentifier);
                    this.idToDataIdentifierLinkKeys.Add(link.Key);
                    id++;
                }
            }

            // Show packages.
            foreach (var package in this.packages)
            {
                // Add package node.
                allItems.Add(new TreeViewItem { id = id, depth = 0, displayName = package.Key.name });
                this.idToData.Add(null);
                this.rootIds.Add(id);
                id++;

                foreach (var dataSet in package.Value)
                {
                    // Add DataSet node.
                    allItems.Add(new TreeViewItem { id = id, depth = 1, displayName = dataSet.name });
                    this.idToData.Add(null);
                    this.rootIds.Add(id);
                    id++;

                    foreach (var entity in dataSet.GetDataSet().GetDataList().Values)
                    {
                        var data = entity as Data;
                        if (data == null)
                        {
                            continue;
                        }

                        // Filter out results that don't contain search string.
                        if (data.Name.IndexOf(this.searchString, 0, StringComparison.CurrentCultureIgnoreCase) == -1)
                        {
                            continue;
                        }

                        allItems.Add(new TreeViewItem { id = id, depth = 2, displayName = data.Name });
                        this.idToData.Add(data);
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
            
            if (selectedData is DataIdentifier)
            {
                var key = this.idToDataIdentifierLinkKeys[selectedId];
                if (!string.IsNullOrEmpty(key))
                {
                    this.onDataIdentifierEntitySelected(selectedData as DataIdentifier, key);
                    return;
                }
            }

            this.onEntitySelected(selectedData);
        }
    }
}