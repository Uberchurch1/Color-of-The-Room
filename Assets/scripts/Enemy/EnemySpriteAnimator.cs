using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpriteAnimator : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Transform target;
    private Enemy parent;
    private Animator spriteAnim;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        target = FindObjectOfType<PlayerMove>().transform;
        parent = GetComponentInParent<Enemy>();
        spriteAnim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 modTarget = target.position;
        modTarget.y = transform.position.y;
        transform.LookAt(modTarget);
        spriteAnim.SetInteger("EnemyType", parent.GetTypeI());
        spriteAnim.SetBool("ShowEnemy", parent.IsInRoom());
        if (parent.IsInRoom())
        {
            spriteRenderer.enabled = true;
        }
        else
        {
            spriteRenderer.enabled = false;
        }
    }
    
}
