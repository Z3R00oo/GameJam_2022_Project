using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLifeController : MonoBehaviour
{
    public int lifeCount = 1;

    public AudioSource extraLifePowerupSoundFX;
    public AudioSource SlowMotionPowerupSoundFX;

    private GameManager gm;

    public GameObject powerupParticle;

    // Start is called before the first frame update
    void Start()
    {
        gm = FindObjectOfType<GameManager>().GetComponent<GameManager>();
    }

    private void Update()
    {
        if(lifeCount > 1)
        {
            powerupParticle.SetActive(true);
        }
        else
        {
            powerupParticle.SetActive(false);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("SlowMotion"))
        {
            gm.SetSlowMotion();
            SlowMotionPowerupSoundFX.Play();
            other.gameObject.SetActive(false);
        }

        if (other.gameObject.CompareTag("ExtraLife"))
        {
            gm.GetExtraLife();
            extraLifePowerupSoundFX.Play();
            other.gameObject.SetActive(false);
        }
    }
}
