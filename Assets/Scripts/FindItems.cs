using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class FindItems : MonoBehaviour
{
    public float radius, maxAngle, roamDistance;
    public GameObject playArea;
    public TextMeshProUGUI messageLog;
    public LayerMask obstructionMask, itemMask;
    public AudioSource equipSound;

    NavMeshAgent agent;

    bool seeItem = false, hasPistol = false, hasGranade = false, isRoaming = false, isCircling = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        // Look for items
        if (!hasPistol || !hasGranade)
        {
            RaycastHit[] fieldOfInterest = Physics.SphereCastAll(transform.position, radius, transform.up, radius, itemMask);
            foreach (RaycastHit hit in fieldOfInterest)
            {
                Debug.DrawLine(transform.position, hit.transform.position, Color.blue);
                if (!Physics.Raycast(transform.position, hit.transform.position, radius, obstructionMask))
                    if ((!hasPistol && hit.collider.gameObject.tag == "Pistol") || (!hasGranade && hit.collider.gameObject.tag == "Granade"))
                    {
                        float angle = Vector3.SignedAngle(transform.forward, hit.transform.position - transform.position, transform.up);
                        if (Mathf.Abs(angle) <= maxAngle)
                        {
                            Debug.DrawLine(transform.position, hit.transform.position, Color.red);
                            if (!seeItem)
                            {
                                seeItem = true;
                                if (hit.collider.gameObject.tag == "Pistol")
                                    moveToItem(hit.transform.position);
                                else if (hit.collider.gameObject.tag == "Granade")
                                    moveToItem(hit.transform.position);
                            }
                        }
                    }
            }
            if (!seeItem && !isRoaming)
                RoamAround();
        }
        else if (!isRoaming)
            RoamAround();
    }

    public void moveToItem(Vector3 itemPos)
    {
        agent.SetDestination(itemPos);
        StartCoroutine(WaitForReachDest(itemPos, false));
    }

    public void RoamAround()
    {
        if (!isCircling)
        {
            isRoaming = true;
            Vector3 moveTarget;
            do
            {
                moveTarget = new Vector3(Random.Range(-roamDistance, roamDistance), 0, Random.Range(-roamDistance, roamDistance));
            } while (!playArea.GetComponent<BoxCollider>().bounds.Contains(transform.position + moveTarget));
            agent.SetDestination(transform.position + moveTarget);
            StartCoroutine(WaitForReachDest(transform.position + moveTarget, true));
        }
    }

    public void circleEnemy(Vector3 enemyPos)
    {

    }

    IEnumerator WaitForReachDest(Vector3 dest, bool roaming)
    {
        float timer = 5f;
        while (Vector3.Distance(transform.position, dest) > 0.3f)
        {
            if (roaming && timer < 0)
                break;

            Debug.DrawLine(transform.position, dest, Color.green);
            timer -= Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }

        if (roaming)
            isRoaming = false;
        else
            seeItem = false;
    }

    public bool HasGranade()
    {
        return hasGranade;
    }

    public bool HasPistol()
    {
        return hasPistol;
    }

    public void foundPistol()
    {
        hasPistol = true;
        equipSound.Play();

        if (hasGranade)
        {
            GetComponent<Soldier>().ReadyUp();
            messageLog.text = messageLog.text + "\n" + name + " is ready to shoot!";
            enabled = false; // stop roaming to follow commander
        }
    }

    public void foundGranade()
    {
        hasGranade = true;
        equipSound.Play();

        if (hasPistol)
        {
            GetComponent<Soldier>().ReadyUp();
            messageLog.text = messageLog.text + "\n" + name + " is ready to shoot!";
            enabled = false; // stop roaming to follow commander
        }
    }
}
