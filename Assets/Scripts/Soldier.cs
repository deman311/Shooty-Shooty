using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Soldier : MonoBehaviour
{
    public GameObject commander, playArea;
    public float FOLLOW_DISTANCE, ROAM_DISTANCE, radius, fov;

    public LayerMask playerMask, obstacleMask;

    NavMeshAgent agent;
    bool seePlayer = false;

    int granadeCount = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator seekAndDestroy()
    {
        while (true)
        {
            if (!seePlayer && Vector3.Distance(agent.destination, transform.position)<0.5f)
            {
                Vector3 move;
                do
                {
                    move = new Vector3(Random.Range(-ROAM_DISTANCE, ROAM_DISTANCE), 0, Random.Range(-ROAM_DISTANCE, ROAM_DISTANCE));
                } while (!playArea.GetComponent<BoxCollider>().bounds.Contains(transform.position + move));
                agent.SetDestination(transform.position + move);
            }
            Debug.DrawLine(transform.position, agent.destination, Color.green);
            yield return new WaitForFixedUpdate();
        }
    }

    IEnumerator lookAround()
    {
        float angleDelta = 50;
        bool cwr = true;
        while (true)
        {
            if (!seePlayer)
            {
                if (cwr)
                    agent.transform.Rotate(transform.up, Time.fixedDeltaTime * 30);
                else
                    agent.transform.Rotate(transform.up, -Time.fixedDeltaTime * 30);
                angleDelta -= Time.fixedDeltaTime * 30;
                if (angleDelta <= 0)
                {
                    angleDelta = 50;
                    cwr = !cwr;
                }
            }
            yield return new WaitForFixedUpdate();
        }
    }

    IEnumerator FollowCommander()
    {
        while (commander != null)
        {
            if (Vector3.Distance(transform.position, commander.transform.position) < FOLLOW_DISTANCE)
                agent.ResetPath();
            else
                agent.SetDestination(commander.transform.position);
            Debug.DrawLine(transform.position, commander.transform.position, Color.white);
            yield return new WaitForFixedUpdate();
        }

        // null means he is not destroyed / dead
        if (commander == null && gameObject != null)
        {
            StartCoroutine(seekAndDestroy());
        }
    }

    IEnumerator ShootPlayer()
    {
        float shootDelay = 0f;
        float granadeDelay = 0f;
        while (true)
        {
            if (shootDelay <= 1f)
                shootDelay += Time.deltaTime;
            if (granadeDelay <= 2)
                granadeDelay += Time.deltaTime;

            RaycastHit[] pointOfInterest = Physics.SphereCastAll(transform.position, radius, transform.up, radius, playerMask);
            bool seenPlayer = false;
            foreach (RaycastHit hit in pointOfInterest)
                if ((name.Contains("Red") && hit.collider.gameObject.name.Contains("Blue")) || (name.Contains("Blue") && hit.collider.gameObject.name.Contains("Red")))
                {
                    Debug.DrawLine(transform.position, hit.transform.position, Color.blue);
                    if (Mathf.Abs(Vector3.SignedAngle(transform.forward, hit.transform.position - transform.position, transform.up)) < 60 && !Physics.Raycast(transform.position, hit.transform.position - transform.position, radius, obstacleMask))
                    {
                        seePlayer = true;
                        seenPlayer = true;
                        Debug.DrawLine(transform.position, hit.transform.position, Color.red);
                        transform.LookAt(hit.transform.position, transform.up);
                        if (shootDelay > 1f)
                        {
                            shootDelay = 0f;
                            GetComponentInChildren<Animator>().Play("Fire");
                        }

                        // Randomize granade throw. --- 50% chance every 2 seconds to throw a granade at the player.
                        if (granadeCount > 0 && granadeDelay < 2)
                        {
                            granadeDelay = 0;
                            int rand = Random.Range(0, 2);
                            if (rand == 1)
                            {
                                // Throw granade 
                                GameObject granade = (GameObject)Instantiate(Resources.Load("ThrownGranade"), null, true);
                                granade.transform.position = transform.position + transform.forward;
                                // Calculate the force according to the distance from the player.
                                granade.GetComponent<Rigidbody>().AddForce((transform.forward + Vector3.up) * 8 * (Vector3.Distance(transform.position, hit.transform.position) / radius), ForceMode.Impulse);
                                granadeCount--;
                            }
                        }
                    }
                }

            if (!seenPlayer)
                seePlayer = false;
                yield return new WaitForFixedUpdate();
        }
    }

    public void ReadyUp()
    {
        granadeCount = 1;

        if(tag == "Soldier")
            StartCoroutine(FollowCommander());
        else
            StartCoroutine(seekAndDestroy());

        StartCoroutine(ShootPlayer());
        StartCoroutine(lookAround());
    }
}
