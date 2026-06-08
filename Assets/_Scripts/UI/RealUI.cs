using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _Scripts.UI
{
    public class RealUI : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        private Renderer _renderer;

        private void Awake()
        {
            _renderer = GetComponent<Renderer>();
            if(_renderer != null)
                _renderer.material.color = Color.gray;
        }

        //메인 카메라에 RayCast가 있고, UI에 EventCam이 있으면 작동함 야르
        public void OnPointerClick(PointerEventData eventData)
        {
            Destroy(gameObject);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _renderer.material.color = Color.green;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _renderer.material.color = Color.gray;
        }
    }
}
