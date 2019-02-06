using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{

    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] public Rigidbody2D rigidBody;
    [SerializeField] private BoxCollider2D boxCollider;

    [SerializeField] private List<ShootPosition> shootPositions = new List<ShootPosition>();

    public LayerMask floorMask;
    public LayerMask defaultMask;

    private float direction = 0;
    public int playerNumber = 0;

    public float speed = 10;
    public float jumpForce = 10;

    public bool invertedFlip = false;

    public bool grounded;
    public bool passingPlatform = false;
    public bool invincible = false;

    public string horizontalAxis;

    private void Start()
    {
        playerNumber = GetComponentInParent<Player>().playerNumber;

        StartCoroutine(Invincible());

        CameraMovement.instance.players.Add(transform);
    }

    private void Update()
    {
        if (playerNumber == 0) return;

        direction = Input.GetAxisRaw("Horizontal_" + playerNumber);

        if (Input.GetButtonDown("Fire_" + playerNumber))
        {
            Shoot();
        }
        if (Input.GetButtonDown("Jump_" + playerNumber) && grounded && !passingPlatform)
        {
            Jump();
        }
        if (Input.GetKeyDown(KeyCode.S) && grounded && !passingPlatform)
        {
            passingPlatform = true;

            gameObject.layer = LayerMask.NameToLayer("Default");
        }

        if (passingPlatform)
        {
            FallPlatform();
        }

        Flip();
        Move();
    }

    private void FixedUpdate()
    {
        CheckFloor();
    }

    private void CheckFloor()
    {
        Vector2 overlapPosition = new Vector2(boxCollider.bounds.center.x, boxCollider.bounds.min.y);
        Vector2 overlapScale = new Vector2(boxCollider.bounds.extents.x, 0.1f);

        grounded = Physics2D.OverlapBox(overlapPosition, overlapScale, 0, floorMask);
    }

    private void Move()
    {
        transform.Translate(new Vector2(direction * speed, 0) * Time.deltaTime);
    }

    private void Jump()
    {
        rigidBody.AddForce(Vector2.up * jumpForce);

        //rigidBody.velocity = new Vector2(rigidBody.velocity.x, jumpForce);
    }

    private void Flip()
    {
        if (direction == 0) return;

        bool flip = direction == 1 ? false : true;

        if (invertedFlip) flip = !flip;

        transform.localScale = new Vector3(flip ? -1 : 1, transform.localScale.y, transform.localScale.z);
    }

    private void FallPlatform()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 2);

        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].tag == "Floor") return;
        }

        gameObject.layer = LayerMask.NameToLayer("Floor");

        passingPlatform = false;
    }

    private void Shoot()
    {
        foreach (ShootPosition shootPosition in shootPositions)
        {
            shootPosition.SpawnBullet();
        }
    }

    private void OnDestroy()
    {
        CameraMovement.instance.players.Remove(transform);
    }

    public IEnumerator Invincible()
    {
        for (int i = 0; i < 12; i++)
        {
            spriteRenderer.enabled = !spriteRenderer.enabled;

            yield return new WaitForSeconds(0.075f);
        }

        spriteRenderer.enabled = true;

        invincible = false;
    }
}
