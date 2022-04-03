using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamageController : MonoBehaviour
{
    public float knockbackDuration;
    public int knockbackStrenght;

    public float stunTime;
    private float currentStunTime;

    private bool isStuned;

    private GameManager gm;
    private PlayerLifeController playerLife;
    private SpawnerController obstaclesSpawner;

    private Animator playerAnim;

    private Rigidbody2D rb;

    public AudioSource smashSoundEffect;
    public AudioSource damageSoundEffect;

    void Start()
    {
        gm = FindObjectOfType<GameManager>().GetComponent<GameManager>();
        playerLife = GetComponent<PlayerLifeController>();
        obstaclesSpawner = FindObjectOfType<SpawnerController>().GetComponent<SpawnerController>();
        rb = GetComponent<Rigidbody2D>(); 

        playerAnim = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        if (isStuned)
        {
            currentStunTime += Time.deltaTime;
            obstaclesSpawner.moveSpeed = obstaclesSpawner.minMoveSpeed;
            playerAnim.SetBool("isStuned", true);
            obstaclesSpawner.gameplayMusicSound.pitch = 1;
        }

        if(currentStunTime >= stunTime)
        {
            currentStunTime = 0;
            isStuned = false;
            playerAnim.SetBool("isStuned", false);
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.CompareTag("Obstacle"))
        {
            if (isStuned == false)
            {
                Knockback(knockbackDuration, knockbackStrenght, other.transform);
                playerLife.lifeCount--;
                isStuned = true;
                damageSoundEffect.Play();
            }

            if(playerLife.lifeCount <= 0) 
            {
                smashSoundEffect.Play();
                obstaclesSpawner.gameplayMusicSound.Stop();
                gm.CallGameOver();
            }
        }
    }

    void Knockback(float duration, float power, Transform obj)
    {
        float timer = 0;

        while(timer < duration)
        {
            timer += Time.deltaTime;
            Vector2 direction = (obj.transform.position - this.transform.position).normalized;
            rb.AddForce(-direction * power);
        }
    }
}
