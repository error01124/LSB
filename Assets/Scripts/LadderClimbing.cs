using UnityEngine;

public class LadderClimbing : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private AudioSource _sound;

    private FirstPersonController _playerMovement;
    private Rigidbody _rb;
    private bool _inside;
    private string _ladderTag = "Ladder";

    private void Start()
    {
        _playerMovement = GetComponent<FirstPersonController>();
        _rb = GetComponent<Rigidbody>();
        _sound.loop = true;
        _sound.enabled = false;
    }


    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag(_ladderTag))
        {
            Debug.Log(other.gameObject.tag);
            _inside = true;
            _rb.useGravity = false;
            _playerMovement.isClimbingLadder = true;
            _sound.enabled = true;
        }   
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag(_ladderTag))
        {
            _inside = false;
            _rb.useGravity = true;
            _playerMovement.isClimbingLadder = false;
            _sound.enabled = false;
        }      
    }

    private void FixedUpdate()
    {
        _playerMovement.velocityY = 0;

        if (_inside)
        {
            float vertical = Input.GetAxis("Vertical");
            Debug.Log(vertical);
            Debug.Log(_playerMovement.isGrounded);
            _playerMovement.velocityY = Input.GetAxis("Vertical") * _speed;
        }
    }
}
