using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarUI : MonoBehaviour
{
    private Slider progressSlider;
    private Quaternion initialQuaternion;

    private void Awake()
    {
        progressSlider = GetComponent<Slider>();
    }

    private void Start()
    {
        Hide();
    }

    public void SetValue(float value)
    {
        progressSlider.value = value;
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
