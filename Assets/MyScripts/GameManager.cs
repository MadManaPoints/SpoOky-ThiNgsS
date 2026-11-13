using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    PlayerMovement player;
    [SerializeField] GameObject[] cams;
    [SerializeField] GameObject pumpkin1, pumpkin2, skull1, skull2, scarecrow1, scarecrow2;
    [SerializeField] GameObject trees;
    Vector3 treePos;
    bool check1, check2, check3, treesDown;
    bool start;
    bool tutorialFinished;
    float treeSpeed = 0.7f;
    void Start()
    {
        player = PlayerMovement.player;

        for(int i = 1; i < cams.Length; i++){
            cams[i].SetActive(false);
        }

        pumpkin2.SetActive(false);
        skull2.SetActive(false);
        scarecrow2.SetActive(false);

        treePos = trees.transform.position;
    }

    void Update()
    {
        CamSwitch();
        HatIsOn();
        if(!treesDown) ReadyToPlay();
    }

    void CamSwitch(){
        if(player.spookyForm){
            for(int i = 0; i < cams.Length; i++){
                if(cams[i].gameObject.name != player.currentST.cam.name){
                    MicrophoneCheck();
                    cams[i].SetActive(false);
                } else {
                    cams[i].SetActive(true);
                }
            }
        } else {
            if(!cams[0].activeInHierarchy) cams[0].SetActive(true);

            for(int i = 1; i < cams.Length; i++){
                if(cams[i].activeInHierarchy){
                    cams[i].SetActive(false);
                }
            }
        }
    }

    void HatIsOn(){
        if(player.hasHat && !start){
            Destroy(pumpkin1); 
            pumpkin2.SetActive(true);
            Destroy(skull1);
            skull2.SetActive(true);
            Destroy(scarecrow1);
            scarecrow2.SetActive(true);
            start = true;
        }
    }

    void MicrophoneCheck(){
        if(!check1 && player.currentST.cam.name == "Pumpkin Cam 3") check1 = true;
        if(!check2 && player.currentST.cam.name == "Skull Cam 2") check2 = true;
        if(!check3 && player.currentST.cam.name == "Scarecrow Cam 1") check3 = true;
    }

    void ReadyToPlay(){
        trees.transform.position = treePos;

        if(check1 && check2 && check3 && treePos.y > 4.0f){
            tutorialFinished = true;
        }

        if(tutorialFinished){
            if(treePos.y > 4.4f){
                treePos.y -= treeSpeed * Time.deltaTime;
            } else {
                Destroy(trees);
                tutorialFinished = false;
                treesDown = true;
            }
        }
    }
}
