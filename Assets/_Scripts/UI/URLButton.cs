using _Scripts.UI;
using UnityEngine;

public class URLButton : MonoBehaviour
{
    [SerializeField] private URLGuideUI urlMover;
    [SerializeField] private string url = @"https://www.google.com/";

    public void ToURL()
    {
        urlMover.GuideURL(url);
    }
}
