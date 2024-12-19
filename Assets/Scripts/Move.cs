using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    public float Speed;
    Rigidbody2D rigid;
    Vector2 movement;
    bool isHorizonMove;
    Animator anim;
    private float minX = 47f;
    private float maxX = 461;
    private float minY = 31f;
    private float maxY = 472;
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        anim.SetFloat("Horizontal",movement.x);
        anim.SetFloat("Vertical", movement.y);
        anim.SetFloat("Speed",movement.sqrMagnitude);
        //if (anim.GetInteger("hAxisRaw") != h)
        //{
        //    anim.SetBool("isChange", true);
        //    anim.SetInteger("hAxisRaw", (int)h);
        //}
        //else if (anim.GetInteger("vAxisRaw") != v)
        //{
        //    anim.SetBool("isChange", true);
        //    anim.SetInteger("vAxisRaw", (int)v);
        //}
        //else
        //{
        //    anim.SetBool("isChange", false);
        //}
        //if (hDown||vUp)
        //{
        //    isHorizonMove = false;
        //}
        //else if (vDown || hUp)
        //{
        //    isHorizonMove = h != 0;
        //}
        Vector3 currentPosition = transform.position;

        // 제한된 영역으로 위치를 고정
        currentPosition.x = Mathf.Clamp(currentPosition.x, minX, maxX);
        currentPosition.y = Mathf.Clamp(currentPosition.y, minY, maxY);

        // 업데이트된 위치를 적용
        transform.position = currentPosition;
    }

    private void FixedUpdate()
    {
        rigid.MovePosition(rigid.position+movement*Speed*Time.deltaTime);
    }
}
