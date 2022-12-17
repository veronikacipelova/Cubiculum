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
                }

                // examine an object
                else if (interactable.tag == "itemExaminable") {
                    Examine(interactable);
                }

                // teleport
                else if (interactable.tag == "portal") {
                    Teleport(interactable);
                }

                // open a puzzle
                else if (interactable.tag == "puzzle") {
                    OpenPuzzle(interactable);
                }
                
            }

        }


    }

    private void AddToInventory(GameObject interactable) {
        Debug.Log("TAKE object");
        // deactivate item
        // add inventory
    }

    private void Examine(GameObject interactable) {
        Debug.Log("EXAMINE");

        bottomPanel.gameObject.SetActive(true);
        string bottomPanelText;

        // select flavor text based on the object's name
        switch (interactable.name) {
            case "puzzleLabyrinth":
                bottomPanelText = "I can't seem to be able to open this table. Maybe there's another way to move the key.";
                break;
            case "puzzleTV":
                bottomPanelText = "The power button seems to be missing. Perhaps there's something round I can replace it with?";
                break;
            case "puzzleBlacklight":
                bottomPanelText = "Not everything is as it seems...";
                break;
            case "puzzlePainting":
                bottomPanelText = "My eyes are not what they used to be… I wish there was something that could help me see all those details.";
                break;
            case "puzzlePainting1":
                bottomPanelText = "What an intricate machine! I could use one of those on my ship.";
                break;
            case "puzzlePainting2":
                bottomPanelText = "A city of so many opportunities, so many hopes… It's like just yesterday I arrived there on this train… I wonder how my old academy is doing.";
                break;
            case "puzzlePainting3":
                bottomPanelText = "Charles, that rascal… I'll never forgive you for stealing my compass! Carrying it with such pride in his pocket at all times, too!";
                break;
            default: bottomPanelText = null; break;
        }
        bottomPanel.text = bottomPanelText;

        Invoke("HideText", bottomPanelText.Length > 77 ? 10f : 5f );
    }

    private void HideText() {
        bottomPanel.gameObject.SetActive(false);
    }

    private void Teleport(GameObject interactable) {
        Debug.Log("TELEPORT");
    }

    private void OpenPuzzle(GameObject interactable) {
        Debug.Log("PUZZLE");
    }
}
