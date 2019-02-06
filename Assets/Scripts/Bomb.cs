using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public Rigidbody2D rigidBody;

    public float fireForce = 100f;
    public float explodeForce = 100f;
    public float explodeTime = 3f;
    public float explodeTimer = 0f;

    public bool exploded = false;

    public Animation animationComponent;

    public AnimationClip slowBlink;
    public AnimationClip fastBlink;
    public AnimationClip explosion;

    private void Start()
    {
        explodeTimer = explodeTime;

        animationComponent.Play(slowBlink.name);

        rigidBody.AddForce(transform.up * fireForce);
    }

    private void Update()
    {
        explodeTimer -= Time.deltaTime;

        if (explodeTimer < 0 && !exploded)
        {
            exploded = transform;

            StartCoroutine(Explode());
        }
        else if (explodeTimer < explodeTime * 0.5f)
        {
            if (!animationComponent.IsPlaying(fastBlink.name))
                animationComponent.Play(fastBlink.name);
        }
    }

    private IEnumerator Explode()
    {
        CameraMovement.instance.Shake();

        yield return new WaitForSeconds(explosion.length);

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 10);

        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].GetComponentInParent<Player>() != null)
            {
                colliders[i].GetComponentInParent<Player>().TakeDamage();

                Vector3 direction = (colliders[i].transform.position - transform.position).normalized;

                colliders[i].GetComponent<Controller>().rigidBody.AddRelativeForce(direction * explodeForce, ForceMode2D.Impulse);
            }
        }

        Destroy(gameObject);
    }
}
