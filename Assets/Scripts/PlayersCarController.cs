using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.Animations;
using UnityEditor.ShaderGraph;
using TMPro;
using System.Collections;

public class PlayersCarController : MonoBehaviour
{
    public Renderer iceCubeRenderer;
    public CanvasGroup freezeCanvasGroup;
    private PlayerControls controls;
    private Rigidbody carRb;

    private bool _isFrozen = false;
    [HideInInspector]
    public bool isFrozen
    {
        get => _isFrozen;
        set
        {
            _isFrozen = value;
            // When frozen, input is disabled and full brake applied in Brake()
            // No rigidbody kinematic or velocity changes here to avoid physics glitches
        }
    }

    private void Awake()
    {
        controls = new PlayerControls();
        carRb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        if (controls == null)
            controls = new PlayerControls();
        controls.Enable();
    }

    private void OnDisable()
    {
        if (controls != null)
            controls.Disable();
    }

    public enum Axel { Front, Rear }

    [Serializable]
    public struct Wheel
    {
        public GameObject wheelModel;
        public WheelCollider wheelCollider;
        public GameObject wheelEffectObj;
        public Axel axel;
    }

    public int playerNum = 1;
    public float health = 100;
    private float maxHealth;

    public float maxAcceleration = 30.0f;
    public float brakeAcceleration = 50.0f;
    public float maxVelocity = 30f;

    public float maxSpeed = 100f;               // Current max speed, adjustable by freeze script
    [HideInInspector]
    public float defaultMaxSpeed = 100f;         // Store default max speed for restore

    private bool isSpeedBoostActive = false;
    private float savedAcceleration;
    private bool isInSlowZone = false;

    public float turnSensitivity = 1.0f;
    public float maxSteerAngle = 30.0f;

    public TextMeshProUGUI healthText;
    public GameObject abilityManager;
    public int abilityCooldownMax;
    private int abilityCooldownCurrent = 0;
    public bool hasAbility = true;
    public List<Wheel> wheels;

    private float lastTeleportTime = -10f;
    private float teleportCooldown = 1.5f;

    private float moveInput;
    private float steerInput;
    private float brakeInput;
    private float abilityInput;

    private Vector3 _centerOfMass;
    private Vector3 playerVelocity;

    void Start()
    {
        carRb.centerOfMass = _centerOfMass;
        maxHealth = health;
        healthText.color = Color.green;
        defaultMaxSpeed = maxSpeed;  // Save initial max speed
    }

    void Update()
    {
        if (health > 0 && !isFrozen)
        {
            GetInputs();
            AnimateWheels();
            WheelEffects();
            playerVelocity = carRb.velocity;
        }
    }

    void LateUpdate()
    {
        if (health > 0)
        {
            Move();
            Steer();
            Brake();
            Ability();
            CheckHealth();
            ClampMaxSpeed();
        }
    }

    IEnumerator CooldownTimer()
    {
        while (abilityCooldownCurrent > 0)
        {
            yield return new WaitForSeconds(1);
            abilityCooldownCurrent -= 1;
        }
    }

    void GetInputs()
    {
        if (isFrozen)
        {
            moveInput = 0f;
            steerInput = 0f;
            brakeInput = 1f; // full brake while frozen
            abilityInput = 0f;
            return;
        }

        if (playerNum == 1)
        {
            moveInput = controls.Player1.Accelerate.ReadValue<float>();
            steerInput = controls.Player1.Steer.ReadValue<float>();
            brakeInput = controls.Player1.Brake.ReadValue<float>();
            abilityInput = controls.Player1.Ability.ReadValue<float>();
        }
        else if (playerNum == 2)
        {
            moveInput = controls.Player2.Accelerate.ReadValue<float>();
            steerInput = controls.Player2.Steer.ReadValue<float>();
            brakeInput = controls.Player2.Brake.ReadValue<float>();
            abilityInput = controls.Player2.Ability.ReadValue<float>();
        }
    }

    void CheckHealth()
    {
        if (health <= 0)
        {
            health = 0;
            healthText.color = Color.red;
            AudioManager.instance.PlaySFX("CarExplode");
        }
        else if (health < 30)
        {
            healthText.color = Color.red;
        }
        else if (health < 65)
        {
            healthText.color = Color.yellow;
        }
        else
        {
            healthText.color = Color.green;
        }
        healthText.text = health.ToString();
    }

    void Move()
    {
        if (isFrozen) return;

        float torque = moveInput * 600f * maxAcceleration * Time.deltaTime;

        // Prevent exceeding maxSpeed
        if (carRb.velocity.magnitude >= maxSpeed) return;

        foreach (var wheel in wheels)
        {
            wheel.wheelCollider.motorTorque = torque;
        }
    }

    void Steer()
    {
        if (isFrozen) return;

        foreach (var wheel in wheels)
        {
            if (wheel.axel == Axel.Front)
            {
                var steerAngle = steerInput * turnSensitivity * maxSteerAngle;
                wheel.wheelCollider.steerAngle = Mathf.Lerp(wheel.wheelCollider.steerAngle, steerAngle, 0.6f);
            }
        }
    }

