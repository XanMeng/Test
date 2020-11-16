using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Poison : MonoBehaviour {

    //IEnumerator DamageOverTime()
    IEnumerator Start()
    {
        for (; ; )
        {
            Collider[] colliders = Physics.OverlapBox(transform.position, transform.localScale / 2.0f, transform.rotation);

            colliders.ToList().ForEach(x =>
            {
                IDamageable damageable = x.transform.GetComponent<IDamageable>();
                if (damageable != null)
                {
                    damageable.Damage(15);
                }

            });

            yield return new WaitForSeconds(1.0f);
        }
    }


}
