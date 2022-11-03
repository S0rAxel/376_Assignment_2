using UnityEngine;
using System.Collections;
using DG.Tweening;

public class Zombie : Enemy
{
    protected override void Start()
    {
        base.Start();
        StartCoroutine(ToDisable());
    }

    private IEnumerator ToDisable()
    {
        yield return new WaitForSeconds(timeToDisable);

        animator.SetBool("isDisable", false);

        yield return new WaitForSeconds(0.5f);

        isDisabled = true;
        boxCollider.enabled = false;

        StartCoroutine(ToRespawn());
    }

    private IEnumerator ToRespawn()
    {
        yield return new WaitForSeconds(timeToRevive);

        animator.SetBool("isDisable", false);
        animator.SetBool("isDead", false);

        yield return new WaitForSeconds(2.0f);
        
        boxCollider.enabled = true;
        isDead = false;
        isDisabled = false;

        StartCoroutine(ToDisable());
    }

    protected override void Move()
    {
        transform.DOMoveX(waypoints[waypointIndex].position.x, movementSpeedTime).OnComplete(() =>
        {
            waypointIndex = Random.Range(0, waypoints.Length); //waypointIndex + 1) % waypoints.Length;
            if (waypoints[waypointIndex].position.x > transform.position.x)
            {
                sprite.flipX = true;
            }
            else
            {
                sprite.flipX = false;
            }
            Move();
        });
    }

    protected override void Death()
    {
        base.Death();
        ScoreManager.Instance.IncreaseScore(Random.Range(1, 4));
    }

}
