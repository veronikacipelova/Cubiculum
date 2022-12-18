using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    Part of this code has been borrowed from this tutorial:
    https://www.youtube.com/watch?v=AoD_F1fSFFg
    from @Solo Game Dev
    This code will have to be merged with Sofia's part
*/

public class TakeObject : MonoBehaviour
{
    [SerializeField] private Transform playerCameraTransform;
    [SerializeField] private LayerMask pickUpLayerMask;
    private float pickupDistance = 2f;
    private GetTaken objectToTake;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if(Physics.Raycast(playerCameraTransform.position, playerCameraTransform.forward, out RaycastHit raycastHit, pickupDistance, pickUpLayerMask))
            {
                Debug.Log(raycastHit.transform);
                var portalObject = raycastHit.transform.GetComponent<Teleport>();
                if (portalObject != null)
                {
                    portalObject.TeleportPlayer();
                    return;
                }
                if (raycastHit.transform.TryGetComponent(out objectToTake))
                {
                    objectToTake.Disappear();
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.I))
            InventoryManager.Instance.showInventory();
        if (Input.GetKeyDown(KeyCode.R))
            InventoryManager.Instance.HighlightNextItem('R');
        if (Input.GetKeyDown(KeyCode.F))
            InventoryManager.Instance.HighlightNextItem('F');
        if (Input.GetKeyDown(KeyCode.Return))
            InventoryManager.Instance.ConfirmSelected();
    }
}
