#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace HierarchyExtensions
{
    [ExecuteInEditMode]
    public class EditorDivider
        : MonoBehaviour
    {
        [ColorUsage(false, false)]
        public Color Color = new Color(0.0f, 0.75f, 1.0f);
        public string Icon = "d_Favorite";

        [MenuItem("GameObject/Editor Divider/Environment", false, 10)]
        private static void CreateEnvironmentDivider() => CreateDivider(new Color(0.180f, 0.800f, 0.443f), "Environment");

        [MenuItem("GameObject/Editor Divider/Lighting", false, 10)]
        private static void CreateLightingDivider() => CreateDivider(new Color(0.945f, 0.768f, 0.058f), "Lighting");

        [MenuItem("GameObject/Editor Divider/Logic", false, 10)]
        private static void CreateLogicDivider() => CreateDivider(new Color(0.905f, 0.298f, 0.235f), "Logic");

        [MenuItem("GameObject/Editor Divider/UI", false, 10)]
        private static void CreateUIDivider() => CreateDivider(new Color(0.203f, 0.596f, 0.858f), "User Interface");

        [MenuItem("GameObject/Editor Divider/Miscellaneous", false, 10)]
        private static void CreateMiscDivider() => CreateDivider(new Color(0.607f, 0.349f, 0.713f), "Miscellaneous");

        [MenuItem("GameObject/Editor Divider/Custom", false, 10)]
        private static void CreateCustomDivider() => CreateDivider(new Color(0.584f, 0.647f, 0.650f), "Custom");

        private static void CreateDivider(Color color, string name)
        {
            EditorDivider divider = new GameObject(name, typeof(EditorDivider)).GetComponent<EditorDivider>();
            divider.Color = color;
            divider.tag = "EditorOnly";
            GameObjectUtility.SetParentAndAlign(divider.gameObject, Selection.activeGameObject);
            Undo.RegisterCreatedObjectUndo(divider.gameObject, "Created divider");
            Selection.activeObject = divider.gameObject;
        }
    }
}
#endif