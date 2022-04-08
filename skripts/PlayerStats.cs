using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;

public class PlayerStats : MonoBehaviour
{
    public Transform HpBar;
    public TextMesh PlayerName;
    public TextMesh LvlTextPro;
    public Text LvlText;
    
    
    public Slider healthBar;
    public Slider ExpSlider;

    private float maxHealth = 0f;
    [HideInInspector] public PhotonView pv;
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
            if (pv.IsMine)
            {
                UpdateSliders();
            }
        }
        get { return exp; }
    }
    private float exp;
    public float expToLevel = 100f;

    public int level;
    
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
        pv = GetComponent<PhotonView>();        
        UpdateParametrs(0);
        InvokeRepeating("Regeneration", 1, .25f);
        InvokeRepeating("ChUpd", 1, .5f);
        pv.RPC("SetName", RpcTarget.All);

        LvlText.text = level.ToString();
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
            LvlText.text = level.ToString();            
        }
    }
    [PunRPC]
    public void SetName()
    {
        PlayerName.text = PhotonNetwork.NickName;
    }    
    public void ChUpd()
    {
        if (pv.IsMine)
        {
            pv.RPC("Upd", RpcTarget.All, maxHealth, currentHealth, level);
        }
    }
    [PunRPC]
    public void Upd(float max, float current, int lvl)
    {
        HpBar.transform.localScale = new Vector3(current / max * 100, 10, 10);
        LvlTextPro.text = lvl.ToString();          
    }
    [PunRPC]
    public void KillMe()
    {

    }
    [PunRPC]
    public void ChangeHealthRPC(float count)
    {
        currentHealth -= count;
    }
    public void ChangeHealth()
    {
        pv.RPC("ChangeHealthRPC", RpcTarget.All, lvlParametrs[level].damage);
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
