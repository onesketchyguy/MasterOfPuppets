using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterPlayer : MonoBehaviour
{
    public Transform player;

    [Range(0, 20)]
    public float maxDistance = 10;

    private List<GameObject> objectsInScene;
    private bool moving = false;

    private bool SetPlayer()
    {
        var playerObject = GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null) player = playerObject.transform;

        return player != null;
    }

    /// <summary>
    /// THIS IS VERY COST INTENSIVE!
    /// </summary>
    private void SetAllObjectsInScene()
    {
        foreach (var item in FindObjectsOfType<Transform>())
        {
            if (objectsInScene.Contains(item.gameObject) || item.CompareTag("Player")) continue;

            bool _continue = false;

            var toAdd = item;
            var parent = item.transform.parent;

            while (parent != null)
            {
                if (objectsInScene.Contains(parent.gameObject) ||
                    parent.gameObject.CompareTag("Player"))
                {
                    _continue = true;
                    break;
                }

                toAdd = parent;
                parent = parent.transform.parent;
            }

            if (_continue) continue;

            objectsInScene.Add(toAdd.gameObject);
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if (player == null)
            if (SetPlayer() == false) return;

        if (Vector3.Distance(player.position, Vector3.zero) >= maxDistance)
        {
            // Start Move
            StartCoroutine(MoveWorld());
        }
    }

    private IEnumerator MoveWorld()
    {
        if (moving) yield break;
        moving = true;

        SetAllObjectsInScene();

        yield return null;
        TimeScaleManager.SetTimeScale(0);

        var playerMoveOffset = player.position;
        player.position = Vector3.zero;

        foreach (var item in objectsInScene)
            item.transform.position -= playerMoveOffset;

        yield return new WaitForEndOfFrame();

        TimeScaleManager.SetTimeScale(1);
        moving = false;
    }
}