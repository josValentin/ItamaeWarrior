using UnityEngine;

namespace Utils.RectTransforms
{
    // Comment to detect on collab
    public enum RectAnchorPoint
    {
        UpperLeft,
        UpperMiddle,
        UpperRight,
        MiddleLeft,
        Middle,
        MiddleRight,
        LowerLeft,
        LowerMiddle,
        LowerRight,
        VerticalLeft,
        verticalMiddle,
        VerticalRight,
        HorizontalUpper,
        HorizontalMiddle,
        HorizontalLower,
        FullExtended
    }


    public static class RectTransformUtils
    {
        /// <summary>
        /// Set the X value in sizeDelta of recTransform
        /// </summary>
        public static void SetAnchorWidth(this RectTransform rect, float X)
        {
            rect.sizeDelta = new Vector2(X, rect.sizeDelta.y);
        }

        /// <summary>
        /// Set the Y value in sizeDelta of recTransform
        /// </summary>
        public static void SetAnchorHeight(this RectTransform rect, float Y)
        {
            rect.sizeDelta = new Vector2(rect.sizeDelta.x, Y);
        }

        /// <summary>
        /// Set the X and Y value in sizeDelta of recTransform
        /// </summary>
        public static void SetAnchorSize(this RectTransform rect, float X, float Y)
        {
            rect.sizeDelta = new Vector2(X, Y);
        }

        /// <summary>
        /// Set the X value in anchoredPosition of recTransform
        /// </summary>
        public static void SetAnchorPosX(this RectTransform rect, float X)
        {
            rect.anchoredPosition = new Vector2(X, rect.anchoredPosition.y);
        }

        /// <summary>
        /// Set the Y value in anchoredPosition of recTransform
        /// </summary>
        public static void SetAnchorPosY(this RectTransform rect, float Y)
        {
            rect.anchoredPosition = new Vector2(rect.anchoredPosition.x, Y);
        }

        /// <summary>
        /// Set the Y value in anchoredPosition of recTransform
        /// </summary>
        public static void AddAnchorPosX(this RectTransform rect, float X)
        {
            rect.anchoredPosition = new Vector2(rect.anchoredPosition.x + X, rect.anchoredPosition.y);
        }

        /// <summary>
        /// Set the Y value in anchoredPosition of recTransform
        /// </summary>
        public static void AddAnchorPosY(this RectTransform rect, float Y)
        {
            rect.anchoredPosition = new Vector2(rect.anchoredPosition.x, rect.anchoredPosition.y + Y);
        }

        /// <summary>
        /// Set the X and Y value in anchoredPosition of recTransform
        /// </summary>
        public static void SetAnchorPos(this RectTransform rect, float X, float Y)
        {
            rect.anchoredPosition = new Vector2(X, Y);
        }

        /// <summary>
        /// Set the X and Y value in localScale of recTransform
        /// </summary>
        public static void SetLocalScale(this RectTransform rect, float X, float Y)
        {
            rect.localScale = new Vector2(X, Y);
        }

        /// <summary>
        /// Clear/Destroys the childs of an object
        /// </summary>
        public static void Clear(this Transform transform, int alowedElementsLength = 0)
        {
            if (transform.childCount > alowedElementsLength)
            {
                for (int i = alowedElementsLength; i < transform.childCount; i++)
                {
                    GameObject.Destroy(transform.GetChild(i).gameObject);
                }
            }
        }



        public static RectAnchorPoint GetAnchorPoint(this RectTransform rect)
        {
            if (rect.pivot == new Vector2(0.5f, 0.5f) && rect.anchorMin == Vector2.zero && rect.anchorMax == Vector2.one)
                return RectAnchorPoint.FullExtended;

            if (rect.pivot == new Vector2(0, 0.5f) && rect.anchorMin == Vector2.zero && rect.anchorMax == Vector2.up)
                return RectAnchorPoint.VerticalLeft;

            if (rect.pivot == new Vector2(1, 0.5f) && rect.anchorMin == Vector2.right && rect.anchorMax == Vector2.one)
                return RectAnchorPoint.VerticalRight;

            return RectAnchorPoint.Middle;
        }

