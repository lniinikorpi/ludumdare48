using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGun
{
    public float FireRate { get; set; }
    public bool IsAutomatic { get; set; }
    public bool Shooting { get;set; }
    public float Damage { get; set; }
    public GameObject Muzzle { get; set; }
    public AudioSource AudioSource { get; set; }
    public Transform MuzzleTransform { get; set; }
    public void Shoot();
    public void UnShoot();
    public void Reload();
    public void SwitchIn();
}
