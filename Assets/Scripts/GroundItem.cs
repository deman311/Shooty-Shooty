using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundItem : MonoBehaviour
{
    public int TYPE; // 0 - pistol , 1 - granade
    public bool isTaken = false; // fix the senario where 2 players try to take it at the same time

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            PlayerControl pc = other.GetComponent<PlayerControl>();
            if (!pc.isUnlockedWeapons())
            {
                if (!pc.HasPistol() && TYPE == 0)
                    pc.enablePistolPickUp(gameObject);
                if (!pc.hasGranade() && TYPE == 1)
                    pc.enableGranadePickUp(gameObject);
            }
        }

        GameObject player = other.gameObject.transform.parent.gameObject; // the player won't return the right one and so it will be ignored.
        if (player.layer == 10 && !isTaken)
        {
            isTaken = true;
            FindItems pScript = player.GetComponent<FindItems>();

            if (TYPE == 0 && !pScript.HasPistol())
            {
                Instantiate(Resources.Load("PistolEquipped"), player.transform, false);
                pScript.foundPistol();
            }
            else if (TYPE == 1 && !pScript.HasGranade())
            {
                Instantiate(Resources.Load("GranadeEquipped"), player.transform, false);
                pScript.foundGranade();
            }
            Destroy(gameObject);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            PlayerControl pc = other.GetComponent<PlayerControl>();
            if (TYPE == 0)
                pc.disablePistolPickUp();
            if (TYPE == 1)
                pc.disableGranadePickUp();
        }
    }
}
