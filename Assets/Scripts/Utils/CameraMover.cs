using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMover : MonoSingleton<CameraMover>
{
    public Rigidbody2D rigidbody2d;
    public Cinemachine.CinemachineVirtualCamera vc;

    public void MoveCamera(Vector2 position)
    {
        transform.position = position;
    }

    void FixedUpdate ()
    {
        rigidbody2d.MovePosition(rigidbody2d.position + new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")) * Time.fixedDeltaTime * vc.m_Lens.FieldOfView * 0.5f);
    }

    void Update()
    {
        vc.m_Lens.FieldOfView = Mathf.Clamp(vc.m_Lens.FieldOfView - Input.mouseScrollDelta.y * 10, 50, 150);
    }
}
