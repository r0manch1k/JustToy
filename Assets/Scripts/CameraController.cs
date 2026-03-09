using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
	[Header( "Settings" )]
	[SerializeField] private Transform _target;
	[SerializeField] private float _distance = 10f;
	[SerializeField] private float _height = 5f;
	[SerializeField] private float _rotateSpeed = 3f;

	[SerializeField] private float sensitivity = 0.15f;
	// [SerializeField] private Vector2 screenOffset = new Vector2( 3f, 2f );

	private float _currentAngle = 0f;
	// private Vector3 _currentPosition;

	private InputAction _lookAction;
	// private Vector3 _initialOffset;
	// private float _initialPitch;
	// private float _yaw;

	private void Awake()
	{
		_lookAction = InputSystem.actions.FindAction( "Player/Look" );
		// _initialOffset = transform.localPosition;
		// _initialPitch = transform.localEulerAngles.x;
	}

	private void Start()
	{
		_currentAngle = transform.eulerAngles.y;
	}

	private void LateUpdate()
	{
		if ( _target == null ) return;

		Vector2 lookInput = _lookAction.ReadValue<Vector2>();

		if ( lookInput.x != 0 )
		{
			_currentAngle += lookInput.x * _rotateSpeed;
		}

		float angleRad = _currentAngle * Mathf.Deg2Rad;
		float offsetX = Mathf.Sin( angleRad ) * _distance;
		float offsetZ = Mathf.Cos( angleRad ) * _distance;

		Vector3 newPosition = _target.position + new Vector3( offsetX, _height, offsetZ );
		transform.position = newPosition;

		transform.LookAt( _target.position + Vector3.up * (_height * 0.5f) );

		// float lookX = lookInput.x;
		// _yaw += lookX * sensitivity;

		// Quaternion orbit = Quaternion.Euler( 0f, _yaw, 0f );
		// transform.localPosition = orbit * _initialOffset;
		// transform.localRotation = Quaternion.Euler(
		// 	_initialPitch - screenOffset.y,
		// 	_yaw + screenOffset.x,
		// 	0f );
	}
}
