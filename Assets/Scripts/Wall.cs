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
        float healthAtStart = hitpoints;

        if (damageLeft < wallMaterial.damageAbsorption)
        {
            hitpoints -= damageLeft;
        }
        else
        {
            hitpoints -= wallMaterial.damageAbsorption;
        }


        if (hitpoints > 0)
        {
            damageLeft -= wallMaterial.damageAbsorption;
        }
        else
        {
            damageLeft -= healthAtStart;
            wallModel.SetActive(false);
        }
    }
}