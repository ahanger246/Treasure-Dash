using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitRoom : MonoBehaviour
{
    [SerializeField] private LevelController manager;

    void OnTriggerEnter2D(Collider2D o) {
        if (o.tag == "Player") {
            manager.nextLevel();
        }
    }
}
