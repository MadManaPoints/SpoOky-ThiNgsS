using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightPumpkins : MonoBehaviour
{
    [SerializeField] GameObject p1, p2, text;
    bool active;
    void Start()
    {
        p1.SetActive(false);
        p2.SetActive(true);
        text.SetActive(false);
    }

    void Update()
    {
        p1.SetActive(active);
        p2.SetActive(!active);

        if(p1.activeInHierarchy){
            text.SetActive(true);
        } else {
            text.SetActive(false);
        }
    }

    void OnMouseEnter(){
        active = true;
    }

    void OnMouseExit(){
        active = false;
    }
}
