using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    private Rigidbody rb;
    private Animator animator;
    private GameObject player;
    private AudioSource movementSound;
    public GameObject camera;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player");
        movementSound = GetComponent<AudioSource>();

        if (player == null)
        {
            Debug.Log("Player tag not found");
        }
        else
        {
            animator = player.GetComponent<Animator>();
            Debug.Log(animator);
        }
    }

    void Update()
    {
        MovePlayer();
    }

    void MovePlayer()
    {
        float moveInputX = 0f;
        float moveInputZ = 0f;

        if (Input.GetKey(KeyCode.A)) moveInputX = -1f;
        if (Input.GetKey(KeyCode.D)) moveInputX = 1f;
        if (Input.GetKey(KeyCode.W)) moveInputZ = 1f;
        if (Input.GetKey(KeyCode.S)) moveInputZ = -1f;

        // Normalize input to avoid diagonal speed boost
        Vector3 movement = camera.transform.forward * moveInputZ + camera.transform.right * moveInputX;
        movement = movement.normalized;
        
        if (movement != Vector3.zero)
        {
            // Rotate player to face the movement
            player.transform.rotation = Quaternion.LookRotation(movement);

            // Running Animation and Sound
            if (animator != null)
            {
                animator.SetBool("isRunning", true);
            }

            if (movementSound != null && !movementSound.isPlaying)
            {
                movementSound.Play();
            }
        }
        else
        {
            // Stop Running Animation and Sound
            if (animator != null)
            {
                animator.SetBool("isRunning", false);
            }

            if (movementSound != null && movementSound.isPlaying)
            {
                movementSound.Stop();
            }
        }

        // Apply velocity to Rigidbody
        rb.velocity = new Vector3(movement.x * moveSpeed, rb.velocity.y, movement.z * moveSpeed);
    }
}

