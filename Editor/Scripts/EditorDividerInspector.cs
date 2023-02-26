namespace HierarchyExtensions
{
    using System;
    using System.Linq;
    using UnityEditor;
    using UnityEngine;

    [CanEditMultipleObjects]
    [CustomEditor(typeof(EditorDivider))]
    public class EditorDividerInspector
        : Editor
    {
        private static readonly Color[] _palette =
        {
            new(0.101f, 0.737f, 0.611f),
            new(0.180f, 0.800f, 0.443f),
            new(0.203f, 0.596f, 0.858f),
            new(0.607f, 0.349f, 0.713f),
            new(0.203f, 0.286f, 0.368f),
            new(0.945f, 0.768f, 0.058f),
            new(0.901f, 0.494f, 0.133f),
            new(0.905f, 0.298f, 0.235f),
            new(0.925f, 0.941f, 0.945f),
            new(0.584f, 0.647f, 0.650f),
        };

        private static string[] _iconOptions =
        {
            "d_Favorite",
            "d_Project",
            "d_FilterByLabel",
            "d_FilterByType",
            "d_Settings",
            "d_SceneViewCamera",
            "d_Lighting",
            "d_Profiler.UIDetails",
            "d_PreMatCube",
            "d_PreMatSphere",
            "d_Profiler.Audio",
            "d_Font Icon"
        };

        public override void OnInspectorGUI()
        {
            var divider = (EditorDivider) target;

            EditorGUILayout.BeginHorizontal();

            var selectedIndex = Array.IndexOf(_iconOptions, divider.Icon);
            if (selectedIndex < 0)
            {
                selectedIndex = 0;
                divider.Icon = _iconOptions[selectedIndex];
                EditorUtility.SetDirty(divider);
            }

            var button = new GUIStyle("RL FooterButton")
            {
                margin = new RectOffset(0, 0, 2, 0),
                fixedWidth = 24,
                fixedHeight = EditorGUIUtility.singleLineHeight,
            };

            var content = new GUIContent(EditorGUIUtility.IconContent(_iconOptions[selectedIndex]));
            content.tooltip = "Change divider icon";
            if (GUILayout.Button(content, button))
            {
                if (_iconOptions.Length - 1 <= selectedIndex)
                    selectedIndex = 0;
                else
                    ++selectedIndex;

                Undo.RecordObject(divider, "Change Divider Icon");
                divider.Icon = _iconOptions[selectedIndex];
                EditorUtility.SetDirty(divider);
            }

            EditorGUI.BeginChangeCheck();
            var color = EditorGUILayout.ColorField(divider.Color);

            var rect = GUILayoutUtility.GetLastRect();
            var w = rect.width;
            rect.y = rect.yMax + 2;
            rect.height = 16;
            ++rect.x;
            rect.width = w/_palette.Length - 3;
            foreach (var item in _palette)
            {
                GUI.backgroundColor = item;
                if (GUI.Button(rect, GUIContent.none, "WhiteBackground"))
                    color = item;

                GUI.backgroundColor = Color.white;
                rect.x = rect.xMax + 1;
            }

            EditorGUILayout.EndHorizontal();
            GUILayout.Space(20);

            if (EditorGUI.EndChangeCheck())
            {
                foreach (var t in targets)
                {
                    Undo.RecordObjects(targets, "Change Divider Color");
                    ((EditorDivider) t).Color = color;
                    EditorUtility.SetDirty(t);
                }
            }

        }
    }
}