        public static void SetAnchorPoint(this RectTransform rect, RectAnchorPoint rectAnchorPoint, bool changePivot = true)
        {
            switch (rectAnchorPoint)
            {
                case RectAnchorPoint.UpperLeft:
                    rect.SetAnchorMin(0, 1);
                    rect.SetAnchorMax(0, 1);
                    if (changePivot)
                        rect.SetPivot(0, 1);

                    break;
                case RectAnchorPoint.UpperMiddle:
                    rect.SetAnchorMin(0.5f, 1);
                    rect.SetAnchorMax(0.5f, 1);
                    if (changePivot)
                        rect.SetPivot(0.5f, 1);
                    break;
                case RectAnchorPoint.UpperRight:
                    rect.SetAnchorMin(1, 1);
                    rect.SetAnchorMax(1, 1);
                    if (changePivot)
                        rect.SetPivot(1, 1);
                    break;
                case RectAnchorPoint.MiddleLeft:
                    rect.SetAnchorMin(0, 0.5f);
                    rect.SetAnchorMax(0, 0.5f);
                    if (changePivot)
                        rect.SetPivot(0, 0.5f);
                    break;
                case RectAnchorPoint.Middle:
                    rect.SetAnchorMin(0.5f, 0.5f);
                    rect.SetAnchorMax(0.5f, 0.5f);
                    if (changePivot)
                        rect.SetPivot(0.5f, 0.5f);
                    break;
                case RectAnchorPoint.MiddleRight:
                    rect.SetAnchorMin(1, 0.5f);
                    rect.SetAnchorMax(1, 0.5f);
                    if (changePivot)
                        rect.SetPivot(1, 0.5f);
                    break;
                case RectAnchorPoint.LowerLeft:
                    rect.SetAnchorMin(0, 0);
                    rect.SetAnchorMax(0, 0);
                    if (changePivot)
                        rect.SetPivot(0, 0);
                    break;
                case RectAnchorPoint.LowerMiddle:
                    rect.SetAnchorMin(0.5f, 0);
                    rect.SetAnchorMax(0.5f, 0);
                    if (changePivot)
                        rect.SetPivot(0.5f, 0);
                    break;
                case RectAnchorPoint.LowerRight:
                    rect.SetAnchorMin(1, 0);
                    rect.SetAnchorMax(1, 0);
                    if (changePivot)
                        rect.SetPivot(1, 0);
                    break;
                case RectAnchorPoint.VerticalLeft:
                    rect.SetAnchorMin(0, 0);
                    rect.SetAnchorMax(0, 1);
                    if (changePivot)
                        rect.SetPivot(0, 0.5f);
                    break;
                case RectAnchorPoint.verticalMiddle:
                    rect.SetAnchorMin(0.5f, 0);
                    rect.SetAnchorMax(0.5f, 1);
                    if (changePivot)
                        rect.SetPivot(0.5f, 0.5f);
                    break;
                case RectAnchorPoint.VerticalRight:
                    rect.SetAnchorMin(1, 0);
                    rect.SetAnchorMax(1, 1);
                    if (changePivot)
                        rect.SetPivot(1, 0.5f);
                    break;
                case RectAnchorPoint.HorizontalUpper:
                    rect.SetAnchorMin(0, 1);
                    rect.SetAnchorMax(1, 1);
                    if (changePivot)
                        rect.SetPivot(0.5f, 1);
                    break;
                case RectAnchorPoint.HorizontalMiddle:
                    rect.SetAnchorMin(0, 0.5f);
                    rect.SetAnchorMax(1, 0.5f);
                    if (changePivot)
                        rect.SetPivot(0.5f, 0.5f);
                    break;
                case RectAnchorPoint.HorizontalLower:
                    rect.SetAnchorMin(0, 0);
                    rect.SetAnchorMax(1, 0);
                    if (changePivot)
                        rect.SetPivot(0.5f, 0);
                    break;
                case RectAnchorPoint.FullExtended:
                    rect.SetAnchorMin(0, 0);
                    rect.SetAnchorMax(1, 1);
                    if (changePivot)
                        rect.SetPivot(0.5f, 0.5f);
                    break;
                default:
                    break;
            }
        }

        public static void SetAnchorMin(this RectTransform rect, float X, float Y)
        {
            rect.anchorMin = new Vector2(X, Y);
        }

