using System;
//using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.Splines;
using TMPro;
using UnityEngine.UI;

public class tesamjah : MonoBehaviour
{
    public tesEnemy[] tesamjahs;
    public GameObject buttonPrefab;
    public Transform buttonTransform;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void showTarget(Action<tesEnemy> onSelected)
    {
        ClearButtons();
        foreach (var enemy in tesamjahs)
        {
            if (enemy != null) 
            {
               /*  //GameObject g = Instantiate(buttonPrefab, buttonTransform.position, buttonTransform.rotation);
                var btn = Instantiate(buttonPrefab, buttonTransform.position, buttonTransform.rotation);
                btn.GetComponentInChildren<Text>().text = enemy.enemyName + "(" + enemy.health + ")";
                
                tesEnemy capturedEnemy = enemy;

                btn.GetComponent<Button>().onClick.AddListener(() =>
                {
                    onSelected?.Invoke(capturedEnemy);  
                    ClearButtons();
                });*/
            }

            
            
        }
        for (int i = 0; i < tesamjahs.Length; i++)
        {

        }
    }

    public void ClearButtons()
    {
        foreach (Transform c in buttonTransform)
            GameObject.Destroy(c.gameObject);


    }
}


