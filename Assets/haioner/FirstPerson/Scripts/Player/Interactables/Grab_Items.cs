using UnityEngine;

[System.Serializable]
public class ObjectsSttngs
{
	public float objDrag = 10f;
	public float objAngularDrag = 10f;
	public bool objUseGravity = false;
	public RigidbodyConstraints objConstraints = RigidbodyConstraints.FreezeRotation;
}

public class Grab_Items : MonoBehaviour
{
    #region Variables

    #region inInspector

	[Header("Controllers")]
    [SerializeField] private PlayerController playerController;
	[SerializeField] private bool canScroll = true;
	[SerializeField] private bool canRotate = true;
	[SerializeField] private bool canThrow = true;
    public Vector2 InputView { private set; get; }

    [Header("Speeds")]
	[SerializeField] private float grabSpeed = 6.5f;
	[SerializeField] private float scrollSpeed = 4.85f;
	[SerializeField] private float rotateSpeed = 10f;

	[Header("Grab Distance")]
	[SerializeField] [Range(0.1f, 6f)] float minDistance = 1f;
	[SerializeField] [Range(1f, 20f)] float maxDistance = 5f;

	[Header("Throw Objects")]
	[SerializeField] [Range(1f, 30f)] float impulseForce = 10f;

	[Header("Layers")]
	[SerializeField] LayerMask layerObjects;

	[Header("Ray")]
	public float sphereRadius;
	public float distance;


	#endregion

	#region notInspector 

	ObjectsSttngs objProperties = new ObjectsSttngs();
	ObjectsSttngs objDefaultProperties = new ObjectsSttngs();

    private DefaultInput _defaultInput;
	
	private Rigidbody targetRB = null;
	private Transform objTransform;
	Vector3 targetPos;
	float targetDistance;

	bool isHingeJoint = false;
	bool applyImpulse = false;
	[HideInInspector] public bool grabbing = false;
	LineRenderer m_lineRenderer;
	GameObject hitPointObject;

	private Vector3 playerOrigin;
	private Vector3 playerDirection;
	Transform playerPos;
	#endregion

	#endregion

	#region Methods

	void Awake()
	{
		playerPos = GetComponentInParent<PlayerController>().gameObject.transform;
		objTransform = transform;
		hitPointObject = new GameObject("hitPoint");
		m_lineRenderer = GetComponent<LineRenderer>();

        _defaultInput = new DefaultInput();
        _defaultInput.View.CameraView.performed += e => InputView = e.ReadValue<Vector2>();
        _defaultInput.Enable();
    }

	void Update()
	{
		if(_defaultInput.Grab.RotateItem.ReadValue<float>() == 0 && !playerController.GetViewState() && grabbing)
            SetViewState(true);

        isBelow();
		Grabbing();
		Impulse();
	}

	void FixedUpdate()
	{
		if (!grabbing) return;
		if (!isHingeJoint && _defaultInput.Grab.RotateItem.ReadValue<float>() == 1) Rotate();

		GrabInputs();
	}

	#endregion

	#region Custom Methods
	bool isBelow()
    {
		bool myHit = false;
		playerOrigin = playerPos.position;
		playerDirection = playerPos.up * distance;
		RaycastHit hit;
		if (Physics.SphereCast(playerOrigin, sphereRadius, playerDirection, out hit,Mathf.Infinity, layerObjects))
        {
            if (grabbing)
            {
				ResetGrab();
				grabbing = false;
			}

			myHit = true;
        }
		return myHit;
    }

	/*
    private void OnDrawGizmosSelected()
    {
		Gizmos.color = Color.yellow;
		playerPos = GetComponentInParent<PlayerController>().gameObject.transform;
		Gizmos.DrawWireSphere(playerPos.position + playerPos.up * distance, sphereRadius);
    }
	*/

