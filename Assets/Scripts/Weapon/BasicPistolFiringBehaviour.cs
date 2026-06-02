using System.Collections;
using UnityEngine;

public class BasicPistolFiringBehaviour : FiringBehaviour
{

    private Coroutine _firingRoutine;

    override public void Fire()
    {
        _firingRoutine = StartCoroutine(FireRoutine());
    }

    public override void StopFiring()
    {
        if(_firingRoutine != null)
            StopCoroutine(_firingRoutine);
    }

    public IEnumerator FireRoutine()
    {
        //TODO: implement
        yield return null;
    }
}
