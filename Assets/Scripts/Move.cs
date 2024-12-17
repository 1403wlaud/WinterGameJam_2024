using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    public float speed;
    private Rigidbody2D rb;
    private Vector2 moveVelocity;
    private float minX = 25f;
    private float maxX = 673f;
    private float minY = 14f;
    private float maxY = 680f;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Vector2 moveInput = new Vector2(
            Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        moveVelocity = moveInput.normalized * speed;
        Vector3 currentPosition = transform.position;

        //// ���ѵ� �������� ��ġ�� ����
        //currentPosition.x = Mathf.Clamp(currentPosition.x, minX, maxX);
        //currentPosition.y = Mathf.Clamp(currentPosition.y, minY, maxY);

        //// ������Ʈ�� ��ġ�� ����
        //transform.position = currentPosition;
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + moveVelocity * Time.fixedDeltaTime);
    }
}
