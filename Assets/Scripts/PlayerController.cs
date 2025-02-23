using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private VariableJoystick variableJoystick;
    [SerializeField] private float speed;
    public GameObject PlayerBasicBullet; //* this code is only for testing
    public float shootInterval = 0.1f; // Shooting every 1 second
    private Animator animator;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    //* this code is only for testing
    private void Start() {
        InvokeRepeating("Shoot", 0f, shootInterval); // Call Shoot() repeatedly every shootInterval seconds
    }

    private void FixedUpdate()
    {   
        float xMoveDirection = variableJoystick.Horizontal;
        float yMoveDirection = variableJoystick.Vertical;
        Vector2 moveDirection = new Vector2(xMoveDirection, yMoveDirection).normalized;
        rb.linearVelocity = moveDirection * speed;

        if (Math.Abs(xMoveDirection) >= 0.7f){
            animator.SetBool("IsTitled", true);

            if (xMoveDirection < 0){
                animator.SetFloat("Direction", 0);
            }
            else{
                animator.SetFloat("Direction", 1);
            }
        }
        else{
            animator.SetBool("IsTitled", false);
        }
    }
    
    //* this code is only for testing
    public void Shoot(){
        Debug.Log("Shooting!");
        GameObject testBullet = Instantiate(PlayerBasicBullet, transform.position, transform.rotation * Quaternion.Euler(0,0,90));
        testBullet.SetActive(true);
    }
}
