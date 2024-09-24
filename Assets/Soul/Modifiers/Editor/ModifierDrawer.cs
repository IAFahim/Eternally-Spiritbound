using Soul.Modifiers.Runtime;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Soul.Modifiers.Editor
{
    [CustomPropertyDrawer(typeof(Modifier))]
    public class ModifierDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var container = new VisualElement();

            // Create the header
            var header = new VisualElement
            {
                style =
                {
                    flexDirection = FlexDirection.Row,
                    backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.2f),
                    height = 22
                }
            };

            var foldout = new Foldout
            {
                text = property.displayName,
                style =
                {
                    flexGrow = 1
                },
                value = property.isExpanded
            };
            header.Add(foldout);

            var valueLabel = new Label
            {
                style =
                {
                    unityTextAlign = TextAnchor.MiddleRight,
                    color = new Color(0.2f, 0.8f, 0.2f),
                    unityFontStyleAndWeight = FontStyle.Bold,
                    minWidth = 100
                }
            };
            header.Add(valueLabel);

            container.Add(header);

            // Create the fields
            var fieldsContainer = new VisualElement
            {
                style =
                {
                    display = property.isExpanded ? DisplayStyle.Flex : DisplayStyle.None
                }
            };

            var baseValueField = new FloatField("Base");
            var multiplierField = new FloatField("Multiplier");
            var additiveField = new FloatField("Additive");

            fieldsContainer.Add(baseValueField);
            fieldsContainer.Add(multiplierField);
            fieldsContainer.Add(additiveField);

            container.Add(fieldsContainer);

            baseValueField.BindProperty(property.FindPropertyRelative("baseValue"));
            multiplierField.BindProperty(property.FindPropertyRelative("multiplier"));
            additiveField.BindProperty(property.FindPropertyRelative("additive"));

            void UpdateValueLabel()
            {
                var baseValue = property.FindPropertyRelative("baseValue").floatValue;
                var multiplier = property.FindPropertyRelative("multiplier").floatValue;
                var additive = property.FindPropertyRelative("additive").floatValue;

                var value = baseValue * multiplier + additive;
                var valueText = $"{baseValue}x{multiplier}+{additive} = {value:F2}";

                valueLabel.text = valueText;

                // Update the tooltip to show the full calculation
                valueLabel.tooltip = valueText;
            }

            baseValueField.RegisterValueChangedCallback(_ => UpdateValueLabel());
            multiplierField.RegisterValueChangedCallback(_ => UpdateValueLabel());
            additiveField.RegisterValueChangedCallback(_ => UpdateValueLabel());

            // Handle foldout state changes
            foldout.RegisterValueChangedCallback(evt =>
            {
                property.isExpanded = evt.newValue;
                fieldsContainer.style.display = evt.newValue ? DisplayStyle.Flex : DisplayStyle.None;
                property.serializedObject.ApplyModifiedProperties();
            });

            // Initial update
            UpdateValueLabel();

            return container;
        }
    }
}