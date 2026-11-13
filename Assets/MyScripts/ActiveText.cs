using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ActiveText : MonoBehaviour
{
    [SerializeField] GameObject obj;
    void Start()
    {
        
    }

    void Update()
    {
        if(obj.activeInHierarchy == true){
            this.gameObject.SetActive(true);
            Debug.Log("YERRRR");
        } else {
            this.gameObject.SetActive(false);
        }
    }
}
