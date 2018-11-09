namespace FoxKit.Modules.Archive
{
    using System.Collections.Generic;

    using UnityEngine;

    //[CreateAssetMenu(menuName = "FoxKit/Package Definition/Fpk", fileName = "New Fpk Definition", order = 4)]
    public class FpkPackageDefinition : PackageDefinition
    {
        /// <summary>
        /// Other fpks referenced by this package.
        /// </summary>
        public List<FpkPackageDefinition> References = new List<FpkPackageDefinition>();
    }
}