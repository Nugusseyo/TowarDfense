using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace _Scripts.UI
{
    public class SpeedButton : MonoBehaviour
    {
        [SerializeField] private Sprite defaultImage; 
        [SerializeField] private Sprite doubleImage; 
        private bool _isDouble = false;
        [SerializeField] private Image image;
    
        public void SpeedDefault() => Time.timeScale = 1f;
        public void SpeedDouble() => Time.timeScale = 1.5f;

        public void ButtonClick()
        {
            if (_isDouble)
            {
                Time.timeScale = 1f;
                image.sprite = defaultImage;
            }
            else
            {
                Time.timeScale = 1.5f;
                image.sprite = doubleImage;
            }

            _isDouble = !_isDouble;
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
            if(Mathf.Approximately(Time.timeScale, 1.5f))
                Time.timeScale = 1f;
        }
    }
}
