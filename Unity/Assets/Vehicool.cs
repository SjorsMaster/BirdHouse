using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vehicool : MonoBehaviour {
    [SerializeField] AudioSource src;
    [SerializeField] float speed = 2.35f;
    private void Awake() {
        src.pitch = Random.Range(0.8f, 1.2f);
        src.Play();
    }

    private void Update() {
        transform.position += transform.up * speed * Time.deltaTime;
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag != "Player")
            Destroy(gameObject);
    }

    private void OnBecameInvisible() {
        Destroy(gameObject);
    }
}
