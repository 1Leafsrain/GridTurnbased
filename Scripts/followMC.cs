using UnityEngine;

public class followMC : MonoBehaviour
{
    public GameObject Player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void Awake()
    {
        Player = GameObject.FindGameObjectWithTag("PlayerSprite"); 
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Player != null)
        {
            transform.position = Player.transform.position * 1f * Time.deltaTime;
        }
        else if(Player == null){
            Player = GameObject.FindGameObjectWithTag("PlayerSprite");
        }
        else
        {
            Debug.Log("Mana jir mc nya kocak");
        }
            
    }
}
