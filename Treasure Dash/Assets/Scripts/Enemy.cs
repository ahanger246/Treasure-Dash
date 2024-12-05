using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] float speed = 3f;
    [SerializeField] private Vector3[] path;
    private int targetIndex;
    private float timer;
    private Vector3 currentWaypoint;

    void Start() {
        PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);
        timer = 1;
        targetIndex = 0;
    }
    
    void Update() {
        timer -= Time.deltaTime;
        if(timer <= 0) {
            PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);
            targetIndex = 0;
            timer = 1.7f;
            currentWaypoint = path[targetIndex++];
        }
    }

    public void OnPathFound(Vector3[] newPath, bool pathSuccess) {
        if (pathSuccess) { 
            path = newPath;
            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");   
        }
    }

    IEnumerator FollowPath() {
        Vector3 currentWaypoint = path[0];

        while (true) {
            if (transform.position == currentWaypoint) {
                targetIndex++;
                if (targetIndex >= path.Length) {
                    yield break;
                }
                currentWaypoint = path[targetIndex];
            }
            transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, speed * Time.deltaTime);

            yield return null;
        }
    }
}
