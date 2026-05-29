using System;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "New InputSO", menuName = "Input SO")]
public class InputSO : ScriptableObject, Controls.IPlayerActions
{
    private Controls _controls;

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

    private void OnEnable()
    {
        if (_controls == null)
        {
            _controls = new Controls();
            _controls.Player.SetCallbacks(this);
        }
        _controls.Player.Enable();
    }

    private void OnDisable()
    {
        _controls.Player.Disable();
    }

    private Vector3 _worldPos;
    private Vector3 _camPos;
    
    public void OnPoint(InputAction.CallbackContext context)
    {
        _camPos = context.ReadValue<Vector2>();
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
}
