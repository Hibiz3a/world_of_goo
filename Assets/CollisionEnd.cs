using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionEnd : MonoBehaviour
{
    [SerializeField] private string gooTag = "Goo";
    private Level_Manger LevelManager;

    private bool isGameOver = false;

    private void Start()
    {
        LevelManager = GetComponentInParent<Level_Manger>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(gooTag) && !isGameOver)
        {
            isGameOver = true;
            LevelManager.EndLevelLoose();
        }
    }
    
}
