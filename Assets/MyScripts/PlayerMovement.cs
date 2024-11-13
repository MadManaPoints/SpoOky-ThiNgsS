using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement player;
    public SpookyThings currentST;
    Rigidbody playerRb;
    [SerializeField] Transform orientation;
    Vector3 moveDirection;
    float hInput;
    float vInput;
    float moveSpeed;
    float walkSpeed = 10.0f;
    float runSpeed = 15.0f;
    public bool playerControl = true;
    bool canTransform;
    public bool spookyForm;
    Vector3 centerScreen = new Vector3(0.5f, 0.5f, 0f);
    [SerializeField] Image ret;
    public MovementState state;
    Color retColor;
    [SerializeField] TextMeshProUGUI eText;
    public enum MovementState{
        walking,
        sprinting,
        crouching,
        spooky
    }

    void Awake(){
        player = this;
    }

    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        retColor = ret.color;
        eText.text = "";
    }

    void Update()
    {
        ret.color = retColor;

        if(playerControl){
            MyInput(); //will limit movement when transformed
            Casting();
        }
        StateHandler();

        if(canTransform && Input.GetKeyDown(KeyCode.E)){
            state = MovementState.spooky;
            playerControl = false;
            spookyForm = true;
            canTransform = false;
        } else if(spookyForm && Input.GetKeyDown(KeyCode.E)){
            state = MovementState.walking;
            playerControl = true;
            spookyForm = false;
        }
    }

    void FixedUpdate(){
        if(playerControl && !playerRb.isKinematic) Movement();
    }

    void Casting(){
        //cast to the center of the screen
        Ray laser = Camera.main.ViewportPointToRay(centerScreen);
        RaycastHit hit = new RaycastHit();

        if(Physics.Raycast(laser, out hit)){
            if(hit.collider.tag == "Spooky Thing" && Vector3.Distance(transform.position, hit.collider.transform.position) <= 4.0f){
                retColor = Color.green;
                currentST = hit.transform.gameObject.GetComponent<SpookyThings>();
                eText.text = "Press E to Enter";
                canTransform = true;
            } else {
                retColor = Color.white;
                eText.text = "";
                canTransform = false;
            }
        } else {
            retColor = Color.white;
            eText.text = "";
            canTransform = false;
        }
    }

    void MyInput(){
        hInput = Input.GetAxisRaw("Horizontal");
        vInput = Input.GetAxisRaw("Vertical");
    }

    void Movement(){
        moveDirection = (orientation.forward * vInput + orientation.right * hInput) * moveSpeed;
        if(hInput !=0 || vInput != 0){
            playerRb.velocity = moveDirection;
        } else {
            playerRb.velocity = Vector3.zero;
        }
    }

    private void StateHandler(){
        if(state == MovementState.spooky){
            retColor = Color.white;
            eText.text = "Press E to Exit";
            retColor.a = 0;
            playerRb.isKinematic = true;
        } else if(Input.GetKey(KeyCode.LeftShift)){
            state = MovementState.sprinting;
            moveSpeed = runSpeed;
        } else {
            state = MovementState.walking;
            moveSpeed = walkSpeed;
        }

        if(state != MovementState.spooky){
            playerRb.isKinematic = false;
        }
    }
}
