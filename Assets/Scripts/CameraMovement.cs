using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public static CameraMovement instance;

    public List<Transform> players = new List<Transform>();

    private Camera camera;
    private Transform cameraParent;


    public Vector3 offset;
    public Vector3 velocity;

    public float smoothTime = 0.5f;

    public float minSize = 10f;
    public float maxSize = 45f;
    public float zoomLimiter = 50f;

    public float shakeAmount = 0.5f;

    private void Awake()
    {
        if (instance == null) instance = this;
        if (instance != this) Destroy(gameObject);
    }

    void Start()
    {
        camera = GetComponent<Camera>();
        cameraParent = camera.transform.parent;
    }

    void LateUpdate()
    {
        Move();
    }

    public void Move()
    {
        if (players.Count == 0) return;

        Bounds bounds = new Bounds();

        foreach (Transform player in players)
        {
            if (player == null) break;

            bounds.Encapsulate(player.position);
        }

        cameraParent.position = Vector3.SmoothDamp(cameraParent.position, bounds.center + offset, ref velocity, smoothTime);

        float newZoom = Mathf.Lerp(minSize, maxSize, bounds.size.x / zoomLimiter);

        camera.orthographicSize = Mathf.Lerp(camera.orthographicSize, newZoom, Time.deltaTime);
    }

    public void Shake()
    {
        StartCoroutine(CameraShake(shakeAmount));
    }

    public IEnumerator CameraShake(float amount)
    {
        Vector3 startPosition = transform.localPosition;

        for (int i = 0; i < 10; i++)
        {
            transform.localPosition = startPosition + Random.insideUnitSphere * amount;

            yield return new WaitForEndOfFrame();
        }

        transform.localPosition = startPosition;
    }
}
