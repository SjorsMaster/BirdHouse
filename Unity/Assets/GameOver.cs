using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOver : MonoBehaviour
{
    [SerializeField] GameObject plr;
    [SerializeField] GameObject gameOver;
    // Update is called once per frame
    void Update()
    {
        if (plr) return;
        gameOver.SetActive(true);
        Destroy(this.gameObject);
    }
}
