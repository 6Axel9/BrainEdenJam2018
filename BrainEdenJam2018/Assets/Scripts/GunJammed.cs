using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class GunJammed : MonoBehaviour {

    [SerializeField] private PlayerGun m_playerGun;

    [SerializeField] private Image m_gunIcon;
    [SerializeField] private Material m_gunMaterial;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (m_playerGun.IsJammed)
        {
            m_gunIcon.color = Color.red;
            m_gunMaterial.color = Color.red;
        }
        else
        {
            m_gunIcon.color = Color.white;
            m_gunMaterial.color = Color.white;
        }
	}
}
