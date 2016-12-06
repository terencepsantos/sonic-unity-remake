using UnityEngine;
using System.Collections;
using System;

public class Enemy : MonoBehaviour, ITakeDamage
{
    private int bulletsAmount = 2;
    private float bulletsRotation = 40;

    public Transform[] BulletsStartingPoints;
    public GameObject EnemyBulletPrefab;

    public enum AnimState
    {
        IsIdle,
        IsActive,
        IsDying
    }
    public AnimState animState { get; private set; }

    public int Health { get; set; }


    public void FireBullets()
    {
        int rotationInverter = 1;

        for (int i = 0; i < bulletsAmount; i++)
        {
            Instantiate(EnemyBulletPrefab, BulletsStartingPoints[i].position, Quaternion.Euler(0, 0, bulletsRotation * rotationInverter));
            rotationInverter *= -1;
        }
    }

    public void TakeDamage()
    {
        Health--;

        if (Health <= 0)
            Death();
    }

    public void Death()
    {
        animState = AnimState.IsIdle;
    }

    public void SetInitialHealth()
    {
        Health = 1;
    }
}
