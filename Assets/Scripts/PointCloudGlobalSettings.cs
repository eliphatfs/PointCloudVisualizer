using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "pointCloudSettings", menuName = "Point Cloud/Global Settings")]
public class PointCloudGlobalSettings : ScriptableObject
{
    public GameObject basePrototype;
    public Material basic, colored, flat, bright;
}
