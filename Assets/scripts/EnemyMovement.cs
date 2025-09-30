using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [Header("Enemy Settings")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private int health = 1;
    [SerializeField] private float boundaryLimit = 12f; // Límite para destruir enemigos fuera del plano

    private Rigidbody rb;
    private Transform player;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        
        // Encontrar al jugador en la escena
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
        
        // Congelar rotación para que el cilindro no se voltee
        if (rb != null)
        {
            rb.freezeRotation = true;
        }
    }
    
    void Update()
    {
        if (player != null)
        {
            // Calcular dirección hacia el jugador (solo en el plano XZ)
            Vector3 direction = (player.position - transform.position);
            direction.y = 0f; // Mantener movimiento horizontal
            direction.Normalize();

            // Mover hacia el jugador
            if (rb != null)
            {
                rb.linearVelocity = new Vector3(direction.x * moveSpeed, rb.linearVelocity.y, direction.z * moveSpeed);
            }
        }

        // Destruir si sale de los límites del plano
        CheckBoundaries();
    }

    void CheckBoundaries()
    {
        Vector3 pos = transform.position;

        // Si está muy lejos del plano, destruir
        if (Mathf.Abs(pos.x) > boundaryLimit || Mathf.Abs(pos.z) > boundaryLimit)
        {
            Destroy(gameObject);
        }
    }
    
    public void TakeDamage(int damage)
    {
        health -= damage;
        
        if (health <= 0)
        {
            Die();
        }
    }
    
    void Die()
    {
        Destroy(gameObject);
    }
    
    void OnCollisionEnter(Collision collision)
    {
        // Si colisiona con el jugador, destruirse y matar al jugador
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerMovement playerMovement = collision.gameObject.GetComponent<PlayerMovement>();
            if (playerMovement != null)
            {
                playerMovement.Die();
            }
            Die();
        }
    }
}