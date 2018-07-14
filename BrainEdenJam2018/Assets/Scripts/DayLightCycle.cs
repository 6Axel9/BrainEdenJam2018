using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayLightCycle : MonoBehaviour {

    private static readonly float SECONDS_IN_MINUTES = 60.0f;

    private Transform m_transform;

    private float m_angle;
    private float m_lerp;

    [SerializeField] private float m_currentTime;
    [SerializeField] private float m_dayLength;
    [SerializeField] private float m_offset;

    [SerializeField] private Color m_dayColor;
    [SerializeField] private Color m_nightColor;

    public float CurrentTime {
        get { return m_currentTime; }
        set { m_currentTime = value; }
    }

    public float DayLength {
        get { return m_dayLength; }
        set { m_dayLength = value; }
    }

    public Color DayColor  {
        get { return m_dayColor; }
        set { m_dayColor = value; }
    }

    public Color NightColor {
        get { return m_nightColor; }
        set { m_nightColor = value; }
    }

    public float Angle {
        get { return m_angle; }
    }

    public float Offset {
        get { return m_offset; }
        set { m_offset = value; }
    }



    // Use this for initialization
    void Start () {
        m_currentTime = 0.0f;
        m_dayLength = 3.0f;
        m_offset = 20.0f;
        m_angle = 0.0f;
	}
	
	// Update is called once per frame
	void Update () {
        m_transform = GetComponent<Transform>();

        if(m_currentTime > m_dayLength * SECONDS_IN_MINUTES) {
            m_currentTime = 0;
        }
        else if (m_currentTime < 0) {
            m_currentTime = m_dayLength * SECONDS_IN_MINUTES;
        }

        m_transform.rotation = Quaternion.Euler(0, 0, Utils.MapRange(m_currentTime, 0.0f, m_dayLength * SECONDS_IN_MINUTES, 0.0f, 360.0f));
        m_angle = Utils.MapRange(m_currentTime, 0.0f, m_dayLength * SECONDS_IN_MINUTES, 0.0f, 360.0f);

        if(m_angle > (360 - m_offset * 0.5f)) {
            m_lerp = Utils.MapRange(m_angle, 360.0f - m_offset * 0.5f, 360.0f, 1, 0.5f);
        }
        else if( m_angle > 0 && m_angle < (0 + m_offset * 0.5f)) {
            m_lerp = Utils.MapRange(m_angle, 0, 0 + m_offset * 0.5f, 0.5f, 0.0f);
        }
        else if( m_angle > (180 - m_offset * 0.5f) && m_angle < (180 + m_offset * 0.5f)) {
            m_lerp = Utils.MapRange(m_angle, 180 - m_offset * 0.5f, 180 + m_offset * 0.5f, 0, 1);
        }

        if (RenderSettings.skybox.HasProperty("_TintColor"))
            RenderSettings.skybox.SetColor("_TintColor", Color.Lerp(m_dayColor, m_nightColor, m_lerp));

        m_currentTime += Time.deltaTime;
    }
}
