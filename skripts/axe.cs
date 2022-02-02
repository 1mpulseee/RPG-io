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
    private bool IsAttack = false;
    public FirstPersonController main;
    private void OnTriggerEnter(Collider other)
    {
        IsAttack = main.IsAttack;
        if (IsAttack)
        {
            if (other.CompareTag("tree"))
            {
                if (other.GetComponentInParent<resource>().ready == true)
                {
                    other.GetComponentInParent<resource>().felling(axeParamerts.lvl);
                    if (other.GetComponentInParent<resource>().treeInfo.ToolLvl <= axeParamerts.lvl)
                    {
                        switch (other.GetComponentInParent<resource>().treeInfo.type)
                        {
                            case "tree":
                                main.woodCount += axeParamerts.damage;
                                break;
                        }
                    }
                }
                other.GetComponentInParent<resource>().ready = false;
                other.GetComponentInParent<resource>().FixAttack();
            }
        }
    }
}
