using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    public TMP_Text messageText;
    // Start is called before the first frame update
    void Start()
    {
        //messageText = GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        
        
    }

    public void GetDamage(string jumlah)
    {
        messageText.text = "-" + jumlah;
        StartCoroutine(ShowsDamage());
    }

    public void GetHealth(string jumlah)
    {
        messageText.text = "HP :" + jumlah;
        //StartCoroutine(ShowsDamage());
    }

    public void getAction(string jumlah)
    {
        messageText.text = "Actions :" + jumlah;
        //StartCoroutine(ShowsDamage());
    }

    private IEnumerator ShowsDamage()
    {
        messageText.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        messageText.gameObject.SetActive(false);
    }
}
