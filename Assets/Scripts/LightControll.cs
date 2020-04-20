using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightControll : MonoBehaviour
{
    public Light theatherLight;

    private void Awake()
    {
        EventManager.RegisterListenerText("ObstacleActivated", CheckLightActivation);
        EventManager.RegisterListenerText("ObstacleDeactivated", CheckLightDeactivation);
        EventManager.RegisterListener("Restart", InitLight);
    }

    public void InitLight()
    {
        theatherLight.intensity = 1f;
    }

    public void CheckLightActivation(string eventName)
    {
        if (eventName.Equals("LightDown"))
        {
            theatherLight.intensity = 0.3f;
        }
    }

    public void CheckLightDeactivation(string eventName)
    {
        if (eventName.Equals("LightDown"))
        {
            theatherLight.intensity = 1f;
        }
    }
}
