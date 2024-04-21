using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/*
    Fade effect for objects when set active
*/
public class Canvas_grounp_fade : MonoBehaviour
{
    // Duration of the fade effect
    public static float fade_time = 0.3141592653f;

    // Fade coroutine
    public static IEnumerator fade(GameObject obj, float from, float to)
    {
        // Activate the object if fading to fully visible
        if (to == 1)
            obj.SetActive(true);

        // Get the CanvasGroup component of the object
        CanvasGroup canvas_group = obj.GetComponent<CanvasGroup>();

        // Set the duration of the fade effect
        float duration = Canvas_grounp_fade.fade_time;

        float elapsedTime = 0f;
        while (elapsedTime <= duration)
        {
            elapsedTime += Time.deltaTime;
            // Apply the fade effect
            canvas_group.alpha = Mathf.Lerp(from, to, elapsedTime / duration);
            yield return null;
        }
        // Ensure alpha is set to the final value
        canvas_group.alpha = to;

        // Deactivate the object if fading to fully transparent
        if (to == 0)
        {
            obj.SetActive(false);
        }
    }

    // Show coroutine
    public static IEnumerator show(GameObject obj)
    {
        float from = 0;
        float to = 1;

        // Activate the object
        obj.SetActive(true);

        CanvasGroup canvas_group = obj.GetComponent<CanvasGroup>();

        // Set the duration of the fade effect
        float duration = Canvas_grounp_fade.fade_time;

        float elapsedTime = 0f;
        while (elapsedTime <= duration)
        {
            elapsedTime += Time.deltaTime;
            // Apply the fade effect
            canvas_group.alpha = Mathf.Lerp(from, to, elapsedTime / duration);
            yield return null;
        }
        // Ensure alpha is set to the final value
        canvas_group.alpha = to;
    }

    /// <summary>
    /// "is_destory"  indicates whether to destroy the object when hidden
    /// </summary>
    public static IEnumerator hide(GameObject obj, bool is_destroy = false)
    {
        float from = 1;
        float to = 0;

        CanvasGroup canvas_group = obj.GetComponent<CanvasGroup>();

        // Set the duration of the fade effect
        float duration = Canvas_grounp_fade.fade_time;

        float elapsedTime = 0f;
        while (elapsedTime <= duration)
        {
            elapsedTime += Time.deltaTime;
            // Apply the fade effect
            canvas_group.alpha = Mathf.Lerp(from, to, elapsedTime / duration);
            yield return null;
        }
        // Ensure alpha is set to the final value
        canvas_group.alpha = to;

        // Deactivate or destroy the object based on the flag
        obj.SetActive(false);
        if (is_destroy)
        {
            Destroy(obj);
        }
    }
}
