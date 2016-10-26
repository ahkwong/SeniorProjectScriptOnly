using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

    public GameObject Fireballobject;
    private Rigidbody rb;
    private Animation anim;
    private Vector3 movement;
    private Vector3 mousepos;
    private Quaternion rotation;
    private float horizontal = 0;
    private float vertical = 0;
    private float movespeed = 2.5f;// * Time.deltaTime; dont do this shit
    private float rotatespeed = 10f;// * Time.deltaTime;
    private bool fireball = false;
    private float fireballspeed = 300f;
    public float fireballatkspd = 1;
    private int substate1 = 0;

    private enum State
    {
        Idle,
        Run,
        Fireball,
        AttackJump,
        Dead
    }

    private State currentState = State.Idle;

    void StateMachine()
    {
        switch (currentState)
        {
            case State.Idle:
                idle_state();
                break;
            case State.Run:
                run_state();
                break;
            case State.Fireball:
                fireball_state();
                break;
            case State.AttackJump:
                atkJump_state();
                break;
            case State.Dead:
                dead_state();
                break;
        }
    }

    void idle_state()
    {
        //animation
        anim.Play("Idle");

        //if player move, go to run state
        if (horizontal!=0||vertical!=0)
            currentState = State.Run;

        //if player attack, go to atk state
        if (fireball)
            currentState = State.Fireball;
    }

    void run_state()
    {
        //animation
        anim.Play("Run");

        //move function
        Move();

        //if player stop moving, go to idle state
        if (horizontal == 0 && vertical == 0)
            currentState = State.Idle;

        //if player attack, go to atk state
        if (fireball)
            currentState = State.Fireball;
    }

    void fireball_state()
    {
        switch(substate1)
        {
            case 0:
                //mouseposition();                    //get mouse position
                //transform.LookAt(mousepos);         //rotate player
                anim["Fireball"].speed = fireballatkspd;
                anim.Play("Fireball");           //play animation
                substate1 = 1;
                break;
            case 1:
                if (!anim.IsPlaying("Fireball"))
                {
                    currentState = State.Idle;
                    substate1 = 0;
                }
                break;
        }
    }

    void atkJump_state()
    {

    }

    void dead_state()
    {

    }

    void Awake()
    {
        //initialization
        anim = GetComponent<Animation>();
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        //Capturing player input
        if (this.gameObject.name == "Player1")
        {
            horizontal = Input.GetAxisRaw("P1Horizontal");
            vertical = Input.GetAxisRaw("P1Vertical");
            fireball = Input.GetKey(KeyCode.F);
        }
        if (this.gameObject.name == "Player2")
        {
            horizontal = Input.GetAxisRaw("P2Horizontal");
            vertical = Input.GetAxisRaw("P2Vertical");
            fireball = Input.GetKey(KeyCode.Keypad1);
        }

        //Statemachine
        StateMachine();
    }

    void mouseposition()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 1000))
        {
            mousepos = new Vector3(hit.point.x, hit.point.y, hit.point.z);
            //Debug.Log(mousepos);
        }
    }

    void Move()
    {
        //setting movement with player input, normalizing, multiply by speed
        movement.Set(horizontal, 0f, vertical);
        movement = movement.normalized;
        movement *= movespeed;

        //rotate object
        if (horizontal != 0 || vertical != 0)
            rotation = Quaternion.LookRotation(movement);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotatespeed);

        //move object
        transform.localPosition += movement;
    }

    //use by animation: entering attack
    void Fireball()
    {
        GameObject FB = GameObject.Instantiate(Fireballobject, transform.FindChild("FireballSpawn").position, Quaternion.identity) as GameObject;
        FB.GetComponent<Rigidbody>().AddForce(transform.forward * fireballspeed);
    }
}