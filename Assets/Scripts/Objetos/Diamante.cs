using System;
using UnityEngine;

public class Diamante : MonoBehaviour, IInteractuable
{
    [SerializeField] private Animator animator;
    public static Action DiamanteRecolectado;
    private bool sePuedeUsar = true;

    public void Interactuar()
    {
        Recolectar();
    }

    private void Recolectar()
    {
        if (!sePuedeUsar) { return; }
        sePuedeUsar = false;
        DiamanteRecolectado?.Invoke();
        animator.SetTrigger("Recoger");
    }

    public void DestruirObjeto()
    {
        Destroy(gameObject);
    }
}
