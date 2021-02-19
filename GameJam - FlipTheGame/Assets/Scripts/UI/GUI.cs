using UnityEngine;
using TMPro;
using System;

public class GUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timer;
    [SerializeField] TextMeshProUGUI deathCount;

    private void LateUpdate()
    {
        if (timer != null)
        {
            InputController.instance.timer += Time.deltaTime;
            timer.text = DisplayTime(InputController.instance.timer);
        }

        if (deathCount)
        {
            deathCount.text = $"Deaths: {InputController.instance.deathCount}";
        }
    }

    string DisplayTime(float timeToDisplay)
    {
        TimeSpan timeSpan = TimeSpan.FromSeconds(timeToDisplay);

        if (timeSpan.Days > 0)
        {
            if (timeSpan.Days < 10)
            {
                return timeSpan.ToString(@"\ d\:hh\:mm\:ss");
            }
            else
            {
                return timeSpan.ToString(@"dd\:hh\:mm\:ss");
            }
        }
        else
        {
            if (timeSpan.Hours > 0)
            {
                if (timeSpan.Hours < 10)
                {
                    return timeSpan.ToString(@"\ h\:mm\:ss");
                }
                else
                {
                    return timeSpan.ToString(@"hh\:mm\:ss");
                }
            }
            else
            {
                if (timeSpan.Minutes > 0)
                {
                    if (timeSpan.Minutes < 10)
                    {
                        return timeSpan.ToString(@"\ m\:ss");
                    }
                    else
                    {
                        return timeSpan.ToString(@"mm\:ss");
                    }
                }
                else
                {
                    if (timeSpan.Seconds < 10)
                    {
                        return timeSpan.ToString(@"\ s");
                    }
                    else
                    {
                        return timeSpan.ToString(@"\ ss");
                    }
                }
            }
        }
    }
}
