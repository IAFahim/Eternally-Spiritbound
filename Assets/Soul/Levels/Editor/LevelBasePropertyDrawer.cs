using Soul.Levels.Runtime;
using UnityEditor;
using UnityEngine;

namespace Soul.Levels.Editor
{
    [CustomPropertyDrawer(typeof(LevelBase), true)]
    public class LevelBasePropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            var target = property.serializedObject.targetObject;
            var levelBase = fieldInfo.GetValue(target) as LevelBase;

            if (levelBase == null)
            {
                EditorGUI.LabelField(position, label.text, "Object is null");
                EditorGUI.EndProperty();
                return;
            }

            Rect foldoutRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
            property.isExpanded = EditorGUI.Foldout(foldoutRect, property.isExpanded, label, true);

            if (property.isExpanded)
            {
                EditorGUI.indentLevel++;

                Rect currentLevelRect = new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight, position.width, EditorGUIUtility.singleLineHeight);
                Rect guidRect = new Rect(position.x, currentLevelRect.y + EditorGUIUtility.singleLineHeight, position.width, EditorGUIUtility.singleLineHeight);
                Rect buttonsRect = new Rect(position.x, guidRect.y + EditorGUIUtility.singleLineHeight, position.width, EditorGUIUtility.singleLineHeight);

                // Display CurrentLevel (read-only)
                EditorGUI.BeginDisabledGroup(true);
                EditorGUI.IntField(currentLevelRect, "Current Level", levelBase.CurrentLevel);
                EditorGUI.EndDisabledGroup();

                // Display GUID (read-only)
                EditorGUI.BeginDisabledGroup(true);
                EditorGUI.TextField(guidRect, "GUID", levelBase.Guid);
                EditorGUI.EndDisabledGroup();

                // Add buttons for increasing and decreasing level
                EditorGUILayout.BeginHorizontal();
                if (GUI.Button(new Rect(buttonsRect.x, buttonsRect.y, buttonsRect.width / 2 - 2, buttonsRect.height), "Decrease Level"))
                {
                    levelBase.DecreaseLevel();
                    EditorUtility.SetDirty(target);
                }
                if (GUI.Button(new Rect(buttonsRect.x + buttonsRect.width / 2 + 2, buttonsRect.y, buttonsRect.width / 2 - 2, buttonsRect.height), "Increase Level"))
                {
                    levelBase.IncreaseLevel();
                    EditorUtility.SetDirty(target);
                }
                EditorGUILayout.EndHorizontal();
                

                EditorGUI.indentLevel--;
            }

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (!property.isExpanded)
                return EditorGUIUtility.singleLineHeight;

            float height = EditorGUIUtility.singleLineHeight * 4; // Foldout + CurrentLevel + GUID + buttons

            // Add height for all visible properties
            SerializedProperty prop = property.Copy();
            SerializedProperty endProp = prop.GetEndProperty();
            if (prop.NextVisible(true))
            {
                do
                {
                    if (SerializedProperty.EqualContents(prop, endProp))
                        break;
                    
                    height += EditorGUI.GetPropertyHeight(prop, true) + EditorGUIUtility.standardVerticalSpacing;
                }
                while (prop.NextVisible(false));
            }

            return height;
        }
    }
}