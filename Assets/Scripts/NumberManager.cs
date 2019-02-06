using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumberManager : MonoBehaviour
{
    public static NumberManager instance;

    public GameObject spawnPoint;

    public int numberOfPlayers;

    private void Awake()
    {
        if (instance == null) instance = this;
        if (instance != this) Destroy(gameObject);
    }

    private void Update()
    {

    }
}
