using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.UI
{
    public class CharacterButton : MonoBehaviour
    {
        [field: SerializeField] public Image Portrait { get; private set; }
        [field: SerializeField] public TextMeshProUGUI TopText { get; private set; }

        [SerializeField] private Image circleImage;
        [SerializeField] private Image redImage;
        [SerializeField] private TextMeshProUGUI remainTime;

        private Button _button;

        private void Awake()
        {
            redImage.enabled = false;
            remainTime.enabled = false;
            circleImage.fillAmount = 0;
            circleImage.enabled = false;
            
            _button = GetComponent<Button>();
        }

        public void CooldownCharacter(float second)
        {
            redImage.enabled = true;
            circleImage.enabled = true;
            remainTime.enabled = true;
            _button.enabled = false;
            StartCoroutine(Cooldown(second));
        }

        private IEnumerator Cooldown(float second)
        {
            float elapsedTime = 0;
            while (elapsedTime < second)
            {
                elapsedTime += Time.deltaTime;
                circleImage.fillAmount = (second - elapsedTime) / second;
                remainTime.text = Math.Round(second - elapsedTime, 1).ToString();
                
                yield return null;
            }

            _button.enabled = true;
            remainTime.enabled = false;
            redImage.enabled = false;
            circleImage.enabled = false;
        }
    }
}
