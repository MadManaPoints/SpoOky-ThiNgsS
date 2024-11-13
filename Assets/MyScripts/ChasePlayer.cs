using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChasePlayer : MonoBehaviour
{
    PlayerMovement player;
    NavMeshAgent agent;
    Animator anim;
    Vector3 startPos;
    Vector3 startRot;
    Vector3 rot;
    float cooldown; 
    bool chooseCooldown;
    bool playerInSight; 
    bool leaveArea;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();    
        anim = GetComponent<Animator>();
        player = PlayerMovement.player;
        startPos = transform.position;
        startRot = transform.localEulerAngles;
        rot = startRot;
    }


    void Update()
    {
        if(!player.spookyForm && playerInSight){
            leaveArea = false;
            chooseCooldown = false;
            MoveToPlayer();
        } else if(player.spookyForm){
            MoveAway();
        } else {
            agent.isStopped = true;
            anim.SetBool("Look", true);
        }
        
        if(agent.velocity == Vector3.zero){
            anim.SetBool("Look", true);
        } else {
            anim.SetBool("Look", false);
        }

    }

    void MoveToPlayer(){
        if(Vector3.Distance(transform.position, player.transform.position) >= 3.0f){
            if(agent.isStopped) agent.isStopped = false;
            agent.SetDestination(player.transform.position);
        } else {
            agent.isStopped = true;            
        }
    }

    void MoveAway(){
        if(!chooseCooldown){
            cooldown = Random.Range(3.0f, 6.0f); 
            chooseCooldown = true;
        }
        cooldown -= Time.deltaTime;
        if(cooldown <= 0f){
            leaveArea = true;
        }

        if(leaveArea && Vector3.Distance(transform.position, startPos) > 5.0f){
            if(agent.isStopped) agent.isStopped = false;
            agent.SetDestination(startPos);
        } else {
            agent.isStopped = true;

            transform.localEulerAngles = rot;
            if(rot.y != startRot.y){
                rot.y += Time.deltaTime; 
            } else {
                rot.y = startRot.y;
            }
        }
    }

    void OnTriggerEnter(Collider col){
        if(col.gameObject.tag == "Player"){
            playerInSight = true;
        }
    }

    void OnTriggerExit(Collider col){
        if(col.gameObject.tag == "Player"){
            playerInSight = false;
        }
    }
}
