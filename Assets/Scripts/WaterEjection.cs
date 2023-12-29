using System.Collections.Generic;
using UnityEngine;

public class WaterEjection : MonoBehaviour
{
    [SerializeField] private float _offHeightDifference = 0.5f;

    private float _waterHeight;
    private List<Rigidbody> _rigidbodies = new List<Rigidbody>();
    private float _gravity = Mathf.Abs(Physics.gravity.y);
    private float _waterDensity = 1000f;
    private float _volume = 1f;

    private void Start()
    {
        var collider = GetComponent<Collider>();
        _waterHeight = collider.bounds.size.y / 2 + transform.position.y;
    }

    private void OnTriggerEnter(Collider other)
    {
        var rigidbody = other.gameObject.GetComponent<Rigidbody>();
        _rigidbodies.Add(rigidbody);
    }

    private void OnTriggerExit(Collider other)
    {
        var rigidbody = other.gameObject.GetComponent<Rigidbody>();
        _rigidbodies.Remove(rigidbody);
    }

    private void FixedUpdate()
    {
        foreach (var rigidbody in _rigidbodies)
        {
            var position = rigidbody.transform.position;

            //var velocity = Vector3.up * _waterDensity * _volume * _gravity;
            //rigidbody.AddForce(velocity, ForceMode.Force);
            var force = -Physics.gravity * rigidbody.mass;
            //Debug.Log(rigidbody.velocity);
            rigidbody.AddForce(force);
        }
    }
}
