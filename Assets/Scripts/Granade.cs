using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Granade : MonoBehaviour
{
    float boomCounter = 3f;
    public ParticleSystem boom;
    bool exploded = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        boomCounter -= Time.deltaTime;
        if (boomCounter < 0 && !exploded)
        {
            exploded = true;
            ParticleSystem effect = Instantiate(boom, null, false);
            effect.transform.position = transform.position;
            RaycastHit[] hits;
            hits = Physics.SphereCastAll(transform.position, 10f, transform.forward);

            if(hits.Length > 0)
                foreach(RaycastHit hit in hits)
                {
                    if (hit.collider.gameObject.layer == 10)
                    {
                        hit.collider.gameObject.GetComponentInParent<HealthController>().Damage(300/( 1 + (int)Vector3.Distance(transform.position, hit.transform.position)));
                    }
                }
            Destroy(gameObject);
        }
    }
}
