using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class kacauu : MonoBehaviour
{
    private Vector2 mousePosition;
    public void Update()
    {
        if(Mouse.current != null)
        {
            mousePosition = Mouse.current.position.ReadValue();

            //Debug.Log("kacauu");
        }
    }

}
