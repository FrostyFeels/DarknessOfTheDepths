using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float currentSpeed, startSpeed, maxSpeed, maxSprintSpeed;
    public float accel, deAccel, sprintAccel, airDeAcell;
    public float timeToMaxSpeed, timeToMaxSprintSpeed, timeToStill, airToStill;
    public float direction, oldDirection;
    public float sprintMultiplayer;
    public float rotation, sprintRotation;

    public Transform body;
    private Rigidbody2D rb;



   
    void Start()
    {
        maxSprintSpeed = maxSpeed * sprintMultiplayer;
        accel = (maxSpeed - startSpeed) / timeToMaxSpeed;
        sprintAccel = (maxSprintSpeed - startSpeed) / timeToMaxSprintSpeed;
        deAccel = maxSprintSpeed / timeToStill;
        airDeAcell = maxSprintSpeed / airToStill;

        rb = gameObject.GetComponent<Rigidbody2D>();
        
    }

    // Update is called once per frame
    void Update()
    {
        oldDirection = direction;

        //Gives a starting speed when player presses a button
        if(Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D)) { currentSpeed = startSpeed; }
        //Gives the direction where the player is facing
        if (Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D)) { direction = -1; } else if (Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A)) { direction = 1; }
        //Gives direction 0 when holding both at the same time
        if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D)) { direction = 0; }
        //Gives acceleration to the player
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.LeftShift)) 
        {
            body.rotation = Quaternion.Euler(0f, 0f, rotation * direction);
            currentSpeed += accel * Time.deltaTime; 
        }
        //Gives deacceleration to player
        if(!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
        {
            body.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            currentSpeed -= deAccel * Time.deltaTime;

            if(currentSpeed <= 0)
            {
                currentSpeed = 0f;
            }
        }

        if (Input.GetKey(KeyCode.LeftShift) && direction != 0)
        {
            currentSpeed += sprintAccel * Time.deltaTime;
            body.transform.rotation = Quaternion.Euler(0f, 0f, sprintRotation * direction);
            currentSpeed = Mathf.Clamp(currentSpeed, -maxSprintSpeed, maxSprintSpeed);
        }
        else if(direction != 0)
        {
            currentSpeed = Mathf.Clamp(currentSpeed, -maxSpeed, maxSpeed);
        }

        if(oldDirection != direction)
        {
            currentSpeed = startSpeed;
        }

    }

    public void FixedUpdate()
    {
        rb.velocity = new Vector2(currentSpeed * direction, rb.velocity.y);
    }
}
