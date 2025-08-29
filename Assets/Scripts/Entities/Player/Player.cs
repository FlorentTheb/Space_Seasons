using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public static Player Instance;
    [SerializeField] private float InvincibilityTreshold = 3f;
    [SerializeField] private Material HealthyMaterial;
    [SerializeField] private Material DamagedMaterial;
    private int CurrentHealth;
    private float InvincibilityTimer;
    public bool IsInvicible;
    public bool IsMagnetOn { get; private set; }
    private float MagnetTimer = 10f;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        InvincibilityTimer = 0f;
        IsInvicible = false;
    }

    public void ActivateMagnet()
    {
        StartCoroutine(MagnetCoroutine());
    }
    private IEnumerator MagnetCoroutine()
    {
        IsMagnetOn = true;
        yield return new WaitForSeconds(MagnetTimer);
        IsMagnetOn = false;
    }

    public void PlayerDamaged()
    {
        StartCoroutine(IsHittenCoroutine());
    }

    private IEnumerator IsHittenCoroutine()
    {
        if (transform.Find("Visual/Character/Body/Head").GetComponent<Renderer>().material != null)
            Debug.Log("Render child found !");
        else
            Debug.Log("Render child NOT found !");
        transform.Find("Visual/Character/Body/Head").GetComponent<Renderer>().material = DamagedMaterial;
        IsInvicible = true;
        Debug.Log("IsInvincible for 4 seconds !");
        yield return new WaitForSeconds(4);
        transform.Find("Visual/Character/Body/Head").GetComponent<Renderer>().material = HealthyMaterial;
        IsInvicible = false;
    }

    void Update()
    {
        if (IsInvicible)
        {
            InvincibilityTimer += Time.deltaTime;
            if (InvincibilityTimer >= InvincibilityTreshold)
            {
                IsInvicible = false;
                InvincibilityTimer = 0f;
            }
        }
    }
}
