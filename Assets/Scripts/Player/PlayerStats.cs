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

    int numberOfWeapons;
    float currentHP;
    float currentAP;
    AudioSource source;
    RangeWeapon[] weapons;
    void Start()
    {
        currentHP = maxHP;
        currentAP = 0;
        HpCounter.text = currentHP.ToString();
        ApCounter.text = currentAP.ToString();
        source = GetComponent<AudioSource>();

        // Activate all weapons to count them
        foreach (Transform weapon in GameObject.Find("Weapons").transform)
        {
            weapon.gameObject.SetActive(true);
        }

        numberOfWeapons = GameObject.FindGameObjectsWithTag("ProjectileWeapon").Length;


        weapons = new RangeWeapon[numberOfWeapons];
        for (int i = 0; i < numberOfWeapons; i++)
        {
            weapons[i] = GameObject.Find("Weapons").transform.GetChild(i).GetComponent<RangeWeapon>();
        }

        // disable the rest
        int k = 0;
        foreach (Transform weapon in GameObject.Find("Weapons").transform)
        {
            if (k != 0)
                weapon.gameObject.SetActive(false);
            k++;
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

    public RangeWeapon getWeapon(string name)
    {
        if (name == "Pistol")
        {
            return weapons[0];
        }
        else if (name == "Assault Rifle")
        {
            return weapons[1];
        }

        return null;
    }

}
