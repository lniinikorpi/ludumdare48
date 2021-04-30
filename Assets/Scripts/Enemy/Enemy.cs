using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.ProceduralSystem;

public class Enemy : MonoBehaviour
{
    public float maxHp;
    private float currentHp;
    public GameObject shooter;
    public float gunDamage = 10;
    public float touchDamage = 10;
    ObjectPooler.Pool pool;
    ObjectPooler objectPooler;
    public float deathAudioVolume;
    public AudioSource audioSource;
    public List<AudioClip> hitClips = new List<AudioClip>();
    public AudioClip deathClip;
    [HideInInspector]
    public bool isAlive = true;
    public EnemyShoot enemyShoot;
    public EnemyMovement enemyMovement;
    public GameObject deathExplosion;
    public float shooterAfterDropHeight = 1;
    // Start is called before the first frame update
    void Start()
    {
        currentHp = maxHp;
        objectPooler = ObjectPooler.instance;
        pool = objectPooler.pools[1];
        if (enemyShoot && !enemyMovement)
        {
            enemyShoot.damage = gunDamage;
        }
    }

    public void TakeDamage(float value, Vector3 hitPoint)
    {
        if(currentHp - value > 0)
        {
            currentHp -= value;
            Vector3 direction = (hitPoint - transform.position).normalized;
            Quaternion bloodRot = Quaternion.LookRotation(direction);
            GameObject blood = objectPooler.SpawnFromPool("EnemyBloodSplatter", hitPoint, bloodRot, pool);
            //blood.transform.parent = gameObject.transform;
            blood.transform.localEulerAngles = new Vector3(0, blood.transform.localEulerAngles.y, 0);
            PlayHitAudio();
        }
        else
        {
            currentHp = 0;
            Die();
        }
    }

    void Die()
    {
        isAlive = false;
        GetComponent<Collider>().enabled = false;
        GameObject obj = Instantiate(deathExplosion, transform.position, Quaternion.identity);
        Explosion exp = obj.GetComponent<Explosion>();
        if(exp)
        {
            exp.PlayDeathSound(deathClip, deathAudioVolume);
        }
        if (enemyMovement)
        {
            if (enemyMovement.enabled)
            {
                enemyMovement.enabled = false;
            }
        }
        if (enemyShoot)
        {
            DropWitch(); 
        }
        gameObject.SetActive(false);
    }

    void DropWitch()
    {
        enemyShoot.animator.SetTrigger("InAir");
    }

    public void PlayHitAudio()
    {
        audioSource.clip = hitClips[Random.Range(0, hitClips.Count - 1)];
        audioSource.Play();
    }
    
    public void SetInAirToTrue()
    {
        enemyShoot.inAir = true;
    }

    public void SetInAirToFalse()
    {
        enemyShoot.inAir = false;
        shooter.transform.position = new Vector3(transform.position.x, GameManager.instance.player.transform.position.y - shooterAfterDropHeight , transform.position.z);
    }
}
