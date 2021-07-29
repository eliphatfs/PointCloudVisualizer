using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEditor;
using LitJson;


[DisallowMultipleComponent]
public class PointCloudRoot : MonoBehaviour
{
    public PointCloudGlobalSettings settings;
    public TextAsset dataSource;
    [Serializable]
    public class RenderKey
    {
        public string key;
        public ERenderType renderType;
        public bool enablePhysics;
        public Color color;
        public Gradient gradient;
        public string externalColorKey;
        public string externalAlphaKey;
        public float pointSize;
        public float hdrFactor;
        public AnimationCurve alphaMappingCurve = AnimationCurve.Linear(0, 0, 1, 1);
        public ERecolorType recolorType;
    }
    [Serializable]
    public class Config
    {
        public int objectIndex = 0;
        public List<RenderKey> renderKeys = new List<RenderKey>();
    }
    public Config config;

    private JsonData _jsonData;
    public void Start()
    {
        _jsonData = JsonMapper.ToObject(dataSource.text)[config.objectIndex];
        foreach (var key in config.renderKeys) Render(key);
    }

    public void Render(RenderKey key)
    {
        var points = JsonMapper.ToObject<List<double[]>>(_jsonData[key.key].ToJson());
        List<double[]> colors = null;
        List<double> alphas = null;
        if (key.renderType != ERenderType.Basic && key.recolorType == ERecolorType.FromData)
            colors = JsonMapper.ToObject<List<double[]>>(_jsonData[key.externalColorKey].ToJson());
        if (key.renderType != ERenderType.Basic && key.recolorType == ERecolorType.FixedWithAlphaData)
            alphas = JsonMapper.ToObject<List<double>>(_jsonData[key.externalAlphaKey].ToJson());
        for (int i = 0; i < points.Count; i++)
        {
            var obj = Instantiate(settings.basePrototype, transform);
            obj.transform.localPosition = points[i].ToVector3();
            obj.transform.localScale = Vector3.one * key.pointSize;
            if (key.enablePhysics) obj.AddComponent<SphereCollider>();
            if (key.renderType != ERenderType.Basic)
                switch (key.recolorType)
                {
                    case ERecolorType.GradientByPointIndex:
                        obj.GetComponent<MeshRenderer>().material.color = key.gradient.Evaluate(i / (float)(points.Count - 1)) * key.hdrFactor;
                        break;
                    case ERecolorType.Fixed:
                        obj.GetComponent<MeshRenderer>().material.color = key.color * key.hdrFactor;
                        break;
                    case ERecolorType.FromData:
                        obj.GetComponent<MeshRenderer>().material.color = colors[i].ToColor() * key.hdrFactor;
                        break;
                    case ERecolorType.FixedWithAlphaData:
                        obj.GetComponent<MeshRenderer>().material.color = colors[i].ToColor().ReplaceAlpha(key.alphaMappingCurve.Evaluate((float)alphas[i])) * key.hdrFactor;
                        break;
                }
        }
    }
}


[CustomEditor(typeof(PointCloudRoot))]
public class PointCloudRootEditor : Editor
{
    public PointCloudRoot edited => target as PointCloudRoot;
    public override void OnInspectorGUI()
    {
        EditorGUILayout.PropertyField(
            serializedObject.FindProperty("dataSource"),
            new GUIContent("Data Source", "Drag data source .json file here")
        );
        EditorGUILayout.PropertyField(
            serializedObject.FindProperty("settings"),
            new GUIContent("Settings", "Drag global settings asset file here")
        );
        EditorGUILayout.PropertyField(
            serializedObject.FindProperty("config").FindPropertyRelative("objectIndex"),
            new GUIContent("Object Index", "Index of rendered object in the list in the .json file")
        );
        if (GUILayout.Button("Add Render Key"))
        {
            Undo.RecordObject(edited, "Add Render Key");
            edited.config.renderKeys.Add(new PointCloudRoot.RenderKey());
        }
        foreach (var key in edited.config.renderKeys)
        {
            Helper.DrawHorizontalLine();
            EditorGUI.BeginChangeCheck();
            var pointsKey = EditorGUILayout.DelayedTextField("Key", key.key);
            var enablePhysics = EditorGUILayout.Toggle("Enable Physics", key.enablePhysics);
            var renderType = (ERenderType)EditorGUILayout.EnumPopup("Render Type", key.renderType);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(edited, "Edit Render Key");
                key.key = pointsKey;
                key.enablePhysics = enablePhysics;
                key.renderType = renderType;
            }
            if (renderType != ERenderType.Basic)
            {
                EditorGUI.BeginChangeCheck();
                var recolorType = (ERecolorType)EditorGUILayout.EnumPopup("Recolor Type", key.recolorType);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(edited, "Edit Recolor Type");
                    key.recolorType = recolorType;
                }
                switch (recolorType)
                {
                    case ERecolorType.None: break;
                    case ERecolorType.Fixed:
                        EditorGUI.BeginChangeCheck();
                        var color = EditorGUILayout.ColorField("Color", key.color);
                        var hdrf = EditorGUILayout.DelayedFloatField("HDR Factor", key.hdrFactor);
                        if (EditorGUI.EndChangeCheck())
                        {
                            Undo.RecordObject(edited, "Edit Recolor");
                            key.color = color;
                            key.hdrFactor = hdrf;
                        }
                        break;
                    case ERecolorType.FixedWithAlphaData:
                        EditorGUI.BeginChangeCheck();
                        var colora = EditorGUILayout.ColorField("Color", key.color);
                        var hdra = EditorGUILayout.DelayedFloatField("HDR Factor", key.hdrFactor);
                        var ak = EditorGUILayout.DelayedTextField("Alpha Key", key.externalAlphaKey);
                        var amc = EditorGUILayout.CurveField("Alpha Curve", key.alphaMappingCurve);
                        if (EditorGUI.EndChangeCheck())
                        {
                            Undo.RecordObject(edited, "Edit Recolor");
                            key.color = colora;
                            key.hdrFactor = hdra;
                            key.externalAlphaKey = ak;
                            key.alphaMappingCurve = amc;
                        }
                        break;
                    case ERecolorType.FromData:
                        EditorGUI.BeginChangeCheck();
                        var ck = EditorGUILayout.DelayedTextField(new GUIContent("Color Key", "Colors can be either RGB or RGBA lists."), key.externalColorKey);
                        var hdre = EditorGUILayout.DelayedFloatField("HDR Factor", key.hdrFactor);
                        if (EditorGUI.EndChangeCheck())
                        {
                            Undo.RecordObject(edited, "Edit Recolor");
                            key.externalColorKey = ck;
                            key.hdrFactor = hdre;
                        }
                        break;
                    case ERecolorType.GradientByPointIndex:
                        EditorGUI.BeginChangeCheck();
                        var gr = EditorGUILayout.GradientField(new GUIContent("Gradient"), key.gradient);
                        var hdrg = EditorGUILayout.DelayedFloatField("HDR Factor", key.hdrFactor);
                        if (EditorGUI.EndChangeCheck())
                        {
                            Undo.RecordObject(edited, "Edit Recolor");
                            key.gradient = gr;
                            key.hdrFactor = hdrg;
                        }
                        break;
                }
            }
        }
        Helper.DrawHorizontalLine();
        serializedObject.ApplyModifiedProperties();
    }
}
