using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldDragHandler : MonoBehaviour
{
    public Camera camera;

    private Vector3 lastMousePosition;

    void OnMouseDown()
    {
        // �}�E�X�̍ŏ��̈ʒu���L�^
        lastMousePosition = Input.mousePosition;
    }

    void OnMouseDrag()
    {
        // ���݂̃}�E�X�ʒu
        Vector3 currentMousePosition = Input.mousePosition;

        // �}�E�X�̈ړ���
        Vector3 delta = currentMousePosition - lastMousePosition;

        // �J�������ړ��i�X�N���[�����W�̍��������[���h���W�ɕϊ��j
        Vector3 move = camera.ScreenToWorldPoint(new Vector3(currentMousePosition.x, currentMousePosition.y, camera.transform.position.z))
                     - camera.ScreenToWorldPoint(new Vector3(lastMousePosition.x, lastMousePosition.y, camera.transform.position.z));

        camera.transform.position -= new Vector3(move.x, move.y, 0);  // Z�͌Œ�

        // �}�E�X�ʒu���X�V
        lastMousePosition = currentMousePosition;
    }
}
