using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootPosition : MonoBehaviour
{
    public GameObject bullet;
    public GameObject parent;

    public float attackSpeed = 0.2f;
    public bool canShoot = true;

    private void Start()
    {
        parent = transform.parent.gameObject;
    }

    public void SpawnBullet()
    {
        if (!canShoot) return;

        canShoot = false;

        GameObject spawnedBullet = Instantiate(bullet, transform.position, transform.rotation);

        if (spawnedBullet.GetComponent<Bullet>() != null)
        {
            spawnedBullet.GetComponent<Bullet>().parent = parent;
        }

        StartCoroutine(Reload());
    }

    public IEnumerator Reload()
    {
        yield return new WaitForSeconds(attackSpeed);

        canShoot = true;
    }
}
