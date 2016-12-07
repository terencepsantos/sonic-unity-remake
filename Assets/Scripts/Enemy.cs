﻿using UnityEngine;
using System.Collections;
using System;

public class Enemy : MonoBehaviour, ITakeDamage
{
    private int bulletsAmount = 2;
    private float bulletsRotation = 40;

    public Transform[] BulletsStartingPoints;
    public SpriteRenderer EnemySpriteRenderer;
    public GameObject EnemyBulletPrefab;
    public GameObject RingPrefab;

    public enum AnimState
    {
        IsIdle,
        IsActive,
        IsDying
    }
    public AnimState animState { get; private set; }

    public int Health { get; set; }


    void Awake()
    {
        gameObject.tag = "Enemy";
        SetInitialHealth(1);
    }


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
        EnemySpriteRenderer.enabled = false;

        var obj = Instantiate(RingPrefab, gameObject.transform.localPosition, Quaternion.identity) as GameObject;

        Destroy(transform.parent.gameObject);
    }


    public void SetInitialHealth(int initialHealth)
    {
        Health = initialHealth;
    }
}
