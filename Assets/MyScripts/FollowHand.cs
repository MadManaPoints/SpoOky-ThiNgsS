using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowHand : MonoBehaviour
{
    [SerializeField] GameObject[] hands;
    [SerializeField] GameObject[] torches;
    [SerializeField] Vector3 offsetPos;

    void Update()
    {
        for(int i = 0; i < hands.Length; i++){
            torches[i].transform.position = hands[i].transform.position + offsetPos;
        }
    }
}
