using UnityEngine;

namespace FoxKit.Modules.DataSet.Editor.Command
{
    public interface IToolbarCommand
    {
        Texture Icon { get; }
        string Tooltip { get; }
        void Execute();
    }
}