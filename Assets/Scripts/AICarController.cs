using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Collections;

public class AICarController : MonoBehaviour
{
    public Transform target; // Player car transform
    public float followDistance = 10f; // Distance to maintain from player
    public float turnSensitivity = 1.0f;

    public int aiNum = 2; // For identifying in game manager
    public List<Wheel> wheels;
    private Rigidbody carRb;

    public float health = 100;
    private float maxHealth;
    public float maxAcceleration = 30.0f;
    public float maxVelocity = 30f;
    public float maxSpeed = 100f;
    [HideInInspector]
    public float defaultMaxSpeed = 100f;         // Store default max speed for restore
    public Image healthbar;
    public Renderer iceCubeRenderer;
    public CanvasGroup freezeCanvasGroup;

    public bool isFrozen = false;
    private bool isSpeedBoostActive = false;
    private bool isInSlowZone = false;
    private float savedAcceleration;

    private float moveInput;
    private float steerInput;
    private Vector3 _centerOfMass;
    private Vector3 aiVelocity;

    [Serializable]
    public enum Axel { Front, Rear }

    [Serializable]
    public struct Wheel
    {
        public GameObject wheelModel;
        public WheelCollider wheelCollider;
        public Axel axel;
    }

    private void Awake()
    {
        carRb = GetComponent<Rigidbody>();
        maxHealth = health;
    }

    private void Start()
    {
        carRb.centerOfMass = _centerOfMass;
        target = GameObject.Find("player1Target").transform;
    }

    private void Update()
    {
        if (health > 0 && !isFrozen)
        {
            CalculateAIInputs();
            AnimateWheels();
            aiVelocity = carRb.velocity;
        }
    }

    private void LateUpdate()
    {
        if (health > 0)
        {
            Move();
            Steer();
            ClampMaxSpeed();
        }

        if (isFrozen || GameManager.instance.gameOver || health <= 0)
        {
            ForceStopCar();
        }
        else
        {
            ReleaseBrakes();
        }
    }

    private void CalculateAIInputs()
    {
        if (target == null) return;

        Vector3 localTarget = transform.InverseTransformPoint(target.position);

        bool targetBehind = localTarget.z < 0f;

        // Always steer toward the target position in local space
        steerInput = Mathf.Clamp(localTarget.x / 5f, -1f, 1f) * turnSensitivity;

        if (targetBehind)
        {
            // If target is behind, keep moving forward to swing around
            moveInput = 1f;
        }
        else
        {
            // Forward if far, reverse if too close
            if (localTarget.z > followDistance) moveInput = 1f;
            else if (localTarget.z < followDistance * 0.5f) moveInput = -0.5f;
            else moveInput = 0f;
        }
    }

    private void Move()
    {
        if (isFrozen) return;

        float torque = moveInput * 600f * maxAcceleration * Time.deltaTime;

        if (carRb.velocity.magnitude >= maxSpeed) return;

        foreach (var wheel in wheels)
        {
            wheel.wheelCollider.motorTorque = torque;
        }
    }

    private void Steer()
    {
        if (isFrozen) return;

        foreach (var wheel in wheels)
        {
            if (wheel.axel == Axel.Front)
            {
                var steerAngle = steerInput * 30f;
                wheel.wheelCollider.steerAngle = Mathf.Lerp(wheel.wheelCollider.steerAngle, steerAngle, 0.6f);
            }
        }
    }

    private void ForceStopCar()
    {
        foreach (var wheel in wheels)
        {
            wheel.wheelCollider.motorTorque = 0f;
            wheel.wheelCollider.brakeTorque = Mathf.Infinity;
        }
    }
    private void ReleaseBrakes()
    {
        foreach (var wheel in wheels)
        {
            wheel.wheelCollider.brakeTorque = 0f;
        }
    }

    private void AnimateWheels()
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

    private void ClampMaxSpeed()
    {
        if (!isSpeedBoostActive && carRb.velocity.magnitude > maxVelocity)
        {
            carRb.velocity = carRb.velocity.normalized * maxVelocity;
        }
    }

    public void CheckHealth()
    {
        if (health <= 0 && !GameManager.instance.gameOver)
        {
            health = 0;
            AudioManager.instance.PlaySFX("CarExplode");
            GameManager.instance.PlayerWins(aiNum);
            GameManager.instance.StartCoroutine(GameManager.instance.playerVFX(aiNum, "explosion"));
        }
        healthbar.fillAmount = (float)health / maxHealth;
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((aiNum == 2 && other.CompareTag("player1Colliders")) || (aiNum == 1 && other.CompareTag("player2Colliders")))
        {
            Rigidbody otherRb = other.attachedRigidbody;
            if (otherRb == null) return;

            Vector3 contactNormal = (other.transform.position - transform.position).normalized;
            float impactAlignment = Vector3.Dot(aiVelocity.normalized, contactNormal);

            if (impactAlignment > 0.5f)
            {
                float relativeSpeed = aiVelocity.magnitude;
                if (otherRb.velocity.magnitude >= 0)
                {
                    relativeSpeed = (aiVelocity - otherRb.velocity).magnitude;
                }
                int damageDealt = Mathf.CeilToInt(relativeSpeed);

                var enemyCar = other.GetComponentInParent<PlayersCarController>();
                if (enemyCar == null) enemyCar = other.GetComponentInParent<PlayersCarController>(); // also damage player
                if (enemyCar != null)
                {
                    enemyCar.health -= damageDealt;
                    AudioManager.instance.PlaySFX("CarDamage");
                    enemyCar.CheckHealth();
                    GameManager.instance.StartCoroutine(GameManager.instance.playerVFX(enemyCar.playerNum, "sparks"));
                }
            }
        }
    }

    public void ApplyInstantSpeedBoost(float boostSpeed, float duration = 2f)
    {
        StopCoroutine("SpeedBoostCoroutine");
        StartCoroutine(SpeedBoostCoroutine(boostSpeed, duration));
    }

    private System.Collections.IEnumerator SpeedBoostCoroutine(float boostSpeed, float duration)
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
            CheckHealth();
        }
    }
}