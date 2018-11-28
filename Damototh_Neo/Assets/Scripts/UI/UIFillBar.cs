using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class UIFillBar : MonoBehaviour 
{
    public enum FillDirection
    {
        Right,
        Left
    }
    [Header("Parameters")]
    [Space]
    [SerializeField] [Range(0f, 1f)] private float _fill;
    [SerializeField] [Range(0f, 1f)] private float _lerpSpeed;
    [SerializeField] private FillDirection _fillDirection = FillDirection.Right;

    [Header("References")]
    [Space]
    [SerializeField] private RectTransform _fillTransform;

    private float _currentFill;

    private void Awake()
    {
        _currentFill = _fill;
    }

    protected virtual void Update()
    {
        UpdateFill();
    }

    private void UpdateFill()
    {
        _currentFill = Mathf.Lerp(_currentFill, _fill, _lerpSpeed);

        _fillTransform.anchoredPosition = _fillTransform.anchoredPosition.SetX((1 - _currentFill) * _fillTransform.sizeDelta.x * (_fillDirection == FillDirection.Right ? -1 : 1));
    }

    public void SetFill(float fill)
    {
        _fill = Mathf.Clamp(fill, 0f, 1f);
    }


    private void OnValidate()
    {
        if (_fillTransform == null)
        {
            return;
        }

        Awake();
        UpdateFill();
    }
}
