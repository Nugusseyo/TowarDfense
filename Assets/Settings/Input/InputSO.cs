using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "New InputSO", menuName = "Input SO")]
[DefaultExecutionOrder(-100)]
public class InputSO : ScriptableObject, Controls.IPlayerActions, Controls.IInGameActions
{
    private Controls _controls;

    public Action OnLeftBtnClick;
    public Action OnLeftBtnClickEnd;
    public bool IsInGame { get; private set; } = true;

    [SerializeField] private LayerMask groundLayer;
    
    private Camera _mainCamera;
    public Camera Cam
    {
        get
        {
            if (_mainCamera == null)
                _mainCamera = Camera.main;
            
            return _mainCamera;
        }
    }

    private Vector3 _worldPos;
    private Vector3 _camPos;
    public Vector2 MousePos => _camPos;

    private void OnEnable()
    {
        if (_controls == null)
        {
            _controls = new Controls();
            _controls.Player.SetCallbacks(this);
            _controls.InGame.SetCallbacks(this);
        }
        _controls.InGame.Enable();
    }

    private void OnDisable()
    {
        _controls.Player.Disable();
        _controls.InGame.Disable();
    }
    
    public void OnPoint(InputAction.CallbackContext context)
    {
        _camPos = context.ReadValue<Vector2>();
    }

    public void OnClick(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            OnLeftBtnClick?.Invoke();
        }
        if(context.canceled)
        {
            OnLeftBtnClickEnd?.Invoke();
        }
    }

    public bool GetMousePos(out Vector3 worldPos)
    {
        worldPos = Vector3.zero;
        if (Cam == null) return false;

        Ray ray = Cam.ScreenPointToRay(Input.mousePosition);
        
        bool isHit = Physics.Raycast(ray, out RaycastHit hit, Cam.farClipPlane, groundLayer);
        
        worldPos = isHit ? hit.point : Vector3.zero; 
    
        return isHit;
    }
    
    private bool TryRaycast(LayerMask layer, out RaycastHit hit)
    {
        hit = default;
        if (Cam == null) return false;

        Ray ray = Cam.ScreenPointToRay(Input.mousePosition);

        return Physics.Raycast(ray, out hit, Cam.farClipPlane, layer);
    }
    
    public bool GetMousePosByGameObject(out Vector3 worldPos, LayerMask targetLayer)
    {
        worldPos = Vector3.zero;
    
        if (!TryRaycast(targetLayer, out RaycastHit hit)) 
            return false;
        
        worldPos = hit.collider.transform.position; 
        return true;
    }
    public GameObject GetGameObject(LayerMask targetLayer)
    {
        if (!TryRaycast(targetLayer, out RaycastHit hit)) 
            return null;
        
        
        return hit.collider.gameObject;
    }

    public void ChangeInput(bool isInGame)
    {
        if (isInGame)
        {
            _controls.Player.Disable();
            _controls.InGame.Enable();
        }
        else
        {
            _controls.Player.Enable();
            _controls.InGame.Disable();
        }
        IsInGame = isInGame;
    }

    #region  InGame Input Region

    public Action OnInGameClick;

    public void OnInPoint(InputAction.CallbackContext context)
    {
        _camPos = context.ReadValue<Vector2>();
    }

    public void OnInClick(InputAction.CallbackContext context)
    {
        if(context.performed)
            OnInGameClick?.Invoke();
    }

    #endregion
    
}
