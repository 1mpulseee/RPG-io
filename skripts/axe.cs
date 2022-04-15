using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class axe : MonoBehaviour
{
    public AxeParamerts axeParamerts;
    [System.Serializable]
    public class AxeParamerts
    {
        public int damage;
        public int lvl;
    }
    public bool IsAttack = false;
    private void OnTriggerEnter(Collider other)
    {
        if (FirstPersonController.Instance == null)
            return;
        IsAttack = FirstPersonController.Instance.IsAttack;
        try
        {
            IsAttack = GetComponentInParent<Bot>().IsAttack;
        } catch{}
        if (IsAttack)
        {
            if (other.CompareTag("tree"))
            {
                if (other.GetComponentInParent<resource>().ready == true)
                {
                    other.GetComponentInParent<resource>().felling(axeParamerts.lvl);
                    if (other.GetComponentInParent<resource>().treeInfo.ToolLvl <= axeParamerts.lvl)
                    {
                        switch (other.GetComponentInParent<resource>().treeInfo.type.ToString())
                        {
                            case "tree":
                                FirstPersonController.Instance.playerStats.WoodCount += axeParamerts.damage;
                                break;
                            case "stone":
                                FirstPersonController.Instance.playerStats.Exp += axeParamerts.damage;
                                break;
                        }
                    }
                }
                other.GetComponentInParent<resource>().ready = false;
                other.GetComponentInParent<resource>().FixAttack();
            }
            if (other.CompareTag("Player") || other.CompareTag("Bot"))
            {
                if (GetComponentInParent<PlayerStats>().pv.IsMine)
                {
                    other.GetComponent<PlayerStats>().ChangeHealth();
                }
            }
        }
    }
}
