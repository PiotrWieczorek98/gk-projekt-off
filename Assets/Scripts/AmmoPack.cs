using UnityEngine;

public class AmmoPack : MonoBehaviour
{
    public int amount;
    public string ammoType;
    public AudioClip pickUpSound;
    public FlashScreen flashScreen;


    PlayerStats ps;
    AudioSource source;
    RangeWeapon weapon;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {

            ps = other.GetComponent<PlayerStats>();
            source = other.GetComponent<AudioSource>();


            weapon = ps.getWeapon(ammoType);
            weapon.addAmmo(amount);


            flashScreen.flash("yellow");
            source.PlayOneShot(pickUpSound);

            Destroy(this.gameObject);
        }
    }
}
