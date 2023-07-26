using System;
using DitzeGames.Effects;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using RDG;
using UnityEngine.Serialization;

[RequireComponent(typeof(Animator))]
public class PlayerControl : MonoBehaviour
{
    //----------- Variables

    [FormerlySerializedAs("_showLogs")]
    [Header("Settings")]
    [SerializeField]
    bool showLogs;

    [FormerlySerializedAs("_Prefix")] [SerializeField]
    string prefix;

    [Header("Player Variables")]
    public float xpos, ypos, zpos, boost;
    [SerializeField] float  xspeed, yspeed, zspeed, targetX, XSPEED, xDiff, GRAVITY, _speedMod, angle, targetAngle, shieldTimer, boostTimer, boostSpeed, boostTime, shieldTime, magnetTime;
    [SerializeField] int lanes, width, laneWidth, health, maxAngle;
    private List<GameObject> _currentItems;

    [Header("ReadyGo")] [SerializeField] private GameObject readyTxtGO, goTxtGO;

    [Header("Health")]
    [SerializeField] private List<Sprite> healthSprites;
    [SerializeField] GameObject healthImage;
    [SerializeField] Text HealthText;

    [Header("Audio")]
    [SerializeField] AudioSource takeDamage, tireScreech, engine, coinPickup, powerUp, milkSplash, clawGrab, clawRelease;

    [Header("Particles")]
    [SerializeField] ParticleSystem breakWall_PS;
    [SerializeField] ParticleSystem[] wheelDust_PS;

    [Header("Animations")]
    [SerializeField] Animator carAnim;

    private Vector2 _touchStart = Vector2.zero;
    private Vector2 _touchEnd = Vector2.zero;
    private Vector2 _touchDelta = Vector2.zero;

    [Header("Inputs")]
    private bool _right, _left, _up, _turnRight, _turnLeft;
    [SerializeField] private bool pInputActive;

    private GameObject _itemManager;

    public int lane;
    public int letterCount;

    private bool _isDead = false;
    private bool _isInvincible = false;
    private bool _isBoosted = false;

    private readonly float _animationDuration = 3.0f;
    private float _startTime;

    private float _targetSpeed, _targetBoost;

    private ShieldScript _shieldScript;

    [SerializeField] Animator clawAnimator;

    private float _stunTimer;
    private float _milkStunTime;

    [FormerlySerializedAs("_currency")] [Header("Currency")]
    public int currency;

    private static readonly int IsSpin = Animator.StringToHash("isSpin");
    private static readonly int IsMoving = Animator.StringToHash("isMoving");
    private static readonly int IsDead = Animator.StringToHash("isDead");

    //----------- Start is called before the first frame update
    void Awake()
    {

        //Disable logger if  not debug build
        Debug.unityLogger.logEnabled = Debug.isDebugBuild;
        
        readyTxtGO.SetActive((true));
        goTxtGO.SetActive((false));

        //-------------------
        GRAVITY = 9.8f;
        XSPEED = 10;
        _speedMod = 1;
        health = 3;
        //-------------------
        width = 10;
        lanes = 3;
        lane = 2;
        //-------------------
        ypos = 1.5f;
        zpos = 0;
        xspeed = 0;
        yspeed = 0;
        zspeed = 10;
        laneWidth = width / lanes;
        targetX = (lane - 2) * laneWidth;
        xpos = 0;
        maxAngle = 45;

        boostTime = 5f;
        boostSpeed = 2.5f;

        shieldTime = 10f + PlayerPrefs.GetFloat("ShieldUp");
        magnetTime = 10f + PlayerPrefs.GetFloat("MagnetUp");

        _targetSpeed = 1;

        _startTime = Time.time;

        _shieldScript = GetComponent<ShieldScript>();


        _itemManager = GameObject.FindGameObjectWithTag("itemManager");
        healthImage.GetComponent<Image>().sprite = healthSprites[3];

        _milkStunTime = 1f;
        _stunTimer = -1f;

        pInputActive = false;

        carAnim.SetBool(IsMoving, true);
        // engine.Play();
    }

    //----------- Update is called once per frame
    void Update()
    {

        if (pInputActive)
        {
            //-----------Player Input Buttons
            _left = Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A);
            _right = Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D);

            //-----------Player shouldn't press an input to jump. Should be automatic
            //up = (Input.GetKeyDown(KeyCode.UpArrow));

            //-----------Mobile Touch Input
            _touchDelta = Vector2.zero;

