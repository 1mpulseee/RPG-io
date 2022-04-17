using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;

public class Bot : MonoBehaviour
{
    private Vector3 pos = Vector3.zero;
    public float attackDis;

    private NavMeshAgent agent;
    public List<GameObject> objs;
    public List<GameObject> enemys;

    public int AxeLvl = 1;
    public int PickaxeLvl = 1;
    public GameObject axes;
    public GameObject pickaxes;

    AnimSyns AnimSyns;
    public int woodCount;
    public int stoneCount;
    public PlayerStats playerStats;


    private Quaternion needRot;
    private Quaternion oldRot;
    public float RotSpeed = 0.1f;
    private float time;
    public float coolDown = 2f;
    public bool IsAttack = false;

    private Animator anim;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        AnimSyns = GetComponent<AnimSyns>();
        InvokeRepeating("CheckPos", 3, 3);
    }
    public void CheckPos()
    {
        Vector3 newPos = new Vector3((int)transform.position.x, (int)transform.position.y, (int)transform.position.z);
        if (pos == newPos)
        {
            playerStats.ChangeHealthCount(10);
        }
        pos = newPos;
    }
    private void FixedUpdate()
    {
        enemys = Manager.instance.players;
        time += Time.deltaTime;
        for (int i = 0; i < enemys.Count; i++)
        {
            if (enemys[i] == null)
            {
                enemys.RemoveAt(i);
            }
        }
        for (int i = 0; i < objs.Count; i++)
        {
            if (objs[i] == null)
            {
                objs.RemoveAt(i);
            }
        }
        if (objs.Count != 0 || enemys.Count != 0)
        {
            anim.SetBool("run", true);
            anim.SetBool("idle", false);
            for (int i = 0; i < objs.Count; i++)
            {
                if (objs[i] == null)
                {
                    objs.RemoveAt(i);
                    return;
                }
            }
            for (int i = 0; i < enemys.Count; i++)
            {
                if (enemys[i] == null)
                {
                    enemys.RemoveAt(i);
                    return;
                }
            }
            GameObject s = null;
            
            if (enemys.Count != 0)
            {
                s = enemys[0];
            }
            if (enemys.Count != 1)
            {
                for (int i = 1; i < enemys.Count; i++)
                {
                    if (Vector3.Distance(transform.position, s.transform.position) > Vector3.Distance(transform.position, enemys[i].transform.position))
                    {
                        if ((Vector3.Distance(transform.position, s.transform.position) > .1f))
                        {
                            if (enemys[i] != this.gameObject)
                            {
                                s = enemys[i];
                            }
                           
                        }
                    }
                }
            }
            if (objs.Count != 0)
            {
                s = objs[0];
            }
            if (objs.Count != 1)
            {
                for (int i = 1; i < objs.Count; i++)
                {
                    if (Vector3.Distance(transform.position, s.transform.position) > Vector3.Distance(transform.position, objs[i].transform.position))
                    {
                        if (Vector3.Distance(transform.position, s.transform.position) > .1f)
                        {
                            s = objs[i];
                        }
                    }
                }
            }
            //Debug.Log(Vector3.Distance(transform.position, s.transform.position));
            if (Vector3.Distance(transform.position, s.transform.position) > attackDis)
            {
                agent.enabled = true;
                try
                {
                    agent.destination = (s.transform.position);
                }
                catch{ }
                
               
            }
            else
            {
                agent.enabled = false;

                if (time > coolDown)
                {
                    time = 0;
                    if (s.GetComponent<resource>().treeInfo.type == resource.TreeInfo.DropDown.tree || s.GetComponent<resource>().treeInfo.type == resource.TreeInfo.DropDown.enemy)
                    {
                        axes.SetActive(true);
                        pickaxes.SetActive(false);
                    }
                    if (s.GetComponent<resource>().treeInfo.type == resource.TreeInfo.DropDown.stone)
                    {
                        axes.SetActive(false);
                        pickaxes.SetActive(true);
                    }
                    AnimSyns.SetAnimTrigger("axe");
                }
            }
        }
        else
        {
            anim.SetBool("run", false);
            anim.SetBool("idle", true);
        }
        transform.rotation = Quaternion.Slerp(transform.rotation, needRot, RotSpeed);
    }
}
