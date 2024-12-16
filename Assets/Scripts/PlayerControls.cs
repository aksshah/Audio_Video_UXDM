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
    public float turnSpeed = 5f;

    
    private PotionCollectorWithPanel potionCollector;


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
        }

        potionCollector = FindObjectOfType<PotionCollectorWithPanel>();

        if (potionCollector == null)
        {
            Debug.LogError("PotionCollectorWithPanel not found in the scene!");
        }
    }

    void Update()
    {
        MovePlayer();

        if (Input.GetMouseButton(0))
        {
            Cursor.lockState = CursorLockMode.Locked;
            MoveCamera();
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
        }
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


    private void OnTriggerEnter(Collider other)
    {
       if (other.CompareTag("collectable"))
        {
            potionCollector?.CollectPotion(other.gameObject);
        }
    }

    private void MoveCamera()
    {
        float x = Input.GetAxis("Mouse X");

        camera.transform.RotateAround(player.transform.position, Vector3.up, x * turnSpeed);
    }
}

