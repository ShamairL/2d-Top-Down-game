using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    public float moveSpeed = 5f;
    public int maxHealth = 100;
    public int currentHealth;

    public HealthBar healthBar;

    private Vector2 movementInput;
    private Rigidbody2D rb;
    private Animator animator;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    private void FixedUpdate()
    {
        // If movemnt imput is not zero, move the player
        if (movementInput != Vector2.zero)
        {
            rb.MovePosition(rb.position + movementInput * moveSpeed * Time.fixedDeltaTime);
        }
        else { 
            animator.SetBool("isWalking", false);
            animator.SetFloat("LastInputX", movementInput.x);
            animator.SetFloat("LastInputY", movementInput.y);
        }
    }

    public void ChangeHealth(int health)
    {
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        healthBar.SetHealth(currentHealth);
    }

    public void OnMove(InputAction.CallbackContext movementValue)
    {
        animator.SetBool("isWalking", true);
        movementInput = movementValue.ReadValue<Vector2>();
        animator.SetFloat("InputX", movementInput.x);
        animator.SetFloat("InputY", movementInput.y);
    }
    public void OnJump(InputAction.CallbackContext context)
    {
        currentHealth -= 20;
        ChangeHealth(currentHealth);
    }
}