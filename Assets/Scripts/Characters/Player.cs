using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    
    [SerializeField] private float speed, jumpForce;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField, Range(1,20)] private float mouseSensX, mouseSensY;
    [SerializeField] private float minVeiw, maxVeiw;
    [SerializeField] private Transform lookAtPoint;

    [SerializeField] private Rigidbody bulletPrefab;
    [SerializeField] private float bulletSpeed;

        private float[] clipSize = {10, 50,1, 100 };
     private float[] AmmoCount = {10, 50, 1, 100 };
     private float[] AmmoReserve = {25, 200, 10, 100 };
     private float[] maxAmmo = {50, 500, 15, 100 };
    private float[] reloadTime = { 2, 1, 4,3 };
    // private int clipSize;

    //private int[,]weaponStats = new int[3,4];


    [SerializeField] private Image healthBar;


    [Header("please work")]
    [SerializeField] private float maxHealth;


    private float _health;
    public TMP_Text ammoCounter;

    private Vector2 currentRotation;

    private bool isGrounded;

    private Vector3 _moveDir;
    private Vector3 _scale;

    private Rigidbody rb;
    private float depth;

    private int currentWeapon;

    [SerializeField] private WeaponBase[] myWeapon;

    private Sniper sniper;
    private LaserGun laser;

    private bool reloading;
    private float Health
    {
        get => _health;
        set
        {
            _health = value;
            healthBar.fillAmount = _health/ maxHealth;
        }
    }

    void Start()
    {
        sniper = myWeapon[2].gameObject.GetComponent<Sniper>();
        laser = myWeapon[3].gameObject.GetComponent<LaserGun>();
        rb = GetComponent<Rigidbody>();
        depth = GetComponent<Collider>().bounds.size.y;
        InputManager.Init(this);
        InputManager.GameMode();
        _health = maxHealth;
        currentWeapon = 0;
        foreach(WeaponBase i in myWeapon)
        {
            i.gameObject.SetActive(false);
        }
        myWeapon[currentWeapon].gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.rotation *  (speed * Time.deltaTime *_moveDir);
        CheckGrounded();
        Health -= Time.deltaTime * 5;
        ammoCounter.text = AmmoCount[currentWeapon].ToString() + "/" + AmmoReserve[currentWeapon].ToString();
    }

    public void SetMovementDirection(Vector3 newDirection)
    {
        _moveDir = newDirection;
    }
   
    public void Jump()
    {
        Debug.Log("JUMP");
        if (isGrounded)
        {
            Debug.Log("I jumped");
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

    }

    private void CheckGrounded()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, depth, groundLayer);

        Debug.DrawRay(transform.position, Vector3.down * depth, Color.green, 0, false);
    }

    /*public void Shoot(float buttonValue)
    {
        Debug.Log(buttonValue);
        if (buttonValue == 1)
        {
            _scale = transform.localScale;
            transform.localScale += _scale;

        }
        else {


            _scale = transform.localScale;
            transform.localScale -= _scale/2;

        }
    }*/

    public void Reload()
    {
        if (!reloading)
        {


            if (AmmoCount[currentWeapon] != clipSize[currentWeapon])
            {
                StartCoroutine("ReloadTime");
            }
        }
        
    }
    public void AddAmmo(int pickUp)
    {
        if(AmmoReserve[currentWeapon] < maxAmmo[currentWeapon])
        {
            AmmoReserve[currentWeapon] += pickUp;
        }
        


    }
    private bool FireState;
    public void Shoot()
    {
        Debug.Log(FireState);
        FireState = !FireState;
        if (AmmoCount[currentWeapon] > 0)
        {





            if (FireState)
                myWeapon[currentWeapon].StartFiring();
            else myWeapon[currentWeapon].StopFiring();
        }
        else myWeapon[currentWeapon].StopFiring();

        if(!FireState && currentWeapon == 3)
        {
            Debug.Log("stop laser");
            laser.StopLaser();
        }
        /*   if (AmmoCount > 0)
           {
               Rigidbody currentProjectile = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
               currentProjectile.AddForce(lookAtPoint.forward * bulletSpeed, ForceMode.Impulse);
               Destroy(currentProjectile.gameObject, 4);
               AmmoCount--;
           }*/
       }


        public void SetLookDirection(Vector2 readValue)
        {
            currentRotation.x += readValue.x * Time.deltaTime * mouseSensX;
            currentRotation.y += readValue.y * Time.deltaTime * mouseSensY;

            transform.rotation = Quaternion.AngleAxis(currentRotation.x, Vector2.up);
            currentRotation.y = Mathf.Clamp(currentRotation.y, minVeiw, maxVeiw);
            lookAtPoint.localRotation = Quaternion.AngleAxis(currentRotation.y, Vector2.right);


        }

    public void SwapWeapon(float weaponIndex)
    {
        //Debug.Log(weaponIndex);
        if (!FireState && !reloading)
        {


            myWeapon[currentWeapon].gameObject.SetActive(false);
            currentWeapon = (int)weaponIndex;
            myWeapon[currentWeapon].gameObject.SetActive(true);
        }

    }
    public void ShotFired(float bulletsConsumed)
    {
        AmmoCount[currentWeapon] -= bulletsConsumed;
        if(AmmoCount[currentWeapon] < 0)
        {
            AmmoCount[currentWeapon] = 0;
        }
    }
    public float GetCurrentAmmo()
    {
        return (AmmoCount[currentWeapon]);
    }
    
    public void Zoom()
    {
        Debug.Log("Player zoom func");
        if(currentWeapon == 2)
        {
            sniper.Zoom();
        }
    }

    private IEnumerator ReloadTime()
    {
        reloading = true;
        yield return new WaitForSeconds(reloadTime[currentWeapon]);
        if (AmmoCount[currentWeapon] < clipSize[currentWeapon])
        {
            var difference = clipSize[currentWeapon] - AmmoCount[currentWeapon];
            if (AmmoReserve[currentWeapon] < difference)
            {
                AmmoCount[currentWeapon] += AmmoReserve[currentWeapon];
                AmmoReserve[currentWeapon] = 0;
            }
            else
            {

                AmmoCount[currentWeapon] += difference;
                AmmoReserve[currentWeapon] -= difference;
            }
        }
        reloading = false;
    }
}
