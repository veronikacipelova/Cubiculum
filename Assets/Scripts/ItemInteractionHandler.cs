using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class ItemInteractionHandler : MonoBehaviour
{
    private float interactionDistance = 2f;
    public LayerMask interactableObjects;

    [Header("Cursor")]
    public RawImage cursor;  // where the cursor picture is
    public Texture cursorDefault;
    public Texture cursorInteractable;

    public TMP_Text cursorLabel;
    private string cursorLabelText;

    public TMP_Text bottomPanel;
    private string bottomPanelText;

    void Start()
    {
        cursor.texture = cursorDefault;
        cursorLabel.gameObject.SetActive(false);
        bottomPanel.gameObject.SetActive(false);
    }

    void FixedUpdate()
    {

        // ray from the viewpoint center (0.5f) -- conntected to viewpoint instead of a mouse
        // in our case these two should be equivalent, but leaving this in just in case
        // var ray = Camera.main.ViewportPointToRay(new Vector3 (0.5f, 0.5f, 0));
        RaycastHit hit;
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        cursor.texture = cursorDefault;
        cursorLabel.gameObject.SetActive(false);

        // Debug.DrawRay(ray.origin, ray.direction * interactionDistance);

        // if distance to an object is < Y
        if (Physics.Raycast(ray, out hit, interactionDistance, interactableObjects)) {
            var interactable = hit.transform.gameObject;

            // change cursor + show action text
            cursor.texture = cursorInteractable;
            cursorLabel.gameObject.SetActive(true);

            switch (interactable.tag) {
                case "itemPickable": cursorLabelText = "Pick up"; break;
                case "itemExaminable": cursorLabelText = "Examine"; break;
                case "portal": cursorLabelText = "Enter"; break;
                default: cursorLabelText = null; break;
            }
            cursorLabel.text = cursorLabelText;



            if (Input.GetKeyDown(KeyCode.E))
            {
                // pick up an item
                if (interactable.tag == "itemPickable") {
                    AddToInventory(interactable);
                    // deactivate item
                    // add inventory
                }

                // examine an object
                else if (interactable.tag == "itemExaminable") {
                    Examine(interactable);
                    // show ui text
                }

                // teleport
                else if (interactable.tag == "portal") {
                    Teleport(interactable);
                }
                
            }

        }


    }

    private void AddToInventory(GameObject interactable) {
        Debug.Log("TAKE object");
    }

    private void Examine(GameObject interactable) {
        Debug.Log("EXAMINE");

        // display flavor (bottom) text for several seconds if the object has flavor text = tagged with "flavored"
        // todo: replace itemExaminable with flavored
        if (interactable.tag == "itemExaminable") {
            bottomPanel.gameObject.SetActive(true);

            bottomPanelText = "I can't seem to be able to open this table. Maybe there's another way to move the key.";
            bottomPanel.text = bottomPanelText;

            Invoke("HideText", 5f);
        }
    }

    private void HideText() {
        bottomPanel.gameObject.SetActive(false);
    }

    private void Teleport(GameObject interactable) {
        Debug.Log("TELEPORT");
    }
}
