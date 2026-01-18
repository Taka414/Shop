using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif    

//[ExecuteInEditMode]
[ExecuteAlways]
public class AxisDistanceSortCameraHelper : MonoBehaviour
{
    void Start()
    {
        Camera camera = GetComponent<Camera>();
        camera.transparencySortMode = TransparencySortMode.CustomAxis;
        camera.transparencySortAxis = new Vector3(0, 1, 1);

#if UNITY_EDITOR
        foreach (SceneView sv in SceneView.sceneViews)
        {
            sv.camera.transparencySortMode = TransparencySortMode.CustomAxis;
            sv.camera.transparencySortAxis = new Vector3(0, 1, 1);
        }
#endif      
    }
}