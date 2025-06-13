using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class EnemyBulletPool : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab; // �e�̃v���n�u
    private ObjectPool<GameObject> enemyBulletPool; // �e�̃v�[��

    void Start()
    {
        // �e�̃I�u�W�F�N�g�v�[����������
        enemyBulletPool = new ObjectPool<GameObject>(
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
                }
                else
                {
                    Debug.LogWarning("�j���ς݂̒e���v�[���ɖ߂����Ƃ��܂����B"); // �x�����b�Z�[�W
                }
            },
            actionOnDestroy: bullet =>
            {
                if (bullet != null)
                {
                    Destroy(bullet); // �e��j��
                }
                else
                {
                    Debug.LogWarning("�j���ς݂̒e��j�󂵂悤�Ƃ��܂����B"); // �x�����b�Z�[�W
                }
            },
            collectionCheck: false, // �R���N�V�����`�F�b�N���I�t
            defaultCapacity: 50, // �����e��
            maxSize: 100 // �ő�T�C�Y
        );
    }

    // �v�[������e�����o��
    public GameObject GetEnemyBullet()
    {
        var bullet = enemyBulletPool.Get();
        if (bullet == null)
        {
            Debug.LogError("�e���擾�ł��܂���ł����B�v�[���̏�Ԃ��m�F���Ă��������B");
        }
        return bullet;
    }




    // �v�[���ɒe��߂�
    public async UniTask ReleaseEnemyBullet(GameObject bullet, float delay = 0)
    {
        if (bullet == null) // null�`�F�b�N
        {
            Debug.LogWarning("�j��ς݂̒e��߂����Ƃ���");
            return; // �������I��
        }

        // �f�B���C���w�肳��Ă���ꍇ�A�ҋ@
        if (delay > 0)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(delay)); // �w�肳�ꂽ�b���ҋ@
        }

        // �v�[���ɒe��߂�
        enemyBulletPool.Release(bullet); 
    }
}
