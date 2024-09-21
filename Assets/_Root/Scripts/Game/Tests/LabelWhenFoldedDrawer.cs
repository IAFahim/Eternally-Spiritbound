using System;
using Alchemy.Editor;
using PancakeEditor.Common;
using UnityEngine;
using UnityEngine.UIElements;

namespace _Root.Scripts.Game.Tests
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Method)]
    public class RandomizeAttribute : Attribute
    {
        public string[] FieldNames { get; private set; }

        public RandomizeAttribute(params string[] fieldNames)
        {
            FieldNames = fieldNames;
        }
    }


    [CustomAttributeDrawer(typeof(RandomizeAttribute))]
    public sealed class RandomizeModeDrawer : AlchemyAttributeDrawer
    {
        public override void OnCreateElement()
        {
            if (Attribute is RandomizeAttribute randomizeAttribute)
            {
                foreach (var fieldName in randomizeAttribute.FieldNames)
                {
                    // // if any field name exist in the member info show value in a new lebel
                    // var value = MemberInfo.GetFieldValue<int>("a");
                    // var label = new Label(value.ToString());
                }
            }
        }
    }
}