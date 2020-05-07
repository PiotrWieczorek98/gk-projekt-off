using UnityEngine;

public class Bonus : MonoBehaviour
{
    public bool isHpBonus;
    public bool isApBonus;
    public float bonusValue;
    public AudioClip pickUpSound;
    public FlashScreen flashScreen;

    PlayerStats ps;
    AudioSource source;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            bool isUsed = false;

            source = other.GetComponent<AudioSource>();
            ps = other.GetComponent<PlayerStats>();

            if (isHpBonus && !ps.isHPFull())
            {
                ps.addHP(bonusValue);
                flashScreen.flash("green");
                isUsed = true;
            }
            if (isApBonus && !ps.isAPFull())
            {
                ps.addAP(bonusValue);
                flashScreen.flash("blue");
                isUsed = true;
            }

            if (isUsed)
            {
                source.PlayOneShot(pickUpSound);
               Destroy(this.gameObject);
            }
        }
    }
}
