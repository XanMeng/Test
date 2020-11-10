using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHealthBar : MonoBehaviour {

    [SerializeField] Image health;

    private void Start()
    {

        GetComponentInParent<Entity>().OnHealthChange += (float _currentHealth, float _maxHealth) =>
        {
            health.fillAmount = _currentHealth / _maxHealth;
        };

    }

    private void Update()
    {
        
        transform.rotation = Camera.main.transform.rotation;
    }
}
