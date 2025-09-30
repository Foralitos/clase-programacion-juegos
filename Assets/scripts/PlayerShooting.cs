using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    [Header("Shooting Settings")]
    [SerializeField] private float bulletSpeed = 20f;
    [SerializeField] private float bulletLifetime = 5f;
    [SerializeField] private float fireRate = 0.5f; // Tiempo entre disparos

    private float nextFireTime = 0f;

    void Update()
    {
        // Disparar con las flechas del teclado
        if (Time.time >= nextFireTime)
        {
            Vector3 shootDirection = Vector3.zero;

            // Detectar qué flecha se presionó
            if (Input.GetKey(KeyCode.UpArrow))
            {
                shootDirection = Vector3.forward; // Disparar hacia adelante
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                shootDirection = Vector3.back; // Disparar hacia atrás
            }
            else if (Input.GetKey(KeyCode.LeftArrow))
            {
                shootDirection = Vector3.left; // Disparar hacia la izquierda
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                shootDirection = Vector3.right; // Disparar hacia la derecha
            }

            // Si se presionó alguna flecha, disparar en esa dirección
            if (shootDirection != Vector3.zero)
            {
                Shoot(shootDirection);
                nextFireTime = Time.time + fireRate;
            }
        }
    }

    void Shoot(Vector3 direction)
    {
        // Crear la esfera
        GameObject bullet = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        bullet.name = "Bullet";

        // Posicionar la bala en la dirección de disparo
        bullet.transform.position = transform.position + direction * 0.7f + Vector3.up * 0.2f;
        bullet.transform.localScale = Vector3.one * 0.3f; // Hacer la bala más pequeña

        // Agregar Rigidbody para física
        Rigidbody bulletRb = bullet.AddComponent<Rigidbody>();
        bulletRb.useGravity = false; // Sin gravedad para que vuele recto

        // Disparar en la dirección especificada
        bulletRb.linearVelocity = direction * bulletSpeed;

        // Agregar script para destruir la bala después de un tiempo
        BulletLifetime bulletLife = bullet.AddComponent<BulletLifetime>();
        bulletLife.Initialize(bulletLifetime);

        // Cambiar material para distinguir las balas
        Renderer bulletRenderer = bullet.GetComponent<Renderer>();
        if (bulletRenderer != null)
        {
            bulletRenderer.material.color = Color.red;
        }
    }
}

// Script simple para destruir las balas después de un tiempo
public class BulletLifetime : MonoBehaviour
{
    private float lifetime = 3f;

    public void Initialize(float lifeTime)
    {
        lifetime = lifeTime;
    }

    void Start()
    {
        Destroy(gameObject, lifetime);
    }
    
    void OnCollisionEnter(Collision collision)
    {
        // Destruir la bala si golpea algo que no sea el jugador
        if (!collision.gameObject.CompareTag("Player"))
        {
            // Si golpea a un enemigo, causarle daño
            EnemyMovement enemy = collision.gameObject.GetComponent<EnemyMovement>();
            if (enemy != null)
            {
                enemy.TakeDamage(1);
            }

            Destroy(gameObject);
        }
    }
}