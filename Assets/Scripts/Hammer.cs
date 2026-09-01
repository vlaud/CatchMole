using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hammer : MonoBehaviour
{
    Animator myAnim;
    public Transform hitPoint;
    public bool IsSwing
    {
        get => myAnim.GetBool("IsSwing");
    }
    // Start is called before the first frame update
    void Start()
    {
        myAnim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Hit()
    {
        myAnim.SetTrigger("OnHit");
    }

    public void HitCheck()
    {
        Collider[] list = Physics.OverlapSphere(hitPoint.position, 0.3f, 1 << LayerMask.NameToLayer("Mole"));
        foreach(Collider col in list)
        {
            col.GetComponent<Mole>()?.OnHit();
        }
    }
}
