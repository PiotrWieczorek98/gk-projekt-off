using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AutomaticGunScriptLPFP : MonoBehaviour {

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

	public Sprite scopeTexture;
	public float scopeTextureSize = 0.01f;
	//Scope camera fov
	[Range(5, 40)]
	public float scope2AimFOV = 25;
	[Space(10)]
	//Toggle iron sights
	public bool ironSights;
	public bool alwaysShowIronSights;
	//Iron sights camera fov
	[Range(5, 40)]
	public float ironSightsAimFOV = 16;
	[Space(10)]

	//Weapon attachments components

	[Header("Scope Model Renderers")]
	[Space(10)]
	//All attachment renderer components

	public SkinnedMeshRenderer scopeRenderer;
	public SkinnedMeshRenderer ironSightsRenderer;
	[Header("Scope Sight Mesh Renderers")]
	[Space(10)]
	//Scope render meshes
	public GameObject scopeRenderMesh;

	[Header("Scope Sight Sprite Renderers")]
	[Space(10)]
	//Scope sight textures
	public SpriteRenderer scopeSpriteRenderer;


	[Header("Weapon Sway")]
	//Enables weapon sway
	[Tooltip("Toggle weapon sway.")]
	public bool weaponSway;

	public float swayAmount = 0.02f;
	public float maxSwayAmount = 0.06f;
	public float swaySmoothValue = 4.0f;

	private Vector3 initialSwayPosition;

	//Used for fire rate
	private float lastFired;
	[Header("Weapon Settings")]
	//How fast the weapon fires, higher value means faster rate of fire
	[Tooltip("How fast the weapon fires, higher value means faster rate of fire.")]
	public float fireRate;
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
	public float bulletForce = 400.0f;

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
		//(some weapons use more than one casing spawn)
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

		//Enable scope renderer
		scopeRenderer.GetComponent
		<SkinnedMeshRenderer> ().enabled = true;
		scopeRenderMesh.SetActive(true);
		//Set the scope sight texture
		scopeSpriteRenderer.GetComponent
			<SpriteRenderer>().sprite = scopeTexture;
		//Set the scope texture size
		scopeSpriteRenderer.transform.localScale = new Vector3 
			(scopeTextureSize, scopeTextureSize, scopeTextureSize);
	}

	private void Start () 
	{
		
		//Save the weapon name
		storedWeaponName = weaponName;
		//Get weapon name from string to text
		currentWeaponText.text = weaponName;
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

			gunCamera.fieldOfView = Mathf.Lerp (gunCamera.fieldOfView, scope2AimFOV, fovSpeed * Time.deltaTime);


			isAiming = true;
			anim.SetBool ("Aim Scope 2", true);

			if (!soundHasPlayed) 
			{
				audioSource.clip = SoundClips.aimSound;
				audioSource.Play ();
	
				soundHasPlayed = true;
			}

				scopeSpriteRenderer.GetComponent
				<SpriteRenderer> ().enabled = true;
		} 
		else 
		{
			//When right click is released
			gunCamera.fieldOfView = Mathf.Lerp(gunCamera.fieldOfView,
				defaultFov,fovSpeed * Time.deltaTime);

			isAiming = false;
			anim.SetBool ("Aim Scope 2", false);
				
			soundHasPlayed = false;

				scopeSpriteRenderer.GetComponent
				<SpriteRenderer> ().enabled = false;
		}
		//Aiming end

		//If randomize muzzleflash is true, genereate random int values
		if (randomMuzzleflash == true) 
		{
			randomMuzzleflashValue = Random.Range (minRandomValue, maxRandomValue);
		}

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
			{
				Reload();
			}
		} 
			
		//AUtomatic fire
		//Left click hold 
		if (Input.GetMouseButton (0) && !outOfAmmo && !isReloading) 
		{
			//Shoot automatic
			if (Time.time - lastFired > 1 / fireRate) 
			{
				lastFired = Time.time;

				//Remove 1 bullet from ammo
				ammoInMag -= 1;

				//Play shoot sound
				shootAudioSource.clip = SoundClips.shootSound;
				shootAudioSource.Play ();

				if (!isAiming) //if not aiming
				{
					anim.Play ("Fire", 0, 0f);
					//If random muzzle is false
					if (!randomMuzzleflash && 
						enableMuzzleflash == true) 
					{
						muzzleParticles.Emit (1);
						//Light flash start
						StartCoroutine(MuzzleFlashLight());
					} 
					else if (randomMuzzleflash == true)
					{
						//Only emit if random value is 1
						if (randomMuzzleflashValue == 1) 
						{
							if (enableSparks == true) 
							{
								//Emit random amount of spark particles
								sparkParticles.Emit (Random.Range (minSparkEmission, maxSparkEmission));
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
				else //if aiming
				{
					anim.Play ("Aim Fire Scope 2", 0, 0f);

					//If random muzzle is false
					if (!randomMuzzleflash) 
					{
						muzzleParticles.Emit (1);
					//If random muzzle is true
					} 
					else if (randomMuzzleflash == true) 
					{
						//Only emit if random value is 1
						if (randomMuzzleflashValue == 1) 
						{
							if (enableSparks == true) 
							{
								//Emit random amount of spark particles
								sparkParticles.Emit (Random.Range (minSparkEmission, maxSparkEmission));
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

				//Spawn bullet from bullet spawnpoint
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
		}

		//Reload 
		if (Input.GetKeyDown (KeyCode.R) && !isReloading ) 
			Reload ();

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


	//Reload
	private void Reload () 
	{
		if(ammoInStorage > 0)
		{
			anim.Play ("Reload Out Of Ammo", 0, 0f);
			audioSource.clip = SoundClips.reloadSoundOutOfAmmo;
			audioSource.Play ();

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
	private void AnimationCheck () {
		
		//Check if reloading
		if (anim.GetCurrentAnimatorStateInfo (0).IsName ("Reload Out Of Ammo") )
		{
			isReloading = true;
		} 
		else 
		{
			isReloading = false;
		}
	}
}