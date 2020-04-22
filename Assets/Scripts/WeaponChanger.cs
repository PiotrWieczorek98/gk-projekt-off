using UnityEngine;

public class WeaponChanger : MonoBehaviour
{
    int selectedWeapon = 0;
    void Start()
    {
        SelectWeapon();
    }

    
    void Update()
    {
        int previousSelectedWeapon = selectedWeapon;


        if(Input.GetKeyDown("1"))
                selectedWeapon = 0;
        else if (Input.GetKeyDown("2") && 2 <= transform.childCount)
                selectedWeapon = 1;
        else if (Input.GetKeyDown("3") && 3 <= transform.childCount)
                selectedWeapon = 2;


        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            if (selectedWeapon >= transform.childCount - 1)
                selectedWeapon = 0;
            else
                selectedWeapon++;
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            if (selectedWeapon <= 0)
                selectedWeapon = transform.childCount - 1;
            else
                selectedWeapon--;
        }

        if (previousSelectedWeapon != selectedWeapon)
            SelectWeapon();
    }

    void SelectWeapon()
    {
        int i = 0;
        foreach(Transform weapon in transform)
        {
            if (i == selectedWeapon)
                weapon.gameObject.SetActive(true);
            else
                weapon.gameObject.SetActive(false);
            i++;
        }
    }
}
