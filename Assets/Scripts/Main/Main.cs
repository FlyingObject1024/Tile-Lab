using UnityEngine;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{
    public Change_scene manager;

    string stage_name = string.Empty;

    /*
    [SerializeField] private PlayerController m_PlayerController = null;
    [SerializeField] private AudioManager m_AudioManager = null;
    [SerializeField] private DataManager m_DataManager = null;
    [SerializeField] private InputManager m_InputManager = null;
    [SerializeField] private UIManager m_UIManager = null;
    [SerializeField] private XRManager m_XRManager = null;
    */

    /// �G���g���[�|�C���g
    private void Start()
    {
        //await InitializeAsync();
        Debug.Log("Start Main");
        manager = FindObjectOfType<Change_scene>();
        //manager.load_entry_scene();
        manager.change_to_title_scene();

        //UpdateLoop(this.GetCancellationTokenOnDestroy()).Forget();
    }

    /// ����������
    /*private async UniTask InitializeAsync()
    {
        m_PlayerController.Initialize();
        m_AudioManager.Initialize();
        m_DataManager.Initialize();
        m_InputManager.Initialize();
        m_UIManager.Initialize();
        m_XRManager.Initialize();

        await UniTask.Yield();
    }*/

    /// Update����
    /*private async UniTaskVoid UpdateLoop(CancellationToken cancellationToken)
    {
        while (true)
        {
            // Update�Ŏ��s���鏈���������ɋL�q���܂��B

            await UniTask.Yield(PlayerLoopTiming.Update, cancellationToken);
        }
    }*/
}

