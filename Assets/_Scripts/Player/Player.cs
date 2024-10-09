using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;

public class Player : MonoBehaviour
{
    [SerializeField] private Transform itemHolderTransform;
    [SerializeField] private Animator batAnimator;
    private HealthSystem healthSystem;
    private MovementSystem movementSystem;

    private List<ItemSO> itemHoldList;
    private List<Transform> transformHoldList;
    [SerializeField] private int maxHoldingCapacity = 2;

    private void Awake()
    {
        healthSystem = GetComponent<HealthSystem>();
        movementSystem = GetComponent<MovementSystem>();

        itemHoldList = new List<ItemSO>();
        transformHoldList = new List<Transform>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && !EventSystem.current.IsPointerOverGameObject())
        {
            Vector2 mousePos = UtilClass.GetMouseWorldPosition();

            float maxDistance = .1f;
            if (NavMesh.SamplePosition(mousePos, out NavMeshHit hit, maxDistance, NavMesh.AllAreas))
            {
                movementSystem.Move(mousePos, null);
            }
        }
    }

    public void AddHoldingItemSO(ItemSO item)
    {
        if (itemHoldList.Count >= maxHoldingCapacity)
        {
            return;
        }

        itemHoldList.Add(item);
        Transform itemTransform = Instantiate(item.itemPrefab, itemHolderTransform);
        transformHoldList.Add(itemTransform);

        float offsetAmount = 0.3f;
        itemTransform.position += new Vector3(0, itemHoldList.Count * offsetAmount); 
    }

    public bool TryTakeItemSO(ItemSO itemSO, out Transform itemTransform) 
    {
        for (int i = 0; i < itemHoldList.Count; i++)
        {
            if (itemHoldList[i] == itemSO)
            {
                itemTransform = transformHoldList[i];
                itemHoldList.Remove(itemHoldList[i]);
                transformHoldList.Remove(transformHoldList[i]);
                return true;
            }
        }

        Debug.Log("Not found");
        itemTransform = null;
        return false;
    } 

    public bool CanHoldMoreItem()
    {
        return itemHoldList.Count < maxHoldingCapacity;
    }

    public HealthSystem GetHealthSystem() => healthSystem;
    public MovementSystem GetMovementSystem() => movementSystem;
    public Transform GetItemHolderTransform() => itemHolderTransform;

    public void BatHit()
    {
        batAnimator.SetTrigger("Attack");
    }
}
