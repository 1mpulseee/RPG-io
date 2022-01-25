using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FirstPersonController : MonoBehaviour
{
    private Quaternion needRot;
    private Quaternion oldRot;
    public float RotSpeed = 0.1f;
    private float time;
    public float coolDown = 2f;
    public Transform modules;
    public List<GameObject> objs;
    public Transform selector;
    public float camH;
    public bool IsAttack = false;

    private Rigidbody rb;
    private Animator anim;

    #region Camera Movement Variables

    public Camera playerCamera;

    public float fov = 60f;
    public bool invertCamera = false;
    public bool cameraCanMove = false;
    public float mouseSensitivity = 2f;
    public float maxLookAngle = 50f;

    // Crosshair
    public bool lockCursor = true;

    #endregion

    public bool playerCanMove = true;
    public float walkSpeed = 5f;
    public float maxVelocityChange = 10f;

    // Internal Variables
    private bool isWalking = false;


    #region Head Bob

    public bool enableHeadBob = true;
    public Transform joint;
    public float bobSpeed = 10f;
    public Vector3 bobAmount = new Vector3(.15f, .05f, 0f);

    // Internal Variables
    private Vector3 jointOriginalPos;
    private float timer = 0;

    #endregion

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        jointOriginalPos = joint.localPosition;
    }

    void Start()
    {
        if(lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        needRot = transform.rotation;
    }

    private void Update()
    {
        if(enableHeadBob)
        {
            HeadBob();
        }
    }

    void FixedUpdate()
    {
        time += Time.deltaTime;
        if (playerCanMove)
        {
            // Calculate how fast we should be moving
            Vector3 targetVelocity = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            Vector3 rotV = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            // Checks if player is walking and isGrounded
            // Will allow head bob
            if (targetVelocity.x != 0 || targetVelocity.z != 0)
            {
                oldRot = transform.rotation;
                transform.rotation = Quaternion.identity;
                isWalking = true;
                anim.SetBool("run", true);
                anim.SetBool("idle", false);
            }
            else
            {
                isWalking = false;
                anim.SetBool("run", false);
                anim.SetBool("idle", true);
            }

            targetVelocity = transform.TransformDirection(targetVelocity) * walkSpeed;
            // Apply a force that attempts to reach our target velocity
            Vector3 velocity = rb.velocity;
            Vector3 velocityChange = (targetVelocity - velocity);
            velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
            velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
            velocityChange.y = 0;
            rb.AddForce(velocityChange, ForceMode.VelocityChange);
            transform.rotation = oldRot;
            if (targetVelocity.x != 0 || targetVelocity.z != 0)
            {
                //Vector3 dir = Vector3.RotateTowards(transform.forward, rotV, walkSpeed, 0.0f);
                Vector3 dir = Vector3.RotateTowards(transform.forward, rotV, 10, 0.0F);
                needRot = Quaternion.LookRotation(dir);
            }
        }
        modules.transform.position = transform.position + new Vector3(0, camH, 0);
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
            selector.transform.position = s.transform.position;
            var dir = transform.position - s.transform.position;
            Vector3 newDir = Vector3.RotateTowards(transform.forward, (s.transform.position - transform.position), 10, 0.0F);
            needRot = Quaternion.LookRotation(newDir);
            if (time > coolDown)
            {
                time = 0;
                anim.SetTrigger("axe");
            }
        }
        else
        {
            selector.transform.position = new Vector3(0, 0, 0);
        }
        transform.rotation = Quaternion.Slerp(transform.rotation, needRot, RotSpeed);
    }
    public void GetObj(GameObject obj)
    {
        objs.Add(obj);
    }
    public void DelObj(GameObject obj)
    {
        objs.Remove(obj);
    }

    //эти 2 метода вызываются через анимации
    public void startAttack()
    {
        IsAttack = true;
    }
    public void stopAttack()
    {
        IsAttack = false;
    }

    private void HeadBob()
    {
        if(isWalking)
        {

            timer += Time.deltaTime * bobSpeed;
            // Applies HeadBob movement
            joint.localPosition = new Vector3(jointOriginalPos.x + Mathf.Sin(timer) * bobAmount.x, jointOriginalPos.y + Mathf.Sin(timer) * bobAmount.y, jointOriginalPos.z + Mathf.Sin(timer) * bobAmount.z);
        }
        else
        {
            // Resets when play stops moving
            timer = 0;
            joint.localPosition = new Vector3(Mathf.Lerp(joint.localPosition.x, jointOriginalPos.x, Time.deltaTime * bobSpeed), Mathf.Lerp(joint.localPosition.y, jointOriginalPos.y, Time.deltaTime * bobSpeed), Mathf.Lerp(joint.localPosition.z, jointOriginalPos.z, Time.deltaTime * bobSpeed));
        }
    }
}