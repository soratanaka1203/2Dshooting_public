using UnityEngine;

public class HealthBarFollow : MonoBehaviour
{
    [SerializeField] private RectTransform healthBar; // �̗̓o�[��RectTransform
    [SerializeField] private Transform boss;          // �{�X��Transform
    [SerializeField] private Camera mainCamera;       // ���C���J����
    [SerializeField] private Vector3 offset;          // ���[���h��Ԃł̃I�t�Z�b�g

    private void Update()
    {
        // �{�X�̃��[���h���W���X�N���[�����W�ɕϊ�
        Vector3 screenPosition = mainCamera.WorldToScreenPoint(boss.position + (offset + new Vector3(0,5,0)));

        // �{�X����ʓ��ɂ���ꍇ�̂ݑ̗̓o�[��\��
        if (screenPosition.z > 0)
        {
            healthBar.position = screenPosition;
        }
        else
        {
            healthBar.gameObject.SetActive(false);
        }
    }
}

