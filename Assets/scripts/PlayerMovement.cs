using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float boundarySize = 9f; // Límites del plano (plano escalado x2 = 20 unidades, menos margen)

    private Rigidbody rb;
    private bool isDead = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Congelar rotación para evitar que el cubo se voltee
        rb.freezeRotation = true;
    }

    public bool IsDead()
    {
        return isDead;
    }

    public void Die()
    {
        if (!isDead)
        {
            isDead = true;
            Debug.Log("¡El jugador ha muerto!");
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // Si colisiona con un enemigo, morir
        if (collision.gameObject.GetComponent<EnemyMovement>() != null)
        {
            Die();
        }
    }
    
    void Update()
    {
        // No moverse si está muerto
        if (isDead) return;

        // Obtener input del jugador con WASD
        float horizontal = 0f;
        float vertical = 0f;

        if (Input.GetKey(KeyCode.W)) vertical += 1f;
        if (Input.GetKey(KeyCode.S)) vertical -= 1f;
        if (Input.GetKey(KeyCode.A)) horizontal -= 1f;
        if (Input.GetKey(KeyCode.D)) horizontal += 1f;
        
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