using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boxInteractable : MonoBehaviour
{

    public GameObject EndTurnButton;
    [SerializeField] public GameObject[] isi;
    // Start is called before the first frame update

    void Awake()
    {

        
    }

    void Start()
    {
        Debug.Log("kena deh");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                Debug.Log("kena deh");
                EndTurnButton.SetActive(true);
            }
        }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("kena deh");
            EndTurnButton.SetActive(false);
        }
    }

    public void dibuka()
    {
        if(isi.Length == 0)
        {
            Debug.Log("Box is empty");
            return;
        }
        int randomIndex = Random.Range(0, isi.Length);
        GameObject item = Instantiate(isi[randomIndex], transform.position, Quaternion.identity);
        Destroy(transform.parent.gameObject);
    }
}
