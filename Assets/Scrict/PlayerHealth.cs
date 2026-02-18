using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    [Header("ค่าพลังชีวิต")]
    public int maxHealth = 3;
    public int currentHealth;

    [Header("ระบบอมตะ (I-Frames)")]
    public float invincibilityDuration = 1.5f;
    private float invincibilityTimer;
    public bool isInvincible = false;

    [Header("UI & Effects")]
    public GameObject[] hearts;
    public Image redFlashImage;
    public SimpleCameraFollow cam;

    [Header("เสียงประกอบ")] // --- ส่วนที่เพิ่มมาใหม่ ---
    public AudioClip deathSound; // ลากไฟล์เสียงมาใส่ตรงนี้
    public AudioClip hurtSound;  // (แถม) เสียงตอนโดนดาเมจ
    private AudioSource audioSource;

    private SpriteRenderer playerSprite;

    void Start()
    {
        currentHealth = maxHealth;
        playerSprite = GetComponent<SpriteRenderer>();

        // ดึง AudioSource ที่เราเพิ่งสร้างมาเก็บไว้ใช้งาน
        audioSource = GetComponent<AudioSource>();

        UpdateHealthUI();
    }

    void Update()
    {
        // กะพริบตัวตอนอมตะ
        if (invincibilityTimer > 0 && currentHealth > 0 && Time.timeScale > 0)
        {
            invincibilityTimer -= Time.deltaTime;
            float blinkSpeed = 0.1f;
            if (playerSprite != null)
                playerSprite.enabled = (Mathf.Repeat(Time.time, blinkSpeed * 2) > blinkSpeed);
        }
        else
        {
            if (playerSprite != null && !playerSprite.enabled) playerSprite.enabled = true;
        }

        // เอฟเฟกต์จอแดงจางหาย
        if (redFlashImage != null && redFlashImage.color.a > 0)
        {
            Color c = redFlashImage.color;
            c.a -= Time.deltaTime * 2f;
            redFlashImage.color = c;
        }
    }

    public void TakeDamage(int damage)
    {
        if (isInvincible || invincibilityTimer > 0 || currentHealth <= 0) return;

        currentHealth -= damage;
        invincibilityTimer = invincibilityDuration;
        UpdateHealthUI();

        // เล่นเสียงตอนเจ็บ (ถ้ามี)
        if (audioSource != null && hurtSound != null)
            audioSource.PlayOneShot(hurtSound);

        if (cam != null) cam.TriggerShake(0.2f, 0.15f);
        if (redFlashImage != null) redFlashImage.color = new Color(1, 0, 0, 0.5f);

        if (currentHealth <= 0) StartCoroutine(EpicDeathSequence());
    }

    public void Heal(int amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        UpdateHealthUI();
    }

    void UpdateHealthUI()
    {
        if (hearts == null || hearts.Length == 0) return;
        for (int i = 0; i < hearts.Length; i++) hearts[i].SetActive(i < currentHealth);
    }

    IEnumerator EpicDeathSequence()
    {
        // --- เล่นเสียงตายตรงนี้! ---
        if (audioSource != null && deathSound != null)
        {
            audioSource.Stop(); // หยุดเสียงอื่นก่อน (เช่นเสียงเดิน/เสียงเพลง)
            audioSource.PlayOneShot(deathSound);
        }

        GetComponent<PlayerController>().enabled = false;
        GetComponent<Collider2D>().enabled = false;
        Rigidbody2D rb = GetComponent<Rigidbody2D>();

        if (playerSprite != null) { playerSprite.enabled = true; playerSprite.sortingOrder = 100; }

        // ดีดตัวขึ้นฟ้า
        rb.linearVelocity = new Vector2(0, 15f);

        float timer = 0f;
        Vector3 startScale = transform.localScale;
        while (timer < 2.0f)
        {
            transform.localScale = Vector3.Lerp(startScale, startScale * 5f, timer / 0.5f);
            transform.Rotate(0, 0, 1000f * Time.deltaTime);
            timer += Time.deltaTime;
            yield return null;
        }
        FindFirstObjectByType<GameOverManager>()?.PlayerDied();
    }
}