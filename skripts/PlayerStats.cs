using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    public Slider healthBar;
    private float maxHealth = 100f;
    private float currentHealth = 100f;
    public Slider ExpSlider;
    public float exp = 0f;
    public float expToLevel = 100f;
    public int level = 0;
    public float speed;
    public float damage;
    

    [System.Serializable]
    public class LvlParametrs
    {
        public float Health;
        public float Exp;
        public float speed;
        public float damage;
    }
    public LvlParametrs[] lvlParametrs;
    void Start()
    {

    }
#if UNITY_EDITOR
    [ContextMenu("SetLvl")]
    public void SetLvl()
    {
        if (lvlParametrs.Length > 1)
        {
            float HP = lvlParametrs[0].Health;
            float exp = lvlParametrs[0].Exp;
            for (int i = 1; i < lvlParametrs.Length; i++)
            {
                HP *= 1.2f;
                exp *= 1.35f;
                HP = (float)Math.Round((double)HP, 0);
                exp = (float)Math.Round((double)exp, 0);
                lvlParametrs[i].Health = HP;
                lvlParametrs[i].Exp = exp;
            }
        }
        else
        {
            Debug.LogError("LowLvl");
        }
    }
#endif
}
