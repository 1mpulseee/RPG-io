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
            try{ main.GetObj(other.gameObject); } catch { }
            try{ GetComponent<Bot>().objs.Add(other.gameObject); } catch { }
        }
        if (other.tag == "Player")
        {
            try { main.GetEnemy(other.gameObject); } catch { }
        }
        if (other.tag == "Bot")
        {
            try { main.GetEnemy(other.gameObject); } catch { }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "tree")
        {
            try { main.DelObj(other.gameObject); } catch { }
            try { GetComponent<Bot>().objs.Remove(other.gameObject); } catch { }
        }
        if (other.tag == "Player")
        {
            try { main.DelEnemy(other.gameObject); } catch { }
        }
        if (other.tag == "Bot")
        {
            try { main.DelEnemy(other.gameObject); } catch { }
        }
    }
}
