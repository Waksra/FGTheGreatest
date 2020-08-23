using System;
using UnityEngine;

public class SceneBoundary : MonoBehaviour
{
    private static SceneBoundary _instance;
    
    private static readonly Vector2 DefaultSize = new Vector2(22, 11);

    private Collider2D _collider;
    private Bounds _bounds;
    private static bool _isInstanceNull;

    private static SceneBoundary GetInstance()
    {
        if (_isInstanceNull)
        {
            GameObject obj = new GameObject("Boundary Box");
            obj.transform.position = Vector3.zero;
            BoxCollider2D coll = obj.AddComponent<BoxCollider2D>();
            coll.size = DefaultSize;
            coll.isTrigger = true;
            _instance = obj.AddComponent<SceneBoundary>();
        }

        return _instance;
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }

        _instance = this;
        _isInstanceNull = false;
        _collider = GetComponent<Collider2D>();
        _bounds = _collider.bounds;
    }

    public static bool IsWithinBounds(Vector2 position)
    {
        return GetInstance()._bounds.Contains(position);
    }
    
    public static void MoveBackToBounds(Transform transform)
    {
        if(IsWithinBounds(transform.position))
            return;

        transform.position = GetInstance()._bounds.ClosestPoint(transform.position);
    }

    private void OnDestroy()
    {
        if (_instance == this)
        {
            _isInstanceNull = true;
        }
    }
}
