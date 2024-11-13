using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    PlayerMovement player;
    [SerializeField] GameObject[] cams;
    void Start()
    {
        player = PlayerMovement.player;

        for(int i = 1; i < cams.Length; i++){
            cams[i].SetActive(false);
        }
    }

    void Update()
    {
        CamSwitch();
    }

    void CamSwitch(){
        if(player.spookyForm){
            for(int i = 0; i < cams.Length; i++){
                if(cams[i].gameObject.name != player.currentST.cam.name){
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
}
