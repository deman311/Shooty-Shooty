using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public ParticleSystem shootSparks, groundHit, objHit, playerHit;
    public AudioSource gunShot;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Shoot()
    {
        shootSparks.Play();
        gunShot.Play();
        RaycastHit hit;
        // add a random recoil
        float RECOIL_FACTOR = 0.03f;
        Vector3 recoil = new Vector3(Random.Range(-RECOIL_FACTOR, RECOIL_FACTOR), Random.Range(-RECOIL_FACTOR, RECOIL_FACTOR), 0);
        Physics.Raycast(transform.position, transform.forward + recoil, out hit, 200f);
        if (hit.collider != null)
        {
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
