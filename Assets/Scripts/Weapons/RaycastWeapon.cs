using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class RaycastWeapon : MonoBehaviour
{
    public float weaponDamage = 0;
    public float weaponRange = 0;
    public int ammoOnStart = 0;
    public int ammoClipSize = 0;
    public bool hasLimitedClip = true;

    public GameObject bulletHole;
    public GameObject bloodSplat;
    public Text ammoCounter;
    public AudioClip shotSound;
    public AudioClip reloadSound;

    Animator animator;
    int ammoLeft;
    int ammoClipLeft;
    AudioSource source;

    void Awake()
    {
        source = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();

        if (ammoOnStart < ammoClipSize)
        {
            ammoClipLeft = ammoOnStart;
            ammoLeft = 0;
        }
        else
        {
            ammoLeft = ammoOnStart - ammoClipSize;
            ammoClipLeft = ammoClipSize;
        }
    }

   void Update()
    {
        //Raycast
        Vector2 bulletOffset = Random.insideUnitCircle * DynamicCrosshair.spread;
        Vector3 randomTarget = new Vector3(Screen.width / 2 + bulletOffset.x, Screen.height / 2 + bulletOffset.y, 0);
        Ray ray = Camera.main.ScreenPointToRay(randomTarget);
        RaycastHit hit;

        //Shot
        if(hasLimitedClip == true)
            ammoCounter.text = ammoClipLeft + "/" + ammoLeft;
        else
            ammoCounter.text =  ammoClipLeft.ToString();

        if (Input.GetButtonDown("Fire1") && ammoClipLeft > 0 && animator.GetBool("reload") == false && animator.GetBool("fire") == false)
        {
            ammoClipLeft--;

            //Shot animation + dynamic crosshair spread
            animator.SetBool("fire", true);
            DynamicCrosshair.changeSpread("shot");

            //Shot sound
            if (source.isPlaying)
                source.Stop();
            source.PlayOneShot(shotSound);

            //Collision
            if (Physics.Raycast(ray, out hit, weaponRange))
            {
                if(hit.transform.CompareTag("Enemy"))
                {
                    if(hit.collider.gameObject.GetComponent<EnemyStates>().currentState == hit.collider.gameObject.GetComponent<EnemyStates>().patrolState ||
                       hit.collider.gameObject.GetComponent<EnemyStates>().currentState == hit.collider.gameObject.GetComponent<EnemyStates>().alertState)
                    {
                        hit.collider.gameObject.SendMessage("alertByShot", transform.parent.transform.position, SendMessageOptions.DontRequireReceiver);
                    }
                    hit.collider.gameObject.SendMessage("gotHit", weaponDamage, SendMessageOptions.DontRequireReceiver);

                    GameObject newBloodSplat = Instantiate(bloodSplat, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal)) as GameObject;
                    newBloodSplat.transform.parent = hit.collider.gameObject.transform;
                }
                else
                {
                    GameObject newHole = Instantiate(bulletHole, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal)) as GameObject;
                    newHole.transform.parent = GameObject.FindGameObjectWithTag("Temp").transform;
                }

            }
        }
        else if (((Input.GetButtonDown("Fire1") && ammoClipLeft <= 0) || (Input.GetKeyDown("r") && ammoClipLeft != ammoClipSize)) 
                    && animator.GetBool("fire") == false && animator.GetBool("reload") == false)
        {
            reload(hasLimitedClip);
        }
    }


    void animationEnded(string boolName)
    {
        if(boolName == "fire")
            animator.SetBool("fire", false);
        else if(boolName == "reload")
            animator.SetBool("reload", false);
    }


    void reload(bool limited)
    {
        if (limited == true)
        {
            int bulletsToReload = ammoClipSize - ammoClipLeft;
            if (ammoLeft >= bulletsToReload)
            {
                ammoLeft -= bulletsToReload;
                ammoClipLeft = ammoClipSize;

                animator.SetBool("reload", true);
                source.PlayOneShot(reloadSound);
            }
            else if (ammoLeft < bulletsToReload && ammoLeft > 0)
            {
                ammoClipLeft += ammoLeft;
                ammoLeft = 0;

                animator.SetBool("reload", true);
                source.PlayOneShot(reloadSound);

            }
        }
        else if(limited == false)
        {
            ammoClipLeft += ammoLeft;
            ammoLeft = 0;
        }   
    }


    public void addAmmo(int amount)
    {
        if (hasLimitedClip)
            ammoLeft += amount;
        else
            ammoClipLeft += amount;
    }
}
