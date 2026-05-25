using System;
using System.Collections;
using System.Collections.Generic;
using _Script;
using _Script.Agent.Operator;
using _Script.Environmental;
using GameLib.PoolObject.Runtime;
using UnityEngine;

public class OperatorManager : MonoBehaviour
{
    [field: SerializeField] public PoolManagerSO PoolManager;
    [field: SerializeField] public PoolItemSO PoolItem;
    [SerializeField] private GameObject spawnTarget;
    [SerializeField] private Grid grid;
    public LayerMask layerMask;

    private Vector3 setNewPosition;
    private BlockTypeEnum scannedBlockType;

    private PlayerOperator holdingTarget = null;
    private Collider targetCollider;

    [ContextMenu("Spawn Target")]
    public void OperatorSpawnStart()
    {
        if (holdingTarget != null) return;
        holdingTarget = PoolManager.Pop<PlayerOperator>(PoolItem);
        targetCollider = holdingTarget.GetComponent<Collider>();
        targetCollider.enabled = false;
    }

    [ContextMenu("DeSpawn Target")]
    public void OperatorSpawnCancel()
    {
        if (holdingTarget == null) return;
        PoolManager.Push(holdingTarget);
        holdingTarget = null;
        targetCollider = null;
    }

    public void OperatorSpawnConfirmed()
    {
        if (holdingTarget == null) return;
        
        Collider[] hit =
            Physics.OverlapBox(setNewPosition, new Vector3(0.5f, 0.5f, 0.5f), Quaternion.identity, layerMask);
        
        foreach (Collider hitTarget in hit)
        {
            if (hitTarget.CompareTag("Operator"))
            {
                OperatorSpawnCancel();
                return;
            }
        }

        holdingTarget.GetComponent<PlayerOperator>().ChangePlayerState(PlayerStateEnum.START);
        
        targetCollider.enabled = true;
        
        holdingTarget = null;
    }

    private void Update()
    {
        if (holdingTarget != null)
        {
            Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(mouseRay, out RaycastHit hit, 100))
            {
                Vector3 newPosition = grid.CellToWorld(grid.WorldToCell(new Vector3(Mathf.Ceil(hit.point.x), 0, Mathf.Ceil(hit.point.z))));
                
                //scannedBlockType = hit.collider.GetComponent<BlockType>().BlockTypeEnum;
                
                setNewPosition = newPosition;
                holdingTarget.transform.position = newPosition;
            }
        }

        if (Input.GetMouseButton(0))
        {
            OperatorSpawnConfirmed();
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(setNewPosition, new Vector3(0.5f, 0.5f, 0.5f));
    }
}
