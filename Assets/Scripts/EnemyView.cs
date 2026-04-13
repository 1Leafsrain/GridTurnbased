using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyView : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI title;
    [SerializeField] public TextMeshProUGUI damages;
    [SerializeField] public TextMeshProUGUI cost;
    [SerializeField] public SpriteRenderer image;
    [SerializeField] public List<PlainEffect> effects;
    [SerializeField] public int Area;

    private Model model;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setup(Model model)
    {
        this.model = model;
        title.text = model.nama;
        cost.text = model.cost;
        Area = model.Area;
        //attack = model.damage;
        foreach (PlainEffect effect in model.effects)
        {
            effects = model.effects;
        }
        image.sprite = model.gambar;
        //damages.text = model.damage.ToString();
    }
}
