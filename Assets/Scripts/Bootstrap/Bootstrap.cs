using CustomInspector;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Bootstrap : MonoBehaviour
{
    [SerializeField, Scene] private int gameScene;
    [SerializeField] private BootstrapData bootstrapData;

    private void Awake()
    {
        QualitySettings.vSyncCount = bootstrapData.vSyncCount;

        LoadGameScene().Forget();
    }

    private async UniTask LoadGameScene()
    {
        //loadingWidget.EnableWidget();

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(gameScene);
        asyncLoad.allowSceneActivation = false;

        while (!asyncLoad.isDone)
        {
            await UniTask.Yield();
        }

        //loadingWidget.DisableWidget();
        Debug.Log("—цена загружена.");

        asyncLoad.allowSceneActivation = true;
    }
}
