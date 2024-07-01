using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;
using TMPro;
using TMPro.EditorUtilities;

public class AudioPlayer : MonoBehaviour
{
    public TMP_InputField filePathInputField;
    public TextMeshProUGUI filenameText;
    public Button loadButton;
    public Button playButton;
    public Button stopButton;
    public Button pauseButton;
    private AudioSource audioSource;
    private string filePath;
    private bool isPaused;
    private FileBrowserHandler fileBrowserHandler;


    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        loadButton.onClick.AddListener(LoadAudio);
        playButton.onClick.AddListener(PlayAudio);
        stopButton.onClick.AddListener(StopAudio);
    }

    void OpenFileBrowser()
    {
        filePath = fileBrowserHandler.OpenFileBrowserDialog();
        if (!string.IsNullOrEmpty(filePath))
        {
            filenameText.text = System.IO.Path.GetFileName(filePath);
            StartCoroutine(LoadAudio(filePath));
        }
    }

    IEnumerator LoadAudio(string path)
    {
        string url = "file:///" + path;
        using (UnityEngine.Networking.UnityWebRequest www = UnityEngine.Networking.UnityWebRequestMultimedia.GetAudioClip(url, AudioType.UNKNOWN))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityEngine.Networking.UnityWebRequest.Result.Success)
            {
                Debug.LogError(www.error);
            }
            else
            {
                audioSource.clip = UnityEngine.Networking.DownloadHandlerAudioClip.GetContent(www);
            }
        }
    }

    //Gets File path
    public void LoadAudio()
    {
        filePath = filePathInputField.text;
        filenameText.text = Path.GetFileName(filePath);
        StartCoroutine(LoadAudioCoroutine(filePath));
    }

    //Gets the load audio from webrequests
    IEnumerator LoadAudioCoroutine(string path)
    {
        string url = "file:///" + path;
        using (WWW www = new WWW(url))
        {
            yield return www;
            audioSource.clip = www.GetAudioClip(false, false);
        }
    }

    //button to play audio
    public void PlayAudio()
    {
        if (audioSource.clip != null)
        {
            audioSource.Play();
        }
    }

    //Pause Audio
    public void PauseAudio()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Pause();
            isPaused = true;
        }
    }

    //Stop audio
    public void StopAudio()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }
}
