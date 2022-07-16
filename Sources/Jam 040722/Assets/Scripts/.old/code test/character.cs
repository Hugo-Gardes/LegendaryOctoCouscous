using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class character : MonoBehaviour
{
    //anim
    Animation animations;

    //vitesse
    public float runsp;
    public float walksp;
    public float turnsp;
    public float currentsp;
    private Vector3 posinit;

    //inputs
    public string[] inputs = {"z", "q", "s", "d"};

    public Vector3 jumpspeed;
    private bool is_fly = false;
    CapsuleCollider playercolider;
    public string currentanim;
    private Vector3 preced_pos;
    public first_game_handle fgh;

    void Start()
    {
        animations = gameObject.GetComponent<Animation>();
        playercolider = gameObject.GetComponent<CapsuleCollider>();
        fgh = GameObject.Find("ScriptLoader").GetComponent<first_game_handle>();
        posinit = transform.position;
    }

    bool is_grounded(float dist)
    {
        return (Physics.Raycast(transform.position, Vector3.down, dist));
    }

    void depla_handle(bool grounded)
    {
        if (Input.GetKey(KeyCode.CapsLock)) {// run
            currentsp = runsp;
            currentanim = "run";
        } else {
            currentsp = walksp;
            currentanim = "walk";
        }
        if (Input.GetKey(inputs[0])) { // front
            transform.Translate(0, 0, currentsp * Time.deltaTime);
            if (grounded)
                animations.Play(currentanim);
        }
        if (Input.GetKey(inputs[1])) { // rot left
            transform.Rotate(0, -turnsp * Time.deltaTime, 0);
        }
        if (Input.GetKey(inputs[2])) { // back
            transform.Translate(0, 0, -(currentsp / 2) * Time.deltaTime);
            if (grounded)
                animations.Play(currentanim);
        }
        if (Input.GetKey(inputs[3])) { // rot right
            transform.Rotate(0, turnsp * Time.deltaTime, 0);
        }
    }

    void jump_handle(bool grounded)
    {
        if ((Input.GetKeyDown(KeyCode.Space) && grounded) || (is_fly && Input.GetKey(KeyCode.Space))) {
            gameObject.GetComponent<Rigidbody>().velocity = jumpspeed;
            animations.Play("jump-up");
        }
        if (!Input.GetKey(inputs[0]) && !Input.GetKey(inputs[2]) && !Input.GetKey(KeyCode.Space) && grounded) { //check no action
            animations.Play("idle");
        }
        if (!is_fly && Input.GetKeyDown(KeyCode.F) && Input.GetKey(KeyCode.LeftControl)) {
            is_fly = true;
            jumpspeed.y = 20;
            Debug.Log("enable fly");
        } else if (is_fly && Input.GetKeyDown(KeyCode.F) && Input.GetKey(KeyCode.LeftControl)) {
            is_fly = false;
            jumpspeed.y = 5;
            Debug.Log("disable fly");
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        bool grounded = is_grounded(0.2f);
        preced_pos = transform.position;
        if (fgh.is_paused) {
            return;
        }
        depla_handle(grounded);
        jump_handle(grounded);
    }
}
