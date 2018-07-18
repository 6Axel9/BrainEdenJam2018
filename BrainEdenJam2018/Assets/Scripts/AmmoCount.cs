using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class AmmoCount : MonoBehaviour {

    [SerializeField] private Gun m_playerGun;
    [SerializeField] private Image[] m_bulletImages;
    [SerializeField] private Text m_ammoCount;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		foreach ( var image in m_bulletImages) {
            image.enabled = false;
        }
        for(int i = 0; i < m_playerGun.m_bulletsInClip; i++) {
            m_bulletImages[i].enabled = true;
        }

        m_ammoCount.text = m_playerGun.m_totalAmmo.ToString();
	}
}
