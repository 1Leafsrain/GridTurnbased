using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class View : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI title;
    [SerializeField] public TextMeshProUGUI damage;
    [SerializeField] public TextMeshProUGUI cost;
    [SerializeField] public SpriteRenderer image;
    [SerializeField] public TextMeshProUGUI Area;

    private Model model;

    public Vector3 posisiAwal;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void setup(Model model)
    {
        this.model = model;
        title.text = model.nama;
        cost.text = model.cost;
        image.sprite = model.gambar;
        damage.text = model.damage.ToString();
        Area.text = model.Area.ToString();
    } 

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnMouseDown()
    {
        posisiAwal = transform.position;

        transform.position = GetMouseWorldPosition();
    }

    public void OnMouseDrag()
    {
        transform.position = GetMouseWorldPosition();
    }

    public void OnMouseUp()
    {
        transform.position = posisiAwal;
    }

    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mouseScreen = Input.mousePosition;
        mouseScreen.z = -Camera.main.transform.position.z; // set depth ke kamera
        return Camera.main.ScreenToWorldPoint(mouseScreen);
    }
}
