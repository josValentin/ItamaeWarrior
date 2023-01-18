using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InBaseCameraData : MonoBehaviour
{
    public static WorldSpaceData worldSpaceData;

    Camera cam;

    private void Awake()
    {
        if (worldSpaceData != null)
            return;

        cam = GetComponent<Camera>();

        Vector2 left_Bottom = cam.ScreenToWorldPoint(Vector2.zero);

        Vector2 right_Top = cam.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));


        worldSpaceData = new WorldSpaceData();

        worldSpaceData.Left = left_Bottom.x;
        worldSpaceData.Bottom = left_Bottom.y;
        worldSpaceData.Right = right_Top.x;
        worldSpaceData.Top = right_Top.y;
    }


    public class WorldSpaceData
    {
        public float Width;
        public float Height;
        public float Left;
        public float Right;
        public float Top;
        public float Bottom;


        public enum VectorTarget
        {
            RightTop,
            LeftTop,
            RightBottom,
            LeftBottom
        }

        public Vector2 GetTargetVector(VectorTarget vectorTarget)
        {
            switch (vectorTarget)
            {
                case VectorTarget.RightTop:
                    return new Vector2(Right, Top);
                case VectorTarget.LeftTop:
                    return new Vector2(Left, Top);
                case VectorTarget.RightBottom:
                    return new Vector2(Right, Bottom);
                case VectorTarget.LeftBottom:
                    return new Vector2(Left, Bottom);
                default:
                    return default;
            }


        }

        public Vector2 GetTargetVectorAdditiveByTarget(VectorTarget vectorTarget, float additiveX, float additiveY, bool invertedX = false, bool invertedY = false)
        {
            additiveX = Mathf.Abs(additiveX) * (!invertedX ? 1 : -1);
            additiveY = Mathf.Abs(additiveY) * (!invertedY ? 1 : -1);

            switch (vectorTarget)
            {
                case VectorTarget.RightTop:
                    return new Vector2(Right + additiveX, Top + additiveY);
                case VectorTarget.LeftTop:
                    return new Vector2(Left - additiveX, Top + additiveY);
                case VectorTarget.RightBottom:
                    return new Vector2(Right + additiveX, Bottom - additiveY);
                case VectorTarget.LeftBottom:
                    return new Vector2(Left - additiveX, Bottom - additiveY);
                default:
                    return default;
            }


        }
    }
}

