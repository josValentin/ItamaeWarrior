using UnityEngine;
using Utils.RectTransforms;

public class StandaloneUIAdapter : MonoBehaviour
{
    [Header("STANDALONE ADAPTER")]
    [Space]
    [SerializeField] private RectTransform rect;
    [SerializeField] private RectAnchorPoint targetAnchorPont;
    [SerializeField] private bool updatePivotOnAdjust = false;
    [Space]
    [SerializeField] private bool customPivot = false;
    [SerializeField] private Vector2 targetPivot;
    [SerializeField] private bool customAnchorPosX= true;
    [SerializeField] private float targetAnchor_X;
    [SerializeField] private bool customAnchorPosY = false;
    [SerializeField] private float targetAnchor_Y;
    [SerializeField] private bool customWidth = false;
    [SerializeField] private float targetWidth;
    [SerializeField] private bool customHeight= false;
    [SerializeField] private float targetHeight;

    // Start is called before the first frame update
#if UNITY_STANDALONE
    void Start()
    {
        if(!rect)
            rect = GetComponent<RectTransform>();

        rect.SetAnchorPoint(targetAnchorPont, updatePivotOnAdjust);

        if (customPivot)
            rect.pivot = targetPivot;

        if (customAnchorPosX)
            rect.SetAnchorPosX(targetAnchor_X);
        
        if(customAnchorPosY)
            rect.SetAnchorPosY(targetAnchor_Y);

        if (customWidth)
            rect.SetAnchorWidth(targetWidth);

        if (customHeight)
            rect.SetAnchorHeight(targetHeight);

    }
#endif
}