        public static void SetAnchorMax(this RectTransform rect, float X, float Y)
        {
            rect.anchorMax = new Vector2(X, Y);
        }

        public static void SetPivot(this RectTransform rect, float X, float Y)
        {
            rect.pivot = new Vector2(X, Y);
        }


        public static bool IsRectOutOfCamera(this RectTransform rect, Canvas canvas, float moreThan = 0f)
        {
            float width = Screen.width / canvas.scaleFactor;
            float height = Screen.height / canvas.scaleFactor;

            float left = -(width / 2);
            float right = width / 2;
            float up = height / 2;
            float down = -(height / 2);

            bool InRight = rect.anchoredPosition.x > right + moreThan;
            bool InLeft = rect.anchoredPosition.x < left + moreThan;
            bool InUp = rect.anchoredPosition.y > up + moreThan;
            bool InDown = rect.anchoredPosition.y < down + moreThan;

            if (InRight || InLeft || InUp || InDown)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static Vector2 WhenRectOutOfCamera(this RectTransform rect, RectTransform RectOverAll, Canvas canvas, float moreThan = 0f)
        {
            float width = Screen.width / canvas.scaleFactor;
            float height = Screen.height / canvas.scaleFactor;

            float left = -(width / 2) - moreThan;
            float right = (width / 2) + moreThan;
            float up = (height / 2) + moreThan;
            float down = -(height / 2) - moreThan;

            bool InRight = rect.GetAnchoredPosIn(RectOverAll).x > right;
            bool InLeft = rect.GetAnchoredPosIn(RectOverAll).x < left;
            bool InUp = rect.GetAnchoredPosIn(RectOverAll).y > up;
            bool InDown = rect.GetAnchoredPosIn(RectOverAll).y < down;

            if (InRight && InUp)
            {
                return new Vector2(right, up);
            }
            else if (InRight && InDown)
            {
                return new Vector2(right, down);
            }
            else if (InLeft && InUp)
            {
                return new Vector2(left, up);
            }
            else if (InLeft && InDown)
            {
                return new Vector2(left, down);
            }
            else if (InRight && !InUp && !InDown)
            {
                return new Vector2(right, rect.GetAnchoredPosIn(RectOverAll).y);
            }
            else if (InLeft && !InUp && !InDown)
            {
                return new Vector2(left, rect.GetAnchoredPosIn(RectOverAll).y);
            }
            else if (InUp && !InRight && !InLeft)
            {
                return new Vector2(rect.GetAnchoredPosIn(RectOverAll).x, up);

            }
            else if (InDown && !InRight && !InLeft)
            {
                return new Vector2(rect.GetAnchoredPosIn(RectOverAll).x, down);

            }
            else
            {
                return Vector2.zero;
            }
        }

        public static Vector2 GetAnchoredPosIn(this RectTransform from, RectTransform to)
        {
            Vector2 localPoint;
            Vector2 fromPivotDerivedOffset = new Vector2(from.rect.width * from.pivot.x + from.rect.xMin, from.rect.height * from.pivot.y + from.rect.yMin);
            Vector2 screenP = RectTransformUtility.WorldToScreenPoint(null, from.position);
            screenP += fromPivotDerivedOffset;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(to, screenP, null, out localPoint);
            Vector2 pivotDerivedOffset = new Vector2(to.rect.width * to.pivot.x + to.rect.xMin, to.rect.height * to.pivot.y + to.rect.yMin);
            return to.anchoredPosition + localPoint - pivotDerivedOffset;
        }

        public static Vector2 GetAnchoredPosIn(this RectTransform from, RectTransform to, Vector2 v)
        {
            Vector2 localPoint;
            Vector2 fromPivotDerivedOffset = new Vector2(from.rect.width * from.pivot.x + from.rect.xMin, from.rect.height * from.pivot.y + from.rect.yMin);
            Vector2 screenP = RectTransformUtility.WorldToScreenPoint(null, from.position);
            screenP += fromPivotDerivedOffset;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(to, screenP, null, out localPoint);
            Vector2 pivotDerivedOffset = new Vector2(to.rect.width * to.pivot.x + to.rect.xMin, to.rect.height * to.pivot.y + to.rect.yMin);
            return to.anchoredPosition + v + localPoint - pivotDerivedOffset;
        }
    }
}

