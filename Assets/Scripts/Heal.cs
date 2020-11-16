using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Heal : MonoBehaviour {

    //IEnumerator DamageOverTime()
    IEnumerator Start()
    {
        for (; ; )
        {
            Collider[] colliders = Physics.OverlapBox(transform.position, transform.localScale / 2.0f, transform.rotation);

            colliders.ToList().ForEach(x =>
            {
                IHealable healable = x.transform.GetComponent<IHealable>();
                if (healable != null)
                {
                    healable.Heal(4);
                }

            });

            // Cool

            yield return new WaitForSeconds(1.0f);
        }
    }


}
