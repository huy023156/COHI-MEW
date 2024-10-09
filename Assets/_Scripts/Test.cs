using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class Test : MonoBehaviour
{
    [SerializeField] private Player player;

    private void Start()
    {
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {

            Debug.Log(PathFinding.Instance.GetGridSystem().GetGridPosition(UtilClass.GetMouseWorldPosition()));
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            player.GetHealthSystem().Damage(2);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            player.GetHealthSystem().Heal(2);
        }
    }
}
