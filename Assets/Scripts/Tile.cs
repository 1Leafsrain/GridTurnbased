using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public GameObject Above;
    Vector2 Vectors2 ;
    // Start is called before the first frame update
    void Start()
    {
        Vectors2 = this.gameObject.transform.position;
        Collider2D hit = Physics2D.OverlapPoint(Vectors2);
        if (hit != null)
        {
            if (hit.gameObject.CompareTag("EnemyHand"))
            {
                Above = hit.gameObject;
            }
        }
        
            
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("EnemyHand"))
        {
            Above = collision.gameObject;
        }
    }
    public GameObject GetAbove()
    {
        return Above;
    }
}
