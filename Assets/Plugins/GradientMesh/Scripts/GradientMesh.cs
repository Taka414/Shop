using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace GradientMesh
{
	[ExecuteInEditMode]
	public class GradientMesh : MonoBehaviour
	{
		private Mesh _mesh;
		private List<Vector3> _vertices = new List<Vector3>(200);
		private List<Vector3> _uvs = new List<Vector3>(200);
		private List<Color> _colors = new List<Color>(200);
		private int[] _triangles;
		

		public Gradient gradient;

		public GradientType gradientType = GradientType.Linear;
		public int sectorCount = 36;

		private List<GradientMeshColor> _colorPoints = new List<GradientMeshColor>(10);

		public int sortingLayer = 0;
		public int orderInLayer = 0;

		private bool _isCanvas = false;
		private MeshFilter _mf;
		private MeshRenderer _mr;
		private CanvasRenderer _cr;
		public Material useMaterial = null;
		
		public int GetTriangleCount { get; private set; }
		
		private void Awake()
		{
			if (gradient == null)
			{
				gradient = new Gradient();
				gradient.SetKeys(
					new GradientColorKey[]
					{
						new GradientColorKey(Color.green, 0.0f),
						new GradientColorKey(Color.red, 1.0f)
					},
					new GradientAlphaKey[]
					{
						new GradientAlphaKey(1f, 0.0f),
						new GradientAlphaKey(1f, 1.0f)
					}
				);
			}
			#if UNITY_EDITOR
			if (useMaterial == null)
			{
				useMaterial = AssetDatabase.GetBuiltinExtraResource<Material>("Sprites-Default.mat");
			}
			#endif
			TakeCareOfComponents();
			BuildMesh();
		}
		private void Update()
		{
			#if UNITY_EDITOR
			Tools.pivotMode = PivotMode.Pivot;
			if (transform.hasChanged)
			{
				if (_isCanvas != IsChildOfCanvas(transform))
				{
					_isCanvas = !_isCanvas;
					TakeCareOfComponents();
					BuildMesh();
				}
			}
			#endif
		}
		private void TakeCareOfComponents()
		{
			if (_isCanvas)
			{
				DestroyImmediate(GetComponent<MeshFilter>());
				DestroyImmediate(GetComponent<MeshRenderer>());
				if (GetComponent<CanvasRenderer>() == null) gameObject.AddComponent<CanvasRenderer>();
				_cr = GetComponent<CanvasRenderer>();
				if (_cr.GetMaterial() == null) _cr.SetMaterial(useMaterial, null);
			}
			else
			{
				DestroyImmediate(GetComponent<CanvasRenderer>());
				if (GetComponent<MeshFilter>() == null) gameObject.AddComponent<MeshFilter>();
				if (GetComponent<MeshRenderer>() == null) gameObject.AddComponent<MeshRenderer>();
				_mf = GetComponent<MeshFilter>();
				_mr = GetComponent<MeshRenderer>();
				if (_mr.sharedMaterial == null) _mr.sharedMaterial = useMaterial;
				sortingLayer = _mr.sortingLayerID;
				orderInLayer = _mr.sortingOrder;
			}
		}

		public void BuildMesh()
		{
			if (sectorCount < 3)
			{
				sectorCount = 3;
			}
			//Reset and clear everything to rebuild from scratch
			if (_mesh == null)
			{
				_mesh = new Mesh { name = "GradientMesh" };
			}
			_mesh.Clear();
			_vertices.Clear();
			_uvs.Clear();
			_colors.Clear();
			BuildGradientPoints();
			if (gradientType == GradientType.Linear)
			{
				BuildLinearMesh();
			}
			else
			{
				BuildRadialMesh();
			}
			_mesh.SetVertices(_vertices);
			_mesh.SetUVs(0, _uvs);
			_mesh.SetColors(_colors);
			_mesh.SetTriangles(_triangles, 0);
			_mesh.RecalculateBounds();
			if (_isCanvas)
			{
				if (_cr == null) _cr = GetComponent<CanvasRenderer>();
				_cr.SetMesh(_mesh);
			}
			else
			{
				if (_mf == null) _mf = GetComponent<MeshFilter>();
				_mf.sharedMesh = _mesh;
			}

			if (_mr != null)
			{
				_mr.sortingLayerID = sortingLayer;
				_mr.sortingOrder = orderInLayer;
			}
		}
		private void BuildGradientPoints()
		{
			_colorPoints.Clear();
			for (var i = 0; i < gradient.colorKeys.Length; i++)
			{
				_colorPoints.Add(new GradientMeshColor(
					gradient.Evaluate(gradient.colorKeys[i].time),
					gradient.colorKeys[i].time
				));
			}
			for (var i = 0; i < gradient.alphaKeys.Length; i++)
			{
				var found = -1;
				for (var j = 0; j < _colorPoints.Count; j++)
				{
					if (Mathf.Approximately(gradient.alphaKeys[i].time, _colorPoints[j].Position))
					{
						found = j;
						break;
					}
				}
				if (found == -1)
				{
					_colorPoints.Add(new GradientMeshColor(gradient.Evaluate(gradient.alphaKeys[i].time),
						gradient.alphaKeys[i].time));
				}
			}
			_colorPoints.Sort((cp1, cp2) => cp1.Position.CompareTo(cp2.Position));
			//Add 0.0 and 1.0 points if they're not explicitly set
			if (_colorPoints[0].Position != 0f)
			{
				_colorPoints.Insert(0, new GradientMeshColor(_colorPoints[0].Color, 0f));
			}
			if (_colorPoints[_colorPoints.Count - 1].Position != 1f)
			{
				_colorPoints.Add(new GradientMeshColor(_colorPoints[_colorPoints.Count - 1].Color, 0f));
			}
			//Handle Fixed gradients
			if (gradient.mode == GradientMode.Fixed)
			{
				const float step = 0.0001f;
				var newColorPoints = new List<GradientMeshColor>(10);
				for (var i = 1; i < _colorPoints.Count; i++)
				{
					newColorPoints.Add(new GradientMeshColor(gradient.Evaluate(_colorPoints[i - 1].Position + step),
						_colorPoints[i - 1].Position + step));
				}
				_colorPoints.AddRange(newColorPoints);
				_colorPoints.Sort((cp1, cp2) => cp1.Position.CompareTo(cp2.Position));
			}
#if UNITY_EDITOR
            if (PlayerSettings.colorSpace == ColorSpace.Linear)
			{
				for (var i = 0; i < _colorPoints.Count; i++)
				{
					_colorPoints[i].Color = _colorPoints[i].Color.linear;
				}
			}
#endif
        }
        private void BuildLinearMesh()
		{
			const int pX = 2;
			var squareNum = -1;
			GetTriangleCount = (pX * _colorPoints.Count) - 2;
			if (_triangles == null || _triangles.Length != GetTriangleCount * 3) _triangles = new int[GetTriangleCount * 3];
			for (var y = 0; y < _colorPoints.Count; y++)
			{
				for (var x = 0; x < pX; x++)
				{
					_vertices.Add(new Vector3(
						(((float)x / (float)(pX - 1)) - 0.5f),
						_colorPoints[y].Position - 0.5f,
						0f
					));
					_uvs.Add(new Vector3(
						((float)x / (float)(pX - 1)),
						_colorPoints[y].Position - 0.5f,
						0f
					));
					_colors.Add(_colorPoints[y].Color);
					if (x > 0 && y > 0)
					{
						var vertexNum = x + (y * pX);
						squareNum++;
						_triangles[squareNum * 6] = vertexNum - pX - 1;
						_triangles[squareNum * 6 + 1] = vertexNum - 1;
						_triangles[squareNum * 6 + 2] = vertexNum;
						_triangles[squareNum * 6 + 3] = vertexNum;
						_triangles[squareNum * 6 + 4] = vertexNum - pX;
						_triangles[squareNum * 6 + 5] = vertexNum - pX - 1;
					}
				}
			}
		}
		private void BuildRadialMesh()
		{
			const float radius = 0.5f;
			GetTriangleCount = sectorCount + (((_colorPoints.Count - 2) * sectorCount) * 2);
			if (_triangles == null || _triangles.Length != GetTriangleCount * 3) _triangles = new int[GetTriangleCount * 3];
			_vertices.Add(Vector3.zero);
			_colors.Add(_colorPoints[0].Color);
			for (var i = 1; i < _colorPoints.Count; i++)
			{
				for (var s = 0; s < sectorCount; s++)
				{
					var a = (((360f / sectorCount) * s) * Mathf.Deg2Rad);
					_vertices.Add(new Vector2(
						(float)(Mathf.Cos(a) * _colorPoints[i].Position * radius),
						(float)(Mathf.Sin(a) * _colorPoints[i].Position * radius)
					));
					_colors.Add(_colorPoints[i].Color);
				}
				if (i == 1)
				{
					for (var s = 0; s < sectorCount; s++)
					{
						_triangles[0 + s * 3] = 0;
						_triangles[1 + s * 3] = (s + 2 > sectorCount ? 1 : s + 2);
						_triangles[2 + s * 3] = s + 1;
					}
				}
				else
				{
					for (var s = 0; s < sectorCount; s++)
					{
						var si = (sectorCount * 3) + (i - 2) * (sectorCount * 6) + s * 6;
						_triangles[0 + si] = ((i - 2) * sectorCount) + s + 1;
						_triangles[1 + si] = ((i - 2) * sectorCount) + (s + 1 < sectorCount ? s + 2 : 1);
						_triangles[2 + si] = ((i - 1) * sectorCount) + s + 1;
						_triangles[3 + si] = ((i - 2) * sectorCount) + (s + 1 < sectorCount ? s + 2 : 1);
						_triangles[4 + si] = ((i - 1) * sectorCount) + (s + 1 < sectorCount ? s + 2 : 1);
						_triangles[5 + si] = ((i - 1) * sectorCount) + s + 1;
					}
				}
			}
		}
		private static bool IsChildOfCanvas(Transform t)
		{
			while (true)
			{
				if (t.GetComponent<Canvas>() != null)
				{
					return true;
				}
				else if (t.parent != null)
				{
					t = t.parent;
				}
				else
				{
					return false;
				}
			}
		}
		public enum GradientType
		{
			Linear,
			Radial
		}
		private class GradientMeshColor
		{
			public Color Color;
			public float Position;
			public GradientMeshColor(Color color, float position)
			{
				Color = color;
				Position = position;
			}
		}
	}
}