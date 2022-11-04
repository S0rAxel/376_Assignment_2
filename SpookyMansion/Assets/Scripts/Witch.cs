using UnityEngine;
using System.Collections;
using DG.Tweening;

public class Witch : Enemy
{
    protected override void Move()
    {
        transform.DOMove(waypoints[waypointIndex].position, movementSpeedTime).OnComplete(() =>
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

    private IEnumerator ToRespawn()
    {
        yield return new WaitForSeconds(timeToRevive);

        animator.SetBool("isDead", false);

        yield return new WaitForSeconds(2.0f);

        base.Spawn();
    }

    protected override void Death()
    {
        base.Death();
        ScoreManager.Instance.IncreaseScore(Random.Range(3, 6));
        StartCoroutine(ToRespawn());
    }

}
