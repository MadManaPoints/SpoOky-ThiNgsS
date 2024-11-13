using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacePlayer : MonoBehaviour
{
    PlayerMovement player;

    void Start(){
        player = PlayerMovement.player;
    }
    void Update()
    {
        transform.LookAt(player.transform);         
    }
}
