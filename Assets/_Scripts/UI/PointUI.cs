using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _Scripts.UI
{
    public class PointUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Image background;
        [SerializeField] private float speed = 10f;

        private float _targetFill = 0;

        private void Awake()
        {
            background.fillAmount = 0;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _targetFill = 1;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _targetFill = 0;
        }

        private void Update()
        {
            if (background == null) return;
            
            background.fillAmount = Mathf.MoveTowards(background.fillAmount, _targetFill, speed * Time.deltaTime);
            //MoveTowards는 A의 값을 B까지 X의 속도로 일정하게 상승시켜준다.
        }
    }
}
