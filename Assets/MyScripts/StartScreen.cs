using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartScreen : MonoBehaviour
{
    [SerializeField] GameObject play, moon;
    [SerializeField] Image fade;
    bool pressedPlay;
    float targetTime = 2.0f;
    Color black; 
    void Start()
    {
        black = fade.color; 
    }

    void Update()
    {
        fade.color = black;

        if(play.activeInHierarchy && Input.GetMouseButtonDown(0)){
            pressedPlay = true;
        }

        if(pressedPlay){
            if(black.a < 255.0f){
                black.a += Time.deltaTime; 
            }
            moon.transform.localEulerAngles += new Vector3(0, 40.0f, -3.0f) * Time.deltaTime;
            if(targetTime > 0){
                targetTime -= Time.deltaTime;
            } else {
                SceneManager.LoadScene("SampleScene");
            }
        }
    }
}
