using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

public static class Helpers
{
    public static IEnumerator WaitAndExecute(float waitTime, System.Action action)
    {
        yield return new WaitForSeconds(waitTime);
        action?.Invoke();
    }

    public static IEnumerator FadeCanvasGroup(CanvasGroup canvasGroup, float targetAlpha, float duration)
    {
        float startAlpha = canvasGroup.alpha;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / duration);
            yield return null;
        }

        canvasGroup.alpha = targetAlpha;
    }

    private static readonly Dictionary<float, WaitForSeconds> WaitCache = new Dictionary<float, WaitForSeconds>();
    public static WaitForSeconds GetWaitForSeconds(float time)
    {
        if (!WaitCache.TryGetValue(time, out var wait)) return wait;
        
        WaitCache[time] = new WaitForSeconds(time);
        return WaitCache[time];
    }

    // Unity-specific
    // private static PointerEventData _eventDataCurrentPosition;
    // private static List<RaycastResult> _results;
    // public static bool IsPointerOverUI()
    // {
    //     _eventDataCurrentPosition = new PointerEventData(EventSystem.current)
    //     {
    //         position = Input.mousePosition
    //     };
    //     _results = new List<RaycastResult>();
    //     EventSystem.current.RaycastAll(_eventDataCurrentPosition, _results);
    //     return _results.Count > 0;
    // }

    // Can be used for adding a 3D object to the UI for example.
    public static Vector2 GetWorldPositionOfCanvasElement(RectTransform element)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            element,
            element.position,
            Camera.main,
            out var result);
        return result;
    }

    public static void DeleteChildren(this Transform t)
    {
        foreach (Transform child in t) Object.Destroy(child.gameObject);
    }

    public static Transform FindDeepChild(Transform aParent, string aName)
    {
        Queue<Transform> queue = new Queue<Transform>();
        queue.Enqueue(aParent);
        while (queue.Count > 0)
        {
            var c = queue.Dequeue();
            if (c.name == aName)
            {
                return c;
            }
            foreach (Transform t in c)
            {
                queue.Enqueue(t);
            }
        }
        return null;
    }
}
