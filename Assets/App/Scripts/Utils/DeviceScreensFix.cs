﻿
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Canvas))]
public class DeviceScreensFix : MonoBehaviour
{
#pragma warning disable
    public RectTransform SaveAreaTransform;
    [SerializeField] private Canvas _canvasParent;
    [SerializeField] private CanvasScaler _canvasScalerParent;
    [SerializeField] private bool useFitting = false;
#pragma warning restore
    private void Awake()
    {
        // ScreenFitting.CalculateCanvasScale(_canvas);

        var safeArea = Screen.safeArea;
        var anchorMin = safeArea.position;
        var anchorMax = anchorMin + safeArea.size;
        anchorMin.x /= _canvasParent.pixelRect.width;
        anchorMin.y /= _canvasParent.pixelRect.height;
        anchorMax.x /= _canvasParent.pixelRect.width;
        anchorMax.y /= _canvasParent.pixelRect.height;
        SaveAreaTransform.anchorMin = anchorMin;
        SaveAreaTransform.anchorMax = anchorMax;
        SaveAreaTransform.offsetMin = Vector2.zero;
        SaveAreaTransform.offsetMax = Vector2.zero;

        if (!useFitting)
        {
            return;
        }
        
        if (ScreenFitting.CurrentAspectRatio.x == 0)
        {
            ScreenFitting.CalculateCanvasScale(_canvasParent);
        }

        if (_canvasScalerParent == null)
            _canvasScalerParent = GetComponent<CanvasScaler>();

        _canvasScalerParent.matchWidthOrHeight = ScreenFitting.CurrentGameScale == CanvasScale.Heigth ? 1 : 0;


    }
}
