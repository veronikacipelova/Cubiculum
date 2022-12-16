using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ItemInteractionHandler : MonoBehaviour
{
    private float interactionDistance = 2f;
    public LayerMask interactableObjects;

    [Header("Cursor")]
    public RawImage cursor;  // where the cursor picture is
    public Texture cursorDefault;
    public Texture cursorInteractable;

    void Start()
    {
        Debug.Log("item interaction launched");
        cursor.texture = cursorDefault;
    }

    void FixedUpdate()
    {

        // ray from the viewpoint center (0.5f) -- conntected to viewpoint instead of a mouse
        // in our case these two should be equivalent, but leaving this in just in case
        // var ray = Camera.main.ViewportPointToRay(new Vector3 (0.5f, 0.5f, 0));
        RaycastHit hit;
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        cursor.texture = cursorDefault;

        // Debug.DrawRay(ray.origin, ray.direction * interactionDistance);

        // if distance to an object is < Y
        if (Physics.Raycast(ray, out hit, interactionDistance, interactableObjects)) {
            var interactable = hit.transform.gameObject;
            cursor.texture = cursorInteractable;

            if (Input.GetKeyDown(KeyCode.E))
            {
                // pick up an item
                if (interactable.tag == "itemPickable") {
                    AddToInventory();
                    // deactivate item
                    // add inventory
                }

                // examine an object
                else if (interactable.tag == "itemExaminable") {
                    Examine();
                    // show ui text
                }

                // teleport
                else if (interactable.tag == "portal") {
                    Teleport();
                }
                
            }

        }


    }

    private void AddToInventory() {
        Debug.Log("TAKE object");
    }

    private void Examine() {
        Debug.Log("EXAMINE");
    }

    private void Teleport() {
        Debug.Log("TELEPORT");
    }
}