    private void Grabbing()
    {
		if (grabbing) //If is grabbing object
		{
			if (canScroll)
			{
				Vector2 zScroll = _defaultInput.Grab.ScrollItem.ReadValue<Vector2>();
				targetDistance += zScroll.y * scrollSpeed;
			}

			targetDistance = Mathf.Clamp(targetDistance, minDistance, maxDistance);
			targetPos = objTransform.position + objTransform.forward * targetDistance;

            if (_defaultInput.Grab.HoldItem.WasPressedThisFrame() || Input.GetMouseButtonUp(0))
            {
                ResetGrab();
                grabbing = false;
            }
            else if (_defaultInput.Grab.ThrowItem.WasPressedThisFrame())
                applyImpulse = true;
        }
		else //If not grabbing object
		{
			if (_defaultInput.Grab.HoldItem.WasPressedThisFrame())
			{
				RaycastHit hitInfo;
				if (Physics.Raycast(objTransform.position, objTransform.forward, out hitInfo, maxDistance, layerObjects))
				{
					Rigidbody rb = hitInfo.collider.GetComponent<Rigidbody>();
					if (rb != null)
					{
						Select(rb, hitInfo.distance);
						grabbing = true;
					}
				}
			}

		}
    }

    private void Select(Rigidbody _Target, float _Distance)
	{
		targetRB = _Target;
		isHingeJoint = _Target.GetComponent<HingeJoint>() != null;

		//Object Default Properties	
		objDefaultProperties.objDrag = targetRB.linearDamping;
		objDefaultProperties.objAngularDrag = targetRB.angularDamping;
		objDefaultProperties.objUseGravity = targetRB.useGravity;
		objDefaultProperties.objConstraints = targetRB.constraints;

		//Target Properties	
		targetRB.linearDamping = objProperties.objDrag;
		targetRB.angularDamping = objProperties.objAngularDrag;
		targetRB.useGravity = objProperties.objUseGravity;
		targetRB.constraints = isHingeJoint ? RigidbodyConstraints.None : objProperties.objConstraints;

		hitPointObject.transform.SetParent(_Target.transform);
		targetDistance = _Distance;
		targetPos = objTransform.position + objTransform.forward * targetDistance;

		hitPointObject.transform.position = targetPos;
		hitPointObject.transform.LookAt(objTransform);

	}

	private void GrabInputs()
	{
		Vector3 hitPointPos = hitPointObject.transform.position;
		Vector3 dif = targetPos - hitPointPos;

		if (isHingeJoint)
			targetRB.AddForceAtPosition(grabSpeed * dif * 100, hitPointPos, ForceMode.Force);
		else
			targetRB.linearVelocity = grabSpeed * dif;

		if (m_lineRenderer != null)
		{
			m_lineRenderer.enabled = true;
			m_lineRenderer.SetPositions(new Vector3[] { targetPos, hitPointPos });
		}
	}

	private void Rotate()
	{
		if (!canRotate) return;
        SetViewState(false);
		targetRB.transform.Rotate(transform.right, InputView.y * rotateSpeed, Space.World);
		targetRB.transform.Rotate(-transform.up, InputView.x * rotateSpeed, Space.World);
	}

	private void SetViewState(bool state)
    {
		playerController.SetViewState(state);
    }

    private void Impulse()
    {
		if (!canThrow) return;

		if (applyImpulse)
		{
			targetRB.linearVelocity = objTransform.forward * impulseForce;
			ResetGrab();
			grabbing = false;
			applyImpulse = false;
		}
	}

    private void ResetGrab()
	{
        SetViewState(true);
        //Reset Object Settings	
        targetRB.linearDamping = objDefaultProperties.objDrag;
		targetRB.angularDamping = objDefaultProperties.objAngularDrag;
		targetRB.useGravity = objDefaultProperties.objUseGravity;
		targetRB.constraints = objDefaultProperties.objConstraints;

		targetRB = null;
		hitPointObject.transform.SetParent(null);

		if (m_lineRenderer != null)
			m_lineRenderer.enabled = false;
	}

	public void ClearGrab()
	{
		ResetGrab();
		grabbing = false;
	}

    #endregion
}


