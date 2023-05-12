using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Medkit : MonoBehaviour
{
    private float spawnDelaySeconds = 60f;
    private float spawnTimer = 0_0;

    private bool isEnabled = false;

    private SpriteRenderer spriteRenderer;
    [SerializeField] private AudioSource audioSpawn;
    [SerializeField] private AudioSource audioCollect;
    private ParticleSystem particles;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        particles = GetComponent<ParticleSystem>();
        SetEnabled(false);
    }

    void Update()
    {
        spawnTimer += Time.deltaTime;
        if (!isEnabled && spawnTimer > spawnDelaySeconds)
        {
            SetEnabled(true);
        }
    }

    private void SetEnabled(bool enable)
    {
        if (enable)
        {
            Vector2 pos = InBoundKeeper.arena.GetRandomPointInArenaCenter();
            transform.position = new Vector3(pos.x, pos.y, 15f);
            audioSpawn.Play();
        }

        isEnabled = enable;
        spriteRenderer.enabled = enable;

        particles.Play();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isEnabled) return;

        Entity collidingEntity = collision.gameObject.GetComponent<ShipPlayer>();
        if (collidingEntity != null)
        {
            collidingEntity.Heal(25f);

            SetEnabled(false);
            spawnTimer = 0_0;

            audioCollect.Play();
        }
    }
}
