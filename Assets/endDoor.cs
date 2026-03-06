using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class endDoor : MonoBehaviour
{
    GameObject player;
    GameObject Reset;
    public GameObject EndTurnButton;
    [SerializeField] public GameObject[] isi;
    // Start is called before the first frame update

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        Reset = GameObject.FindGameObjectWithTag("grid");

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
       
        int randomIndex = Random.Range(0, isi.Length);
        //GameObject item = Instantiate(isi[randomIndex], transform.position, Quaternion.identity);
        Reset = GameObject.FindGameObjectWithTag("grid");
        Reset.GetComponent<GenerateGridTile>().restartScene();
        
    }
}
