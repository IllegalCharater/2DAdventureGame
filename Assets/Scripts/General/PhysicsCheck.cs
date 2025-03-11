using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsCheck : MonoBehaviour
{
    Collider2D coll;
    PlayerController player;
    private Rigidbody2D rb;
    [Header("检测参数")] 
    public bool manual;//手动检测
    public bool isPlayer;
    public Vector2 bottomOffset;
    public Vector2 leftOffset;
    public Vector2 rightOffset;
    public float checkRadius;
    public LayerMask groundLayer;
    [Header("状态")]
    public bool isGround;
    public bool touchLeftWall;
    public bool touchRightWall;
    public bool onWall;
    private void Awake()
    {
        coll = GetComponent<Collider2D>();
        if (!manual&&coll!=null)
        {
            rightOffset = new Vector2((coll.bounds.size.x + coll.offset.x) / 2,
                coll.bounds.size.y/2);
            leftOffset=new Vector2(-rightOffset.x,rightOffset.y);
        }

        if (isPlayer)
        {
            player = GetComponent<PlayerController>();
            rb = GetComponent<Rigidbody2D>();
        }
    }

    private void Update()
    {
        Check();
    }

    public void Check()
    {
        //检测地面
        isGround = onWall ? Physics2D.OverlapCircle((Vector2)transform.position+new Vector2(bottomOffset.x*transform.localScale.x,bottomOffset.y),checkRadius,groundLayer) : 
            Physics2D.OverlapCircle((Vector2)transform.position+new Vector2(bottomOffset.x*transform.localScale.x,0),checkRadius,groundLayer);
        //墙体判断
        touchLeftWall=Physics2D.OverlapCircle((Vector2)transform.position+leftOffset,checkRadius,groundLayer);
        touchRightWall=Physics2D.OverlapCircle((Vector2)transform.position+rightOffset,checkRadius,groundLayer);
        //墙壁上
        if(isPlayer)
            onWall=(touchLeftWall&&player.inputDirection.x<0f||touchRightWall&&player.inputDirection.x>0f)&&rb.velocity.y<0;
    }

    private void OnDrawGizmosSelected()
    {
        if(onWall)
            Gizmos.DrawWireSphere((Vector2)transform.position+new Vector2(bottomOffset.x*transform.localScale.x,bottomOffset.y),checkRadius);
        else 
            Gizmos.DrawWireSphere((Vector2)transform.position+new Vector2(bottomOffset.x*transform.localScale.x,0),checkRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position+leftOffset,checkRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position+rightOffset,checkRadius);
    }
}
