using System;
using UnityEngine;


public class Util {
    static Vector2[] ScreenSpaceCoords = new Vector2[8];

/// <summary>
/// Calculates the bounds that contain all of the models from the given array of renderers.
/// </summary>
/// <param name="renderers"> The renderers of an object we are trying to find the bounds of. </param>
/// <returns> The object's bounds. </returns>
    public static Bounds getBounds(Renderer[] renderers) {
        if(renderers == null) throw new ArgumentNullException("Parameter renderers cannot be null.");
        if(renderers.Length == 0) throw new ArgumentException("Array renderers cannot be empty.");
        
        Bounds bounds = renderers[0].bounds;
        for(int i = 0; i < renderers.Length; i++) bounds.Encapsulate(renderers[i].bounds);

        return bounds;
    }

/// <summary>
/// Calculates where a marker should be placed on the screen for an object bounded by bounds.
/// </summary>
/// <param name="bounds"> The worldspace bounds of the object whose marker's position we are calculating. </param>
/// <param name="camera"> The camera for which we are calculating the position for. </param>
/// <returns> The screen coordinates for where to place the marker. </returns>
    public static Vector2 getMarkerPos(Bounds bounds, Camera camera) {
        calculateScreenSpaceCoords(bounds, camera);

        float maxY = ScreenSpaceCoords[0].y;
        float minX = ScreenSpaceCoords[0].x;
        float maxX = ScreenSpaceCoords[0].x;

        for(int i = 0; i < 8; i++) {
            if(ScreenSpaceCoords[i].y > maxY) maxY = ScreenSpaceCoords[i].y;
            if(ScreenSpaceCoords[i].x > maxX) maxX = ScreenSpaceCoords[i].x;
            if(ScreenSpaceCoords[i].x < minX) minX = ScreenSpaceCoords[i].x;
        }

        return new Vector2((minX + maxX) / 2, maxY);
    }

    private static void calculateScreenSpaceCoords(Bounds bounds, Camera camera) {
        ScreenSpaceCoords[0] = camera.WorldToScreenPoint(new Vector3(bounds.center.x + bounds.extents.x, bounds.center.y + bounds.extents.y, bounds.center.z + bounds.extents.z));
        ScreenSpaceCoords[1] = camera.WorldToScreenPoint(new Vector3(bounds.center.x + bounds.extents.x, bounds.center.y + bounds.extents.y, bounds.center.z - bounds.extents.z));
        ScreenSpaceCoords[2] = camera.WorldToScreenPoint(new Vector3(bounds.center.x + bounds.extents.x, bounds.center.y - bounds.extents.y, bounds.center.z + bounds.extents.z));
        ScreenSpaceCoords[3] = camera.WorldToScreenPoint(new Vector3(bounds.center.x + bounds.extents.x, bounds.center.y - bounds.extents.y, bounds.center.z - bounds.extents.z));
        ScreenSpaceCoords[4] = camera.WorldToScreenPoint(new Vector3(bounds.center.x - bounds.extents.x, bounds.center.y + bounds.extents.y, bounds.center.z + bounds.extents.z));
        ScreenSpaceCoords[5] = camera.WorldToScreenPoint(new Vector3(bounds.center.x - bounds.extents.x, bounds.center.y + bounds.extents.y, bounds.center.z - bounds.extents.z));
        ScreenSpaceCoords[6] = camera.WorldToScreenPoint(new Vector3(bounds.center.x - bounds.extents.x, bounds.center.y - bounds.extents.y, bounds.center.z + bounds.extents.z));
        ScreenSpaceCoords[7] = camera.WorldToScreenPoint(new Vector3(bounds.center.x - bounds.extents.x, bounds.center.y - bounds.extents.y, bounds.center.z - bounds.extents.z));
    }

    public static void drawBounds(Bounds bounds)
    {
        Vector3 frontTopRight = new Vector3(bounds.center.x + bounds.extents.x, bounds.center.y + bounds.extents.y, bounds.center.z + bounds.extents.z);
        Vector3 frontTopLeft = new Vector3(bounds.center.x + bounds.extents.x, bounds.center.y + bounds.extents.y, bounds.center.z - bounds.extents.z);
        Vector3 frontBottomRight = new Vector3(bounds.center.x + bounds.extents.x, bounds.center.y - bounds.extents.y, bounds.center.z + bounds.extents.z);
        Vector3 frontBottomLeft = new Vector3(bounds.center.x + bounds.extents.x, bounds.center.y - bounds.extents.y, bounds.center.z - bounds.extents.z);
        Vector3 backTopRight = new Vector3(bounds.center.x - bounds.extents.x, bounds.center.y + bounds.extents.y, bounds.center.z + bounds.extents.z);
        Vector3 backTopLeft = new Vector3(bounds.center.x - bounds.extents.x, bounds.center.y + bounds.extents.y, bounds.center.z - bounds.extents.z);
        Vector3 backBottomRight = new Vector3(bounds.center.x - bounds.extents.x, bounds.center.y - bounds.extents.y, bounds.center.z + bounds.extents.z);
        Vector3 backBottomLeft = new Vector3(bounds.center.x - bounds.extents.x, bounds.center.y - bounds.extents.y, bounds.center.z - bounds.extents.z);

        //Draw top square
        Debug.DrawLine(frontTopLeft, frontTopRight, Color.red);
        Debug.DrawLine(frontTopRight, backTopRight, Color.red);
        Debug.DrawLine(backTopRight, backTopLeft, Color.red);
        Debug.DrawLine(backTopLeft, frontTopLeft, Color.red);
        
        //Draw bottom square
        Debug.DrawLine(frontBottomLeft, frontBottomRight, Color.red);
        Debug.DrawLine(frontBottomRight, backBottomRight, Color.red);
        Debug.DrawLine(backBottomRight, backBottomLeft, Color.red);
        Debug.DrawLine(backBottomLeft, frontBottomLeft, Color.red);

        //Draw connecting lines
        Debug.DrawLine(frontTopLeft, frontBottomLeft, Color.red);
        Debug.DrawLine(frontTopRight, frontBottomRight, Color.red);
        Debug.DrawLine(backTopLeft, backBottomLeft, Color.red);
        Debug.DrawLine(backTopRight, backBottomRight, Color.red);
    }
}