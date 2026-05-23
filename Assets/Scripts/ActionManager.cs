using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionManager : MonoBehaviour
{
    public static ActionManager Instance;
    [SerializeField] public List<PlainEffect> effects = new List<PlainEffect>();
    [SerializeField] public List<GameObject> targets = new List<GameObject>();
    [SerializeField] public List<int> intervals = new List<int>();

    public enum Turn
    {
        Post,
        onBattle,
        After
    }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {

    }

    public void daftarEffect(PlainEffect effect, GameObject target, int interval)
    {
        effects.Add(effect);
        targets.Add(target);
        intervals.Add(interval);
    }

    public void ExecuteEffects()
    {
        for (int i = 0; i < effects.Count; i++)
        {
            if (effects[i] != null && targets[i] != null)
            {
                effects[i].OnAfterBattle(targets[i], intervals[i]);
                Debug.Log(effects[i] + " executed on target: " + targets[i].name);
            }
            else
            {
                Debug.LogAssertion("kosong");
            }
        }

        effects.Clear();
        intervals.Clear();
        targets.Clear();
    }
}
