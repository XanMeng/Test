using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Entity : MonoBehaviour, ISelectable , ITargetable , IMovable , IDamageable
{
    [SerializeField] GameObject visual;
    [SerializeField] GameObject visualTarget;

    NavMeshAgent agent;
    Animator anim;

    float healthPoint = 20;
    float maxHealthPoint = 20;

    public delegate void DelegateHealthChange(float _currentHealth, float _maxHealth);
    public event DelegateHealthChange OnHealthChange = (float _currentHealth, float _maxHealth) => { };

    private void Awake()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        agent.destination = transform.position;
    }

    private void Update()
    {
        if (agent.remainingDistance < 0.01f)
        {
            agent.avoidancePriority = 50;
        }
        anim.SetFloat("Velocity", agent.velocity.magnitude);
    }

    public void Deselect()
    {
        visual.SetActive(false);
        //GetComponent<MeshRenderer>().material.color = Color.white;
    }

    public void Select()
    {
        visual.SetActive(true);
        //GetComponent<MeshRenderer>().material.color = Color.red;
    }

    void IMovable.Move(Vector3 _destination)
    {
        agent.destination = _destination;
        agent.avoidancePriority = 1;
    }

    void ITargetable.Target()
    {
        visualTarget.SetActive(true);
        //transform.localScale *= 1.2f; //new Vector3(2.0f, 2.0f, 2.0f);
    }

    void ITargetable.Untarget()
    {
        visualTarget.SetActive(false);
        //transform.localScale /= 1.2f;// new Vector3(1.0f, 1.0f, 1.0f);
    }

    void IDamageable.Damage(float _amout)
    {
        healthPoint -= _amout;
        OnHealthChange(healthPoint, maxHealthPoint);
        if (healthPoint <= 0.0f)
        {
            Destroy(gameObject);
        }
    }
}
