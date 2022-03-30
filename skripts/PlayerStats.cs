using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    public Animator HpAnim;
    public Slider healthBar;
    public Slider ExpSlider;

    private float maxHealth = 0f;
    public float CurrentHealth
    {
        set
        {
            currentHealth = value;
            UpdateSliders();
        }
        get { return currentHealth; }
    }
    private float currentHealth = 10f;

    public float WoodCount;

    public float Exp
    {
        set
        {
            exp = value;
            UpdateSliders();
        }
        get { return exp; }
    }
    
    
    private float exp;
    public float expToLevel = 100f;
    
    public int level = 0;
    
    public float speed;
    public float damage;
    public float Regen;

    [System.Serializable]
    public class LvlParametrs
    {
        public float Health;
        public float Exp;
        public float speed;
        public float damage;
        public float Regenerate;
    }
    public LvlParametrs[] lvlParametrs;
    void Start()
    {
        UpdateParametrs(0);
        InvokeRepeating("Regeneration", 1, .25f);
    }
    public void Regeneration()
    {
        if (CurrentHealth < maxHealth && WoodCount > 0)
        {
            float RgCount = maxHealth - CurrentHealth;
            if (WoodCount < RgCount)
            {
                RgCount = WoodCount;
            }
            if (Regen < RgCount)
            {
                RgCount = (int)lvlParametrs[level].Regenerate;
            }
            CurrentHealth += RgCount;
            WoodCount -= RgCount;
        }
    }
    private void FixedUpdate()
    {
        if (Exp > expToLevel)
        {
            level++;
            Exp = 0;
            UpdateParametrs(level);
        }
    }
    public void UpdateParametrs(int lvl)
    {
        maxHealth = lvlParametrs[lvl].Health;
        CurrentHealth = maxHealth;
        expToLevel = lvlParametrs[lvl].Exp;
        speed = lvlParametrs[lvl].speed;
        damage = lvlParametrs[lvl].damage;
        Regen = lvlParametrs[lvl].Regenerate;
        UpdateSliders();
    }
    public void UpdateSliders()
    {
        healthBar.maxValue = maxHealth;
        healthBar.value = CurrentHealth;
        ExpSlider.maxValue = expToLevel;
        ExpSlider.value = Exp;
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
            float Regen = lvlParametrs[0].Regenerate;
            for (int i = 1; i < lvlParametrs.Length; i++)
            {
                Regen *= 1.18f;
                HP *= 1.2f;
                exp *= 1.35f;
                speed *= 0.95f;
                damage *= 1.18f;
                Regen = (float)Math.Round((double)Regen, 2);
                HP = (float)Math.Round((double)HP, 0);
                exp = (float)Math.Round((double)exp, 0);
                speed = (float)Math.Round((double)speed, 2);
                damage = (float)Math.Round((double)damage, 0);
                lvlParametrs[i].Regenerate = Regen;
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
