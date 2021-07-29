using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEditor;

/// <summary>
/// Helper to snapshot transforms (local coordinates), for quick adjustments in play mode.
/// Temporary data may be lost after restarting editor.
/// </summary>
[DisallowMultipleComponent]
public class TransformSnapshot : MonoBehaviour
{
    public struct Data
    {
        public Vector3 position;
        public Vector3 scale;
        public Quaternion rotation;
    }

    public static Data[] storage = new Data[5];
}

[CustomEditor(typeof(TransformSnapshot))]
public class TransformSnapshotEditor : Editor
{
    public TransformSnapshot edited => target as TransformSnapshot;
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        for (var i = 0; i < TransformSnapshot.storage.Length; i++)
        {
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button($"Load {i}")) Load(i);
            if (GUILayout.Button($"Store {i}")) Store(i);
            EditorGUILayout.EndHorizontal();
        }
    }

    public void Store(int i)
    {
        TransformSnapshot.storage[i] = new TransformSnapshot.Data
        {
            position = edited.transform.localPosition,
            scale = edited.transform.localScale,
            rotation = edited.transform.localRotation,
        };
    }

    public void Load(int i)
    {
        edited.transform.localPosition = TransformSnapshot.storage[i].position;
        edited.transform.localScale = TransformSnapshot.storage[i].scale;
        edited.transform.localRotation = TransformSnapshot.storage[i].rotation;
    }
}
