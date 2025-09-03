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
    [SerializeField] private Image MagnetTimerImage;
    [SerializeField] private float MagnetTimer = 10f;
    private int CurrentHealth;
    private float InvincibilityTimer;
    public bool IsInvicible;
    public bool IsMagnetOn { get; private set; }

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
        IsMagnetOn = false;
        MagnetTimerImage.transform.parent.gameObject.SetActive(false);
        MagnetTimerImage.fillAmount = 1;
    }

    public void ActivateMagnet()
    {
        MagnetTimerImage.transform.parent.gameObject.SetActive(true);
        IsMagnetOn = true;
        StartCoroutine(MagnetCoroutine());
    }
    private IEnumerator MagnetCoroutine()
    {
        float timer = 0f;
        while (timer < MagnetTimer)
        {
            timer += Time.deltaTime;
            MagnetTimerImage.fillAmount = 1f - (timer / MagnetTimer);
            yield return null;
        }
        IsMagnetOn = false;
        MagnetTimerImage.transform.parent.gameObject.SetActive(false);
        MagnetTimerImage.fillAmount = 1;
    }

    public void PlayerDamaged()
    {
        StartCoroutine(IsHittenCoroutine());
    }

    private IEnumerator IsHittenCoroutine()
    {
        transform.Find("Visual/Character/Body/Head").GetComponent<Renderer>().material = DamagedMaterial;
        IsInvicible = true;
        // Debug.Log("IsInvincible for 4 seconds !");
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
