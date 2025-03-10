using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Team
{
    None,
    Player,
    Enemy
}

public class Entity : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float health = 100f;
    [SerializeField] private Team team;
    [SerializeField] private int scoreValue = 0_0;

    [SerializeField] private GameObject deathParticler;

    [SerializeField] private AudioSource hitSound;

    [SerializeField] private Material flashMaterial;
    private float flashDurationSeconds = 0.1f;
    private Coroutine flashRoutine;

    protected Rigidbody2D rigidBody;
    private SpriteRenderer spriteRenderer;
    private Material material;

    private float bounceAwayForce = 3f;

    private bool isDead = false;

    protected virtual void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        material = spriteRenderer.material;
    }

    public Team GetTeam()
    {
        return team;
    }
    public void SetTeam(Team newTeam)
    {
        team = newTeam;
    }
    public float GetHealth()
    {
        return health;
    }
    public float GetMaxHealth()
    {
        return maxHealth;
    }
    public int GetScoreValue()
    {
        return scoreValue;
    }

    public void Damage(float damage)
    {
        if (this is ShipPlayer)
        {
            ShipPlayer playerClass = (ShipPlayer)this;
            playerClass.DisableNearMissReward();
        }

        hitSound.Play();

        Flash();
        health -= damage;
        if (health <= 0_0 && !isDead)
        {
            GameManager.AddScore(scoreValue);
            DestroySelf();
        }
    }

    public virtual void DestroySelf(bool noParticles = false)
    {
        if (deathParticler && !noParticles)
        {
            GameObject instance = Instantiate(deathParticler);
            instance.transform.position = transform.position;
        }

        isDead = true;
        Destroy(gameObject);
    }

    public void BounceAwayFrom(Transform bounceTarget)
    {
        rigidBody.AddForce((transform.position - bounceTarget.position).normalized * bounceAwayForce, ForceMode2D.Impulse);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Entity collidingEntity = collision.gameObject.GetComponent<Entity>();
        if (collidingEntity != null)
        {
            BounceAwayFrom(collision.transform);
        }
    }

    private IEnumerator FlashRoutine()
    {
        spriteRenderer.material = flashMaterial;
        yield return new WaitForSeconds(flashDurationSeconds);
        spriteRenderer.material = material;
        flashRoutine = null;
    }

    private void Flash()
    {
        if (flashRoutine != null) StopCoroutine(flashRoutine);
        flashRoutine = StartCoroutine(FlashRoutine());
    }

    public void HealToFull()
    {
        health = maxHealth;
    }

    public void Heal(float healAmount)
    {
        health = Mathf.Min(health + healAmount, maxHealth);
    }
}
