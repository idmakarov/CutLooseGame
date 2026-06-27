using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Dialog : MonoBehaviour
{
    static Dialog _instance;
    public static Dialog Instance
    {
        get
        {
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
    float tutorialFadeTime = 0.35f;

    List<string> phrases;
    int currentPhraseId;

    public bool DialogInProgress;

    GameObject showAfterPhrases;
    bool showingFinalObject;

    bool allowTutorial;

    public Image tutorialImage;

    void Awake()
    {
        if (playerInput == null)
        {
            Debug.LogWarning("You should assign PlayerInput! Using .Find() for now.");
            playerInput = GameObject.Find("PlayerInput").GetComponent<PlayerInput>();
        }

        inputAction = playerInput.actions.FindAction("Jump");

        canvasGroup.alpha = 0f;

        if (tutorialImage != null)
        {
            var c = tutorialImage.color;
            c.a = 0f;
            tutorialImage.color = c;
        }
    }

    public void Open(List<string> newPhrases, bool showTutorial = true)
    {
        if (newPhrases == null || newPhrases.Count == 0)
            return;

        allowTutorial = showTutorial;

        showAfterPhrases = allowTutorial ? tutorialImage?.gameObject : null;
        showingFinalObject = false;

        if (showAfterPhrases != null)
            showAfterPhrases.SetActive(false);

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
        if (tutorialImage != null && showingFinalObject)
        {
            tutorialImage.DOFade(0f, tutorialFadeTime);
        }

        canvasGroup.DOFade(0f, openTweenTime).OnComplete(() =>
        {
            DialogInProgress = false;

            if (showAfterPhrases != null)
            {
                showAfterPhrases.SetActive(false);
                showAfterPhrases = null;
            }

            showingFinalObject = false;

            gameObject.SetActive(false);
            playerMovement.enabled = true;
        });
    }

    void Update()
    {
        if (!DialogInProgress)
            return;

        if (!inputAction.WasPressedThisFrame())
            return;

        if (showingFinalObject)
        {
            if (allowTutorial && showAfterPhrases != null)
            {
                canvasGroup.DOFade(0f, tutorialFadeTime);
                showAfterPhrases.SetActive(true);
                tutorialImage.DOFade(1f, tutorialFadeTime);
            }

            Close();
            return;
        }

        currentPhraseId++;

        if (currentPhraseId < phrases.Count)
        {
            textMeshProUGUI.text = phrases[currentPhraseId];
        }
        else
        {
            if (allowTutorial && showAfterPhrases != null)
            {
                showAfterPhrases.SetActive(true);
                showingFinalObject = true;

                canvasGroup.DOFade(0f, tutorialFadeTime);
                tutorialImage.DOFade(1f, tutorialFadeTime);
            }
            else
            {
                Close();
            }
        }
    }
}