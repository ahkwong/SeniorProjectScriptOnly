using UnityEngine;
using System.Collections;

public class MonsterStatus : MonoBehaviour {

    private int health = 100;
    private Animation anim;
    private float distance;
    private float rotatespeed = 10f;// * Time.deltaTime; dont do this shit
    private float movespeed = 0.25f;// * Time.deltaTime;
    private float aggrorange = 2f;
    private float meleerange = 0.5f;
    private Vector3 playerpos;
    private bool attackcooldown = false;
    private bool deadanimation = false;
    private bool dmganimation = false;

    private enum State
    {
        Idle,
        Run,
        Attack,
        TakeDmg,
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
            case State.Attack:
                atk_state();
                break;
            case State.TakeDmg:
                dmg_state();
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

        //if player is within aggro range go to run state
        if (distance <= aggrorange)
            currentState = State.Run;
    }

    void run_state()
    {
        //animation
        anim.Play("Idle");

        //look at the player
        Vector3 pos = playerpos - transform.position;
        Quaternion newrot = Quaternion.LookRotation(pos);
        transform.rotation = Quaternion.Slerp(transform.rotation, newrot, rotatespeed);

        //move towards the player until melee range
        if (distance > meleerange)
            transform.position = Vector3.Lerp(transform.position, playerpos, movespeed);
        else
            currentState = State.Attack;
        
        //go back to idle if player is out of aggro range
        if (distance > aggrorange)
            currentState = State.Idle;
    }

    void atk_state()
    {
        //attack once
        if (!attackcooldown)
        {
            anim["Attack"].speed = 0.5f;
            anim.Play("Attack");
            attackcooldown = true;
        }

        //if player is out of melee range, go to run state
        if (distance > meleerange)
            currentState = State.Run;

        //reset attackcooldown
        if(!anim.IsPlaying("Attack"))
            attackcooldown = false;
    }

    void dmg_state()
    {
        if (!dmganimation)
        {
            anim.Play("TakeDmg");
            dmganimation = true;
        }
        if (!anim.IsPlaying("TakeDmg"))
        {
            currentState = State.Idle;
            dmganimation = false;
        }
    }

    void dead_state()
    {
        //play dead animation once
        if (!deadanimation)
        {
            anim["Die"].speed = 0.5f;
            anim.Play("Die");
            deadanimation = true;
        }

        //destroy gameobject after dead animation is finished
        if (!anim.IsPlaying("Die"))
        {
            Destroy(gameObject);
        }

    }

    void Awake()
    {
        //initialization
        anim = GetComponent<Animation>();
    }

	void FixedUpdate ()
    {
        //state machine
        StateMachine();
        
        //capturing player position
        playerpos = GameObject.FindGameObjectWithTag("Player").transform.position;
        distance = Vector3.Distance(playerpos, transform.position);

        //go to dead state from any state if health is less than 1
        if (health < 1)
        {
            Debug.Log(gameObject.name + " Dead");
            currentState = State.Dead;
        }

        //Debug.Log(gameObject.name + " " + health);
        //Debug.Log(gameObject.name + " " + currentState);
	}

    //calls when a trigger hit this object
    void OnTriggerEnter (Collider col)
    {
        //checking if it is a bullet
        if (col.tag == "Bullet")
        {
            currentState = State.TakeDmg;
            health -= 20;
        }
    }
}
