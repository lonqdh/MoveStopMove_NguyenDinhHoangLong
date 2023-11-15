using UnityEngine;

public class PatrolState : IState<Bot>
{
    private float patrolDuration = 3f; // Time to patrol in seconds
    private float patrolTimer;

    public void OnEnter(Bot bot)
    {
        // Initialize patrol behavior
        bot.walkPointSet = false;
        patrolTimer = 0f;

        // Start patrolling
        Patrol(bot);
    }

    public void OnExecute(Bot bot)
    {
        // Continue patrolling
        Patrol(bot);

        patrolTimer += Time.deltaTime;
        if (patrolTimer >= patrolDuration)
        {
            bot.ChangeState(new IdleState());
        }
    }

    public void OnExit(Bot bot)
    {
        // Cleanup or additional logic on exit
    }

    private void Patrol(Bot bot)
    {
        if (!bot.walkPointSet)
        {
            SearchWalkPoint(bot);
        }
        else
        {
            bot.agent.SetDestination(bot.walkPoint);

            if (Vector3.Distance(bot.transform.position, bot.walkPoint) < 1f)
            {
                bot.walkPointSet = false;
            }

            bot.ChangeAnim("Run");
        }
    }

    private void SearchWalkPoint(Bot bot)
    {
        // Generate random walk point within range
        float randomX = Random.Range(-bot.walkPointRange, bot.walkPointRange);
        float randomZ = Random.Range(-bot.walkPointRange, bot.walkPointRange);

        bot.walkPoint = new Vector3(bot.transform.position.x + randomX, bot.transform.position.y, bot.transform.position.z + randomZ);

        bot.agent.SetDestination(bot.walkPoint);

        bot.walkPointSet = true;
    }
}
