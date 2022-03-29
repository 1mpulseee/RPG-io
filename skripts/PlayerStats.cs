using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    public Slider healthBar;
    public Slider ExpSlider;
    
    private float maxHealth = 100f;
    private float currentHealth = 100f;
    
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
        UpdateParametrs(0);
    }
    private void FixedUpdate()
    {
        if (exp > expToLevel)
        {
            level++;
            exp = 0;
            UpdateParametrs(level);
        }
    }
    public void UpdateParametrs(int lvl)
    {
        maxHealth = lvlParametrs[lvl].Health;
        currentHealth = maxHealth;
        expToLevel = lvlParametrs[lvl].Exp;
        speed = lvlParametrs[lvl].speed;
        damage = lvlParametrs[lvl].damage;
        UpdateSliders();
    }
    public void UpdateSliders()
    {
        healthBar.maxValue = maxHealth;
        healthBar.value = currentHealth;
        ExpSlider.maxValue = expToLevel;
        ExpSlider.value = exp;
    }
#if UNITY_EDITOR
    [ContextMenu("SetLvl")]
    public void SetLvl()
    {
        if (lvlParametrs.Length > 1)
        {
            float HP = lvlParametrs[0].Health;
            float exp = lvlParametrs[0].Exp;
            float speed = lvlParametrs[0].speed;
            float damage = lvlParametrs[0].damage;
            for (int i = 1; i < lvlParametrs.Length; i++)
            {
                HP *= 1.2f;
                exp *= 1.35f;
                speed *= 0.95f;
                damage *= 1.1f;
                HP = (float)Math.Round((double)HP, 0);
                exp = (float)Math.Round((double)exp, 0);
                speed = (float)Math.Round((double)speed, 2);
                damage = (float)Math.Round((double)damage, 0);
                lvlParametrs[i].Health = HP;
                lvlParametrs[i].Exp = exp;
                lvlParametrs[i].speed = speed;
                lvlParametrs[i].damage = damage;
            }
        }
        else
        {
            Debug.LogError("LowLvl");
        }
    }
#endif
}
