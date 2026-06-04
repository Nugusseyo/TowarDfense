using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SpeedButton : MonoBehaviour
{
    [SerializeField] private Sprite defaultImage; 
    [SerializeField] private Sprite doubleImage; 
    private bool isDouble = false;
    private Image _image;
    
    public void SpeedDefault() => Time.timeScale = 1f;
    public void SpeedDouble() => Time.timeScale = 2f;

    private void Awake()
    {
        _image = GetComponent<Image>();
    }

    public void ButtonClick()
    {
        if (isDouble)
        {
            Time.timeScale = 1f;
            _image.sprite = defaultImage;
        }
        else
        {
            Time.timeScale = 2f;
            _image.sprite = doubleImage;
        }

        isDouble = !isDouble;
    }

    #if UNITY_EDITOR
    void Update()
    {
        if (Keyboard.current.pKey.wasPressedThisFrame)
        {
            Time.timeScale += 1f;
        }

        if (Keyboard.current.oKey.wasPressedThisFrame)
        {
            Time.timeScale -= 1f;
        }
    }
    #endif

    private void OnDisable()
    {
        Time.timeScale = 1f;
    }
}
