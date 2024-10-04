using Soul.Tickers.Runtime;
using UnityEditor;
using UnityEngine;

namespace Soul.Tickers.Editor
{
    [CustomPropertyDrawer(typeof(IntervalTicker))]
    public class IntervalTickerDrawer : PropertyDrawer
    {
        private const float ProgressBarHeight = 20f;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            // Calculate rects
            var intervalRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
            var progressRect = new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight + 2,
                position.width, ProgressBarHeight);

            // Draw interval field
            var intervalProp = property.FindPropertyRelative("interval");
            EditorGUI.PropertyField(intervalRect, intervalProp, new GUIContent("Interval"));

            // Draw progress bar
            var currentTickProp = property.FindPropertyRelative("currentTick");

            float progress = (float)currentTickProp.intValue / intervalProp.intValue;
            EditorGUI.ProgressBar(progressRect, progress,
                $"Progress: {currentTickProp.intValue}/{intervalProp.intValue}");
            
            if (progressRect.Contains(Event.current.mousePosition))
            {
                EditorGUI.LabelField(progressRect, new GUIContent("Progress: " + progress.ToString("P")));
            }

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight  + ProgressBarHeight + 4;
        }
    }
}