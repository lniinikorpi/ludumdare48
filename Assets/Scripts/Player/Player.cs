using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.ProceduralSystem;
public class Player : MonoBehaviour
{
    [SerializeField]
    private float maxHealth = 100;
    private float currentHealth;

    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private List<AudioClip> gruntSounds = new List<AudioClip>();
    public float getHitAgainTime = 1;
    float canTakeHit;
    bool invulnerable = false;
    PostProcessManager postProcessManager;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        postProcessManager = GetComponentInChildren<PostProcessManager>();
        HUDManager.instance.Updatehealth((int)currentHealth);
    }
    
    private void Update()
    {
        if(Time.time >= canTakeHit)
        {
            if(invulnerable)
            {
                invulnerable = false;
            }
        }
    }

    public void TakeDamage(float value)
    {
        if(invulnerable)
        {
            return;
        }
        print("hit taken");
        StartCoroutine(postProcessManager.HitFlash(.6f));
        invulnerable = true;
        canTakeHit = Time.time + getHitAgainTime;
        if(currentHealth - value > 0)
        {
            currentHealth -= value;
            audioSource.clip = gruntSounds[Random.Range(0, gruntSounds.Count - 1)];
            audioSource.Play();
        }
        else
        {
            currentHealth = 0;
            Die();
        }
        HUDManager.instance.Updatehealth((int)currentHealth);
    }

    void Die()
    {
        HUDManager.instance.GameOver();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            TakeDamage(other.GetComponent<Enemy>().touchDamage);
        }
        if (other.CompareTag("Portal"))
        {
            ProceduralSystem.Instance.GoToNextLevel();
        }
    }

    public void OnPause()
    {
        if(Time.timeScale > 0)
        {
            HUDManager.instance.Pause();
        }
        else
        {
            HUDManager.instance.Resume();
        }
    }
}
