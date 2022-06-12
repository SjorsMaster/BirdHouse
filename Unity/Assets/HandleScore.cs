using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HandleScore : MonoBehaviour
{
    [SerializeField]Text scoreText;
    int score = 0;
    public void addScore(int value) {
        score += value;
        scoreText.text = $"SCORE:\n{score}";
    }
}