            if (Input.touches.Length > 0)
            {
                switch (Input.touches[0].phase)
                {
                    case TouchPhase.Began:
                        _touchStart = Input.touches[0].position;
                        break;
                    case TouchPhase.Ended:
                    {
                        _touchEnd = Input.touches[0].position;

                        _touchDelta = _touchStart - _touchEnd;

                        if (_touchDelta.x < -50)
                        {
                            _right = true;
                            //engineTurn.Play();
                        }
                        else if (_touchDelta.x > 50)
                            _left = true;
                        //engineTurn.Play();

                        //else if (touch_delta.y < -50)
                        //up = true;
                        break;
                    }
                }
            }
        }

        //-----------When dead, breaks update code, stopping game
        if (_isDead)
            return;

        if (health <= 0)
        {
            Death();
        }

        //-----------controls
        if (_left)
        {
            if (lane > 1)
            {
                lane -= 1;
                targetX = (lane - 2) * laneWidth; ;
            }
        }
        if (_right)
        {
            if (lane < lanes)
            {
                lane += 1;
                targetX = (lane - 2) * laneWidth; ;
            }
        }

        //---------- Speed Up Smooth
        if (_speedMod < _targetSpeed)
        {
            _speedMod += 0.1f;
        }
        if (boost < _targetBoost)
        {
            boost += 0.1f;
        }
        if (boost > _targetBoost)
        {
            boost -= 0.1f;
        }

        //------------movement
        targetAngle = 0;
        if (xpos < targetX)
        {
            xspeed = XSPEED;
            targetAngle = maxAngle;
        }
        if (xpos > targetX)
        {
            xspeed = -XSPEED;
            targetAngle = -maxAngle;
        }
        if (ypos > 1.5)
        {
            yspeed -= GRAVITY * Time.deltaTime;
        }

        if (angle != targetAngle)
        {
            if (angle < targetAngle)
            {
                angle += 3;
            }
            else
            {
                angle -= 3;
            }
        }

        //---------------stopping movemnt+rounding
        xDiff = xpos - targetX;
        if ((xspeed != 0) && ((0.1f * _speedMod) > xDiff && (-0.1f * _speedMod) < xDiff))
        {
            xpos = targetX;
            xspeed = 0f;
        }
        if (ypos < 0)
        {
            ypos = 0;
            yspeed = 0;
        }

        //--------------moving player
        //xpos += Vector3.Lerp(transform.position, targetX, Time.fixedDeltaTime * xspeed * difficulty);
        xpos += xspeed * Time.deltaTime * _speedMod;
        ypos += yspeed * Time.deltaTime * _speedMod;
        zpos += zspeed * Time.deltaTime * _speedMod * boost;

        //----------- Player cannot change X or Y input during animation period
        if (!pInputActive) {
            if (Time.time - _startTime > _animationDuration)
            {
                pInputActive = true;
                
                readyTxtGO.SetActive((false));
                goTxtGO.SetActive((true));
            }
            
        }

        if (Time.time - _startTime > _animationDuration + 1)
        {
            readyTxtGO.SetActive((false));
            goTxtGO.SetActive((false));
        }

        if (_stunTimer != -1)
        {
            if (_stunTimer > 0)
            {
                _stunTimer -= Time.deltaTime;
                pInputActive = false;
            }
            else
            {
                _stunTimer = -1f;
                pInputActive = true;
                angle = -15;
                carAnim.SetBool(IsSpin, false);

                foreach (ParticleSystem wheelDustPS in wheelDust_PS)
                {
                    wheelDustPS.Play();
                }
            }
        }
        else
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, angle + 90, 0));
        }

        if (boostTimer != -1)
        {
            if (boostTimer >= 0)
            {
                boostTimer -= Time.deltaTime;
            }
            else
            {
                boostTimer = -1;
                _targetBoost = 1;

                _isBoosted = false;
                Log("_isBoosted: " + _isBoosted);
            }
        }

        //----------- Player has full input
        transform.position = new Vector3(xpos, ypos, zpos);

    }
    
    //----------- Used for increasing the player speed incrementally from the score script
    public void SetSpeed(float modifier)
    {
        _targetSpeed += modifier;
    }

    //----------- Trigger Collider with tag and switch system
    private void OnTriggerEnter(Collider other)
    {

        _isInvincible = _shieldScript.GetComponent<ShieldScript>().isInvincible || shieldTimer > 0;

        switch (other.gameObject.tag)
        {
            //----------- Pickup Trigger Cases

            //----------- Coin Pickup Case
            case "Coin":
                currency++;

                Destroy(other.gameObject);
                Log("Pickup_Coin " + currency);

                coinPickup.Play();

                break;

            //----------- Damage Trigger Cases
            case "Spike":
                if (_isInvincible == false)
                {
                    health -= 2;
                    HealthText.text = ("Health: " + health).ToString();
                    HealthImageChange();

                    if (Camera.main != null) Camera.main.GetComponent<CameraEffects>().Shake();
                    Log("Hit_Spike");
                    //-----------Play damage audio
                    takeDamage.Play();
                    Vibration.Vibrate(400);
                    StartCoroutine(_shieldScript.Invincibility(4));
                    //StartCoroutine(_shieldScript.Flash(0.5f, 0.1f));
                }

                //-----------When health becomes 0, run Death method
                if (health <= 0)
                {
                    Death();
                }

                break;

            case "BreakWall":
                if (_isInvincible == false)
                {
                    health -= 1;
                    HealthText.text = ("Health: " + health).ToString();
                    HealthImageChange();

                    if (Camera.main != null) Camera.main.GetComponent<CameraEffects>().Shake();
                    Log("Hit_BreakWall");
                    //-----------Play damage audio
                    takeDamage.Play();
                    Vibration.Vibrate(400);
                    //-----------Hide gameobject and create particle effect
                    
                    StartCoroutine(_shieldScript.Invincibility(4));
                }
                Destroy(other.gameObject);
                var o = other.gameObject;
                Instantiate(breakWall_PS, o.transform.position, o.transform.rotation);

                //-----------When health becomes 0, run Death method
                if (health <= 0)
                {
                    Death();
                }

                break;

            case "Milk":
                if (_isInvincible == false)
                {
                    //health -= 1;
                    HealthText.text = ("Health: " + health).ToString();
                    HealthImageChange();

                    if (Camera.main != null) Camera.main.GetComponent<CameraEffects>().Shake();
                    Log("Hit_Milk");
                    Vibration.Vibrate(400);
                    milkSplash.Play();
                    tireScreech.Play();
                    //-----------Take damage and Play damage audio
                    //takeDamage.Play();
                    //StartCoroutine(_shieldScript.Invincibility(4));

                    _stunTimer = _milkStunTime;
                    pInputActive = false;
                    carAnim.SetBool(IsSpin, true);

                    foreach (ParticleSystem wheelDust_ps in wheelDust_PS)
                    {
                        wheelDust_ps.Stop();
                    }
                }

                break;

            case "Shield":
                powerUp.Play();
                //play sound shield
                //draw shield
                if (!_isBoosted)
                {
                    StartCoroutine(_shieldScript.Invincibility(shieldTime));
                }

                Destroy(other.gameObject);
                break;

            case "Health":
                powerUp.Play();
                if (health != 3)
                {
                    health++;
                    HealthImageChange();
                    //playsound healthpickup

                }
                Destroy(other.gameObject);
                break;

            case "Letter":
                powerUp.Play();
                if (letterCount < 5)
                {
                    letterCount++;
                }
                //letterImage.GetComponent<Image>().sprite = letterSprites[letterCount];
                //play sound letterpickup
                Destroy(other.gameObject);
                break;

            case "Magnet":
                _currentItems = _itemManager.GetComponent<ItemManager>().currentItems;
                powerUp.Play();
                foreach (var i in _currentItems.Where(i => i != null).Where(i => i.CompareTag("Coin")))
                {
                    i.GetComponent<coinControl>().magnetTimer = magnetTime;
                }
                Destroy(other.gameObject);
                break;

            //----------- Default Case
            default:
                break;
                
        }

    }

    void HealthImageChange()
    {
        healthImage.GetComponent<Image>().sprite = health != -1 ? healthSprites[health] : healthSprites[0];
    }

    //----------- Death Method
    private void Death()
    {
    //----------- sets health to 0, updates health indicator and marks death boolean to true
    health = 0;
    HealthText.text = ("Health: " + (int)health).ToString();
    _isDead = true;
        carAnim.SetBool(IsMoving, false);
        carAnim.SetBool(IsSpin, false);
        StartCoroutine(ClawAnimationWait());

    foreach (ParticleSystem wheelDust_ps in wheelDust_PS)
    {
        wheelDust_ps.Stop();
    }

        //----------- Grabs score script to send high score data
        GetComponent<Score>().OnDeath();

    //----------- Send to console
    Log("Death");
    }

    public void Boost()
    {
        if (letterCount == 5)
        {
            Vibration.Vibrate(100);
            letterCount = 0;
            boostTimer = boostTime;
            _targetBoost = boostSpeed;
            StartCoroutine(_shieldScript.Invincibility(boostTime + 1));
            _isBoosted = true;
            Log("_isBoosted: " + _isBoosted);
        }
    }

    //-------- Logging Control Method
    private void Log(object message)
    {
        if (showLogs)
            Debug.Log(prefix + message);
    }


    private IEnumerator ClawAnimationWait()
    {
        clawAnimator.SetBool(IsDead, true);
        clawGrab.Play();
        yield return new WaitForSeconds(3);
        clawRelease.Play();
    }
}
