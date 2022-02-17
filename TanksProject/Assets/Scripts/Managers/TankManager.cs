using System;
using UnityEngine;

[Serializable]
public class TankManager
{
    public Color m_PlayerColor;            
    public Transform m_SpawnPoint;
    public bool isNPC = false;
    [HideInInspector] public int m_PlayerNumber;             
    [HideInInspector] public string m_ColoredPlayerText;
    [HideInInspector] public GameObject m_Instance;          
    [HideInInspector] public int m_Wins;                     


    private TankMovement m_Movement;       
    private TankShooting m_Shooting;
    private GameObject m_CanvasGameObject;
    
    private TankNPC _tankNPC;

    public void AddToPathFinding(TankManager tank)
    {
        _tankNPC.playersTanks.Add(tank.m_Instance.transform);
    }

    public virtual void Setup()
    {
        if (isNPC) {
            _tankNPC = m_Instance.GetComponent<TankNPC>();

            m_ColoredPlayerText = "<color=#" + ColorUtility.ToHtmlStringRGB(m_PlayerColor) + ">NPC " + m_PlayerNumber + "</color>";
        }
        else {
            m_Movement = m_Instance.GetComponent<TankMovement>();
            m_Shooting = m_Instance.GetComponent<TankShooting>();

            m_Movement.m_PlayerNumber = m_PlayerNumber;
            m_Shooting.m_PlayerNumber = m_PlayerNumber;

            m_ColoredPlayerText = "<color=#" + ColorUtility.ToHtmlStringRGB(m_PlayerColor) + ">PLAYER " + m_PlayerNumber + "</color>";
        }
        
        m_CanvasGameObject = m_Instance.GetComponentInChildren<Canvas>().gameObject;

        MeshRenderer[] renderers = m_Instance.GetComponentsInChildren<MeshRenderer>();

        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].material.color = m_PlayerColor;
        }
    }


    public virtual void DisableControl()
    {
        if (isNPC) {
            //Debug.Log("esto es " + _tankNPC.navMeshAgent);
            //_tankNPC.navMeshAgent.enabled = false;
            _tankNPC.enabled = false;
        }
        else {
            m_Movement.enabled = false;
            m_Shooting.enabled = false;
        }
        
        m_CanvasGameObject.SetActive(false);
    }


    public virtual void EnableControl()
    {
        if (isNPC) {
            _tankNPC.enabled = true;
            //_tankNPC.navMeshAgent.enabled = true;
        }
        else {
            m_Movement.enabled = true;
            m_Shooting.enabled = true;
        }

        m_CanvasGameObject.SetActive(true);
    }


    public void Reset()
    {
        m_Instance.transform.position = m_SpawnPoint.position;
        m_Instance.transform.rotation = m_SpawnPoint.rotation;

        m_Instance.SetActive(false);
        m_Instance.SetActive(true);
    }
}