    void Brake()
    {
        if (isFrozen)
        {
            // Apply full brake torque to lock wheels while frozen
            foreach (var wheel in wheels)
            {
                wheel.wheelCollider.motorTorque = 0f;
                wheel.wheelCollider.brakeTorque = Mathf.Infinity;
            }
            return;
        }

        if (brakeInput != 0 || moveInput == 0)
        {
            foreach (var wheel in wheels)
            {
                wheel.wheelCollider.brakeTorque = 300 * brakeAcceleration * Time.deltaTime;
            }
        }
        else
        {
            foreach (var wheel in wheels)
            {
                wheel.wheelCollider.brakeTorque = 0;
            }
        }
    }

    void Ability()
    {
        if (isFrozen) return;

        if (abilityInput != 0 && playerNum == 1 && abilityCooldownCurrent == 0 && hasAbility)
        {
            abilityManager.SendMessage("ActivateAbility", 2);
            abilityCooldownCurrent = abilityCooldownMax;
            StartCoroutine(CooldownTimer());
        }
        if (abilityInput != 0 && playerNum == 2 && abilityCooldownCurrent == 0 && hasAbility)
        {
            abilityManager.SendMessage("ActivateAbility", 1);
            abilityCooldownCurrent = abilityCooldownMax;
            StartCoroutine(CooldownTimer());
        }
    }

    void AnimateWheels()
    {
        foreach (var wheel in wheels)
        {
            Quaternion rot;
            Vector3 pos;
            wheel.wheelCollider.GetWorldPose(out pos, out rot);
            wheel.wheelModel.transform.position = pos;
            wheel.wheelModel.transform.rotation = rot;
        }
    }

    void WheelEffects()
    {
        foreach (var wheel in wheels)
        {
            if (brakeInput != 0 && wheel.wheelCollider.isGrounded && carRb.velocity.magnitude >= 2.0f)
            {
                wheel.wheelEffectObj.GetComponentInChildren<TrailRenderer>().emitting = true;
            }
            else
            {
                wheel.wheelEffectObj.GetComponentInChildren<TrailRenderer>().emitting = false;
            }
        }
    }

    void ClampMaxSpeed()
    {
        if (!isSpeedBoostActive && carRb.velocity.magnitude > maxVelocity)
        {
            carRb.velocity = carRb.velocity.normalized * maxVelocity;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((playerNum == 1 && other.CompareTag("player2Colliders")) || (playerNum == 2 && other.CompareTag("player1Colliders")))
        {
            Rigidbody otherRb = other.attachedRigidbody;
            if (otherRb == null) return;

            Vector3 contactNormal = (other.transform.position - transform.position).normalized;
            float impactAlignment = Vector3.Dot(playerVelocity.normalized, contactNormal);

            if (impactAlignment > 0.5f)
            {
                float relativeSpeed = playerVelocity.magnitude;
                if (otherRb.velocity.magnitude >= 0)
                {
                    relativeSpeed = (playerVelocity - otherRb.velocity).magnitude;
                }
                int damageDealt = Mathf.CeilToInt(relativeSpeed);

                var enemyCar = other.GetComponentInParent<PlayersCarController>();
                if (enemyCar != null)
                {
                    enemyCar.health -= damageDealt;
                    AudioManager.instance.PlaySFX("CarDamage");
                    enemyCar.SendMessage("CheckHealth");
                }
            }
        }
    }

    private bool healingStatus = false;

    public void HealingOverTime(bool isHealing)
    {
        healingStatus = isHealing;
        if (healingStatus)
        {
            StartCoroutine(HealPerSecond());
        }
        else
        {
            StopCoroutine(HealPerSecond());
        }
    }

    public IEnumerator HealPerSecond()
    {
        while (healingStatus)
        {
            if (health > 0)
            {
                health += 1;
            }
            if (health > maxHealth)
            {
                health = maxHealth;
            }
            yield return new WaitForSeconds(0.2f);
        }
    }

    public void ApplyInstantSpeedBoost(float boostSpeed, float duration = 2f)
    {
        StopCoroutine("SpeedBoostCoroutine");
        StartCoroutine(SpeedBoostCoroutine(boostSpeed, duration));
    }

    private IEnumerator SpeedBoostCoroutine(float boostSpeed, float duration)
    {
        isSpeedBoostActive = true;
        Vector3 forwardVelocity = transform.forward * boostSpeed;
        carRb.velocity = new Vector3(forwardVelocity.x, carRb.velocity.y, forwardVelocity.z);
        yield return new WaitForSeconds(duration);
        isSpeedBoostActive = false;
    }

    public void EnterSlowZone(float targetSpeed, float slowAccel)
    {
        if (isInSlowZone) return;

        isInSlowZone = true;
        savedAcceleration = maxAcceleration;
        maxAcceleration = slowAccel;

        Vector3 horizontalDir = new Vector3(carRb.velocity.x, 0f, carRb.velocity.z).normalized;
        carRb.velocity = horizontalDir * targetSpeed + Vector3.up * carRb.velocity.y;
    }

    public void ExitSlowZone()
    {
        if (!isInSlowZone) return;

        maxAcceleration = savedAcceleration;
        isInSlowZone = false;
    }

    public bool CanTeleport()
    {
        return Time.time - lastTeleportTime >= teleportCooldown;
    }

    public void RegisterTeleport()
    {
        lastTeleportTime = Time.time;
    }
}