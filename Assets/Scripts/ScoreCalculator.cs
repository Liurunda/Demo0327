using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;
public class ScoreCalculator : MonoBehaviour{
    
    int last_color = -1;
    int total_score = 0;
    int combo = 0;
    public TMP_Text scoreUI;
    public void UpdateScore(int color){
        if(color == last_color){
            combo += 1;
        }else{
            combo = 1;
        }
        last_color = color;
        total_score += combo;
        scoreUI.text = "Score: " + total_score.ToString() + "   " +  "Combo:" + combo.ToString();
    }
}