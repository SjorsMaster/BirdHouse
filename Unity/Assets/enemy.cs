using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy : MonoBehaviour {
    [SerializeField] float speed = 2.35f;
    [SerializeField] float drift = 8000;
    [SerializeField] bool seen = false, red, canSubtract = true;
    [SerializeField] HandleScore score;
    string dieBy = "BulletBlue";
    private void Start() {
        if (red)
            dieBy = "BulletRed";
        drift = Random.Range(0, 10);
        score = GameObject.FindObjectOfType<HandleScore>();
    }

    private void Update() {
        transform.position += (-Vector3.up * speed + (Vector3.right * (float)Mathf.Sin(Time.time* drift))) * Time.deltaTime;
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == dieBy) {
            canSubtract = false;
            Destroy(gameObject);
        }
    }

    private void OnBecameVisible() {
        seen = true;
    }

    private void OnBecameInvisible() {
        if (seen) {
            if(canSubtract)score.addScore(-50);
            Destroy(gameObject);
        }
    }
}
