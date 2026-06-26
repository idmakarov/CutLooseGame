using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

[RequireComponent(typeof(RawImage), typeof(VideoPlayer))]
public class VideoBackground : MonoBehaviour
{
    private RawImage rawImage;
    private VideoPlayer videoPlayer;

    void Awake()
    {
        rawImage = GetComponent<RawImage>();
        videoPlayer = GetComponent<VideoPlayer>();
    }

    void Start()
    {
        videoPlayer.playOnAwake = false;
        videoPlayer.isLooping = true;
        videoPlayer.renderMode = VideoRenderMode.RenderTexture;
        videoPlayer.targetTexture = new RenderTexture(1920, 1080, 0);
        
        rawImage.texture = videoPlayer.targetTexture;
        
        videoPlayer.Play();
    }

    void OnDestroy()
    {
        if (videoPlayer.targetTexture != null)
        {
            videoPlayer.targetTexture.Release();
        }
    }
}