using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonEffects : MonoBehaviour, IPointerClickHandler, IPointerExitHandler, IPointerEnterHandler
{
    private Button _thisButton;
    
    public AudioClip onSelect;

    public AudioClip onClick;
    
    private AudioSource _thisAudioSource;
    
    void Start()
    {
        _thisButton = gameObject.GetComponent<Button>();
        _thisAudioSource = gameObject.GetComponent<AudioSource>();
    }   

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_thisButton.interactable && gameObject.activeSelf)
        {
            gameObject.transform.localScale = gameObject.transform.localScale * 1.05f;
            PlayAudioSource(onSelect);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (_thisButton.interactable && gameObject.activeSelf)
        {
            var scaleX = gameObject.transform.localScale.x;

            if (scaleX > 1f)
            {
                gameObject.transform.localScale = gameObject.transform.localScale / 1.05f;
            }
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (_thisButton.interactable && gameObject.activeSelf)
        {
            StopAllSounds();

            var scale = gameObject.transform.localScale.x;

            if (scale > 1f)
            {
                gameObject.transform.localScale = gameObject.transform.localScale / 1.05f;
            }

            PlayAudioSource(onClick);
        }
    }
    
    private void PlayAudioSource(AudioClip toPlay)
    {
        if (_thisAudioSource.isPlaying)
        {
            _thisAudioSource.Stop();
        }

        _thisAudioSource.clip = toPlay;
        
        _thisAudioSource.Play();
    }

    private void StopAllSounds()
    {
        var all = FindObjectsByType<AudioSource>(FindObjectsSortMode.None);

        for (int x = 0; x < all.Length; x++)
        {
            if (all[x].gameObject.GetComponent<AudioSource>())
            {
                all[x].gameObject.GetComponent<AudioSource>().Stop();
            }
        }
    }
}
