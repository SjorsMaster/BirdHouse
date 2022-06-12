using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class incomingwoo : MonoBehaviour {
    [SerializeField] float speed = 15;
    [SerializeField] bool seen = false;

    private void Update() {
        transform.position += -Vector3.up * speed * Time.deltaTime;
    }

    private void OnBecameVisible() {
        seen = true;
    }

    private void OnBecameInvisible() {
        if (seen)
            Destroy(gameObject);
    }
}
