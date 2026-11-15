using UnityEngine;
using UnityEngine.InputSystem;

public class LogicaMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody2D player;
    [SerializeField] private Transform InitDir;
    [SerializeField] private BoxCollider2D Coliziune;
    [SerializeField] private BoxCollider2D ColiziuneLava;
    [SerializeField] private CircleCollider2D playerCollider;
    [SerializeField] private GameObject prefabPlayer;

    [SerializeField] private float upForceSpeed = 9f;
    [SerializeField] private float jumpSpeed = 5f;
    [SerializeField] private float waitDuration = 0.5f;

    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask lavaLayer;
    [SerializeField] private LayerMask prefabPlayerLayer;
    [SerializeField] private float checkRadius = 0.15f;
    [SerializeField] private LayerMask lavaLazerLayer;
    
    private bool isGrounded;
    private bool isTouchingDeadPrefab;
    private bool wasTouchingDeadPrefab;
    private bool isLava;
    private bool isLavaLazer;

    private bool spacePressed;
    private bool dPressed;
    private bool aPressed;

    private bool isWaiting = false;
    private float currentTimer;

    private Vector2 currentSpawnPosition = new Vector2(0f, -2.73f);

    void Update()
    {
        if (Keyboard.current == null) return;

        if (Keyboard.current.spaceKey.wasPressedThisFrame) spacePressed = true;
        if (Keyboard.current.aKey.wasPressedThisFrame) aPressed = true;
        if (Keyboard.current.dKey.wasPressedThisFrame) dPressed = true;
    }

    void FixedUpdate()
    {
        Vector2 bottomPos = (Vector2)playerCollider.bounds.center + Vector2.down * playerCollider.bounds.extents.y;
        
        isGrounded = Physics2D.OverlapCircle(bottomPos, checkRadius, groundLayer);
        isTouchingDeadPrefab = Physics2D.OverlapCircle(playerCollider.bounds.center, playerCollider.bounds.extents.y + 0.08f, prefabPlayerLayer);
        isLava = Physics2D.OverlapCircle(playerCollider.bounds.center, playerCollider.bounds.extents.y + 0.05f, lavaLayer);
        isLavaLazer = Physics2D.OverlapCircle(playerCollider.bounds.center, playerCollider.bounds.extents.y + 0.05f, lavaLazerLayer);

        if (isGrounded && player.linearVelocity.y <= 0.01f)
        {
            player.linearVelocity = Vector2.zero;
            player.angularVelocity = 0f;
        }

        if (isTouchingDeadPrefab && !wasTouchingDeadPrefab)
        {
            StartWait();
        }
        else if (!isTouchingDeadPrefab && isWaiting)
        {
            CancelWait();
        }

        if (isWaiting)
        {
            currentTimer -= Time.fixedDeltaTime;
            if (currentTimer <= 0f)
            {
                CancelWait();
            }
        }

        wasTouchingDeadPrefab = isTouchingDeadPrefab;

        if (spacePressed)
        {
            if (isGrounded || isTouchingDeadPrefab)
            {
                CancelWait();
                player.AddForce(Vector2.up * upForceSpeed, ForceMode2D.Impulse);
            }
            spacePressed = false;
        }
        
        if (dPressed)
        {
            if (isGrounded || isTouchingDeadPrefab)
            {
                CancelWait();
                Vector2 dir = new Vector2(1f, 1f);
                player.linearVelocity = dir * jumpSpeed;
                player.AddForce(dir * upForceSpeed, ForceMode2D.Impulse);
            }
            dPressed = false;
        }
        
        if (aPressed)
        {
            if (isGrounded || isTouchingDeadPrefab)
            {
                CancelWait();
                Vector2 dir = new Vector2(-1f, 1f);
                player.linearVelocity = dir * jumpSpeed;
                player.AddForce(dir * upForceSpeed, ForceMode2D.Impulse);
            }
            aPressed = false;
        }

        if (isLava)
        {
            Vector2 prefabSpawnPos = new Vector2(player.transform.position.x, player.transform.position.y - 1);
            Instantiate(prefabPlayer, prefabSpawnPos, Quaternion.identity);
            Respawn();
        }
    }

    void StartWait()
    {
        isWaiting = true;
        currentTimer = waitDuration;
        player.angularDamping = 0f;
        player.gravityScale = 0.1f;
        player.linearVelocity = Vector2.zero;
    }
    public void DieInLava()
    {
        Vector2 prefabSpawnPos = new Vector2(player.transform.position.x, player.transform.position.y - 1);
        Instantiate(prefabPlayer, prefabSpawnPos, Quaternion.identity);
        Respawn();
    }
    void CancelWait()
    {
        isWaiting = false;
        player.gravityScale = 1f;
    }

    public void SetCheckPoint(Vector2 newSpawnPos)
    {
        currentSpawnPosition = newSpawnPos;
    }

    void Respawn()
    {
        CancelWait();
        wasTouchingDeadPrefab = false;
        player.linearVelocity = Vector2.zero;
        player.angularVelocity = 0f;
        
        player.transform.position = currentSpawnPosition;
        player.position = currentSpawnPosition;
        Physics2D.SyncTransforms();
    }
}