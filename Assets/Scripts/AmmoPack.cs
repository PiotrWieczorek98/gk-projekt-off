using UnityEngine;

public class AmmoPack : MonoBehaviour
{
    public int amount;
    public string ammoType;
    public AudioClip pickUpSound;

    PlayerStats ps;
    FlashScreen fs;
    AudioSource source;
    RaycastWeapon raycastWeapon;
    ProjectileWeapon projectileWeapon;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {

            ps = other.GetComponent<PlayerStats>();
            source = other.GetComponent<AudioSource>();
            fs = ps.flashScreen;

            if(ammoType == "pistol" || ammoType == "shotgun")
            {
                raycastWeapon = ps.getRaycastWeapon(ammoType);
                raycastWeapon.addAmmo(amount);
            }
            else
            {
                projectileWeapon = ps.getProjectileWeapon(ammoType);
                projectileWeapon.addAmmo(amount);
            }

            fs.flash("yellow");
            source.PlayOneShot(pickUpSound);

            Destroy(this.gameObject);
        }
    }
}
