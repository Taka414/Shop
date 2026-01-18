#if UNITY_EDITOR
//
// (C) 2022 Takap.
//

using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Takap.UI.Editor
{
    public static class SpritterUtility
    {
        [MenuItem("GameObject/UI/Spritter/Horizontal", priority = 10)]
        public static void AddSpritterHorizontal()
        {
            var g = new GameObject { name = "HorizontalSpritter" };

            Transform parent = GetParentTransform();
            g.transform.SetParent(parent, false);
            g.AddComponent<HorizontalSpritter>();
            Selection.activeGameObject = g;

            Undo.RegisterCreatedObjectUndo(g, "HorizontalSplitter Created");
            EditorUtility.SetDirty(g);
        }

        [MenuItem("GameObject/UI/Spritter/Vertical", priority = 10)]
        public static void AddSpritterVertical()
        {
            var g = new GameObject { name = "VerticalSpritter" };

            Transform parent = GetParentTransform();
            g.transform.SetParent(parent, false);
            g.AddComponent<VerticalSpritter>();
            Selection.activeGameObject = g;

            Undo.RegisterCreatedObjectUndo(g, "VerticalSpritter Created");
            EditorUtility.SetDirty(g);
        }

        private static Transform GetParentTransform()
        {
            Transform parent;
            if (Selection.activeGameObject != null &&
                Selection.activeGameObject.GetComponentInParent<Canvas>() != null)
            {
                parent = Selection.activeGameObject.transform;
            }
            else
            {
                Canvas c = GetCanvas();
                AddAdditionalShaderChannelsToCanvas(c);
                parent = c.transform;
            }

            return parent;
        }

        private static Canvas GetCanvas()
        {
            StageHandle handle = StageUtility.GetCurrentStageHandle();
            if (!handle.FindComponentOfType<Canvas>())
            {
                EditorApplication.ExecuteMenuItem("GameObject/UI/Canvas");
            }

            Canvas c = handle.FindComponentOfType<Canvas>();
            return c;
        }

        internal static void AddAdditionalShaderChannelsToCanvas(Canvas c)
        {
            AdditionalCanvasShaderChannels additionalShaderChannels = c.additionalShaderChannels;
            additionalShaderChannels |= AdditionalCanvasShaderChannels.TexCoord1;
            additionalShaderChannels |= AdditionalCanvasShaderChannels.TexCoord2;
            c.additionalShaderChannels = additionalShaderChannels;
        }
    }
}

#endif