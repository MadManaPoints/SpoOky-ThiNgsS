using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor;

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
    float catAnimTimer = 0f;
    public bool playerControl;
    bool canTransform;
    public bool hasHat;
    public bool spookyForm;
    public int spotted;
    bool canRun = true;
    Vector3 centerScreen = new Vector3(0.5f, 0.5f, 0f);
    [SerializeField] Image ret, fill, bgFill, cat;
    [SerializeField] Sprite cat1, cat2;
    [SerializeField] GameObject sliderObj;
    [SerializeField] Slider staminaBar;
    public MovementState state;
    Color retColor, fillColor, bgColor;
    [SerializeField] TextMeshProUGUI eText;
    public enum MovementState{
        walking,
        sprinting,
        sprintCooldown,
        recovery,
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
        fillColor = fill.color;
        bgColor = bgFill.color;
        retColor.a = 0;
        fillColor.a = 0;
        bgColor.a = 0;
        eText.text = "";
        sliderObj.SetActive(false);
        cat.sprite = cat1;
    }

    void Update()
    {
        ret.color = retColor;
        fill.color = fillColor;
        bgFill.color = bgColor;

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
        if(spotted != 0){
            cat.sprite = cat2;
            catAnimTimer = 1.0f;
        } else if(catAnimTimer > 0f){
            catAnimTimer -= Time.deltaTime;
        } else {
            catAnimTimer = 0f;
            cat.sprite = cat1;
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
            if(hit.collider.tag == "Spooky Thing" && hasHat && Vector3.Distance(transform.position, hit.collider.transform.position) <= 4.0f){
                retColor = Color.green;
                currentST = hit.transform.gameObject.GetComponent<SpookyThings>();
                eText.text = "Press E to Enter";
                canTransform = true;
            } else if(hit.collider.tag == "Hat" && Vector3.Distance(transform.position, hit.collider.transform.position) <= 4.0f){
                retColor = Color.green;
                eText.text = "Press E to Put On";
                if(Input.GetKeyDown(KeyCode.E)){
                    Destroy(hit.collider.gameObject);
                    hasHat = true;
                }
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
        if(state == MovementState.spooky && canRun){
            retColor = Color.white;
            eText.text = "Press E to Exit";
            retColor.a = 0;
            playerRb.isKinematic = true;
            KillBar();
        } else if(Input.GetKey(KeyCode.LeftShift) && canRun && playerRb.velocity != Vector3.zero && staminaBar.value > 1.0f){
            state = MovementState.sprinting;
            moveSpeed = runSpeed;
            staminaBar.value -= Time.deltaTime * 20.0f;
            FillBar();
        } else if(staminaBar.value < 1.0f && canRun){
            state = MovementState.recovery;
            canRun = false;
            fillColor = Color.red;
        } else if(state == MovementState.recovery && !canRun){
            moveSpeed = walkSpeed * 0.5f; 
            staminaBar.value += Time.deltaTime * 15.0f;
            if(staminaBar.value > 99.0){
                fillColor = Color.green;
                canRun = true;
            }
        } else {
            state = MovementState.walking;
            moveSpeed = walkSpeed;
            if(staminaBar.value < 99.0f){
                FillBar();
                staminaBar.value += Time.deltaTime * 20.0f;
            } else {
                KillBar();
            }
            
        }

        if(state != MovementState.spooky){
            playerRb.isKinematic = false;
        }
    }

    void FillBar(){
        sliderObj.SetActive(true);
        if(fillColor.a < 1.0f){
            fillColor.a += Time.deltaTime * 5.0f;
        } else {
            fillColor.a = 1.0f;
        }
        if(bgColor.a < 1.0f){
            bgColor.a += Time.deltaTime * 5.0f;
        } else {
            bgColor.a = 1.0f;
        }
    }

    void KillBar(){
        if(fillColor.a > 0f){
            fillColor.a -= Time.deltaTime * 5.0f;
        } else {
            fillColor.a = 0f;
        }
        if(bgColor.a > 0f){
            bgColor.a -= Time.deltaTime * 5.0f;
        } else {
            bgColor.a = 0f;
        }

        if(fillColor.a == 0f && bgColor.a == 0f){
            sliderObj.SetActive(false);
        }
    }
}
