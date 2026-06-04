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
            BoardManager.Instance.OnOperatorCountChanged -= HandleOperatorCountChanged;
        }

        private void HandleOperatorCountChanged(int count)
        {
            _limitText.text = $"Limit : {count}";
        }
    }
}
