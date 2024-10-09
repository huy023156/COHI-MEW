using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GridDebugObject : MonoBehaviour
{
    private object gridObject;
    [SerializeField] private TextMeshPro debugText;

    private void Awake()
    {
        transform.position += new Vector3(0, 0.02f, 0);
    }

    public virtual void SetGridObject(object gridObject)
    {
        this.gridObject = gridObject;
    }

    protected virtual void Update()
    {
        debugText.text = gridObject.ToString();
    }
}
