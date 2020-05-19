﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropTargetComponent : MonoBehaviour
{
    /// <summary>
    /// The current state of the drop target. 
    /// </summary>
    public bool Active { get; private set; }

    /// <summary>
    /// Components used for drop target
    /// </summary>
    private DropTargetManager targetManager;
    private Collider targetCollider;
    
    /// <summary>
    /// The amount of points given when this drop target is hit.
    /// </summary>
    [SerializeField] int scoreValue;


    private void Start()
    {
        targetManager = transform.parent.GetComponent<DropTargetManager>();
        targetCollider = GetComponent<Collider>();

        TableManager.Manager.RegisterScores(this, scoreValue);

        Active = true;
    }

    /// <summary>
    /// Collision Detection on a drop target.
    /// </summary>
    /// <param name="other">The other game object</param>
    private void OnTriggerEnter(Collider other)
    {
        GameObject ball = other.gameObject;
        if (!ball.CompareTag("Ball")) return;

        Debug.Log("Hit a Drop Target");

        if(Active)
        {
            DropTarget();
        }
    }

    /// <summary>
    /// Drops the target.
    /// </summary>
    public void DropTarget()
    {
        Active = false;

        targetCollider.enabled = false;
        transform.localScale = new Vector3(0.6f, 0.02f, 0.6f);

        TableManager.Manager.Score(this);

        if(targetManager != null)
        {
            targetManager.CheckForActiveTargets();
        }
    }

    /// <summary>
    /// Resets the target back to upright.
    /// </summary>
    public void ResetTarget()
    {
        StartCoroutine(EnableHitbox());
    }

    /// <summary>
    /// IEnumerator to delay the target reset process. 
    /// </summary>
    /// <returns></returns>
    private IEnumerator EnableHitbox()
    {
        yield return new WaitForSeconds(0.5f);

        transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);

        targetCollider.enabled = true;

        Active = true;
    }
}