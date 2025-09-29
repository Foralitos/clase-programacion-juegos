using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float boundarySize = 9f; // Límites del plano (plano escalado x2 = 20 unidades, menos margen)

    private Rigidbody rb;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        
        // Congelar rotación para evitar que el cubo se voltee
        rb.freezeRotation = true;
    }
    
    void Update()
    {
        // Obtener input del jugador
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        
        // Crear vector de movimiento
        Vector3 movement = new Vector3(horizontal, 0f, vertical).normalized;
        
        // Aplicar movimiento
        if (movement.magnitude > 0.1f)
        {
            rb.linearVelocity = new Vector3(movement.x * moveSpeed, rb.linearVelocity.y, movement.z * moveSpeed);
        }
        else
        {
            // Detener movimiento horizontal cuando no hay input
            rb.linearVelocity = new Vector3(0f, rb.linearVelocity.y, 0f);
        }
        
        // Aplicar límites del plano
        ApplyBoundaries();
    }
    
    void ApplyBoundaries()
    {
        Vector3 pos = transform.position;
        
        // Limitar posición X
        if (pos.x > boundarySize)
        {
            pos.x = boundarySize;
            rb.linearVelocity = new Vector3(0f, rb.linearVelocity.y, rb.linearVelocity.z);
        }
        else if (pos.x < -boundarySize)
        {
            pos.x = -boundarySize;
            rb.linearVelocity = new Vector3(0f, rb.linearVelocity.y, rb.linearVelocity.z);
        }
        
        // Limitar posición Z
        if (pos.z > boundarySize)
        {
            pos.z = boundarySize;
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, rb.linearVelocity.y, 0f);
        }
        else if (pos.z < -boundarySize)
        {
            pos.z = -boundarySize;
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, rb.linearVelocity.y, 0f);
        }
        
        // Aplicar nueva posición
        transform.position = pos;
    }
}