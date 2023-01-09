using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    Own code and idea behind the code by Veronika
*/

public class Teleport : MonoBehaviour
{
    public GameObject Player;
    public Transform otherPortal;
    public int direction;
    public bool isActive;
    private Vector3 offset;  // position offest
    public float offsetValue;
    private Vector3 playerRotation;  // player's rotation (the direction the player faces once they enter a room)

    // Start is called before the first frame update
    void Start()
    {
        //to teleport the player in front of the portal depending on which wall the portal is
        var offsetx = 0f;
        var offsetz = 0f;

        // rotate the player so they don't face the direction they faced before the teleport
        var playerRotY = 0f;

        switch (direction)
        {
            case 0: // right wall
                offsetx = -offsetValue;
                offsetz = 0;
                playerRotY = -90f;
                break;

            case 1: // top wall
                offsetx = 0;
                offsetz = offsetValue;
                playerRotY = 0f;
                break;

            case 2: // left wall
                offsetx = offsetValue;
                offsetz = 0;
                playerRotY = 90f;
                break;
            
            case 3: // bottom wall
                offsetx = 0;
                offsetz = -offsetValue;
                playerRotY = 180f;
                break;

            default:
                Debug.Log("PORTAL: incorrect direction value entered");
                break;
        }

        offset = new Vector3(offsetx, 0, offsetz);
        playerRotation = new Vector3(0, playerRotY, 0);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TeleportPlayer()
    {
        if(isActive == true)
        {
            Player.transform.position = otherPortal.transform.position + offset
            Player.transform.eulerAngles = playerRotation;
        }
    }
}
