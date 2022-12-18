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
    private Vector3 offset;
    public float offsetValue;
    // Start is called before the first frame update
    void Start()
    {
        //to teleport the player in front of the portal depending on which wall the portal is
        var offsetx = 0f;
        var offsetz = 0f;
        switch (direction)
        {
            case 3: //bottom wall
                offsetx = 0;
                offsetz = -offsetValue;
                break;
            case 0: //left wall
                offsetx = -offsetValue;
                offsetz = 0;
                break;
            case 2: //right wall
                offsetx = offsetValue;
                offsetz = 0;
                break;
            case 1: //top wall
                offsetx = 0;
                offsetz = offsetValue;
                break;
        }
        offset = new Vector3(offsetx,0,offsetz);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TeleportPlayer()
    {
        if(isActive == true)
        {
            Player.transform.position = otherPortal.transform.position + offset;
        }
    }
}
