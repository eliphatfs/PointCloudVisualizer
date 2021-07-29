using UnityEngine;

public enum ERenderType
{
    [InspectorName("Basic (Low CPU Cost)")]
    Basic,
    Colored,
    Flat,
    Bright
}

public static class RenderTypeExtensions
{
    public static Material GetMaterialFromSettings(this ERenderType renderType, PointCloudGlobalSettings pointCloudGlobalSettings)
    {
        switch (renderType)
        {
            case ERenderType.Basic:
                return pointCloudGlobalSettings.basic;
            case ERenderType.Colored:
                return pointCloudGlobalSettings.colored;
            case ERenderType.Flat:
                return pointCloudGlobalSettings.flat;
            case ERenderType.Bright:
                return pointCloudGlobalSettings.bright;
            default:
                throw new System.ArgumentOutOfRangeException(nameof(renderType));
        }
    }
}