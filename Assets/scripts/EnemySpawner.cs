using UnityEngine;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] private GameObject[] enemyPrefabs; // Array de prefabs de enemigos
    [SerializeField] private int maxEnemies = 10; // Máximo de enemigos en escena
    [SerializeField] private float spawnInterval = 2f; // Tiempo entre spawns
    [SerializeField] private float spawnDistance = 12f; // Distancia desde el centro para spawnear

    [Header("Spawn Area")]
    [SerializeField] private float spawnAreaSize = 9f; // Área de spawn (dentro de límites del plano)

    private float nextSpawnTime = 0f;
    private List<GameObject> activeEnemies = new List<GameObject>();
    
    void Update()
    {
        // Limpiar referencias nulas (enemigos destruidos)
        activeEnemies.RemoveAll(enemy => enemy == null);
        
        // Spawnear si es momento y hay espacio
        if (Time.time >= nextSpawnTime && activeEnemies.Count < maxEnemies)
        {
            SpawnEnemy();
            nextSpawnTime = Time.time + spawnInterval;
        }
    }
    
    void SpawnEnemy()
    {
        if (enemyPrefabs == null || enemyPrefabs.Length == 0)
        {
            Debug.LogWarning("No hay prefabs de enemigos asignados al spawner");
            return;
        }
        
        // Elegir un prefab aleatorio
        GameObject prefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
        
        if (prefab == null)
        {
            Debug.LogWarning("Prefab de enemigo es null");
            return;
        }
        
        // Generar posición aleatoria en los bordes del plano
        Vector3 spawnPosition = GetRandomSpawnPosition();
        
        // Crear el enemigo
        GameObject enemy = Instantiate(prefab, spawnPosition, Quaternion.identity);
        activeEnemies.Add(enemy);
    }
    
    Vector3 GetRandomSpawnPosition()
    {
        // Elegir un lado aleatorio (norte, sur, este, oeste)
        int side = Random.Range(0, 4);
        float x = 0f;
        float z = 0f;
        
        switch (side)
        {
            case 0: // Norte
                x = Random.Range(-spawnAreaSize, spawnAreaSize);
                z = spawnDistance;
                break;
            case 1: // Sur
                x = Random.Range(-spawnAreaSize, spawnAreaSize);
                z = -spawnDistance;
                break;
            case 2: // Este
                x = spawnDistance;
                z = Random.Range(-spawnAreaSize, spawnAreaSize);
                break;
            case 3: // Oeste
                x = -spawnDistance;
                z = Random.Range(-spawnAreaSize, spawnAreaSize);
                break;
        }
        
        return new Vector3(x, 1f, z); // Y=1 para que aparezca sobre el plano
    }
    
    // Método para mostrar el área de spawn en el editor
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(Vector3.zero, new Vector3(spawnDistance * 2, 0.1f, spawnDistance * 2));
    }
}