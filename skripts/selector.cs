using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class selector : MonoBehaviour
{
    public FirstPersonController main;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "tree")
        {
            main.GetObj(other.gameObject);
        }
        if (other.tag == "Player")
        {
            main.GetEnemy(other.gameObject);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "tree")
        {
            main.DelObj(other.gameObject);
        }
        if (other.tag == "Player")
        {
            main.DelEnemy(other.gameObject);
        }
    }
}
