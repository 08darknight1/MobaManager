using UnityEngine;

public class NewTimer : MonoBehaviour
{
    private bool _iniciar;

    private float _tempoPrivate;

    private bool _sinal;
    
    void Update()
    {
        if (_iniciar)
        {
            _tempoPrivate = _tempoPrivate - Time.deltaTime;
            if (_tempoPrivate <= 0)
            {
                _sinal = true;
            }
        }
    }

    public void Iniciar(float tempo)
    {
        if (_iniciar != true)
        {
            _tempoPrivate = tempo;
            _iniciar = true;
        }
    }

    public bool Sinalizar()
    {
        return _sinal;
    }

    public bool RetornarIniciar()
    {
        return _iniciar;
    }

    public void Reiniciar()
    {
        _iniciar = false;
        _sinal = false;
    }
}
