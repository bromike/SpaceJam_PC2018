using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerBehaviour : MonoBehaviour
{
    /*Player Stats.*/
    [SerializeField]
    private int MaxHp;
    private int currentHp;
    [SerializeField]
    private int MaxShieldPts;
    private int currentShieldPts;
    [SerializeField]
    private int maxBullet;
    private int currentBullet;
    [SerializeField]
    private int carriedBullets;
    [SerializeField]
    private int maxCarriedBullets;
    [SerializeField]
    private int grenadesNumber;

    /*Weapons Variables*/
    public GameObject bullet;
    private float fireRate;

    /*Falling Damage Variables.*/
    private float fallheight;
    private float fallDamage;
    [SerializeField]
    private float fallingCoef;

    /*Player Controls Acces.*/
    private PlayerControls control;
    private Animator playerGunAnime;
    public Transform rifleBarrel;
    private bool isReloading;

    /*Player Canvas*/
    private Text Hpbar;
    private Text Ammo;

    private IEnumerator fallingCoroutine;
    private IEnumerator reloading;


    /*Player Damage*/
    bool damaged;
    
    private Image damageImage;
    public Color flashColour = new Color(1f, 0f, 0f, 0.1f);
    public float flashSpeed = 5f;
    public Slider healthSlider;
    public Slider shieldSlider;
    bool isDead = false;

    void Start()
    {
        control = GetComponent<PlayerControls>();
        playerGunAnime = GetComponentInChildren<Animator>();
        fallheight = 9.0f;
        fallDamage = 0;
        Hpbar = GameObject.FindObjectOfType<Text>();
        Ammo = GameObject.FindObjectOfType<Text>();
        currentHp = MaxHp;
        currentShieldPts = MaxShieldPts;
        currentBullet = maxBullet;
        isReloading = false;
        fallingCoroutine = CalculateFallDamage();
        reloading = Reload();
    }

    void Update()
    {
        //Falling Damage Coroutine.
        if (!control.isGrounded() && !Physics.Raycast(transform.position, Vector3.down, fallheight))
        {
            StartCoroutine(fallingCoroutine);
        }
        currentHp = Mathf.Clamp(currentHp, 0, MaxHp);

        //Reloads.
        if (Input.GetButtonDown("Reload") && currentBullet < maxBullet && carriedBullets != 0)
        {
            isReloading = true;
            StartCoroutine(reloading);
        }

        //damage from weapons
        damageImage.color = damaged ? flashColour : Color.Lerp(damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
        Ammo.text = "" + currentBullet;
    }

    IEnumerator CalculateFallDamage()
    {
        while (true)                /*This loop serve as a restart for the coroutine. Otherwise, the coroutine will "restart" where it left off last time it was stopped.*/
        {
            while (!control.isGrounded())
            {
                fallDamage += fallingCoef * (int)Mathf.Log10(Mathf.Abs(control.GetVelocity().y)) * Time.deltaTime;
                yield return null;
            }
            currentHp -= (int)fallDamage;
            fallDamage = 0;
            StopCoroutine(fallingCoroutine);
            yield return null;
        }
    }

    IEnumerator Reload()
    {
        while (true)
        {
            if (!Input.GetButton("Fire1") && isReloading)
            {
                playerGunAnime.SetTrigger("Reload");
                carriedBullets -= (maxBullet - currentBullet);
                carriedBullets = Mathf.Clamp(carriedBullets, 0, maxCarriedBullets);
                currentBullet = maxBullet;
                yield return new WaitForSeconds(1.50f);             //1.50 is the time the Reload animation takes.
            }
            isReloading = false;
            StopCoroutine(reloading);
            yield return null;
        }
    }
    
    public void Shooting()
    {
        if (fireRate < Time.time && !isReloading)
        {
            fireRate = Time.time + 0.1f;
            currentBullet -= 1;
            currentBullet = Mathf.Clamp(currentBullet, 0, maxBullet);
            if (currentBullet > 0)
                Instantiate(bullet, rifleBarrel.transform.position, rifleBarrel.transform.rotation);
        }
    }
    
    public void TakeDamage(int amount)
    {
        if (amount < 0)
            return;

        damaged = true;
                
        //Shield Damage
        if(currentShieldPts >= amount)
        {
            currentShieldPts -= amount;
        }

        else if(currentShieldPts > 0 && currentShieldPts < amount)
        {
            amount -= currentShieldPts;
            currentShieldPts = 0;
        }

        shieldSlider.value = currentShieldPts;
        
        //Life damage
        if(currentShieldPts == 0 && !isDead)
        {
            currentHp -= amount;
        }

        healthSlider.value = currentHp;

        if(currentHp <= 0 && !isDead)
        {
            Death();
        }
    }

    void Death()
    {
        isDead = true;
        //more...
    }

    public void SetCurrentHP()
    {

    }
}
