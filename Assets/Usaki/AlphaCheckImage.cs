using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Usaki
{
    public class AlphaCheckImage : Image
    {
        public override bool IsRaycastLocationValid(Vector2 screenPoint, Camera eventCamera)
        {
            // Convert screen point to local point within the RectTransform
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                rectTransform,
                screenPoint,
                eventCamera,
                out Vector2 localPoint
            );

            // Normalize the local point to a percentage relative to the rect
            Rect rect = rectTransform.rect;
            Vector2 normalizedPoint = new Vector2(
                (localPoint.x - rect.x) / rect.width,
                (localPoint.y - rect.y) / rect.height
            );

            // If the point is outside the rect, return false
            if (normalizedPoint.x < 0 || normalizedPoint.x > 1 || normalizedPoint.y < 0 || normalizedPoint.y > 1)
            {
                return false;
            }

            // Get the texture associated with the image
            Sprite sprite = this.sprite;
            if (sprite == null || sprite.texture == null)
            {
                return false;
            }

            // Convert normalized point to texture coordinates
            Texture2D texture = sprite.texture;
            Rect textureRect = sprite.textureRect;
            Vector2 textureCoord = new Vector2(
                textureRect.x + textureRect.width * normalizedPoint.x,
                textureRect.y + textureRect.height * normalizedPoint.y
            );

            // Convert texture coordinates to pixel coordinates
            int x = Mathf.FloorToInt(textureCoord.x);
            int y = Mathf.FloorToInt(textureCoord.y);

            // Check if the alpha value at the pixel is greater than 0
            Color pixelColor = texture.GetPixel(x, y);
            return pixelColor.a > 0;

        }
    }
}
