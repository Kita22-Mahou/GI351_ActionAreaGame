using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System;
using Unity.Mathematics;
public class Tree : MonoBehaviour
{
    [Header("TreeHP")]
    [SerializeField] private float maxHp = 50f;
    public float currentHp;

    [Header("Wood Drop")]

    [SerializeField] private GameObject woodPrefab;
    [SerializeField] private int minWoodDrop = 1;
    [SerializeField] private int maxWoodDrop = 5;
    [SerializeField] private float dropRadius = 1f;

    private void Awake()
    {
        currentHp = maxHp;
    }
    
    public void TakeDamage(float damage)
    {
        currentHp -= damage;

        Debug.Log("Tree HP: " + currentHp);

        if (currentHp < 0) 
        { 
            currentHp = 0;
           Destroy(gameObject);
            DropWood();
        }
    }

    void DropWood() 
    {
        int woodAmount = UnityEngine.Random.Range(minWoodDrop, maxWoodDrop + 1);

        Debug.Log("Wood Drop");

        for (int i = 0; i < woodAmount; i++) 
        {
            Vector2 randomPosition = UnityEngine.Random.insideUnitCircle * dropRadius;

            Vector3 spawnPosition = transform.position + (Vector3)randomPosition;

            Instantiate(woodPrefab,spawnPosition,quaternion.identity);
        }
    }
}
