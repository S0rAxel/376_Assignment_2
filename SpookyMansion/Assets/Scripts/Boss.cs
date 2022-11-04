using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Enemy
{
    public virtual void TakeDamage(int damage)
    {
        if (!isDead)
        {
            health--;
            ScoreManager.Instance.IncreaseScore(1);
            if (health <= 0)
            {
                Death();
            }
        }
    }
}
