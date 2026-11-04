using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public float fallLimit = -10f;

    private Rigidbody2D rb;
    private Animator animator;
    private PlayerAttack playerAttack;

    private float moveInput;
    private float touchMove;   // направление от экранных кнопок
    private bool isGrounded;
    private bool isResetting;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerAttack = GetComponent<PlayerAttack>();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        moveInput = 0f;
        touchMove = 0f;
        isGrounded = false;
        isResetting = false;

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.simulated = true;
        }

        if (animator != null)
        {
            animator.enabled = true;

            if (HasParameter("Speed", AnimatorControllerParameterType.Float))
                animator.SetFloat("Speed", 0f);
        }
    }

    private void Update()
    {
        if (isResetting)
            return;

        if (transform.position.y < fallLimit)
        {
            isResetting = true;
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            return;
        }

        moveInput = 0f;

        // Управление клавиатурой
        if (Keyboard.current != null)
        {
            if (Keyboard.current.aKey.isPressed ||
                Keyboard.current.leftArrowKey.isPressed)
                moveInput = -1f;

            if (Keyboard.current.dKey.isPressed ||
                Keyboard.current.rightArrowKey.isPressed)
                moveInput = 1f;

            if ((Keyboard.current.spaceKey.wasPressedThisFrame ||
                 Keyboard.current.wKey.wasPressedThisFrame ||
                 Keyboard.current.upArrowKey.wasPressedThisFrame)
                && isGrounded)
            {
                Jump();
            }

            if (Keyboard.current.jKey.wasPressedThisFrame)
                Attack();
        }

        // Управление пальцем (экранные кнопки)
        if (touchMove != 0f)
            moveInput = touchMove;

        // Поворот персонажа
        if (moveInput > 0f)
        {
            Vector3 scale = transform.localScale;
            scale.x = -Mathf.Abs(scale.x);
            transform.localScale = scale;
        }
        else if (moveInput < 0f)
        {
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x);
            transform.localScale = scale;
        }

        if (HasParameter("Speed", AnimatorControllerParameterType.Float))
            animator.SetFloat("Speed", Mathf.Abs(moveInput));
    }

    // ===== Методы для экранных кнопок =====

    public void SetTouchMove(float value)
    {
        touchMove = value;
    }

    public void TouchJump()
    {
        if (isGrounded && !isResetting && Time.timeScale > 0f)
            Jump();
    }

    public void TouchAttack()
    {
        if (!isResetting && Time.timeScale > 0f)
            Attack();
    }

    // ======================================

    private void Jump()
    {
        if (rb == null)
            return;

        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        isGrounded = false;

        if (HasParameter("Jump", AnimatorControllerParameterType.Trigger))
            animator.SetTrigger("Jump");
    }

    private void Attack()
    {
        if (HasParameter("Attack", AnimatorControllerParameterType.Trigger))
            animator.SetTrigger("Attack");

        if (playerAttack != null)
            playerAttack.Attack();
    }

    private void FixedUpdate()
    {
        if (!isResetting && rb != null && rb.simulated)
        {
            rb.linearVelocity = new Vector2(
                moveInput * moveSpeed,
                rb.linearVelocity.y);
        }
    }

    private bool HasParameter(
        string parameterName,
        AnimatorControllerParameterType type)
    {
        if (animator == null || animator.runtimeAnimatorController == null)
            return false;

        foreach (AnimatorControllerParameter parameter in animator.parameters)
        {
            if (parameter.name == parameterName && parameter.type == type)
                return true;
        }

        return false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            isGrounded = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            isGrounded = false;
    }
}