using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed;
    public float damage;

    public GameObject particles;

    public GameObject parent;

    void Update()
    {
        Move();
    }

    public void Move()
    {
        transform.Translate(Vector2.up * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject != parent)
        {
            if (collision.GetComponent<Controller>() != null)
            {
                Player player = collision.GetComponentInParent<Player>();

                player.TakeDamage();

                int direction = collision.transform.position.x > transform.position.x ? 1 : -1;

                Vector3 position = new Vector3(collision.transform.position.x, transform.position.y, transform.position.z);
                Quaternion rotation = Quaternion.Euler(0, 0, 90 * direction);

                Instantiate(particles, position, rotation);

                CameraMovement.instance.Shake();
            }

            Destroy(gameObject);
        }
    }
}
