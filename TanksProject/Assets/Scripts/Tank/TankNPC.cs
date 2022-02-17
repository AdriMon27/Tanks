using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TankNPC : MonoBehaviour
{
    public List<Transform> playersTanks;
    public Rigidbody m_Shell;
    public Transform m_FireTransform;
    public AudioSource m_ShootingAudio;
    public AudioClip m_ChargingClip;
    public AudioClip m_FireClip;
    public float m_MinLaunchForce = 15f;
    public float m_MaxLaunchForce = 30f;

    //public NavMeshAgent navMeshAgent { get => _navMeshAgent; }
    private NavMeshAgent _navMeshAgent;
    private Vector3 _nearestPlayer;
    private float m_CurrentLaunchForce;
    private float _timeUntilNextShot = 3f;
    private float _timeWithoutShot = 0f;

    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _nearestPlayer = SearchNearestPlayer();

        _navMeshAgent.SetDestination(_nearestPlayer);

        m_CurrentLaunchForce = m_MaxLaunchForce/2;
    }

    private void Update()
    {
        if (_nearestPlayer != transform.position && _navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance && _timeWithoutShot >= _timeUntilNextShot) {
            //preparar el disparo y soltarlo
            Fire();
            _timeWithoutShot = 0f;
        }

        _nearestPlayer = SearchNearestPlayer();
        _navMeshAgent.SetDestination(_nearestPlayer);

        _timeWithoutShot += Time.deltaTime;
    }

    private void OnDisable()
    {
        _navMeshAgent.enabled = false;
    }

    private void OnEnable()
    {
        _navMeshAgent.enabled = true;
    }

    //funcion llamada desde el GameManager nada más empezar la partida
    private Vector3 SearchNearestPlayer()
    {
        float nearestDistance = float.PositiveInfinity;
        float aux;
        int indexNumber = -1;

        for (int i = 0; i < playersTanks.Count; i++) {
            if (!playersTanks[i].gameObject.activeSelf) {
                continue;
            }

            aux = (playersTanks[i].position - transform.position).magnitude;
            
            if (aux < nearestDistance) {
                nearestDistance = aux;
                indexNumber = i;
            }
        }

        return (indexNumber > -1) ? playersTanks[indexNumber].position : transform.position;
    }

    private void Fire()
    {
        // Instantiate and launch the shell.
        //m_Fired = true;

        Rigidbody shellInstance = Instantiate(m_Shell, m_FireTransform.position, m_FireTransform.rotation) as Rigidbody;

        shellInstance.velocity = m_CurrentLaunchForce * m_FireTransform.forward;

        m_ShootingAudio.clip = m_FireClip;
        m_ShootingAudio.Play();

        m_CurrentLaunchForce = m_MinLaunchForce;
    }
}
