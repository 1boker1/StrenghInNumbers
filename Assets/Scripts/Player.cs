using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private List<GameObject> numberPrefab = new List<GameObject>();

    public int actualNumber = 1;
    public int health = 1;

    public int playerNumber;

    public Transform spawnPoint;

    public GameObject numberGameObject;

    private void Awake()
    {
        if (numberGameObject == null)
        {
            numberGameObject = Instantiate(numberPrefab[actualNumber - 1], spawnPoint.position, Quaternion.identity);
        }

        numberGameObject.transform.parent = this.transform;
    }

    private void SpawnNumber()
    {
        GameObject previousNumber = numberGameObject;

        numberGameObject = Instantiate(numberPrefab[actualNumber - 1], spawnPoint.position, Quaternion.identity);
        numberGameObject.transform.parent = this.transform;

        Destroy(previousNumber.gameObject);
    }

    public void TakeDamage()
    {
        if (GetComponentInChildren<Controller>().invincible) return;

        health = Mathf.Clamp(health - 1, 0, actualNumber);

        if (health == 0)
        {
            actualNumber = Mathf.Clamp(actualNumber - 1, 1, 9);

            health = actualNumber;

            GameObject previousNumber = numberGameObject;

            CameraMovement.instance.players.Remove(previousNumber.transform);

            numberGameObject = Instantiate(numberPrefab[actualNumber - 1], spawnPoint.position, Quaternion.identity);
            numberGameObject.transform.parent = this.transform;

            Destroy(previousNumber);
        }
    }
}
