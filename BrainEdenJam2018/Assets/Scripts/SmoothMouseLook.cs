using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[AddComponentMenu("Camera-Control/Smooth Mouse Look")]
public class SmoothMouseLook : MonoBehaviour {

    /// <summary>
    /// This is a wrapper around the original unity Mouse Look script (https://gist.github.com/M-Pixel/e8aa79890297b975994e).
    /// 
    /// Added smoothness by scaling the camera movement speed on the mouse acceleration, each "step" of the mouse is stored in an array.
    /// </summary>


    [SerializeField] private float sensitivityX;
    [SerializeField] private float sensitivityY;

    [SerializeField] private float m_minimumX;
    [SerializeField] private float m_maximumX;

    [SerializeField] private float m_minimumY;
    [SerializeField] private float m_maximumY;

    [SerializeField] private float m_frame_counter;

    private float m_rotationX;
	private float m_rotationY;

	private List<float> m_rot_arrayX = new List<float>();
	private float m_rot_averageX;	

	private List<float> m_rot_arrayY = new List<float>();
	private float m_rot_averageY;

    private Quaternion originalRotation;

	public float Sensibility
	{
		set{sensitivityX = value;
			sensitivityY = value;}

		get{return sensitivityX;}
	}

    public float MinimumX
    {
        get
        {
            return m_minimumX;
        }

        set
        {
            m_minimumX = value;
        }
    }

    public float MaximumX
    {
        get
        {
            return m_maximumX;
        }

        set
        {
            m_maximumX = value;
        }
    }

    public float MinimumY
    {
        get
        {
            return m_minimumY;
        }

        set
        {
            m_minimumY = value;
        }
    }

    public float MaximumY
    {
        get
        {
            return m_maximumY;
        }

        set
        {
            m_maximumY = value;
        }
    }

    public float FrameCounter
    {
        get
        {
            return m_frame_counter;
        }

        set
        {
            m_frame_counter = value;
        }
    }

    void Start ()
	{		
		sensitivityX = sensitivityY = 1;
		Rigidbody rb = GetComponent<Rigidbody>();	
		if (rb)
			rb.freezeRotation = true;
		originalRotation = transform.localRotation;
	}

	void Update ()
	{
		m_rot_averageY = 0f;
		m_rot_averageX = 0f;

	
		m_rotationY += Input.GetAxis("Mouse Y") * sensitivityX;
		m_rotationX += Input.GetAxis("Mouse X") * sensitivityY;
		

		m_rot_arrayY.Add(m_rotationY);
		m_rot_arrayX.Add(m_rotationX);

		if (m_rot_arrayY.Count >= m_frame_counter) {
			m_rot_arrayY.RemoveAt(0);
		}
		if (m_rot_arrayX.Count >= m_frame_counter) {
			m_rot_arrayX.RemoveAt(0);
		}

		for(int j = 0; j < m_rot_arrayY.Count; j++) {
			m_rot_averageY += m_rot_arrayY[j];
		}
		for(int i = 0; i < m_rot_arrayX.Count; i++) {
			m_rot_averageX += m_rot_arrayX[i];
		}

		m_rot_averageY /= m_rot_arrayY.Count;
		m_rot_averageX /= m_rot_arrayX.Count;

		m_rot_averageY = ClampAngle (m_rot_averageY, m_minimumY, m_maximumY);
		m_rot_averageX = ClampAngle (m_rot_averageX, m_minimumX, m_maximumX);

		Quaternion yQuaternion = Quaternion.AngleAxis (m_rot_averageY, Vector3.left);
		Quaternion xQuaternion = Quaternion.AngleAxis (m_rot_averageX, Vector3.up);

		transform.localRotation = originalRotation * xQuaternion * yQuaternion;

	}

	public static float ClampAngle (float angle, float min, float max)
	{
		angle = angle % 360;
		if ((angle >= -360F) && (angle <= 360F)) {
			if (angle < -360F) {
				angle += 360F;
			}
			if (angle > 360F) {
				angle -= 360F;
			}			
		}
		return Mathf.Clamp (angle, min, max);
	}


}