using System;
using UnityEngine;

public class Wall : MonoBehaviour
{
    private float hitpoints;
    public WallMaterial wallMaterial;
    private AudioSource audioSource;
    private GameObject wallModel;

    private void Awake()
    {
        wallModel = transform.GetChild(0).gameObject;
        hitpoints = wallMaterial.hitPoints;
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = wallMaterial.hitSound;
    }

    public void OnHit(ref float damageLeft)
    {
        audioSource.Play();
        if (damageLeft < wallMaterial.damageAbsorption)
        {
            hitpoints -= damageLeft;
            damageLeft = 0;
        }
        else
        {
            hitpoints -= wallMaterial.damageAbsorption;
            damageLeft -= wallMaterial.damageAbsorption;
        }
        
        if (hitpoints <= 0)
        {
            wallModel.SetActive(false);
        }
    }
}
