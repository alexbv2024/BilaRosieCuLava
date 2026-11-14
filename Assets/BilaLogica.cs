using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class LogicaMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody2D player;
    [SerializeField] private float upForceSpeed=9f;
    [SerializeField] private Transform InitDir;
    [SerializeField] private BoxCollider2D Coliziune;
    [SerializeField] private BoxCollider2D ColiziuneLava;
    [SerializeField] private CircleCollider2D playerCollider;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float checkRadius = 0.15f;
    [SerializeField] private float jumpSpeed=9f;
    [SerializeField] private LayerMask lavaLayer;
    private bool isGrounded;
    
    private Vector2 dir = Vector2.up;
    private bool spacePressed;
    private bool dPressed;
    private bool aPressed;
    
    void Update()
    {
        
        if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            spacePressed = true;
        }
        if (Keyboard.current.aKey.isPressed)
        {
            aPressed = true;
        }
        if (Keyboard.current.dKey.isPressed)
        {
            dPressed = true;
        }
    }
    void FixedUpdate()
    {
        Vector2 bottomPos = (Vector2)playerCollider.bounds.center + Vector2.down * playerCollider.bounds.extents.y;
        isGrounded = Physics2D.OverlapCircle(bottomPos, checkRadius, groundLayer);
        if (spacePressed)
        {
            if (isGrounded)
            {
               spacePressed = false;
                           if (dPressed)
                           {
                               dir=new Vector2(1f, 1f).normalized;
                               dPressed = false;
                           }
                           else if (aPressed)
                           {
                               dir=new Vector2(-1f, 1f).normalized;
                               aPressed = false;
                           }
                           player.linearVelocity = dir * jumpSpeed;
                           player.AddForce(dir * upForceSpeed,ForceMode2D.Impulse);
                           
            }
            
        }
        
        
        
    }
    
}
