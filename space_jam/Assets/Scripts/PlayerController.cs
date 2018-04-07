using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    //public float jumpSpeed = 8.0F;
    //public float gravity = 20.0F;
    private Vector3 moveDirection = Vector3.zero;
    public int playerId = 0;

    public List<AudioClip> audioWalks;
    public List<AudioClip> audioHammer;
    public List<AudioClip> audioDash;

    //[HideInInspector]
    public bool isRdy = false;

    // TODO: put these in common class (GameManager?)
    private float gravity = 20.0F;
    public float speed = 6.0F;
    [Range(1.0f, 5.0f)]
    public float zSpeed = 1.0f;

    public float smashCooldownTime = 1100f; // in ms
    public float smashSpeed = 0.35f;
    public float smashTime = 900f;          // in ms

    public float dashCooldownTime = 2000f;  // in ms
    public float dashSpeed = 25f;
    public float dashTime= 50f;             // in ms
    public float otherDashSpeed = 6.0f;

    private bool dashCooldown = false;
    private bool dashActive = false;
    private bool smashCooldown = false;
    private bool smashActive = false;
    private bool actionCooldown = false;

    private CharacterController controller;
    private Collider hammerCollider;
    private Animator animator;

    void Start()
    {
        hammerCollider = this.GetComponentInChildren<HammerPush>().gameObject.GetComponent<Collider>();
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }
    void Update()
    {
        Random.InitState((int)Time.time * 10);

        if(controller.isGrounded)
        {
            if (smashCooldown)
                moveDirection = new Vector3(Input.GetAxis("Horizontal_" + playerId), 0, Input.GetAxis("Vertical_" + playerId)) * 0.25f;

            if (!dashActive)
                moveDirection = new Vector3(Input.GetAxis("Horizontal_" + playerId), 0, Input.GetAxis("Vertical_" + playerId));

            if (moveDirection != Vector3.zero)
            {
                if (!GetComponent<AudioSource>().isPlaying)
                    GetComponent<AudioSource>().PlayOneShot(audioWalks[Random.Range(0,audioWalks.Count-1)]);
                
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(moveDirection.normalized), 0.15f);
                if(animator.GetBool("walking") == false)
                    animator.SetBool("walking", true);
            }
            else
            {
                if (animator.GetBool("walking") == true)
                    animator.SetBool("walking", false);
            }
                

            if (Input.GetButton("Fire1_" + playerId) && !dashCooldown && !actionCooldown) //dash
            {
                GetComponent<AudioSource>().PlayOneShot(audioDash[Random.Range(0, audioDash.Count - 1)]);
                Invoke("ActionCooldown", 1f);
                actionCooldown = true;
                animator.SetTrigger("dash");
                Invoke("ResetDashCooldown", dashCooldownTime / 1000.0f);
                dashCooldown = true;
                Invoke("ResetDash", dashTime / 1000.0f);
                dashActive = true;
            }

            if(Input.GetButton("Smash_" + playerId) && !smashCooldown && !actionCooldown) //smash
            {
                
                Invoke("ActionCooldown", 1.2f);
                actionCooldown = true;
                
                animator.SetTrigger("smash");
                Invoke("ResetSmashCooldown", smashCooldownTime / 1000.0f);
                smashCooldown = true;
                Invoke("ResetSmash", smashTime / 1000.0f);
                Invoke("playSmashSound", 500f / 1000f);
                smashActive = true;
            }

            if (dashActive)
            {
                controller.Move(new Vector3(-transform.right.z, 0, transform.right.x) * dashSpeed * Time.deltaTime);
                return;
            }
            moveDirection *= speed;
            moveDirection.z *= zSpeed;
        }

        moveDirection.y -= gravity * Time.deltaTime;
        controller.Move(moveDirection * Time.deltaTime);
    }
    void ActionCooldown()
    {
        actionCooldown = false;
    }
    void ResetDash()
    {
        dashActive = false;
        // Animator: reset bool/trigger here
    }
    void ResetDashCooldown()
    {
        // Cooldown UI : hide here
        dashCooldown = false;
    }

    void ResetSmash()
    {
        hammerCollider.enabled = false;
        smashActive = false;
        // Animator: reset bool/trigger here
    }
    void playSmashSound()
    {
        GetComponent<AudioSource>().PlayOneShot(audioHammer[Random.Range(0, audioHammer.Count-1)]);
    }
    void ResetSmashCooldown()
    {
        smashCooldown = false;
    }

    void OnCollisionEnter (Collision col)
    {
        Debug.Log("PlayerController.OnCollisionEnter");
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        //Rigidbody body = hit.collider.attachedRigidbody;
        //if (body == null || body.isKinematic)
        //    return;

        GameObject other = hit.gameObject; //hit.collider.gameObject...
        if (other)
        {
            if (other.CompareTag("Weapon"))
            {
                other.SendMessage("ReceiveMessage", hit.gameObject.GetInstanceID());
                Debug.Log("Parent from Weapon is " + other.transform.parent);
                controller.Move(new Vector3(-hit.moveDirection.x, 0, -hit.moveDirection.z) * Time.deltaTime * 100);
            }
            CharacterController otherController = other.GetComponent<CharacterController>();
            if(otherController)
            {
                //otherController.Move(new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z) * Time.deltaTime * otherDashSpeed);
            }
        }
    }
}
