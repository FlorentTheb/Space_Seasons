using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public static Player Instance;
    [SerializeField] private Material HealthyMaterial;
    [SerializeField] private Material DamagedMaterial;
    [SerializeField] private Image MagnetTimerImage;
    [SerializeField] private float MagnetTimer = 10f;
    [SerializeField] private float InvincibilityTimer = 3f;
    private Animator ControlAnimator;
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
        ControlAnimator = GetComponent<Animator>();
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
        ControlAnimator.SetBool("IsHit", true);
        IsInvicible = true;
        yield return new WaitForSeconds(InvincibilityTimer);
        IsInvicible = false;
    }
}
