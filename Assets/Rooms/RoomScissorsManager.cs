using System.Collections.Generic;
using DG.Tweening;
using HPhysic;
using UnityEngine;

public class RoomScissorsManager : MonoBehaviour
{
    [SerializeField] ButtonPhysical scissorsButtonPhysical;
    [SerializeField] Movement playerMovement;
    [SerializeField] CableController cableController;

    [SerializeField] Transform cutscenePlayerPosT;
    [SerializeField] ForcedConnect cutsceneCableForcedConnect;
    [SerializeField] Transform cutsceneCameraPosT;

    [SerializeField] Animation scissorsAnimator;

    [SerializeField] GameObject playerCameraGo;
    [SerializeField] GameObject cutsceneCameraGo;

    [SerializeField] SlidingDoor finishSlidingDoor;

    [SerializeField] GameObject gameoverTrigger;
    [SerializeField] GameObject gameoverGlow;

    private Sequence cutsceneSequence;


    void Start()
    {
        LeverManager.Instance.OnCompleteChanged.AddListener(LeverManagerChanged);
        scissorsButtonPhysical.OnClick.AddListener(ScissorsButtonPressed);
    }

    void LeverManagerChanged(bool isComplete)
    {
        scissorsButtonPhysical.SetInteractable(isComplete);
    }

    [ContextMenu("ScissorsButtonPressed")]
    void ScissorsButtonPressed()
    {
        // Kill any existing sequence to prevent conflicts
        cutsceneSequence?.Kill();
        
        playerMovement.enabled = false;
        cableController.enabled = false;
        
        cutsceneSequence = DOTween.Sequence();
        
        cutsceneSequence.AppendCallback(() => 
        {
            playerCameraGo.SetActive(false);
            cutsceneCameraGo.SetActive(true);
        });
        
        cutsceneSequence.AppendInterval(1.5f); // Wait 0.5 seconds
        
        cutsceneSequence.AppendCallback(() => 
        {
            cableController.DisconnectOptional();
        });
        
        cutsceneSequence.Append(playerMovement.transform.DOMove(cutscenePlayerPosT.position, 1f));
        
        cutsceneSequence.AppendInterval(1f); // Wait after movement
        
        cutsceneSequence.AppendCallback(() => 
        {
            cableController.ResetPoints();
            cutsceneCableForcedConnect.ForceConnectCables();
        });

        cutsceneSequence.AppendInterval(1f);

        cutsceneSequence.AppendCallback(() => 
        {
            int extraPointCount = cableController.physicCable.points.Count - cableController.minSegmentCount;
            for (int i = 0; i < extraPointCount; i++)
            {
                cableController.physicCable.RemovePoint();
            }
        });
        
        cutsceneSequence.AppendInterval(1f);
        
        cutsceneSequence.AppendCallback(() => 
        {
            scissorsAnimator.Play();
        });
        
        cutsceneSequence.AppendInterval(2f); // Wait for animation
        
        cutsceneSequence.AppendCallback(() => 
        {
            cableController.DisconnectAll();
            cutsceneCableForcedConnect.ForceDisconnectCables();
        });

        cutsceneSequence.AppendCallback(() => 
        {
            playerCameraGo.SetActive(true);
            cutsceneCameraGo.SetActive(false);
            playerMovement.enabled = true;
            finishSlidingDoor.Open();
            gameoverTrigger.SetActive(true);
            gameoverGlow.SetActive(true);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        });
    }

    public void ReleaseMouse()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}