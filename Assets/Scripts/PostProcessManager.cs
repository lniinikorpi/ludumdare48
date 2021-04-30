using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;

public class PostProcessManager : MonoBehaviour
{
    private Volume volume;
    private Vignette vignette;
    // Start is called before the first frame update
    void Start()
    {
        volume = GetComponent<Volume>();
        VolumeProfile profile = volume.sharedProfile;
        profile.TryGet(out vignette);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator HitFlash(float max)
    {
        float step = 3f * Time.deltaTime;
        print("flashing");
        while (vignette.intensity.value <= max)
        {
            vignette.intensity.value += step;
            yield return new WaitForSeconds(.02f);
        }
        vignette.intensity.value = max;
        while (vignette.intensity.value >= 0)
        {
            vignette.intensity.value -= step;
            yield return new WaitForSeconds(.02f);
        }
        vignette.intensity.value = 0;

    }
}
