using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Dialog : MonoBehaviour
{
    static Dialog _instance;
    public static Dialog Instance {
        get {
            if (_instance == null)
            {
                _instance = FindFirstObjectByType<Dialog>(FindObjectsInactive.Include);
            }
            return _instance;
        }
    }

    [SerializeField] Movement playerMovement;
    PlayerInput playerInput;
    InputAction inputAction;
    
    [SerializeField] TextMeshProUGUI textMeshProUGUI;
    [SerializeField] CanvasGroup canvasGroup;

    float openTweenTime = 0.5f;

    List<string> phrases;
    int currentPhraseId;

    public bool DialogInProgress;


    void Awake()
    {
        if (playerInput == null)
        {
            Debug.LogWarning("You should assign PlayerInput! Using .Find() for now.");
            playerInput = GameObject.Find("PlayerInput").GetComponent<PlayerInput>();
        }

        inputAction = playerInput.actions.FindAction("Jump");

        canvasGroup.alpha = 0f;
    }

    public void Open(List<string> newPhrases)
    {
        if (newPhrases == null) return;
        if (newPhrases.Count == 0) return;
        playerMovement.enabled = false;

        currentPhraseId = 0;
        phrases = newPhrases;
        textMeshProUGUI.text = phrases[currentPhraseId];
        gameObject.SetActive(true);
        canvasGroup.DOFade(1f, openTweenTime).OnComplete(() => 
        {
            DialogInProgress = true;
        });
    }

    public void Close()
    {
        canvasGroup.DOFade(0f, openTweenTime).OnComplete(() => 
        {
            DialogInProgress = false;
            gameObject.SetActive(false);
            playerMovement.enabled = true;
        });
    }

    void Update()
    {
        if (DialogInProgress)
        if (inputAction.WasPressedThisFrame())
        {
            currentPhraseId++;
            if (currentPhraseId <= phrases.Count - 1)
            {
                textMeshProUGUI.text = phrases[currentPhraseId];
            }
            else
            {
                Close();
            }
        }
    }
}
