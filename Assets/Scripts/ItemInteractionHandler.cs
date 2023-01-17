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

    private GetTaken objectToTake;


    // background for puzzles (and various UI elements)
    [Header("UI")]
    public RawImage uiBackground;
    private bool isUiBackgroundActive = false;
    public GridLayoutGroup controls;
    private bool isControlsActive = false;

    Rigidbody rb;
    public PlayerMovement PlayerMovement;
    // public MoveCamera MoveCamera;

    [Header("CC")]
    public GameObject curtain;  // hides the collectibles
    public GameObject portalEnter; // portal for starting the game [to R1]
    public GameObject portalExit;  // portal for exiting the game [to the main menu]
    public RawImage charlesLetter;  // charles letter zoomed in so that the player can read it
    private bool isCharlesLetterActive = false;

    [Header("Collectibles")]
    public GameObject collectibleR1;
    public GameObject collectibleR2;
    public GameObject collectibleR3;
    public GameObject collectibleR4;
    public GameObject collectibleR5;
    public GameObject collectibleR6;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // [CC] charles letter zoom-in
        charlesLetter.gameObject.SetActive(isCharlesLetterActive);

        // [CC] portals
        portalEnter.SetActive(true);
        portalExit.SetActive(false);

        // UI elements
        cursor.texture = cursorDefault;
        cursorLabel.gameObject.SetActive(false);
        bottomPanel.gameObject.SetActive(false);
        uiBackground.gameObject.SetActive(false);
        controls.gameObject.SetActive(false);
        
        // minimaps
        minimap.gameObject.SetActive(false);
        minimapFolder = "Map pieces/";

        // [CC] collectibles
        collectibleR1.SetActive(false);
        collectibleR2.SetActive(false);
        collectibleR3.SetActive(false);
        collectibleR4.SetActive(false);
        collectibleR5.SetActive(false);
        collectibleR6.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
            InventoryManager.Instance.showInventory();
        if (Input.GetKeyDown(KeyCode.R))
            InventoryManager.Instance.HighlightNextItem('R');
        if (Input.GetKeyDown(KeyCode.F))
            InventoryManager.Instance.HighlightNextItem('F');
        if (Input.GetKeyDown(KeyCode.Return))
            InventoryManager.Instance.ConfirmSelected();

        // show the controls screen -> show the ui bg + the controls text overlay
        if (Input.GetKeyDown(KeyCode.C)) {
            isControlsActive = !isControlsActive;
            isUiBackgroundActive = !isUiBackgroundActive;

            controls.gameObject.SetActive(isControlsActive);
            uiBackground.gameObject.SetActive(isUiBackgroundActive);
        }

        // flip the visibility of minimap if M is pressed
        if (Input.GetKeyDown(KeyCode.M)) {
            isMinimapActive = !isMinimapActive;
        }
        minimap.gameObject.SetActive(isMinimapActive);

        // charles letter related
        if (isCharlesLetterActive) {
            SetPlayerMovement(charlesLetter, isCharlesLetterActive);
        }

        // puzzle ui related
        if (isUiBackgroundActive) {
            SetPlayerMovement(uiBackground, isUiBackgroundActive);
        }
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
                switch (interactable.tag) {
                    case "itemPickable":
                        if (hit.transform.TryGetComponent(out objectToTake)) {
                            objectToTake.Disappear();
                        }
                        // AddToInventory(interactable, hit);
                        break;
                    case "itemCollectible": AddCollectible(interactable);; break;
                    case "itemExaminable": Examine(interactable); break;
                    case "itemMap": AddMinimap(interactable); break;
                    case "portal": Teleport(interactable, hit); break;
                    case "puzzle": OpenPuzzle(interactable); break;
                    default: break;
                }                
            }
        }
    }

    private void SetPlayerMovement(RawImage uiElement, bool isUiElementActive) {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            // bring back player's movement
            PlayerMovement.enabled = true;
            // MoveCamera.enabled = true;

            // hide the ui element
            isUiElementActive = false;
            uiElement.gameObject.SetActive(isUiElementActive);
        }
    }

    private void Examine(GameObject interactable) {
        Debug.Log("EXAMINE");

        // if the examined object is charles letter, open a 2d image and exit function upon esc
        if (interactable.name == "charlesLetter") {
            PlayerMovement.enabled = false;
            // MoveCamera.enabled = false;

            SetPlayerMovement(charlesLetter, isCharlesLetterActive);

            isCharlesLetterActive = true;
            charlesLetter.gameObject.SetActive(isCharlesLetterActive);
            return;
        }

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

        // if a collectible is picked up, it is displayed in CC -> SetActive the trophy object in CC
        switch (interactable.name) {
            case "collectible-r1": collectibleR1.SetActive(true); break;
            case "collectible-r2": collectibleR2.SetActive(true); break;
            case "collectible-r3": collectibleR3.SetActive(true); break;
            case "collectible-r4": collectibleR4.SetActive(true); break;
            case "collectible-r5": collectibleR5.SetActive(true); break;
            case "collectible-r6": collectibleR6.SetActive(true); break;
            default: break;
        }

        // destroy the collectible - we don't need to add it to the inventory
        Destroy(interactable);
    }

    private void HideText() {
        bottomPanel.gameObject.SetActive(false);
    }

    private void Teleport(GameObject interactable, RaycastHit hit) {
        Debug.Log("TELEPORT");
        
        var portalObject = hit.transform.GetComponent<Teleport>();
        portalObject.TeleportPlayer();

        // check to see if the player entered the portal to then captain's cabin [end game]
        if (interactable.name == "portalCC") {
            Debug.Log("to CC");

            // unveil the shelf with collectibles
            curtain.SetActive(false);

            // hide the START portal, show the END portal
            portalEnter.SetActive(false);
            portalExit.SetActive(true);
        }

        // portal that potentially takes the player to main menu
        // currently it resets the game by launching the player into R1 (getting them stuck in a lore loop)
        else if (interactable.name == "portalExit") {
            Debug.Log("to MAIN MENU");
            ResetGame();
        }
    }

    private void ResetGame() {
        // [CC] no need to reset the portals/hide the curtain -- we will arrive at the same scene

        // [CC] remove the collectibles from the shelves, if any were present

        // activate collected objects again -- maps, collectibles, puzzle objects

        // reset the puzzles
    }

    private void OpenPuzzle(GameObject interactable) {
        Debug.Log("PUZZLE");

        isUiBackgroundActive = true;
        uiBackground.gameObject.SetActive(isUiBackgroundActive);

        PlayerMovement.enabled = false;
        // MoveCamera.enabled = false;
    }

    private void AddMinimap(GameObject interactable) {
        Debug.Log("MINIMAP");
        isMinimapActive = true;

        // set the corresponding picked up minimap variable to true
        switch (interactable.name) {
            case "map-r1": r1 = true; break;
            case "map-r2": r2 = true; break;
            case "map-r3": r3 = true; break;
            case "map-r4": r4 = true; break;
            case "map-r5": r5 = true; break;
            case "map-r6": r6 = true; break;
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
