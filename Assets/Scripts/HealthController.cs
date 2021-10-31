using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class HealthController : MonoBehaviour
{
    public int HP = 100;
    public GameObject deadPrefab;
    public TextMeshProUGUI messageLog;
    public AudioSource footsteps;

    NavMeshAgent agent;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponentInParent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (tag != "Player")
        {
            if (agent.velocity != Vector3.zero && !footsteps.isPlaying)
                footsteps.Play();
            else if (agent.velocity == Vector3.zero)
                footsteps.Stop();
        }
    }

    public void Damage(int amount, Vector3 shooterPosition) {
        HP -= amount;

        if (tag == "Player")
            GetComponent<PlayerControl>().startBlood();
        else
            StartCoroutine(lookAt(shooterPosition, 1f));

        if(HP <= 0)
        {
            GameObject dead = Instantiate(deadPrefab, null, false);
            dead.transform.position = transform.position;
            messageLog.text = messageLog.text + "\n" + name + " is dead.";

            if (tag == "Player")
                GetComponent<PlayerControl>().Die();

            Destroy(gameObject);
        }
    }

    IEnumerator lookAt(Vector3 enemyPos, float duration)
    {
        float counter = 0;
        while(counter < duration)
        {
            transform.LookAt(enemyPos, transform.up);
            counter += Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }
    }

    public void Damage(int amount)
    {
        HP -= amount;

        if (tag == "Player")
            GetComponent<PlayerControl>().startBlood();

        if (HP <= 0)
        {
            GameObject dead = Instantiate(deadPrefab, null, false);
            dead.transform.position = transform.position;
            messageLog.text = messageLog.text + "\n" + name + " is dead.";

            if (tag == "Player")
                GetComponent<PlayerControl>().Die();

            Destroy(gameObject);
        }
    }

    public int getHP()
    {
        return HP;
    }
}
