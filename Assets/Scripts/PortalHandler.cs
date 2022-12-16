using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalHandler : MonoBehaviour
{

    // PLAYER
    private Vector3 playerPosition;

    // PORTALS
    private GameObject CCR1;
    private GameObject R1R3;
    private GameObject R1R6;
    private GameObject R2R4;
    private GameObject R2R6;
    private GameObject R3R4;
    private GameObject R3R5;
    private GameObject R6CC;

    // ROOMS
    private GameObject CC;
    private GameObject R1;
    private GameObject R2;
    private GameObject R3;
    private GameObject R4;
    private GameObject R5;
    private GameObject R6;


    void Start()
    {
        // map all ROOMS to game objects
        CC = GameObject.FindGameObjectWithTag("CC");
        R1 = GameObject.FindGameObjectWithTag("R1");
        R2 = GameObject.FindGameObjectWithTag("R2");
        R3 = GameObject.FindGameObjectWithTag("R3");
        R4 = GameObject.FindGameObjectWithTag("R4");
        R5 = GameObject.FindGameObjectWithTag("R5");
        R6 = GameObject.FindGameObjectWithTag("R6");

        // map all PORTALS to game objects
        CCR1 = GameObject.FindGameObjectWithTag("CCR1");
        R1R3 = GameObject.FindGameObjectWithTag("R1R3");
        R1R6 = GameObject.FindGameObjectWithTag("R1R6");
        R2R4 = GameObject.FindGameObjectWithTag("R2R4");
        R2R6 = GameObject.FindGameObjectWithTag("R2R6");
        R3R4 = GameObject.FindGameObjectWithTag("R3R4");
        R3R5 = GameObject.FindGameObjectWithTag("R3R5");
        R6CC = GameObject.FindGameObjectWithTag("R6CC");

        // make tutorial and portal within it active
        CC.SetActive(true);
        CCR1.SetActive(true);
    }

    void Update()
    {
        // player's in Captain's cabin
        if (CC.activeSelf) {
            // walk into R1 - re/start the game
            // walk away. stop the game
        }

        if (R1.activeSelf) {
            // R3 -- player walks in the area of R3
                // activate R3 + portals
                // deactivate R1 + portals
                // teleport the player X units in front of the portal they're coming out of
            // R6
        }

        if (R2.activeSelf) {
            // R4
            // R6
        }

        if (R3.activeSelf) {
            // R1
            // R4
            // R5
        }

        if (R4.activeSelf) {
            // R2
            // R3
        }

        if (R5.activeSelf) {
            // R3
        }

        if (R6.activeSelf) {
            // R1
            // R2
            // Exit
        }
    }

}
