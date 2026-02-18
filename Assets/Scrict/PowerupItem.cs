using UnityEngine;
using System.Collections;

public class PowerupItem : MonoBehaviour
{
    [Header("การเดิน")]
    public float moveSpeed = 2f;
    private int direction = 1;
    private Rigidbody2D rb;
    private bool isCollected = false;

    [Header("Buff")]
    public float duration = 5f;
    public float scaleMultiplier = 1.3f;
    public float speedMultiplier = 1.5f;

    void Start() { rb = GetComponent<Rigidbody2D>(); if (rb) rb.freezeRotation = true; }

    void FixedUpdate()
    {
        if (!isCollected && rb != null) rb.linearVelocity = new Vector2(moveSpeed * direction, rb.linearVelocity.y);
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (isCollected) return;
        if (col.gameObject.CompareTag("Player")) StartCoroutine(ApplyPowerup(col.gameObject));
        else if (col.contacts.Length > 0 && Mathf.Abs(col.contacts[0].normal.x) > 0.5f) direction *= -1;
    }

    IEnumerator ApplyPowerup(GameObject player)
    {
        isCollected = true;
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<Collider2D>().enabled = false;

        SpriteRenderer sr = player.GetComponent<SpriteRenderer>();
        if (sr != null) sr.enabled = true;

        Time.timeScale = 0f;
        Vector3 originalScale = player.transform.localScale;
        Vector3 targetScale = originalScale * scaleMultiplier;

        for (int i = 0; i < 3; i++)
        {
            player.transform.localScale = targetScale;
            yield return new WaitForSecondsRealtime(0.1f);
            player.transform.localScale = originalScale;
            yield return new WaitForSecondsRealtime(0.1f);
        }
        player.transform.localScale = targetScale;
        Time.timeScale = 1f;

        PlayerHealth health = player.GetComponent<PlayerHealth>();
        PlayerController move = player.GetComponent<PlayerController>();
        if (health) { health.Heal(1); health.isInvincible = true; }
        if (move) move.moveSpeed *= speedMultiplier;

        float t = 0;
        while (t < duration)
        {
            if (sr) { sr.enabled = true; sr.color = Color.HSVToRGB(Mathf.PingPong(Time.time * 5f, 1f), 1f, 1f); }
            t += Time.deltaTime;
            yield return null;
        }

        if (sr) sr.color = Color.white;
        if (health) health.isInvincible = false;
        player.transform.localScale = originalScale;
        if (move) move.moveSpeed /= speedMultiplier;
        Destroy(gameObject);
    }
}