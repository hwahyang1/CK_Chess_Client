using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using NaughtyAttributes;

namespace Chess_Client.GameScene
{
	/// <summary>
	/// Description
	/// </summary>
	public class CameraController : MonoBehaviour
	{
		[Header("Config")]
		[SerializeField]
	    private Transform target;

		[SerializeField]
	    private float rotationSpeed = 5.0f;

	    [SerializeField]
	    private float zoomSpeed = 5.0f;

	    [SerializeField]
	    [Range(0, 1)]
	    private float rotationDamping = 0.9f;

	    [SerializeField]
	    [Range(0, 1)]
	    private float zoomDamping = 0.9f;

	    [SerializeField]
	    private float minDistance = 3.0f;
	    
	    [SerializeField]
	    private float maxDistance = 20.0f;

	    [SerializeField]
	    private float minVerticalAngle = -20f;
	    
	    [SerializeField]
	    private float maxVerticalAngle = 80f;
	    
	    [Header("Status")]
	    [SerializeField, ReadOnly]
	    private float currentDistance;
	    
	    [SerializeField, ReadOnly]
	    private float yaw;
	    
	    [SerializeField, ReadOnly]
	    private float pitch;
	    
	    [SerializeField, ReadOnly]
	    private float yawVelocity;
	    
	    [SerializeField, ReadOnly]
	    private float pitchVelocity;
	    
	    [SerializeField, ReadOnly]
	    private float zoomVelocity;

	    private void Start()
	    {
	        Vector3 offset = transform.position - target.position;
	        currentDistance = offset.magnitude;

	        Vector3 angles = transform.eulerAngles;
	        yaw = angles.y;
	        pitch = angles.x;
	    }

	    private void LateUpdate()
	    {
	        // rotation
	        bool isDragging = Input.GetMouseButton(1);
	        if (isDragging)
	        {
	            float mouseX = Input.GetAxis("Mouse X");
	            float mouseY = Input.GetAxis("Mouse Y");

	            yawVelocity = mouseX * rotationSpeed;
	            pitchVelocity = -mouseY * rotationSpeed;
	        }
	        else
	        {
	            yawVelocity *= rotationDamping;
	            pitchVelocity *= rotationDamping;
	        }

	        yaw += yawVelocity;
	        pitch += pitchVelocity;
	        pitch = Mathf.Clamp(pitch, minVerticalAngle, maxVerticalAngle);

	        // zoom
	        float scroll = Input.GetAxis("Mouse ScrollWheel");
	        if (Mathf.Abs(scroll) > 0.0001f)
	        {
	            zoomVelocity = scroll * zoomSpeed * 10f;
	        }
	        else
	        {
	            zoomVelocity *= zoomDamping;
	            if (Mathf.Abs(zoomVelocity) < 0.01f)
	            {
		            zoomVelocity = 0f;
	            }
	        }

	        currentDistance -= zoomVelocity * Time.deltaTime;
	        currentDistance = Mathf.Clamp(currentDistance, minDistance, maxDistance);

	        // calculate
	        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);
	        Vector3 direction = rotation * Vector3.back;
	        transform.position = target.position + direction * currentDistance;
	        transform.LookAt(target);
	    }
	}
}
