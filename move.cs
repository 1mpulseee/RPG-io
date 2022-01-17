using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move : MonoBehaviour
{
    private Rigidbody rb;
    public float speed;
    public List<GameObject> objs;
    public Transform sf;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        Vector3 targetVelocity = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        // Checks if player is walking and isGrounded
        // Will allow head bob
        if (targetVelocity.x != 0 || targetVelocity.z != 0)
        {
            Vector3 dir = Vector3.RotateTowards(transform.forward, targetVelocity, 10, 0.0f);
            transform.rotation = Quaternion.LookRotation(dir);
        }
        Vector3 inputs = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        rb.AddRelativeForce(inputs * speed);

        if (objs.Count != 0)
        {
            GameObject s = null;
            s = objs[0];
            if (objs.Count != 1)
            {
                for (int i = 1; i < objs.Count; i++)
                {
                    if (Vector3.Distance(transform.position, s.transform.position) > Vector3.Distance(transform.position, objs[i].transform.position))
                    {
                        s = objs[i];
                    }
                }
            }
            sf.transform.position = s.transform.position;
        }
        else
        {
            sf.transform.position = new Vector3(0, 0, 0);
        }
    }
    public void GetObj(GameObject obj)
    {
        objs.Add(obj);
    }
    public void DelObj(GameObject obj)
    {
        objs.Remove(obj);
    }
}
