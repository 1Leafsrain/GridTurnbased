using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class tesPlayer : MonoBehaviour//, IPointerClickHandler
{
    public string cardName = "Fireball";
    public int damage = 5;
    public bool requiresTarget = true;

    public tesEnemy[] daftarTarget;
    public tesamjah tesamjah;

    public GameObject buttons;

    public tesamjah battleManager;

    public void Start()
    {
        Debug.Log("woee1");
        // refresTarget();
    }
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            Debug.Log("woee1");
            OnPointerClick();
        }
    }

    public void OnPointerClick()
    {
        Debug.Log("woee1");
        //tesEnemy[] enemies = FindObjectsByType<tesEnemy>();

       
       /* List<tesEnemy> list = new List<tesEnemy>(enemies);
        foreach (tesEnemy enemy in list) 
        { 
            var g = Instantiate(buttons);
            g.GetComponentInChildren<TextMeshPro>().text = enemy.enemyName + "(" + enemy.health + ")"; 
            tesEnemy capturedEnemy = enemy;
            Debug.Log("woee2");
            g.GetComponent<Button>().onClick.AddListener(() =>
            {
                if (g == null)
                {
                    return;
                }
                //onSelected?.Invoke(capturedEnemy);
                //ClearButtons();
            });

        }*/
        
        //TargetSelector.Instance.ShowTargets(list, OnTargetSelected);
    }

    void OnTargetSelected(tesEnemy target)
    {
        if (target == null) return;
        //target.TakeDamage(damage);
        // tambahan: play anim, remove card, dsb.
    }

    public void OnCardClicked()
    {
        if (!requiresTarget)
        {
            Debug.Log(cardName + " used (no target)");
            return;
        }

        battleManager.showTarget((tesEnemy enemy) =>
        {
            UseOn(enemy);
        });
    }

    private void UseOn(tesEnemy target)
    {
        Debug.Log(cardName + " used on " + target.enemyName);
        //target.TakeDamage(damage);
    }

    public void refresTarget()
    {
        daftarTarget = tesamjah.tesamjahs;
    }
}
