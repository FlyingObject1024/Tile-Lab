using UnityEngine;

public class StageClickHandler : MonoBehaviour
{
    // �X�e�[�W���i���l�j
    public string stageName;

    // �O���X�N���v�g�̎Q��
    public Change_scene manager;

    void OnMouseDown()
    {
        if (manager != null)
        {
            manager.change_to_puzzle_scene(stageName); // �O���X�N���v�g�̊֐����Ă�
        }
        else
        {
            Debug.LogWarning("StageManager ���ݒ肳��Ă��܂���");
        }
    }
}
