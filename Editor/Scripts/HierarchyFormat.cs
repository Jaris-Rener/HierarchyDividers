using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace HierarchyExtensions
{
    public static class HierarchyFormat
    {
        private static GUIStyle _dividerLabelStyle;
        private static GUIStyle _childLabelStyle;
        private static string[] _visibleObjs;

        private static Color _backgroundColor => EditorGUIUtility.isProSkin
            ? new Color32(56, 56, 56, 255)
            : new Color32(194, 194, 194, 255);

        [InitializeOnLoadMethod]
        public static void Init()
        {
            EditorApplication.hierarchyWindowItemOnGUI += HierarchyWindowItem_CB;
        }

        private static void Test()
        {
            Debug.Log("This is a test :)");
        }

        private static void UpdateVisibleObjects()
        {
            // Holy heck this method really sucks - I could not find a callback or anything like onHierarchyItemExpanded.
            var window = Resources
                .FindObjectsOfTypeAll<EditorWindow>()
                .First(x => x.GetType().Name == "SceneHierarchyWindow");

            var type = window.GetType();
            var method = type.GetMethod("GetCurrentVisibleObjects", BindingFlags.Public | BindingFlags.Instance);
            var objs = (string[])method?.Invoke(window, null);
            _visibleObjs = objs;
        }

        private static void HierarchyWindowItem_CB(int id, Rect rect)
        {
            _dividerLabelStyle ??= new GUIStyle(EditorStyles.label) { fontStyle = FontStyle.Bold };
            _childLabelStyle ??= new GUIStyle(EditorStyles.label);

            var obj = EditorUtility.InstanceIDToObject(id);
            var gameObject = obj as GameObject;

            if (gameObject == null)
                return;

            var divider = gameObject.GetComponentInParent<EditorDivider>();
            if (divider == null)
                return;

            var isRoot = divider.gameObject == gameObject;

            if (Event.current.type is not EventType.Repaint)
                return;

            var isSelected = Selection.objects.Contains(obj);

            // Draw the background colour
            var backgroundRect = new Rect(rect);
            backgroundRect.x = 32f;
            backgroundRect.width += 512f;
            backgroundRect.height -= 2;
            backgroundRect.y += 1f;
            GUI.backgroundColor = _backgroundColor;
            GUI.Box(backgroundRect, GUIContent.none, "OverrideMargin");

            if (isRoot)
            {
                GUI.backgroundColor = divider.Color;
                GUI.Box(backgroundRect, GUIContent.none, "OverrideMargin");
            }
            else
            {
                GUI.backgroundColor = new Color(divider.Color.r, divider.Color.g, divider.Color.b, 0.1f);
                GUI.Box(backgroundRect, GUIContent.none, "OverrideMargin");
            }

            // Draw the selection box overlay
            if (isSelected)
            {
                var selectionColour = new Color(0f, 0.49f, 1f, 0.5f);
                GUI.backgroundColor = selectionColour;
                GUI.Box(backgroundRect, GUIContent.none, "OverrideMargin");
            }

            GUI.backgroundColor = Color.white;

            const float colorFactor = 0.35f;
            var contentColor = isRoot
                ? new Color(divider.Color.r*colorFactor, divider.Color.g*colorFactor, divider.Color.b*colorFactor)
                : Color.white;

            var iconRect = new Rect(rect);
            iconRect.width = 20;
            iconRect.height = 20;
            iconRect.x += -2f;
            iconRect.y += -2f;
            GUI.color = contentColor;
            GUI.Label(iconRect, isRoot
                ? EditorGUIUtility.IconContent(divider.Icon)
                : EditorGUIUtility.IconContent("d_GameObject Icon"));
            GUI.color = Color.white;

            var labelRect = new Rect(rect);
            labelRect.y -= 2f;
            labelRect.x += 17f;
            labelRect.height += 2f;
            GUI.contentColor = contentColor;
            GUI.Label(labelRect, gameObject.name, isRoot ? _dividerLabelStyle : _childLabelStyle);
            GUI.contentColor = Color.white;

            if (gameObject.transform.childCount > 0)
            {
                UpdateVisibleObjects();
                var expanded = !_visibleObjs.Contains(gameObject.transform.GetChild(0).name);
                var foldoutRect = new Rect(rect);
                foldoutRect.x -= 16f;
                GUI.color = contentColor;
                GUI.Label(foldoutRect, EditorGUIUtility.IconContent(expanded ? "IN foldout act" : "IN foldout act on"));
                GUI.color = Color.white;
            }

            EditorApplication.RepaintHierarchyWindow();
        }
    }
}