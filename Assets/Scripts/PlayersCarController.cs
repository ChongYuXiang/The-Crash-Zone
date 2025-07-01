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
    private PlayerControls controls;

    // Set up control detection
    private void Awake()
    {
        controls = new PlayerControls();
    }
    private void OnEnable()
    {
        controls.Enable();
    }
    private void OnDisable()
    {
        controls.Disable();
    }

    // Wheel classes
    public enum Axel
    {
        Front,
        Rear
    }
    [Serializable]
    public struct Wheel
    {
        public GameObject wheelModel;
        public WheelCollider wheelCollider;
        public GameObject wheelEffectObj;
        public ParticleSystem smokeParticle;
        public Axel axel;
    }

    // Public Variables
    public int playerNum = 1;
    public float health = 100;
    private float maxHealth;

    public float maxAcceleration = 30.0f;
    public float boostAcceleration = 60.0f;
    public float brakeAcceleration = 50.0f;
    public float maxVelocity = 10f;


    public float turnSensitivity = 1.0f;
    public float maxSteerAngle = 30.0f;

    public TextMeshProUGUI healthText;
    public GameObject abilityManager;
    public int abilityCooldownMax;
    private int abilityCooldownCurrent = 0;
    public List<Wheel> wheels;

    // Inputs
    private float moveInput;
    private float steerInput;
    private float brakeInput;
    private float abilityInput;

    private Rigidbody carRb;
    private Vector3 _centerOfMass;
    private Transform target = null;
    private Vector3 toTarget;


    void Start()
    {
        carRb = GetComponent<Rigidbody>();
        carRb.centerOfMass = _centerOfMass;
        maxHealth = health;
        healthText.color = Color.green;
    }

    // Called at the START of each frame
    void Update()
    {
        // Retrieve inputs and update vfx/animation
        GetInputs();
        AnimateWheels();
        WheelEffects();
    }

    // Called at the END of each frame
    void LateUpdate()
    {
        // Car movement
        Move();
        Steer();
        Brake();
        Ability();
        // Get velocity towards enemy player
        if (target != null)
        {
            toTarget = (target.position - transform.position).normalized;
        }
        // Health check
        CheckHealth();

        // Speed check
        ClampMaxSpeed();
    }

    IEnumerator CooldownTimer()
    {
        while (abilityCooldownCurrent > 0)
        {
            yield return new WaitForSeconds(1); // Wait 1 second
            abilityCooldownCurrent -= 1;
            Debug.Log(abilityCooldownCurrent);
        }
        yield return new WaitForEndOfFrame();
    }

    void GetInputs() // Read inputs
    {
        GameObject targetObj = null;

        // Check if the car is controlled by player 1 or player 2 to read the correct inputs
        if (playerNum == 1) // Player 1
        {
            moveInput = controls.Player1.Accelerate.ReadValue<float>();
            steerInput = controls.Player1.Steer.ReadValue<float>();
            brakeInput = controls.Player1.Brake.ReadValue<float>();
            abilityInput = controls.Player1.Ability.ReadValue<float>();
            targetObj = GameObject.Find("player2Target");
        }
        else if (playerNum == 2) // Player 2
        {
            moveInput = controls.Player2.Accelerate.ReadValue<float>();
            steerInput = controls.Player2.Steer.ReadValue<float>();
            brakeInput = controls.Player2.Brake.ReadValue<float>();
            abilityInput = controls.Player2.Ability.ReadValue<float>();
            targetObj = GameObject.Find("player1Target");
        }

        if (targetObj != null)
        {
            target = targetObj.transform;
        }
        else
        {
            target = null;
        }
    }

    void CheckHealth()
    {
        if (health <= 0)
        {
            health = 0;
            healthText.color = Color.red;
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

    void Move() // Forward and backward movement and boosting with ability key
    {
        float torque = moveInput * 600f * maxAcceleration * Time.deltaTime;

        foreach (var wheel in wheels) // Apply the calculated torque to each drive wheel
        {
            wheel.wheelCollider.motorTorque = torque;
        }
    }

    void Steer() // Left and right steering
    {
        foreach (var wheel in wheels) // For each wheel,
        {
            if (wheel.axel == Axel.Front) // If wheel is front wheel,
            {
                // Turn wheel
                var _steerAngle = steerInput * turnSensitivity * maxSteerAngle;
                wheel.wheelCollider.steerAngle = Mathf.Lerp(wheel.wheelCollider.steerAngle, _steerAngle, 0.6f);
            }
        }
    }

    void Brake() // Stop vehicle movement
    {
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
        if (abilityInput != 0 && playerNum == 1 && abilityCooldownCurrent == 0)
        {
            abilityManager.SendMessage("ActivateAbility", 2);
            abilityCooldownCurrent = abilityCooldownMax;
            StartCoroutine(CooldownTimer());
        }
        if (abilityInput != 0 && playerNum == 2 && abilityCooldownCurrent == 0)
        {
            abilityManager.SendMessage("ActivateAbility", 1);
            abilityCooldownCurrent = abilityCooldownMax;
            StartCoroutine(CooldownTimer());
        }
    }

    void AnimateWheels() // Spin and rotate wheels
    {
        foreach (var wheel in wheels) // For each wheel,
        {
            // Get rotation of wheel colliders
            Quaternion rot;
            Vector3 pos;
            wheel.wheelCollider.GetWorldPose(out pos, out rot);

            // Match transform of colliders with mesh
            wheel.wheelModel.transform.position = pos;
            wheel.wheelModel.transform.rotation = rot;
        }
    }


    void WheelEffects() // VFX for wheels when braking
    {
        foreach (var wheel in wheels) // For each wheel,
        {
            // Check if braking, if car in grounded, and if the car is still moving
            if (brakeInput != 0 && wheel.axel == Axel.Rear && wheel.wheelCollider.isGrounded == true && carRb.velocity.magnitude >= 2.0f)
            {
                // Enable effects
                wheel.wheelEffectObj.GetComponentInChildren<TrailRenderer>().emitting = true;
                //wheel.smokeParticle.Emit(1);
            }
            else
            {
                // Disable effects
                wheel.wheelEffectObj.GetComponentInChildren<TrailRenderer>().emitting = false;
            } 
        }
    }

    void ClampMaxSpeed()
    {
        if (carRb.velocity.magnitude > maxVelocity)
        {
            carRb.velocity = carRb.velocity.normalized * maxVelocity;
        }
    }

    // Collision of players
    private void OnTriggerEnter(Collider other)
    {
        if ((playerNum == 1 && other.CompareTag("player2Colliders")) || (playerNum == 2 && other.CompareTag("player1Colliders")))
        {

            Rigidbody otherRb = other.attachedRigidbody;
            if (otherRb == null) return;

            // Your direction of movement
            Vector3 myVelocity = carRb.velocity;
            Vector3 contactNormal = (other.transform.position - transform.position).normalized;

            // Check if you are moving INTO the other player
            float impactAlignment = Vector3.Dot(myVelocity.normalized, contactNormal);

            // if player is moving towards the other car
            if (impactAlignment > 0.5f)
            {
                float relativeSpeed = myVelocity.magnitude;
                if (otherRb.velocity.magnitude >= 0)
                {
                    relativeSpeed = (myVelocity - otherRb.velocity).magnitude;
                }
                int damageDealt = Mathf.CeilToInt(relativeSpeed) * 3;

                // Deal damage to enemy
                var enemyCar = other.GetComponentInParent<PlayersCarController>();
                if (enemyCar != null)
                {
                    enemyCar.health -= damageDealt;
                    Debug.Log($"[Player {playerNum}] dealt {damageDealt} from {damageDealt/3}. Relative Speed: {relativeSpeed}. Enemy Health: {enemyCar.health}");
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
        while (healingStatus == true)
        {
            if (health > 0)
            {
                health += 1;
            }
            if (health > maxHealth)
            {
                health = maxHealth;
            }
            yield return new WaitForSeconds(0.2f); // Wait seconds
        }
        yield return new WaitForEndOfFrame();
    }

    // Jack's thingy   
    public void ApplyInstantSpeedBoost(float boostSpeed)
    {
        Vector3 forwardVelocity = transform.forward * boostSpeed;
        carRb.velocity = new Vector3(forwardVelocity.x, carRb.velocity.y, forwardVelocity.z);
        Debug.Log("Speed boost applied!");
    }

}