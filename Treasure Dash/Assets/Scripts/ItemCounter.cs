using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCounter : MonoBehaviour
{
    [SerializeField] private Transform arrowPos;
    [SerializeField] private int goal;

    public int gemCount;
    public GameObject exitArrow;
    // Start is called before the first frame update
    void Start()
    {
        // Deactivate the exit arrow so the location is a mystery
        exitArrow.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // Once all of the gems are collected, open the door and reveal the exit
        if(gemCount == goal)
        {
            // Reactivate the arrow to reveal the exit location
            exitArrow.SetActive(true);
            Destroy(gameObject);

        }
    }
}
