using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControl : MonoBehaviour
{
    float horiz, verti, mHoriz, mVerti;

    public float TURN_RATE, MOVE_RATE, THROW_FORCE;
    public GameObject health, bloodhit;
    public TextMeshProUGUI missionLog;
    public GameObject deathCam;
    public TextMeshProUGUI actionText;
    public AudioSource footsteps, equipSound;
    public Collider groundCollider;

    Collider myCollider;
    HealthController hc;
    float xRotation = 0, yRotation = 0, shotDelay = 0;
    bool isGrounded = false, hasPistol = false, canPickupPistol = false, canPickupGranade = false, unlockedWeapons = false;
    int granadeCount = 0, HP = 100;

    CharacterController characterController;
    Rigidbody rbody;

    Vector3 PISTOL_HOLD_LocalPos = new Vector3(-0.311f, -0.411f, 0.212f);
    Vector3 GRANADE_HOLD_LocalPos = new Vector3(0.339f, 0.964f, 0.802f);

    // Start is called before the first frame update
    void Start()
    {
        myCollider = GetComponent<Collider>();
        rbody = GetComponent<Rigidbody>();
        hc = GetComponent<HealthController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        actionText.text = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (isGrounded)
            rbody.velocity = Vector3.zero;
        else
            rbody.velocity = new Vector3(0, -1.5f, 0);

        horiz = Input.GetAxis("Horizontal");
        verti = Input.GetAxis("Vertical");
        mHoriz = Input.GetAxis("Mouse X");
        mVerti = Input.GetAxis("Mouse Y");

        xRotation -= mVerti;
        xRotation = Mathf.Clamp(xRotation, -70f, 70f);
        yRotation += mHoriz;
        Camera.main.transform.localRotation = Quaternion.Euler(new Vector3(xRotation, yRotation, 0) * TURN_RATE * Time.fixedDeltaTime);

        // cheat weapons
        /*        if (Input.GetKeyDown(KeyCode.P) && !hasPistol)
                {
                    PickUpPistol();
                    updateGranadeCount(1);
                }*/

        if (Input.GetKeyDown(KeyCode.L))
            Instantiate(Resources.Load("LMG"), Camera.main.transform, false);

        if (Input.GetKey(KeyCode.LeftShift))
            MOVE_RATE = 180;
        else
            MOVE_RATE = 90;

        if (Input.GetMouseButtonDown(0))
        {
            if (canPickupPistol)
            {
                PickUpPistol();
                disablePistolPickUp();
            }
            if (canPickupGranade)
            {
                updateGranadeCount(1);
                disableGranadePickUp();
            }
        }

        if (unlockedWeapons)
        {
            if (shotDelay <= 1f)
                shotDelay += Time.deltaTime;

            if (hasPistol && Input.GetKeyDown(KeyCode.Space) && shotDelay > 1f)
            {
                shotDelay = 0;
                GetComponentInChildren<Animator>().Play("Fire");
            }

            // Throw granade 
            if (hasGranade() && Input.GetKeyDown(KeyCode.Q))
            {
                GameObject granade = (GameObject)Instantiate(Resources.Load("ThrownGranade"), null, true);
                granade.transform.position = transform.position + Camera.main.transform.forward;
                granade.GetComponent<Rigidbody>().AddForce((Camera.main.transform.forward + Vector3.up) * THROW_FORCE, ForceMode.Impulse);
                updateGranadeCount(-1);
            }
        }

        updateHP();
    }

    private void FixedUpdate()
    {
        moveControls();
    }

    public void moveControls()
    {
        Vector3 movement = new Vector3(horiz, 0, verti);
        if (movement.magnitude > 1f)
            movement = movement.normalized;

        if (movement != Vector3.zero && !footsteps.isPlaying)
            footsteps.Play();
        else if (movement == Vector3.zero)
            footsteps.Stop();

        movement = Camera.main.transform.TransformVector(movement);

        movement.y = rbody.velocity.y;
        rbody.velocity = movement * Time.fixedDeltaTime * MOVE_RATE;
    }

    public void PickUpPistol()
    {
        GameObject pistol = (GameObject)Instantiate(Resources.Load("Pistol"), GameObject.Find("Hands").transform, false);
        pistol.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        pistol.transform.localPosition = PISTOL_HOLD_LocalPos;
        hasPistol = true;
        Destroy(tempPistol);

        equipSound.Play();
        checkLoadout();
    }

    public void updateGranadeCount(int delta)
    {
        granadeCount += delta;
        Destroy(tempGranade);

        GameObject.Find("Granade UI").GetComponentInChildren<TextMeshProUGUI>().text = "" + granadeCount;

        if(delta > 0)
            equipSound.Play();
        checkLoadout();
    }

    public void checkLoadout()
    {
        if (hasPistol && hasGranade())
        {
            unlockedWeapons = true;
            missionLog.text = missionLog.text + "\nWeapons Unlocked!";
            missionLog.color = Color.green;
            GameObject.Find("Cursor").GetComponent<Image>().enabled = true;
        }
    }

    public void updateHP()
    {
        HP = hc.getHP();
        health.GetComponentInChildren<Animator>().SetFloat("speedMultiplier", (150-HP)/50);
        health.GetComponentInParent<TextMeshProUGUI>().text = "" + HP;
    }

    public bool hasGranade()
    {
        if (granadeCount > 0)
            return true;
        return false;
    }

    public bool HasPistol()
    {
        return hasPistol;
    }

    GameObject tempPistol, tempGranade;
    public void enablePistolPickUp(GameObject thePistol)
    {
        actionText.text = "[ Press LMB to pickup Pistol ]";
        canPickupPistol = true;
        tempPistol = thePistol;
    }

    public void disablePistolPickUp()
    {
        tempPistol = null;
        actionText.text = null;
        canPickupPistol = false;
    }

    public void enableGranadePickUp(GameObject theGranade)
    {
        actionText.text = "[ Press LMB to pickup Granade ]";
        canPickupGranade = true;
        tempGranade = theGranade;
    }

    public void disableGranadePickUp()
    {
        tempGranade = null;
        actionText.text = null;
        canPickupGranade = false;
    }

    public void Die()
    {
        deathCam.SetActive(true);
    }

    bool isShot;
    public void startBlood()
    {
        isShot = false;
        StartCoroutine(showBlood());
    }

    public bool isUnlockedWeapons()
    {
        return unlockedWeapons;
    }

    IEnumerator showBlood()
    {
        float timer = 0.1f;
        
        Image blood = bloodhit.GetComponent<Image>();
        blood.enabled = true;
        
        // reset alpha to 1
        var reset = blood.color;
        reset.a = 1f;
        blood.color = reset;

        isShot = true;
        while (blood.color.a > 0f && isShot)
        {
            timer -= Time.deltaTime;

            if (timer < 0)
            {
                timer = 0.1f;
                Color temp = blood.color;
                temp.a -= 0.1f;
                blood.color = temp;
            }

            yield return new WaitForFixedUpdate();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider == groundCollider)
            isGrounded = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider == groundCollider)
            isGrounded = false;
    }
}
