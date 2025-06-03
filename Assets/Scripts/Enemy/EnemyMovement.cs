using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : RoleBase
{

    protected override void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        characterController = GetComponent<CharacterController>();
    }
    protected override void Update()
    {
        base.Update();
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(new Vector3(groundCheckPoint.position.x
        ,groundCheckPoint.position.y + pointOffset,groundCheckPoint.position.z),checkRadius);
    }
}
