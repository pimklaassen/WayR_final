using HoloToolkit.Unity.InputModule;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Video;

public class VideoOnClick : MonoBehaviour
{
    
    private VideoPlayer vp;

    private void Start()
    {
        vp = this.transform.GetComponent<VideoPlayer>();
        switch (this.transform.parent.name)
        {
            case "Zurich":
                vp.url = Application.streamingAssetsPath +"/ZurichAd.mp4";
                break;
            case "Lausanne":
                vp.url = Application.streamingAssetsPath + "/HololensTrailer.mp4";
                break;
            case "Grindelwald":
                vp.url = Application.streamingAssetsPath + "/BluePlanetTrailer.mp4";
                break;
        }
    }

    private void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.tag == "video")
                {
                    if (vp.isPlaying)
                    {
                        vp.Pause();
                    }
                    else
                    {
                        vp.Play();
                    }
                }
            }
        }
    }
    
}
