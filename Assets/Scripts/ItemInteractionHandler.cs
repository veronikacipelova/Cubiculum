using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
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

    [Header("Map")]
    public RawImage minimap;

    private Texture minimapTexture;
    private bool isMinimapActive = false;

    // name of path (where the minimaps are located) and minimap image itself
    private string minimapFolder;
    private string minimapName;

    // variables showing whether a map piece was already picked up or not
    private bool r1 = false;
    private bool r2 = false;
    private bool r3 = false;
    private bool r4 = false;
    private bool r5 = false;
    private bool r6 = false;


    // background for puzzles (and various UI elements)
    [Header("UIBackground")]
    public RawImage uiBackground;
    private bool isUiBackgroundActive = false;

    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        uiBackground.gameObject.SetActive(false);

        cursor.texture = cursorDefault;
        cursorLabel.gameObject.SetActive(false);
        bottomPanel.gameObject.SetActive(false);
        
        minimap.gameObject.SetActive(false);
        minimapFolder = "Map pieces/";
    }

    void Update()
    {
        // flip the visibility of minimap if M is pressed
        if (Input.GetKeyDown(KeyCode.M)) {
            isMinimapActive = !isMinimapActive;
        }
        minimap.gameObject.SetActive(isMinimapActive);

        if (Input.GetKeyDown(KeyCode.Escape)) {
            isUiBackgroundActive = false;
        }
        uiBackground.gameObject.SetActive(isUiBackgroundActive);
    }

    void FixedUpdate()
    {
        // ray from the viewpoint center (0.5f) -- connected to viewpoint instead of a mouse
        RaycastHit hit;
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        cursor.texture = cursorDefault;
        cursorLabel.gameObject.SetActive(false);

        // if distance to an object is < Y
        if (Physics.Raycast(ray, out hit, interactionDistance, interactableObjects)) {
            var interactable = hit.transform.gameObject;

            // change cursor + show action text
            cursor.texture = cursorInteractable;
            cursorLabel.gameObject.SetActive(true);

            switch (interactable.tag) {
                case "itemPickable": cursorLabelText = "Pick up"; break;
                case "itemCollectible": cursorLabelText = "Pick up"; break;
                case "itemExaminable": cursorLabelText = "Examine"; break;
                case "itemMap": cursorLabelText = "Pick up"; break;
                case "portal": cursorLabelText = "Enter"; break;
                case "puzzle": cursorLabelText = "Start the puzzle"; break;
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

                // pick up a collectible
                else if (interactable.tag == "itemCollectible") {
                    AddCollectible(interactable);
                }

                // add map
                else if (interactable.tag == "itemMap") {
                    // show picked up map automatically
                    isMinimapActive = true;
                    AddMinimap(interactable);
                }

                // teleport
                else if (interactable.tag == "portal") {
                    Teleport(interactable);
                }

                // open a puzzle
                else if (interactable.tag == "puzzle") {
                    isUiBackgroundActive = true;
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
            case "puzzlePaintingCharles":
                bottomPanelText = "Charles: Ah, this little thing? Served me well while it lasted, and now, perhaps, it will serve you.";
                break;
            case "puzzlePainting1":
                bottomPanelText = "What intricate machines! I could use one of those on my ship.";
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


    private void AddCollectible(GameObject interactable) {
        Debug.Log("COLLECTIBLE");
        // SetActive the trophy object in CC

        // destroy the collectible - we don't need to add it to the inventory
        Destroy(interactable);
    }

    private void HideText() {
        bottomPanel.gameObject.SetActive(false);
    }

    private void Teleport(GameObject interactable) {
        Debug.Log("TELEPORT");

        // check to see if the player entered the portal to then captain's cabin [end game]
        if (interactable.name == "portalCC") {
            Debug.Log("to CC");
            rb.position = new Vector3(-0.27f, 1.5f, 10.13f);

        }

        // portal that takes the player to main menu
        else if (interactable.name == "portalExit") {
            Debug.Log("to MAIN MENU");
        }
    }

    private void OpenPuzzle(GameObject interactable) {
        Debug.Log("PUZZLE");
    }

    private void AddMinimap(GameObject interactable) {
        Debug.Log("MINIMAP");

        // set the corresponding picked up minimap variable to true
        switch (interactable.name) {
            case "r1": r1 = true; break;
            case "r2": r2 = true; break;
            case "r3": r3 = true; break;
            case "r4": r4 = true; break;
            case "r5": r5 = true; break;
            case "r6": r6 = true; break;
            default: break;
        }

        // decide on which map to display
        minimapName = getMinimapName();       


        // load and set the actual texture of the minimap
        minimapTexture = Resources.Load<Texture>(minimapFolder + minimapName);
        minimap.texture = minimapTexture;
        minimap.gameObject.SetActive(isMinimapActive);

        // destroy the map object -- it's not needed anymore
        Destroy(interactable);
    }

    private string getMinimapName() {
        // 1 -- r3
             if ( !r1 && !r2 && r3 && !r4 && !r5 && !r6 )
            minimapName = "1_r3";
        else if ( !r1 &&  r2 && !r3 && !r4 && !r5 && !r6 )
            minimapName = "1_r2";
        else if (  r1 && !r2 && !r3 && !r4 && !r5 && !r6 )
            minimapName = "1_r1";
        else if ( !r1 && !r2 && !r3 && !r4 &&  r5 && !r6 )
            minimapName = "1_r5";
        else if ( !r1 && !r2 && !r3 &&  r4 && !r5 && !r6 )
            minimapName = "1_r4";
        else if ( !r1 && !r2 && !r3 && !r4 && !r5 &&  r6 )
            minimapName = "1_r6";

        // 2 -- r3 - rX
        else if (  r1 && !r2 && r3 && !r4 && !r5 && !r6 )
            minimapName = "2_r3-r1";
        else if ( !r1 &&  r2 && r3 && !r4 && !r5 && !r6 )
            minimapName = "2_r3-r2";
        else if ( !r1 && !r2 && r3 &&  r4 && !r5 && !r6 )
            minimapName = "2_r3-r4";
        else if ( !r1 && !r2 && r3 && !r4 &&  r5 && !r6 )
            minimapName = "2_r3-r5";
        else if ( !r1 && !r2 && r3 && !r4 && !r5 &&  r6 )
            minimapName = "2_r3-r6";

        // 3 -- r3 - r2 - rX
        else if (  r1 &&  r2 && r3 && !r4 && !r5 && !r6 )
            minimapName = "3_r3-r2-r1";
        else if ( !r1 &&  r2 && r3 &&  r4 && !r5 && !r6 )
            minimapName = "3_r3-r2-r4";
        else if ( !r1 &&  r2 && r3 && !r4 &&  r5 && !r6 )
            minimapName = "3_r3-r2-r5";
        else if ( !r1 &&  r2 && r3 && !r4 && !r5 &&  r6 )
            minimapName = "3_r3-r2-r6";

        // 3 -- r3 - r1 - rX
        else if (  r1 && !r2 && r3 &&  r4 && !r5 && !r6 )
            minimapName = "3_r3-r1-r4";
        else if (  r1 && !r2 && r3 && !r4 &&  r5 && !r6 )
            minimapName = "3_r3-r1-r5";
        else if (  r1 && !r2 && r3 && !r4 && !r5 &&  r6 )
            minimapName = "3_r3-r1-r6";

        // 3 -- r3 - r4 / r5 / r6
        else if ( !r1 && !r2 && r3 &&  r4 &&  r5 && !r6 )
            minimapName = "3_r3-r5-r4";
        else if ( !r1 && !r2 && r3 && !r4 &&  r5 &&  r6 )
            minimapName = "3_r3-r5-r6";
        else if ( !r1 && !r2 && r3 &&  r4 &&  !r5 &&  r6 )
            minimapName = "3_r3-r4-r6";



        // 2 -- r2 - rX
        else if (  r1 &&  r2 && !r3 && !r4 && !r5 && !r6 )
            minimapName = "2_r2-r1";
        else if ( !r1 &&  r2 && !r3 &&  r4 && !r5 && !r6 )
            minimapName = "2_r2-r4";
        else if ( !r1 &&  r2 && !r3 && !r4 &&  r5 && !r6 )
            minimapName = "2_r2-r5";
        else if ( !r1 &&  r2 && !r3 && !r4 && !r5 &&  r6 )
            minimapName = "2_r2-r6";

        // 3 -- r2 - r1 - rX
        else if (  r1 &&  r2 && !r3 &&  r4 && !r5 && !r6 )
            minimapName = "3_r2-r1-r4";
        else if (  r1 &&  r2 && !r3 && !r4 &&  r5 && !r6 )
            minimapName = "3_r2-r1-r5";
        else if (  r1 &&  r2 && !r3 && !r4 && !r5 &&  r6 )
            minimapName = "3_r2-r1-r6";

        // 3 -- r2 - r4 / r5 / r6
        else if ( !r1 &&  r2 && !r3 &&  r4 && !r5 &&  r6 )
            minimapName = "3_r2-r4-r6";
        else if ( !r1 &&  r2 && !r3 &&  r4 &&  r5 && !r6 )
            minimapName = "3_r2-r5-r4";
        else if ( !r1 &&  r2 && !r3 && !r4 &&  r5 &&  r6 )
            minimapName = "3_r2-r5-r6";



        // 2 -- r1 - rX
        else if (  r1 && !r2 && !r3 &&  r4 && !r5 && !r6 )
            minimapName = "2_r1-r4";
        else if (  r1 && !r2 && !r3 && !r4 &&  r5 && !r6 )
            minimapName = "2_r1-r5";
        else if (  r1 && !r2 && !r3 && !r4 && !r5 &&  r6 )
            minimapName = "2_r1-r6";

        // 3 -- r1 - rX - rY
        else if (  r1 && !r2 && !r3 &&  r4 && !r5 &&  r6 )
            minimapName = "3_r1-r4-r6";
        else if (  r1 && !r2 && !r3 &&  r4 &&  r5 && !r6 )
            minimapName = "3_r1-r5-r4";
        else if (  r1 && !r2 && !r3 && !r4 &&  r5 &&  r6 )
            minimapName = "3_r1-r5-r6";

        // 3 -- r5 - r4 - r6
        else if ( !r1 && !r2 && !r3 &&  r4 &&  r5 &&  r6 )
            minimapName = "3_r5-r4-r6";



        // 2 -- r5 / r4 / r6
        else if ( !r1 && !r2 && !r3 &&  r4 &&  r5 && !r6 )
            minimapName = "2_r5-r4";
        else if ( !r1 && !r2 && !r3 && !r4 &&  r5 &&  r6 )
            minimapName = "2_r5-r6";
        else if ( !r1 && !r2 && !r3 &&  r4 && !r5 &&  r6 )
            minimapName = "2_r4-r6";



        // 4 -- no r3 - rX
        else if ( !r1 &&  r2 && !r3 &&  r4 &&  r5 &&  r6 )
            minimapName = "4_no-r3-r1";
        else if (  r1 && !r2 && !r3 &&  r4 &&  r5 &&  r6 )
            minimapName = "4_no-r3-r2";
        else if (  r1 &&  r2 && !r3 && !r4 &&  r5 &&  r6 )
            minimapName = "4_no-r3-r4";
        else if (  r1 &&  r2 && !r3 &&  r4 && !r5 &&  r6 )
            minimapName = "4_no-r3-r5";
        else if (  r1 &&  r2 && !r3 &&  r4 &&  r5 &&  !r6 )
            minimapName = "4_no-r3-r6";

        
        // 4 -- no r2 - rX
        else if ( !r1 && !r2 &&  r3 &&  r4 &&  r5 &&  r6 )
            minimapName = "4_no-r2-r1";
        else if (  r1 && !r2 &&  r3 && !r4 &&  r5 &&  r6 )
            minimapName = "4_no-r2-r4";
        else if (  r1 && !r2 &&  r3 &&  r4 && !r5 &&  r6 )
            minimapName = "4_no-r2-r5";
        else if (  r1 && !r2 &&  r3 &&  r4 &&  r5 && !r6 )
            minimapName = "4_no-r2-r6";

        // 4 -- no r1 - rX
        else if ( !r1 &&  r2 &&  r3 && !r4 &&  r5 &&  r6 )
            minimapName = "4_no-r1-r4";
        else if ( !r1 &&  r2 &&  r3 &&  r4 && !r5 &&  r6 )
            minimapName = "4_no-r1-r5";
        else if ( !r1 &&  r2 &&  r3 &&  r4 &&  r5 && !r6 )
            minimapName = "4_no-r1-r6";

        // 4 -- no r4 / r5 / r6
        else if (  r1 &&  r2 &&  r3 && !r4 &&  r5 && !r6 )
            minimapName = "4_no-r4-r6";
        else if (  r1 &&  r2 &&  r3 && !r4 && !r5 &&  r6 )
            minimapName = "4_no-r5-r4";
        else if (  r1 &&  r2 &&  r3 &&  r4 && !r5 && !r6 )
            minimapName = "4_no-r5-r6";


        
        // 5 -- no rX
        else if ( !r1 &&  r2 &&  r3 &&  r4 &&  r5 &&  r6 )
            minimapName = "5_no-r1";
        else if (  r1 && !r2 &&  r3 &&  r4 &&  r5 &&  r6 )
            minimapName = "5_no-r2";
        else if (  r1 &&  r2 && !r3 &&  r4 &&  r5 &&  r6 )
            minimapName = "5_no-r3";
        else if (  r1 &&  r2 &&  r3 && !r4 &&  r5 &&  r6 )
            minimapName = "5_no-r4";
        else if (  r1 &&  r2 &&  r3 &&  r4 && !r5 &&  r6 )
            minimapName = "5_no-r5";
        else if (  r1 &&  r2 &&  r3 &&  r4 &&  r5 && !r6 )
            minimapName = "5_no-r6";

        // 6 - ALL
        else if (  r1 &&  r2 &&  r3 &&  r4 &&  r5 &&  r6 )
            minimapName = "6_all";

        return minimapName;
    }
}
