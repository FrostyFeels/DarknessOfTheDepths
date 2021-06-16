using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJump : MonoBehaviour
{

    public float jumpCount;
    public float jumpForce;

    public bool jump;

    public float fJumpPressedRemember;
    public float fJumpPressedRememberTime = 0.2f;

    public float smallGravity, heavyGrafity;

    BoxCollider2D bc;
    Rigidbody2D rb;
    public Transform body;
    [SerializeField] LayerMask groundMask;

    public bool grounded;

    // Start is called before the first frame update
    void Start()
    {
        bc = gameObject.GetComponentInChildren<BoxCollider2D>();
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        fJumpPressedRemember -= Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            fJumpPressedRemember = fJumpPressedRememberTime;
            if (!isGrounded() && jumpCount > 0)
            {
                jump = true;
                fJumpPressedRemember = 0f;
                jumpCount--;
            }
        }
        if (isGrounded() && fJumpPressedRemember > 0f)
        {
            jump = true;
            jumpCount = 1;

        }



        //Changes gravity depeneding on if you hold the button or not
        if (rb.velocity.y < 0)
        {
            rb.gravityScale = heavyGrafity;
        }
        else if (rb.velocity.y > 0 && Input.GetKey(KeyCode.Space))
        {
            rb.gravityScale = smallGravity;
        }
        else
        {
            rb.gravityScale = 6f;
        }

    }

    private void FixedUpdate()
    {
        if (jump)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jump = false;
        }
    }



    public bool isGrounded()
    {
        RaycastHit2D raycasthit2d = Physics2D.BoxCast(bc.bounds.center, bc.bounds.size, 0f, Vector2.down, .1f, groundMask);
        grounded = raycasthit2d.collider;
        return raycasthit2d.collider != null;
    }
}
