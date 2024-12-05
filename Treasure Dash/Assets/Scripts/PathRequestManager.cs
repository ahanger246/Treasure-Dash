using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathRequestManager : MonoBehaviour
{
    Queue<request> requestQueue = new Queue<request>();
    request currentRequest;

    static PathRequestManager instance;
    EnemyMove enemyMove;

    bool processPath;

    void Awake() {
        instance = this;
        enemyMove = GetComponent<EnemyMove>();
    }

    public static void RequestPath(Vector3 pathStart, Vector3 pathEnd, Action<Vector3[], bool> callback) { 
        request newRequest = new request(pathStart, pathEnd, callback);
        instance.requestQueue.Enqueue(newRequest);
        instance.TryProcessNext();
    }

    void TryProcessNext() {
        if (!processPath && requestQueue.Count > 0) { 
            currentRequest = requestQueue.Dequeue();
            processPath = true;
           enemyMove.StartFindingPath(currentRequest.pathStart, currentRequest.pathEnd);
        }
    }

    public void FinishedPath(Vector3[] path, bool success) { 
        currentRequest.callback(path, success);
        processPath = false;
        TryProcessNext();
    }

    struct request {
        public Vector3 pathStart;
        public Vector3 pathEnd;
        public Action<Vector3[], bool> callback;

        public request(Vector3 start, Vector3 end, Action<Vector3[], bool> _callback) { 
            pathStart = start;
            pathEnd = end;
            callback = _callback;
        }
    }
}
