using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainGun : MonoBehaviour, IGun
{
    [SerializeField]
    private float fireRate;
    public float FireRate { get { return fireRate; } set { fireRate = value; } }
    [SerializeField]
    private bool isAutomatic;
    public bool IsAutomatic { get { return IsAutomatic; } set { IsAutomatic = value; } }
    [SerializeField]
    private float damage;
    public float Damage { get { return damage; } set { damage = value; } }
    [SerializeField]
    private GameObject muzzle;
    public GameObject Muzzle { get { return muzzle; } set { muzzle = value; } }
    [SerializeField]
    private AudioSource audioSource;
    public AudioSource AudioSource { get { return audioSource; } set { audioSource = value; } }

    private bool shooting;
    public bool Shooting { get { return shooting; } set { shooting = value; } }
    [SerializeField]
    private Transform muzzleTransform;
    public Transform MuzzleTransform { get { return muzzleTransform; } set { muzzleTransform = value; } }
    [SerializeField]
    private GameObject model;
    public LayerMask layerMask;
    private float canShoot = 0;
    private float currentBarrelSpeed = 0;
    ObjectPooler objectPooler;
    public Vector3 modelInitPosition;
    bool firstSpawn = true;

    private void Start()
    {
        objectPooler = ObjectPooler.instance;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (shooting)
        {
            FireWeapon();
        }
        else
        {
            SlowDownBarrel();
        }
    }

    public void Reload()
    {
        throw new System.NotImplementedException();
    }

    public void Shoot()
    {
        shooting = true;
    }

    void FireWeapon()
    {
        if (currentBarrelSpeed <= 2000)
        {
            currentBarrelSpeed += 30;
        }
        else
        {
            if (Time.time >= canShoot)
            {
                canShoot = Time.time + 1 / fireRate;
                audioSource.Play();
                GunShake();
                muzzle.SetActive(true);
                RaycastHit hit;
                if (Physics.Raycast(muzzleTransform.position, muzzleTransform.forward, out hit, Mathf.Infinity, layerMask))
                {
                    Debug.DrawRay(muzzleTransform.position, muzzleTransform.forward * hit.distance, Color.red, 2f);
                    if (hit.transform.CompareTag("Enemy"))
                    {
                        hit.transform.GetComponent<Enemy>().TakeDamage(damage, hit.point);
                    }
                    else
                    {
                        objectPooler.SpawnFromPool("WallParticles", hit.point, Quaternion.Euler(new Vector3(0, hit.transform.position.y, 0)), objectPooler.pools[2]);
                    }
                    GameObject obj = objectPooler.SpawnFromPool("BulletTrail", muzzleTransform.position, Quaternion.identity, objectPooler.pools[3]);
                    BulletTrail bulletTrail = obj.GetComponent<BulletTrail>();
                    bulletTrail.Initialize(muzzleTransform.position, hit.point);
                }
                else
                {
                    Debug.DrawRay(muzzleTransform.position, muzzleTransform.forward * 1000, Color.white, 2f);
                }
            }
        }
        model.transform.Rotate(new Vector3(0, 0, currentBarrelSpeed * Time.deltaTime));
    }

    public void UnShoot()
    {
        shooting = false;
        if(firstSpawn)
        {
            firstSpawn = false;
            modelInitPosition = model.transform.localPosition;
        }
        else
        {
            model.transform.localPosition = modelInitPosition;
        }
    }

    void SlowDownBarrel()
    {
        muzzle.SetActive(false);
        if (currentBarrelSpeed > 0)
        {
            currentBarrelSpeed -= 20;
            model.transform.Rotate(new Vector3(0, 0, currentBarrelSpeed * Time.deltaTime));
        }
        else
        {
            currentBarrelSpeed = 0;
        }
    }

    void GunShake()
    {
        if(modelInitPosition == Vector3.zero)
        {
            modelInitPosition = model.transform.localPosition;
        }
        Vector3 newPos = new Vector3(Random.Range(-.01f, .01f), Random.Range(-.01f, .01f), 0);
        model.transform.localPosition = modelInitPosition + newPos;
    }

    public void SwitchIn()
    {
        currentBarrelSpeed = 0;
        UnShoot();
    }
}
