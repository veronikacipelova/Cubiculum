using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ItemInteractable : MonoBehaviour
{

    private Rigidbody player;
    public Rigidbody Player => player;

    void Awake()
    {
        player = GetComponent<Rigidbody>();
    }
}
