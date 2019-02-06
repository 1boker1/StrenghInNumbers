using System.Collections;
using UnityEngine;

public class TestPowerUp : MonoBehaviour, IPowerUp
{
    public SpriteRenderer spriteRenderer;
    public Collider2D myTrigger;

    public float appliedTime = 10f;

    public IEnumerator ApplyPowerup(Controller number)
    {
        Debug.Log("Apply");

        myTrigger.enabled = false;
        spriteRenderer.enabled = false;

        yield return new WaitForSeconds(appliedTime);

        Debug.Log("Disable");

        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Controller>() != null)
        {
            StartCoroutine(ApplyPowerup(collision.GetComponent<Controller>()));
        }
    }
}
