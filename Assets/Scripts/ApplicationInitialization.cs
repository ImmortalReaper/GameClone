using BayatGames.SaveGameFree;
using DG.Tweening;
using UnityEngine;

public class ApplicationInitialization : MonoBehaviour
{
    [SerializeField] int targetFramerate = 60;
    [SerializeField] int tweenersCapacity = 500;
    [SerializeField] int sequencesCapacity = 125;

    [ContextMenu("Delete Save Data")]
    void DeleteSaveData() => SaveGame.Clear(); 
    
    void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = targetFramerate;
        DOTween.SetTweensCapacity(tweenersCapacity, sequencesCapacity);
    }
}
