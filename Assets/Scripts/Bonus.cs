using UnityEngine;

public class Bonus : MonoBehaviour
{
    public bool isHpBonus;
    public bool isApBonus;
    public float bonusValue;
    public AudioClip pickUpSound;

    PlayerStats ps;
    FlashScreen fs;
    AudioSource source;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            bool isUsed = false;

            source = other.GetComponent<AudioSource>();
            ps = other.GetComponent<PlayerStats>();
            fs = ps.flashScreen;

            if (isHpBonus && !ps.isHPFull())
            {
                ps.addHP(bonusValue);
                isUsed = true;
            }
            if (isApBonus && !ps.isAPFull())
            {
                ps.addAP(bonusValue);
                isUsed = true;
            }

            if (isUsed)
            {
                source.PlayOneShot(pickUpSound);
                fs.flash("yellow");
               Destroy(this.gameObject);
            }
        }
    }
}
