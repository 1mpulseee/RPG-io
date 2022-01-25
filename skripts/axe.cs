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
                Debug.Log("удар по дереву");
            }
        }
    }
}
