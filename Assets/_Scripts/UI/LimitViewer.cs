using System;
using _Scripts.Managers.Board;
using TMPro;
using UnityEngine;

namespace _Scripts.UI
{
    public class LimitViewer : MonoBehaviour
    {
        private TextMeshProUGUI _limitText;

        private void Awake()
        {
            _limitText = GetComponent<TextMeshProUGUI>();
        }

        private void OnEnable()
        {
            BoardManager.Instance.OnOperatorCountChanged += HandleOperatorCountChanged;
        }

        private void OnDisable()
        {
            if(BoardManager.Instance != null)
                BoardManager.Instance.OnOperatorCountChanged -= HandleOperatorCountChanged;
        }

        private void HandleOperatorCountChanged(int count)
        {
            _limitText.text = $"배치 가능 요원 : {count}";
        }
    }
}
