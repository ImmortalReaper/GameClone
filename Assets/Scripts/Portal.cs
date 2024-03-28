using BayatGames.SaveGameFree;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    [SerializeField] string sceneName;

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out PlayerController playerController))
        {
            SceneManager.LoadScene(sceneName);
            SaveGame.Clear();
        }
    }
}
