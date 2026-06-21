using UnityEngine;
using UnityEngine.EventSystems;

namespace _Scripts.Cameras
{
    public class ViewScroller : MonoBehaviour
    {
        [Header("Input")]
        [SerializeField] private InputSO inputSO;

        [Header("Transform")]
        [SerializeField] private Transform startTrm;
        [SerializeField] private Transform endTrm;
    
        [Header("Setting")]
        [SerializeField] private float sensitive = 1.5f;

        private Camera _mainCam;
        private float _normal = 0f;
        private bool _isDragging = false;
        private Vector2 _prevMousePos;

        public Camera MainCam
        {
            get
            {
                if(_mainCam == null)
                    _mainCam = Camera.main;
                return _mainCam;
            }
        }

        private void OnEnable()
        {
            if (inputSO == null) return;
            inputSO.ChangeInput(false);
            
            inputSO.OnLeftBtnClick += HandleDragStart;
            inputSO.OnLeftBtnClickEnd += HandleDragEnd;
        }

        private void OnDisable()
        {
            if (inputSO == null) return;
            
            inputSO.OnLeftBtnClick -= HandleDragStart;
            inputSO.OnLeftBtnClickEnd -= HandleDragEnd;
        }

        private void HandleDragStart()
        {
            _isDragging = true;
            _prevMousePos = inputSO.MousePos; 
        }

        private void HandleDragEnd()
        {
            _isDragging = false;
        }

        private void Update()
        {
            if (startTrm == null || endTrm == null || MainCam == null || inputSO == null) return;
            
            if (_isDragging)
            {
                Vector2 currentMousePos = inputSO.MousePos;
                
                float deltaX = currentMousePos.x - _prevMousePos.x;
                float moveAmount = (deltaX / Screen.width) * sensitive;
                _normal -= moveAmount;
                
                _normal = Mathf.Clamp01(_normal);
                _prevMousePos = currentMousePos;
            }
            _mainCam.transform.position = Vector3.Lerp(startTrm.position, endTrm.position, _normal);
        }
    }
}