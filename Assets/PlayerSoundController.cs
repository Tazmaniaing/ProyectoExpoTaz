using UnityEngine;

public class PlayerSoundController : MonoBehaviour
{
    [Header("Audios cortos (OneShot)")]
    [SerializeField] private AudioClip sonidoSaltar;
    [SerializeField] private AudioClip sonidoCaida;
    [SerializeField] private AudioClip sonidoAtaque;
    [SerializeField] private AudioClip sonidoRecibirDaño;
    [SerializeField] private AudioClip sonidoMuerte;

    [Header("Audios en loop (Background)")]
    [SerializeField] private AudioClip sonidoMov1; // pasos

    private AudioSource audioSourceOneShot; // para sonidos cortos
    private AudioSource audioSourceLoop;    // para loop

    private void Awake()
    {
        // Crea dos AudioSource
        audioSourceOneShot = gameObject.AddComponent<AudioSource>();
        audioSourceOneShot.loop = false;
        audioSourceOneShot.playOnAwake = false;

        audioSourceLoop = gameObject.AddComponent<AudioSource>();
        audioSourceLoop.loop = true;
        audioSourceLoop.playOnAwake = false;
    }

    // 🔊 OneShot
    public void playSaltar() => audioSourceOneShot.PlayOneShot(sonidoSaltar);
    public void playCaida() => audioSourceOneShot.PlayOneShot(sonidoCaida);
    public void playAtaque()
    {
        if (sonidoAtaque != null)
        {
            audioSourceOneShot.PlayOneShot(sonidoAtaque);
            Debug.Log("▶️ Reproduciendo sonido de ataque");
        }
        else
        {
            Debug.LogWarning("⚠️ No se asignó sonidoAtaque en el Inspector");
        }
    }
    public void playRecibirDaño() => audioSourceOneShot.PlayOneShot(sonidoRecibirDaño);
    public void playMuerte() => audioSourceOneShot.PlayOneShot(sonidoMuerte);

    // 🔁 Loop (pasos)
    public void playMov1Loop(bool activar)
    {
        if (activar)
        {
            if (!audioSourceLoop.isPlaying)
            {
                audioSourceLoop.clip = sonidoMov1;
                audioSourceLoop.Play();
            }
        }
        else
        {
            if (audioSourceLoop.isPlaying)
            {
                audioSourceLoop.Stop();
            }
        }
    }
}
