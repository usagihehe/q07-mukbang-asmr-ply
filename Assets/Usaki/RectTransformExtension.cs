using UnityEngine;

public static class RectTransformExtension
{
    // -----------------------------------------------------------------------
    // INTERNAL HELPERS
    // -----------------------------------------------------------------------

    private static Vector2 WorldToScreenPoint(Vector3 worldPosition, Canvas canvas, Camera camera)
    {
        switch (canvas.renderMode)
        {
            case RenderMode.ScreenSpaceOverlay:
                // Overlay: UI sống trong screen space, world pos của UI element chính là screen pos
                return new Vector2(worldPosition.x, worldPosition.y);

            case RenderMode.ScreenSpaceCamera:
                return (camera ?? canvas.worldCamera).WorldToScreenPoint(worldPosition);

            default: // WorldSpace
                return (camera ?? Camera.main).WorldToScreenPoint(worldPosition);
        }
    }

    // -----------------------------------------------------------------------
    // CANVAS SYNC — Chuyển vị trí từ World/GameObject/Transform sang Canvas
    // -----------------------------------------------------------------------

    public static RectTransform SyncPosCanvas(
        this RectTransform rectTransform,
        GameObject target,
        Canvas canvas,
        Camera camera = null)
    {
        if (target == null || canvas == null)
            return rectTransform;

        return rectTransform.SyncPosCanvas(target.transform, canvas, camera);
    }

    public static RectTransform SyncPosCanvas(
        this RectTransform rectTransform,
        Transform target,
        Canvas canvas,
        Camera camera = null)
    {
        if (target == null || canvas == null)
            return rectTransform;

        return rectTransform.SyncPosCanvas(target.position, canvas, camera);
    }

    public static RectTransform SyncPosCanvas(
        this RectTransform rectTransform,
        Vector3 worldPosition,
        Canvas canvas,
        Camera camera = null)
    {
        if (canvas == null)
            return rectTransform;

        Vector2 screenPoint = WorldToScreenPoint(worldPosition, canvas, camera);
        Camera projectionCamera = canvas.renderMode == RenderMode.ScreenSpaceOverlay
            ? null
            : (camera ?? canvas.worldCamera);

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.GetComponent<RectTransform>(),
            screenPoint,
            projectionCamera,
            out Vector2 localPoint);

        rectTransform.localPosition = localPoint;
        return rectTransform;
    }

    public static RectTransform CanvasPos(
        GameObject target,
        Canvas canvas,
        Camera camera = null)
    {
        if (target == null || canvas == null)
            return null;

        return CanvasPos(target.transform, canvas, camera);
    }

    public static RectTransform CanvasPos(
        Transform target,
        Canvas canvas,
        Camera camera = null)
    {
        if (target == null || canvas == null)
            return null;

        RectTransform rectTransform = target.GetComponent<RectTransform>();
        if (rectTransform == null) return null;

        return rectTransform.SyncPosCanvas(target, canvas, camera);
    }

    // -----------------------------------------------------------------------
    // CANVAS LOCAL POINT — Chuyển world position thành toạ độ local trên Canvas
    // -----------------------------------------------------------------------

    public static Vector2 CanvasLocalPoint(
        Vector3 worldPosition,
        Canvas canvas,
        Camera camera = null)
    {
        if (canvas == null)
            return Vector2.zero;

        Vector2 screenPoint = WorldToScreenPoint(worldPosition, canvas, camera);
        Camera projectionCamera = canvas.renderMode == RenderMode.ScreenSpaceOverlay
            ? null
            : (camera ?? canvas.worldCamera);

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.GetComponent<RectTransform>(),
            screenPoint,
            projectionCamera,
            out Vector2 localPoint);

        return localPoint;
    }

    public static Vector2 CanvasLocalPoint(
        Transform target,
        Canvas canvas,
        Camera camera = null)
    {
        return target == null
            ? Vector2.zero
            : CanvasLocalPoint(target.position, canvas, camera);
    }

    // -----------------------------------------------------------------------
    // WORLD POSITION — Chuyển ngược từ vị trí Canvas về World Position
    // -----------------------------------------------------------------------

    public static Vector3 Pos(
        this RectTransform rectTransform,
        Canvas canvas,
        Camera camera = null,
        float depth = 0f)
    {
        if (canvas == null)
            return rectTransform.position;

        Camera projectionCamera = canvas.renderMode == RenderMode.ScreenSpaceOverlay
            ? null
            : (camera ?? canvas.worldCamera);

        Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(
            projectionCamera,
            rectTransform.position);

        switch (canvas.renderMode)
        {
            case RenderMode.ScreenSpaceOverlay:
                {
                    Camera cam = camera ?? Camera.main;
                    Vector3 screenPos = new Vector3(screenPoint.x, screenPoint.y,
                        depth == 0f ? Mathf.Abs(cam.transform.position.z) : depth);
                    return cam.ScreenToWorldPoint(screenPos);
                }

            case RenderMode.ScreenSpaceCamera:
                {
                    Camera cam = camera ?? canvas.worldCamera ?? Camera.main;
                    Vector3 screenPos = new Vector3(screenPoint.x, screenPoint.y,
                        depth == 0f ? canvas.planeDistance : depth);
                    return cam.ScreenToWorldPoint(screenPos);
                }

            case RenderMode.WorldSpace:
                return rectTransform.position;

            default:
                return rectTransform.position;
        }
    }

    public static Vector3 CanvasLocalPos(
        Canvas canvas,
        Vector2 localPoint,
        Camera camera = null,
        float depth = 0f)
    {
        if (canvas == null)
            return Vector3.zero;

        RectTransform canvasRect = canvas.GetComponent<RectTransform>();
        Camera projectionCamera = canvas.renderMode == RenderMode.ScreenSpaceOverlay
            ? null
            : (camera ?? canvas.worldCamera);

        Vector3 worldPointOnCanvas = canvasRect.TransformPoint(localPoint);
        Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(projectionCamera, worldPointOnCanvas);

        switch (canvas.renderMode)
        {
            case RenderMode.ScreenSpaceOverlay:
                {
                    Camera cam = camera ?? Camera.main;
                    Vector3 screenPos = new Vector3(screenPoint.x, screenPoint.y,
                        depth == 0f ? Mathf.Abs(cam.transform.position.z) : depth);
                    return cam.ScreenToWorldPoint(screenPos);
                }

            case RenderMode.ScreenSpaceCamera:
                {
                    Camera cam = camera ?? canvas.worldCamera ?? Camera.main;
                    Vector3 screenPos = new Vector3(screenPoint.x, screenPoint.y,
                        depth == 0f ? canvas.planeDistance : depth);
                    return cam.ScreenToWorldPoint(screenPos);
                }

            case RenderMode.WorldSpace:
                return worldPointOnCanvas;

            default:
                return worldPointOnCanvas;
        }
    }
}
