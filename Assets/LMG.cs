using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LMG : MonoBehaviour
{
    public AudioSource lmgSound;
    public ParticleSystem sparks, groundHit, objHit, playerHit;

    Animator anim;

    float timer = 0;
    float hitdelay = 0;
    float FIRE_RATE = 0.05f;
    float RECOIL_FACTOR = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (hitdelay < FIRE_RATE)
            hitdelay += Time.deltaTime;

        if (Input.GetKey(KeyCode.Space))
        {
            timer += Time.deltaTime;

            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("lmgFire"))
                anim.Play("lmgFire");
            if (!lmgSound.isPlaying || timer > 4f)
            {
                lmgSound.Stop();
                timer = 0;
                lmgSound.Play();
            }
            if (!sparks.isPlaying)
                sparks.Play();

            RaycastHit hit;
            Vector3 recoil = new Vector3(Random.Range(-RECOIL_FACTOR, RECOIL_FACTOR), Random.Range(-RECOIL_FACTOR, RECOIL_FACTOR), 0);

            if (Physics.Raycast(transform.position, transform.forward + recoil, out hit, 200f) && hitdelay >= FIRE_RATE)
            {
                hitdelay = 0f;

                Debug.DrawLine(transform.position, hit.point, Color.yellow);
                ParticleSystem effect = null;
                if (hit.collider.gameObject.tag == "Ground")
                    effect = Instantiate(groundHit, null, false);
                else if (hit.collider.gameObject.layer == 10)
                {
                    hit.collider.gameObject.GetComponentInParent<HealthController>().Damage(25, transform.position);
                    effect = Instantiate(playerHit, null, false);
                }
                else
                {
                    effect = Instantiate(objHit, null, false);
                    StartCoroutine(playDing(hit.transform.position));
                }
                effect.transform.position = hit.point;
            }
        }
        else
        {
            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("LMGidle"))
                anim.Play("LMGidle");
            if(lmgSound.isPlaying)
                lmgSound.Stop();
            if (sparks.isPlaying)
                sparks.Stop();
        }
    }

    IEnumerator playDing(Vector3 position)
    {
        GameObject ding = Instantiate(Resources.Load("MetalDing"), null, false) as GameObject;
        ding.transform.position = position;
        AudioSource dingSound = ding.GetComponent<AudioSource>();
        while (dingSound.isPlaying)
            yield return new WaitForFixedUpdate();
        Destroy(ding);
    }
}
