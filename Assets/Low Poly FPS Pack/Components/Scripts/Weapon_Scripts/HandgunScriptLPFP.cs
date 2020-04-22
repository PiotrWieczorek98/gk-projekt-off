using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HandgunScriptLPFP : MonoBehaviour {

	//Animator component attached to weapon
	Animator anim;

	[Header("Gun Camera")]
	//Main gun camera
	public Camera gunCamera;

	[Header("Gun Camera Options")]
	//How fast the camera field of view changes when aiming 
	[Tooltip("How fast the camera field of view changes when aiming.")]
	public float fovSpeed = 15.0f;
	//Default camera field of view
	[Tooltip("Default value for camera field of view (40 is recommended).")]
	public float defaultFov = 40.0f;

	[Header("UI Weapon Name")]
	[Tooltip("Name of the current weapon, shown in the game UI.")]
	public string weaponName;
	private string storedWeaponName;

	[Header("Gun icon")]
	//Gun icon
	public Sprite gunIcon;
	[Tooltip("Name of the object shown in the game UI.")]
	public Image HUDIcon;

	//Iron sights camera fov
	[Range(5, 40)]
	public float ironSightsAimFOV = 16;

	[Header("Iron Sight Model Renderers")]
	//Weapon attachments components
	public SkinnedMeshRenderer ironSightsRenderer;



	[Header("Weapon Sway")]
	//Enables weapon sway
	[Tooltip("Toggle weapon sway.")]
	public bool weaponSway;

	public float swayAmount = 0.02f;
	public float maxSwayAmount = 0.06f;
	public float swaySmoothValue = 4.0f;

	private Vector3 initialSwayPosition;

	[Header("Weapon Settings")]

	public float sliderBackTimer = 1.58f;
	private bool hasStartedSliderBack;

	//Eanbles auto reloading when out of ammo
	[Tooltip("Enables auto reloading when out of ammo.")]
	public bool autoReload;
	//Delay between shooting last bullet and reloading
	public float autoReloadDelay;
	//Check if reloading
	private bool isReloading;

	//Check if running
	private bool isRunning;
	//Check if aiming
	private bool isAiming;
	//Check if walking
	private bool isWalking;

	//How much ammo is currently left
	private int ammoInMag;
	//How much ammo is left in storage
	public int ammoInStorage;
	//Maximum ammount of ammo
	[Tooltip("Maximum ammount of ammo.")]
	public int ammoMax;
	//Total amount of ammo
	[Tooltip("How much ammo in one clip.")]
	public int clipSize;
	//Check if out of ammo
	private bool outOfAmmo;

	[Header("Bullet Settings")]
	//Bullet
	[Tooltip("How much force is applied to the bullet when shooting.")]
	public float bulletForce = 400;

	[Header("Muzzleflash Settings")]
	public bool randomMuzzleflash = false;
	//min should always bee 1
	private int minRandomValue = 1;

	[Range(2, 25)]
	public int maxRandomValue = 5;

	private int randomMuzzleflashValue;

	public bool enableMuzzleflash = true;
	public ParticleSystem muzzleParticles;
	public bool enableSparks = true;
	public ParticleSystem sparkParticles;
	public int minSparkEmission = 1;
	public int maxSparkEmission = 7;

	[Header("Muzzleflash Light Settings")]
	public Light muzzleflashLight;
	public float lightDuration = 0.02f;

	[Header("Audio Source")]
	//Main audio source
	public AudioSource audioSource;
	//Audio source used for shoot sound
	public AudioSource shootAudioSource;

	[Header("UI Components")]
	public Text currentWeaponText;
	public Text currentAmmoText;
	public Text storageAmmoText;

	[System.Serializable]
	public class prefabs
	{  
		[Header("Prefabs")]
		public Transform bulletPrefab;
		public Transform casingPrefab;
	}
	public prefabs Prefabs;
	
	[System.Serializable]
	public class spawnpoints
	{  
		[Header("Spawnpoints")]
		//Array holding casing spawn points 
		//Casing spawn point array
		public Transform casingSpawnPoint;
		//Bullet prefab spawn from this point
		public Transform bulletSpawnPoint;
	}
	public spawnpoints Spawnpoints;

	[System.Serializable]
	public class soundClips
	{
		public AudioClip shootSound;
		public AudioClip silencerShootSound;
		public AudioClip takeOutSound;
		public AudioClip reloadSoundOutOfAmmo;
		public AudioClip reloadSoundAmmoLeft;
		public AudioClip aimSound;
	}
	public soundClips SoundClips;

	private bool soundHasPlayed = false;

	private void Awake () 
	{
		//Set the animator component
		anim = GetComponent<Animator>();

		//Set current ammo to total ammo value
		ammoInMag = clipSize;

		muzzleflashLight.enabled = false;

		//enable ironsights renderer
		ironSightsRenderer.GetComponent
		<SkinnedMeshRenderer> ().enabled = true;
	}

	private void Start () 
	{
		//Save the weapon name
		storedWeaponName = weaponName;
		//Get weapon name from string to text
		currentWeaponText.text = weaponName;
		//Set total ammo text from total ammo int
		storageAmmoText.text = ammoInStorage.ToString();

		//Weapon sway
		initialSwayPosition = transform.localPosition;

		//Set the shoot sound to audio source
		shootAudioSource.clip = SoundClips.shootSound;
	}

	private void LateUpdate () 
	{
		//Weapon sway
		if (weaponSway == true) 
		{
			float movementX = -Input.GetAxis ("Mouse X") * swayAmount;
			float movementY = -Input.GetAxis ("Mouse Y") * swayAmount;
			//Clamp movement to min and max values
			movementX = Mathf.Clamp 
				(movementX, -maxSwayAmount, maxSwayAmount);
			movementY = Mathf.Clamp 
				(movementY, -maxSwayAmount, maxSwayAmount);
			//Lerp local pos
			Vector3 finalSwayPosition = new Vector3 
				(movementX, movementY, 0);
			transform.localPosition = Vector3.Lerp 
				(transform.localPosition, finalSwayPosition + 
				initialSwayPosition, Time.deltaTime * swaySmoothValue);
		}


		// Set icon
		HUDIcon.sprite = gunIcon;
		currentWeaponText.text = storedWeaponName.ToString();
		currentAmmoText.text = ammoInMag.ToString();
		storageAmmoText.text = ammoInStorage.ToString();
	}
	
	private void Update () {

		//Aiming
		//Toggle camera FOV when right click is held down
		if(Input.GetButton("Fire2") && !isReloading && !isRunning) 
		{

			gunCamera.fieldOfView = Mathf.Lerp (gunCamera.fieldOfView, ironSightsAimFOV, fovSpeed * Time.deltaTime);
			isAiming = true;

			anim.SetBool ("Aim", true);

			if (!soundHasPlayed) 
			{
				audioSource.clip = SoundClips.aimSound;
				audioSource.Play ();
	
				soundHasPlayed = true;
			}
		} 
		else 
		{
			//When right click is released
			gunCamera.fieldOfView = Mathf.Lerp(gunCamera.fieldOfView, defaultFov,fovSpeed * Time.deltaTime);

			isAiming = false;
			soundHasPlayed = false;
			anim.SetBool("Aim", false);

		}
		//Aiming end

		//If randomize muzzleflash is true, genereate random int values
		if (randomMuzzleflash == true)
			randomMuzzleflashValue = Random.Range (minRandomValue, maxRandomValue);

		//Continosuly check which animation 
		//is currently playing
		AnimationCheck ();

		//Play knife attack 1 animation when Q key is pressed
		if (Input.GetKeyDown (KeyCode.Q)) 
		{
			anim.Play ("Knife Attack 1", 0, 0f);
		}
		//Play knife attack 2 animation when F key is pressed
		if (Input.GetKeyDown (KeyCode.F)) 
		{
			anim.Play ("Knife Attack 2", 0, 0f);
		}

		//If out of ammo
		if (ammoInMag == 0) 
		{
			//Toggle bool
			outOfAmmo = true;
			//Auto reload if true
			if (autoReload == true && !isReloading) 		
				Reload();	
				
			//Set slider back
			anim.SetBool ("Out Of Ammo Slider", true);
			//Increase layer weight for blending to slider back pose
			anim.SetLayerWeight (1, 1.0f);
		} 
		else 
		{
			//Toggle bool
			outOfAmmo = false;
			//anim.SetBool ("Out Of Ammo", false);
			anim.SetLayerWeight (1, 0.0f);
		}

		//Shooting 
		if (Input.GetMouseButtonDown (0) && !outOfAmmo && !isReloading) 
		{
			anim.Play ("Fire", 0, 0f);
				
			//Remove 1 bullet from ammo
			ammoInMag -= 1;

			// Play audio
			shootAudioSource.clip = SoundClips.shootSound;
			shootAudioSource.Play ();

			//Light flash start
			StartCoroutine(MuzzleFlashLight());

			if (!isAiming) //if not aiming
			{
				anim.Play ("Fire", 0, 0f);
				muzzleParticles.Emit (1);

				if (enableSparks == true) 
				{
					//Emit random amount of spark particles
					sparkParticles.Emit (Random.Range (1, 6));
				}
			} 
			else //if aiming
			{
				anim.Play ("Aim Fire", 0, 0f);
					
				//If random muzzle is false
				if (!randomMuzzleflash) 
				{
					muzzleParticles.Emit (1);
				} 
				else if (randomMuzzleflash == true) 
				{
					//Only emit if random value is 1
					if (randomMuzzleflashValue == 1) 
					{
						if (enableSparks == true) 
						{
							//Emit random amount of spark particles
							sparkParticles.Emit (Random.Range (1, 6));
						}
						if (enableMuzzleflash == true) 
						{
							muzzleParticles.Emit (1);
							//Light flash start
							StartCoroutine (MuzzleFlashLight ());
						}
					}
				}
			}
				
			//Spawn bullet at bullet spawnpoint
			var bullet = (Transform)Instantiate (
				Prefabs.bulletPrefab,
				Spawnpoints.bulletSpawnPoint.transform.position,
				Spawnpoints.bulletSpawnPoint.transform.rotation);

			//Add velocity to the bullet
			bullet.GetComponent<Rigidbody>().velocity = 
			bullet.transform.forward * bulletForce;

			//Spawn casing prefab at spawnpoint
			Instantiate (Prefabs.casingPrefab, 
				Spawnpoints.casingSpawnPoint.transform.position, 
				Spawnpoints.casingSpawnPoint.transform.rotation);
		}

		//Reload 
		if (Input.GetKeyDown (KeyCode.R) && !isReloading) 
		{
			//Reload
			Reload ();

			if (!hasStartedSliderBack) 
			{
				hasStartedSliderBack = true;
				StartCoroutine (HandgunSliderBackDelay());
			}
		}

		//Walking when pressing down WASD keys
		if (Input.GetKey (KeyCode.W) && !isRunning || 
			Input.GetKey (KeyCode.A) && !isRunning || 
			Input.GetKey (KeyCode.S) && !isRunning || 
			Input.GetKey (KeyCode.D) && !isRunning) 
		
			anim.SetBool ("Walk", true);
		else 
			anim.SetBool ("Walk", false);
		

		//Running when pressing down W and Left Shift key
		if ((Input.GetKey (KeyCode.W) && Input.GetKey (KeyCode.LeftShift))) 
		
			isRunning = true;
		else
			isRunning = false;
		
		//Run anim toggle
		if (isRunning == true) 
			anim.SetBool ("Run", true);
		else
			anim.SetBool ("Run", false);
	}

	private IEnumerator HandgunSliderBackDelay () 
	{
		//Wait set amount of time
		yield return new WaitForSeconds (sliderBackTimer);
		//Set slider back
		anim.SetBool ("Out Of Ammo Slider", false);
		//Increase layer weight for blending to slider back pose
		anim.SetLayerWeight (1, 0.0f);

		hasStartedSliderBack = false;
	}

	//Reload
	private void Reload () 
	{
		if (ammoInStorage > 0)
		{
			anim.Play("Reload Out Of Ammo", 0, 0f);
			audioSource.clip = SoundClips.reloadSoundOutOfAmmo;
			audioSource.Play();

			if (ammoInStorage + ammoInMag >= clipSize)
			{
				ammoInStorage += ammoInMag;
				ammoInMag = clipSize;
				ammoInStorage -= clipSize;
			}
			else
			{
				ammoInMag += ammoInStorage;
				ammoInStorage = 0;
			}
		}

	}

	//Show light when shooting, then disable after set amount of time
	private IEnumerator MuzzleFlashLight () 
	{
		muzzleflashLight.enabled = true;
		yield return new WaitForSeconds (lightDuration);
		muzzleflashLight.enabled = false;
	}

	//Check current animation playing
	private void AnimationCheck () 
	{
		//Check if reloading
		//Check both animations
		if (anim.GetCurrentAnimatorStateInfo (0).IsName ("Reload Out Of Ammo"))
		{
			isReloading = true;
		} 
		else 
		{
			isReloading = false;
		}
	}
}