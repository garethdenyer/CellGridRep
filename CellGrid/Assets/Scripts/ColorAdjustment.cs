using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
 
public class ColorAdjustment : MonoBehaviour
{
    //on Empty Holder

    public Slider exposureSld;
    public Volume volume; //Scene Visuals intem in Hierarchy
    private ColorAdjustments thisExposure;  //coloradjustments??

    private void Start()
    {
        exposureSld.value = 0.5f;
        ExposureCtrl();
    }

    public void ExposureCtrl()
    {
        VolumeProfile proflile = volume.sharedProfile;  //the profile of ScenVisuals?
 
 
        volume.profile.TryGet(out thisExposure);
        thisExposure.postExposure.value = exposureSld.value;
 
        if(exposureSld.value == 0)
            thisExposure.active = false;
        else
            thisExposure.active = true;
    }
}