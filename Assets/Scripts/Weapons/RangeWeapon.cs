﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RangeWeapon: MonoBehaviour 
{

	[Header("Animator")]
	//Animator component attached to weapon
	public Animator anim;

	[Header("Cameras")]
	public Camera gunCamera;
	public Camera mainCamera;

	[Header("Gun Camera Options")]
	[Tooltip("How fast the camera field of view changes when aiming.")]
	public float fovSpeed = 15.0f;
	[Tooltip("Default value for camera field of view (40 is recommended).")]
	public float defaultFov = 40.0f;

	[Header("UI Weapon Name")]
	[Tooltip("Name of the current weapon, shown in the game UI.")]
	public string weaponName;
	private string storedWeaponName;

	[Header("Gun icon")]
	public Sprite gunIcon;
	[Tooltip("Name of the object shown in the game UI.")]
	public Image HUDIcon;

	[Header("Gun type (choose one)")]
	public bool isAutomaticWeapon = false;
	public bool isPumpActionWeapon = false;
	// Used for shotgun reload mechanic
	private bool nextShell = false;
	public float fireRate = 0.2f;
	public float recoilStrength = 0.2f;
	private float nextFire = 0.0f;

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

	//Eanbles auto reloading when out of ammo
	[Tooltip("Enables auto reloading when out of ammo.")]
	public bool autoReload;
	//Delay between shooting last bullet and reloading
	public float autoReloadDelay;
	//Check if reloading
	private bool isReloading;

	private bool isRunning;
	private bool isAiming;

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
	[Tooltip("How much damage the bullet deals")]
	public float bulletDamage = 1;

	[Header("Muzzleflash Settings")]
	public bool randomMuzzleflash = false;
	//min should always bee 1
	private int minRandomValue = 1;

	[Range(2, 25)]
	public int maxRandomValue = 5;

	private int randomMuzzleflashValue;

	public ParticleSystem muzzleParticles;
	public ParticleSystem sparkParticles;
	public int minSparkEmission = 1;
	public int maxSparkEmission = 7;

	[Header("Muzzleflash Light Settings")]
	public Light muzzleflashLight;
	public float lightDuration = 0.02f;

	[Header("Audio Source")]
	public AudioSource audioSource;

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


	private void Start () 
	{
		//Set the animator component
		anim = GetComponent<Animator>();

		//Set current ammo to total ammo value
		ammoInMag = clipSize;

		//Save the weapon name
		storedWeaponName = weaponName;
		//Get weapon name from string to text
		currentWeaponText.text = weaponName;
		//Set total ammo text from total ammo int
		storageAmmoText.text = ammoInStorage.ToString();

		//Weapon sway
		initialSwayPosition = transform.localPosition;
	}
	
	private void Update () 
	{
		//Aiming
		//Toggle camera FOV when right click is held down
		if(Input.GetButton("Fire2") && !isReloading && !isRunning) 
		{

			gunCamera.fieldOfView = Mathf.Lerp (gunCamera.fieldOfView, ironSightsAimFOV, fovSpeed * Time.deltaTime);
			anim.SetBool ("Aim", true);
			// Force play animation to avoid transition delay
			if(!isAiming)
				anim.Play("Aim In");

			isAiming = true;

		}
		else 
		{
			//When right click is released
			gunCamera.fieldOfView = Mathf.Lerp(gunCamera.fieldOfView, defaultFov,fovSpeed * Time.deltaTime);

			isAiming = false;
			anim.SetBool("Aim", false);

		}
		//Aiming end

		//If randomize muzzleflash is true, genereate random int values
		if (randomMuzzleflash == true)
			randomMuzzleflashValue = Random.Range (minRandomValue, maxRandomValue);

		//Continosuly check which animation 
		//is currently playing
		AnimationCheck ();


		//If out of ammo
		if (ammoInMag == 0) 
		{
			//Toggle bool
			outOfAmmo = true;
			//Auto reload if true
			if (autoReload == true && !isReloading && ammoInStorage > 0) 		
				Reload();	
				
		} 
		else 
		{
			//Toggle bool
			outOfAmmo = false;
		}

		//Shooting 
		if (!outOfAmmo && !isReloading &&
			(!isAutomaticWeapon && Input.GetMouseButtonDown(0) ||
			(isAutomaticWeapon && Input.GetMouseButton(0))) &&
			Time.time > nextFire)
		{
			nextFire = Time.time + fireRate;
			//Remove 1 bullet from ammo
			ammoInMag -= 1;

			//Light flash start
			StartCoroutine(MuzzleFlashLight());

			if (!isAiming) //if not aiming
			{
				anim.Play ("Fire", 0, 0f);

				//Emit random amount of spark particles
				sparkParticles.Emit(Random.Range (minSparkEmission, maxSparkEmission));
				muzzleParticles.Emit(1);
				StartCoroutine(MuzzleFlashLight());

			}
			else //if aiming
			{
				anim.Play("Aim Fire", 0, 0f);
				//Emit random amount of spark particles
				sparkParticles.Emit(Random.Range(minSparkEmission, maxSparkEmission));
				muzzleParticles.Emit(1);
				StartCoroutine(MuzzleFlashLight());

			}

			//Spawn bullet at bullet spawnpoint
			var bullet = Transform.Instantiate (
				Prefabs.bulletPrefab,
				Spawnpoints.bulletSpawnPoint.transform.position,
				Spawnpoints.bulletSpawnPoint.transform.rotation);

			//Add velocity to the bullet
			bullet.GetComponent<Rigidbody>().velocity = 
			bullet.transform.forward * bulletForce;

			// Add values
			bullet.tag = "Bullet";
			bullet.gameObject.layer = LayerMask.NameToLayer("Player");
			bullet.GetComponent<BulletScript>().damage = bulletDamage;

			//Spawn casing prefab at spawnpoint
			Instantiate (Prefabs.casingPrefab, 
				Spawnpoints.casingSpawnPoint.transform.position, 
				Spawnpoints.casingSpawnPoint.transform.rotation);

			// Weapon recoil
			var randomNumberX = Random.Range(-recoilStrength * 2, 0);
			var randomNumberY = Random.Range(-recoilStrength, recoilStrength);

			var player = GameObject.FindGameObjectWithTag("Player");
			player.GetComponent<PlayerMovement>().rotation.x += randomNumberX;
			player.GetComponent<PlayerMovement>().rotation.y += randomNumberY;
		}


		//Reload 
		if ((Input.GetKeyDown (KeyCode.R) && !isReloading && ammoInMag != clipSize && ammoInStorage > 0) ||
			(isPumpActionWeapon && nextShell && !isReloading)) 
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

	private void LateUpdate()
	{
		//Weapon sway
		if (weaponSway == true)
		{
			float movementX = -Input.GetAxis("Mouse X") * swayAmount;
			float movementY = -Input.GetAxis("Mouse Y") * swayAmount;
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


	//Reload
	private void Reload () 
	{
		if(!isPumpActionWeapon)
		{
			anim.Play("Reload", 0, 0f);

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
		else if (isPumpActionWeapon)
		{
			if (!nextShell)
			{
				anim.Play("Reload", 0, 0f);
				isReloading = true;
			}

			if (!isReloading)
			{
				anim.Play("Insert Shell", 0, 0f);

				ammoInStorage--;
				ammoInMag++;
			}

			if (ammoInStorage > 0 && ammoInMag < clipSize)
				nextShell = true;
			else
				nextShell = false;
		}


	}

	public void addAmmo(int amount)
	{
		ammoInStorage += amount;
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
		if (anim.GetCurrentAnimatorStateInfo(0).IsName("Reload") ||
			anim.GetCurrentAnimatorStateInfo(0).IsName("Insert Shell"))
			isReloading = true;
		else 
			isReloading = false;
	}
}
