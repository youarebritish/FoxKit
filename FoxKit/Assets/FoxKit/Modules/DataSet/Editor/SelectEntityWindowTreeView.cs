namespace FoxKit.Modules.DataSet.Editor
{
    using System;
    using System.Collections.Generic;

    using FoxKit.Modules.DataSet.FoxCore;

    using UnityEditor.IMGUI.Controls;

    using UnityEngine.Assertions;

    public class SelectEntityWindowTreeView : TreeView
    {
        private readonly List<Data> entities;

        private readonly Action<Data> onEntitySelected;

        private readonly List<Data> idToData = new List<Data>();

        private string searchString = string.Empty;

        /// <summary>
        /// TreeViewItem ID of the "None" option.
        /// </summary>
        private const int NoneId = 1;

        public SelectEntityWindowTreeView(TreeViewState state, List<Data> entities, Action<Data> onEntitySelectedCallback)
            : base(state)
        {
            this.entities = entities;
            this.onEntitySelected = onEntitySelectedCallback;
            this.showAlternatingRowBackgrounds = true;
            this.useScrollView = true;
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

            // Add placeholder for the root.
            this.idToData.Add(null);

            // Add placeholder for "None."
            this.idToData.Add(null);

            var allItems = new List<TreeViewItem> { new TreeViewItem { id = 1, depth = 0, displayName = "None" } };

            var id = 2;
            foreach (var entity in this.entities)
            {
                // Filter out results that don't contain search string.
                if (entity.Name.IndexOf(this.searchString, 0, StringComparison.CurrentCultureIgnoreCase) == -1)
                {
                    continue;
                }

                var item = new TreeViewItem{id = id, depth = 0, displayName = entity.Name};
                allItems.Add(item);
                this.idToData.Add(entity);
                id++;
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

            if (selectedId == NoneId)
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