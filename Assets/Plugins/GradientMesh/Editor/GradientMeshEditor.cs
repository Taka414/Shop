using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Reflection;

namespace GradientMesh
{
	[CustomEditor(typeof(GradientMesh))]
	public class GradientMeshEditor : Editor
	{
		private GradientMesh _script;
		[MenuItem("GameObject/2D Object/Gradient Mesh")]
		private static void Create()
		{
			var go = new GameObject();
			go.AddComponent<GradientMesh>();
			go.name = "GradientMesh";
			go.GetComponent<GradientMesh>().useMaterial = AssetDatabase.GetBuiltinExtraResource<Material>("Sprites-Default.mat");
			if (SceneView.lastActiveSceneView != null)
			{
				go.transform.position = new Vector3(SceneView.lastActiveSceneView.pivot.x, SceneView.lastActiveSceneView.pivot.y, 0f);
			}
			if (Selection.activeGameObject != null)
			{
				go.transform.parent = Selection.activeGameObject.transform;
			}
			Selection.activeGameObject = go;
		}
		private void Awake()
		{
			_script = (GradientMesh)target;
		}
		public override void OnInspectorGUI()
		{
			var forceRepaint = false;
			if (Event.current.type == EventType.ValidateCommand)
			{
				if (Event.current.commandName == "UndoRedoPerformed")
				{
					forceRepaint = true;
				}
			}
			GUILayout.Space(5);
			EditorGUI.BeginChangeCheck();
			var gradient = EditorGUILayout.GradientField("Gradient", _script.gradient);
			if (EditorGUI.EndChangeCheck())
			{
				Undo.RecordObject(_script, "Change gradient");
				_script.gradient = gradient;
				forceRepaint = true;
				EditorUtility.SetDirty(_script);
			}
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.PrefixLabel(" ");
			if (GUILayout.Button(new GUIContent("Reverse", "Reverse the direction of the gradient")))
			{
				Undo.RecordObject(_script, "Gradient reverse");
				var gck = new GradientColorKey[_script.gradient.colorKeys.Length];
				for (var i = 0; i <= _script.gradient.colorKeys.Length - 1; i++)
				{
					gck[i] = _script.gradient.colorKeys[_script.gradient.colorKeys.Length - 1 - i];
					gck[i].time = 1f - gck[i].time;
				}
				var gak = new GradientAlphaKey[_script.gradient.alphaKeys.Length];
				for (var i = 0; i <= _script.gradient.alphaKeys.Length - 1; i++)
				{
					gak[i] = _script.gradient.alphaKeys[_script.gradient.alphaKeys.Length - 1 - i];
					gak[i].time = 1f - gak[i].time;
				}
				_script.gradient = new Gradient();
				_script.gradient.SetKeys(gck, gak);
				forceRepaint = true;
				EditorUtility.SetDirty(_script.transform);
			}
			EditorGUILayout.EndHorizontal();
			var gradientType = (GradientMesh.GradientType)EditorGUILayout.EnumPopup("Gradient type", _script.gradientType);
			if (gradientType != _script.gradientType)
			{
				Undo.RecordObject(_script, "Change gradient type");
				_script.gradientType = gradientType;
				forceRepaint = true;
				EditorUtility.SetDirty(_script);
			}
			if (_script.gradientType == GradientMesh.GradientType.Radial)
			{
				var sectorCount = EditorGUILayout.IntField(new GUIContent("Sector count", 
					"Number of sides the circle will have"), _script.sectorCount);
				if (sectorCount != _script.sectorCount)
				{
					Undo.RecordObject(_script, "Change sector count");
					_script.sectorCount = sectorCount;
					forceRepaint = true;
					EditorUtility.SetDirty(_script);
				}
			}
			GUILayout.Box(
				new GUIContent(_script.GetTriangleCount > 1
					? "The mesh has " + _script.GetTriangleCount.ToString() + " triangles"
					: (_script.GetTriangleCount == 1 ? "The mesh is just one triangle" : "The mesh has no triangles")),
				EditorStyles.helpBox);
			GUILayout.Space(10);
			var layerIDs = GetSortingLayerUniqueIDs();
			var layerNames = GetSortingLayerNames();
			var selected = -1;
			for (var i = 0; i < layerIDs.Length; i++)
			{
				if (layerIDs[i] == _script.sortingLayer)
				{
					selected = i;
				}
			}
			if (selected == -1)
			{
				for (var i = 0; i < layerIDs.Length; i++)
				{
					if (layerIDs[i] == 0)
					{
						selected = i;
					}
				}
			}
			EditorGUI.BeginChangeCheck();
			selected = EditorGUILayout.Popup("Sorting Layer", selected, layerNames);
			if (EditorGUI.EndChangeCheck())
			{
				Undo.RecordObject(_script, "Change sorting layer");
				_script.sortingLayer = layerIDs[selected];
				EditorUtility.SetDirty(_script);
			}
			EditorGUI.BeginChangeCheck();
			var order = EditorGUILayout.IntField("Order in Layer", _script.orderInLayer);
			if (EditorGUI.EndChangeCheck())
			{
				Undo.RecordObject(_script, "Change order in layer");
				_script.orderInLayer = order;
				EditorUtility.SetDirty(_script);
			}
			if (GUI.changed || forceRepaint)
			{
				var ed = (Editor[])Resources.FindObjectsOfTypeAll<Editor>();
				for (var i = 0; i < ed.Length; i++)
				{
					if (ed[i].GetType() == typeof(GradientMeshEditor))
					{
						ed[i].Repaint();
					}
				}
				_script.BuildMesh();
				SceneView.RepaintAll();
			}
		}
		private static int[] GetSortingLayerUniqueIDs()
		{
			var internalEditorUtilityType = typeof(InternalEditorUtility);
			var sortingLayerUniqueIDsProperty = internalEditorUtilityType.GetProperty("sortingLayerUniqueIDs",
				BindingFlags.Static | BindingFlags.NonPublic);
			return (int[])sortingLayerUniqueIDsProperty.GetValue(null, new object[0]);
		}
		private static string[] GetSortingLayerNames()
		{
			var internalEditorUtilityType = typeof(InternalEditorUtility);
			var sortingLayersProperty = internalEditorUtilityType.GetProperty("sortingLayerNames",
					BindingFlags.Static | BindingFlags.NonPublic);
			return (string[])sortingLayersProperty.GetValue(null, new object[0]);
		}
	}
}