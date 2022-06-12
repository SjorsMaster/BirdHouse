using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wallorwalltever : MonoBehaviour
{
    [SerializeField] bool destructable;
    private void OnCollisionEnter2D(Collision2D collision) {
        if (!destructable && collision.gameObject.tag == "Player")
            Destroy(collision.gameObject);
    }
}
