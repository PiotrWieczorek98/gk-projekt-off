using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    public int maxHP = 100;
    public int maxAP = 100;
    public Text HpCounter;
    public Text ApCounter;
    public AudioClip hitSound;
    public FlashScreen flashScreen;
    public int oofCounter = 5;

    int numberOfProjectileWeapons;
    int numberOfRaycastWeapons;
    int hitsRemaining;
    float currentHP;
    float currentAP;
    AudioSource source;
    RaycastWeapon[] raycastWeapons;
    ProjectileWeapon[] projectileWeapons;
    void Start()
    {
        currentHP = maxHP;
        currentAP = 0;
        hitsRemaining = oofCounter;
        HpCounter.text = currentHP.ToString();
        ApCounter.text = currentAP.ToString();
        source = GetComponent<AudioSource>();

        // Activate all weapons to count them
        foreach (Transform weapon in GameObject.Find("Weapons").transform)
        {
            weapon.gameObject.SetActive(true);
        }

        numberOfProjectileWeapons = GameObject.FindGameObjectsWithTag("ProjectileWeapon").Length;
        numberOfRaycastWeapons = GameObject.FindGameObjectsWithTag("RaycastWeapon").Length;

        // wyłączenie pozostałych broni
        int k = 0;
        foreach (Transform weapon in GameObject.Find("Weapons").transform)
        {
            if(k != 0) 
                weapon.gameObject.SetActive(false);
            k++;
        }

        raycastWeapons = new RaycastWeapon[numberOfRaycastWeapons];
        projectileWeapons = new ProjectileWeapon[numberOfProjectileWeapons];
        for (int i = 0; i < numberOfRaycastWeapons; i++)
        {
            raycastWeapons[i] = GameObject.Find("Weapons").transform.GetChild(i).GetComponent<RaycastWeapon>();
        }
        for (int i = 0; i < numberOfProjectileWeapons; i++)
        {
            projectileWeapons[i] = GameObject.Find("Weapons").transform.GetChild(numberOfRaycastWeapons + i).GetComponent<ProjectileWeapon>();
        }
    }

    void Update()
    {
        currentAP = Mathf.Clamp(currentAP, 0, maxAP);
        currentHP = Mathf.Clamp(currentHP, 0, maxHP);
        HpCounter.text = currentHP.ToString();
        ApCounter.text = currentAP.ToString();
    }

    void gotHit(float damage)
    {
        //Flash screen with red color
        flashScreen.flash("red");

        if (hitsRemaining > 0)
            hitsRemaining--;
        else if (hitsRemaining <= 0)
        {
            source.PlayOneShot(hitSound);
            hitsRemaining = oofCounter;
        }

        if(currentAP > 0)
        {
            if (damage >= currentAP)
            {
                damage -= currentAP;
                currentHP -= damage;
                currentAP = 0;
            }
            else
                currentAP -= damage;
        }
        else
            currentHP -= damage;
    }

    public void addHP(float value)
    {
        currentHP += value;
    }
    public void addAP(float value)
    {
        currentAP += value;
    }
    public bool isHPFull()
    {
        if (currentHP == maxHP)
            return true;
        return false;
    }
    public bool isAPFull()
    {
        if (currentAP == maxAP)
            return true;
        return false;
    }

    public RaycastWeapon getRaycastWeapon(string name)
    {
        if (name == "pistol")
        {
            return raycastWeapons[0];
        }
        else if (name == "shotgun")
        {
            return raycastWeapons[1];
        }

        return null;
    }

    public ProjectileWeapon getProjectileWeapon(string name)
    {
        if (name == "rocket launcher")
        {
            return projectileWeapons[0];
        }
        return null;
    }
}
