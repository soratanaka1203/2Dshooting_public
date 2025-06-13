using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.SocialPlatforms.Impl;

public class ItemPool : MonoBehaviour
{
    [SerializeField] private GameObject itemPrefab; // �A�C�e���̃v���n�u
    //�A�C�e���̃X�v���C�g
    [SerializeField] private Sprite shiledSprite;
    [SerializeField] private Sprite scoreSprite;
    [SerializeField] private Sprite plusBulletSprite;
    private ObjectPool<GameObject> itemPool; // �A�C�e���̃v�[��

    // Start is called before the first frame update
    void Start()
    {
        // �A�C�e���̃I�u�W�F�N�g�v�[����������
        itemPool = new ObjectPool<GameObject>(
            createFunc: () => Instantiate(itemPrefab), // �V�����A�C�e���𐶐�����֐�
            actionOnGet: item =>
            {
                if (item != null)
                {
                    item.SetActive(true); // �A�C�e�����A�N�e�B�u�ɂ���
                }
            },
            actionOnRelease: item =>
            {
                if (item != null)
                {
                    item.SetActive(false); // �A�C�e�����A�N�e�B�u�ɂ���
                }
            },
            actionOnDestroy: item =>
            {
                if (item != null)
                {
                    Destroy(item); // �A�C�e����j��
                }
            },
            collectionCheck: false, // �R���N�V�����`�F�b�N���I�t
            defaultCapacity: 10, // �����e��
            maxSize: 20 // �ő�T�C�Y
        );
    }

    public GameObject GetItem(string itemName)
    {
        var item = itemPool.Get();
        if (item == null)
        {
            Debug.LogError("�A�C�e�����擾�ł��܂���ł����B�v�[���̏�Ԃ��m�F���Ă��������B");
            return null;
        }
        switch (itemName)
        {
            case "Score":
                item.GetComponent<SpriteRenderer>().sprite = scoreSprite;
                item.GetComponent<Item>().itemType = Item.ItemType.Score;
                return item;
            case "PlusBullet":
                item.GetComponent<SpriteRenderer>().sprite = plusBulletSprite;
                item.GetComponent<Item>().itemType = Item.ItemType.PlusBullet;
                return item;
            case "Shield":
                item.GetComponent<SpriteRenderer>().sprite = shiledSprite;
                item.GetComponent<Item>().itemType = Item.ItemType.Shield;
                return item;
        }
        return item;
    }
    
    public void ReleaseItem(GameObject item)
    {
        if (item == null)
        {
            Debug.LogError("�A�C�e�����擾�ł��܂���ł����B");
        }
        else
        {
            itemPool.Release(item);
        }
    }
}
