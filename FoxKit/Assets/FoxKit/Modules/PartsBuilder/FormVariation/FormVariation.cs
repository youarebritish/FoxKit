namespace FoxKit.Modules.PartsBuilder.FormVariation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;
    using FoxKit.Core;
    using OdinSerializer;

    /// <summary>
    /// A set of form variation options used by the FoxEngine. 
    /// </summary>
    [CreateAssetMenu(fileName = "New Form Variation", menuName = "FoxKit/Form Variation", order = 2)]
    public class FormVariation : SerializedScriptableObject
    {
        [NonSerialized, OdinSerialize]
        public Dictionary<FormVariationCategory, FormVariationOptionSet> Options = CreateDictionary();

        [HideInInspector]
        public bool IsReadOnly;

        /// <summary>
        /// Default FormVariation constructor.
        /// </summary>
        public FormVariation()
        {
        }

        /// <summary>
        /// Makes a FoxKit FormVariation from a FoxLib FormVariation..
        /// </summary>
        /// <param name="formVariation">The FoxLib FormVariation to convert.</param>
        /// <param name="str32DictFunc">An StrCode32 dictionary funtion used unhashing names.</param>
        /// <param name="str64DictFunc">An PathFileNameCode64 dictionary funtion used unhashing file names.</param>
        /// <returns>The FoxKit FormVariation.</returns>
        public static FormVariation Convert(FoxLib.FormVariation.FormVariation formVariation, Func<uint, string> str32DictFunc, Func<ulong, string> str64DictFunc)
        {
            FormVariation newFormVariation = null;
            newFormVariation.Options = new Dictionary<FormVariationCategory, FormVariationOptionSet>();

            newFormVariation.Options.Add(FormVariationCategory.STATIC, new FormVariationOptionSet());
            newFormVariation.Options[FormVariationCategory.STATIC].Options[0] = FormVariationOptionSetOption.Convert(formVariation, str32DictFunc, str64DictFunc);

            foreach (var variableEntry in formVariation.Variables)
            {
                if (variableEntry.Type != 0)
                {
                    newFormVariation.Options.Add((FormVariationCategory)variableEntry.Type, FormVariationOptionSet.Convert(variableEntry, str32DictFunc, str64DictFunc));
                }
            }

            return newFormVariation;
        }

        /// <summary>
        /// Makes a FoxLib FormVariation from a FoxKit FormVariation.
        /// </summary>
        /// <returns>The FoxLib FormVariation.</returns>
        public FoxLib.FormVariation.FormVariation Convert()
        {
            FormVariationOptionSet manual = this.Options[FormVariationCategory.STATIC];

            FoxLib.FormVariation.VariableFormVariationEntry[] entries = new FoxLib.FormVariation.VariableFormVariationEntry[this.Options.Count - 1];
            for (int i = 0; i < this.Options.Count; i++)
            {
                var optionSet = this.Options.ElementAt(i);
                if (optionSet.Key != FormVariationCategory.STATIC)
                {
                    entries[i] = optionSet.Value.Convert(optionSet.Key);
                }
            }

            return manual.Options[0].Convert(entries);
        }

        private static Dictionary<FormVariationCategory, FormVariationOptionSet> CreateDictionary()
        {
            var enumValues = Enum.GetValues(typeof(FormVariationCategory));
            var dict = new Dictionary<FormVariationCategory, FormVariationOptionSet>(enumValues.Length);
            
            foreach (FormVariationCategory value in enumValues)
            {
                var optionSet = new FormVariationOptionSet();
                optionSet.Options.Add(new FormVariationOptionSetOption());
                dict.Add(value, optionSet);
            }

            return dict;
        }
    }
}