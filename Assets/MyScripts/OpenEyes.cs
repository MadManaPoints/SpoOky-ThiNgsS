using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpenEyes : MonoBehaviour
{
    PlayerMovement player;
    [SerializeField] GameObject eye1, eye2;
    Vector3 eye1Pos, eye2Pos;
    float speed = 1.2f;
    public bool eyesOpen;
    private int index = 0;
    private bool fin;
    void Start()
    {
        eye1Pos = eye1.transform.position;
        eye2Pos = eye2.transform.position;

        player = PlayerMovement.player;
    }

    void Update()
    {
        eye1.transform.position = eye1Pos;
        eye2.transform.position = eye2Pos;

        if(eye1Pos.y < 4.0f){
            eye1Pos.y += speed * Time.deltaTime;
        } else if(eye1.activeInHierarchy){
            eye1.SetActive(false);
            index++;
        }

        if(eye2Pos.y > 1.0f){
            eye2Pos.y -= speed * Time.deltaTime;
        } else if(eye2.activeInHierarchy){
            eye2.SetActive(false);
            index++;
        }

        if(index == 2 && !fin){
            player.playerControl = true;
            fin = true;
        }
    }
}
