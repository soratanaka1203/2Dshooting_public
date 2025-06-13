using UnityEngine;
using UnityEngine.Pool;
using Cysharp.Threading.Tasks;
using System;

public class BulletPool : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab; // �e�̃v���n�u
    private ObjectPool<GameObject> bulletPool; // �e�̃v�[��

    void Start()
    {
        // �e�̃I�u�W�F�N�g�v�[����������
        bulletPool = new ObjectPool<GameObject>(
            createFunc: () => Instantiate(bulletPrefab), // �V�����e�𐶐�����֐�
            actionOnGet: bullet =>
            {
                if (bullet != null)
                {
                    bullet.SetActive(true); // �e���A�N�e�B�u�ɂ���
                }
            },
            actionOnRelease: bullet =>
            {
                if (bullet != null)
                {
                    bullet.SetActive(false); // �e���A�N�e�B�u�ɂ���
                    Debug.Log("�e���A�N�e�B�u�ɂ��Ė߂�");
                }
            },
            actionOnDestroy: bullet =>
            {
                if (bullet != null)
                {
                    Destroy(bullet); // �e��j��
                }
            },
            collectionCheck: false, // �R���N�V�����`�F�b�N���I�t
            defaultCapacity: 50, // �����e��
            maxSize: 250 // �ő�T�C�Y
        );
    }

    // �v�[������e�����o��
    public GameObject GetBullet()
    {
        var bullet = bulletPool.Get();
        if (bullet == null)
        {
            Debug.LogError("�e���擾�ł��܂���ł����B�v�[���̏�Ԃ��m�F���Ă��������B");
            return null;
        }
        return bullet;
    }



    //�v�[���ɕԋp����
    public async UniTask ReleaseBullet(GameObject bullet, float delay = 0)
    {
        if (bullet == null)
        {
            Debug.LogWarning("�j��ς݂̒e��߂����Ƃ���");
            return;
        }

        if (delay > 0)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(delay)); // �e���ҋ@���Ԍ�Ƀv�[���ɖ߂�
        }

        bullet.SetActive(false);
        bulletPool.Release(bullet); // �v�[���ɖ߂�
    }


}
