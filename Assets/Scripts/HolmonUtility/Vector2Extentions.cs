namespace HolmonUtility
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public static class Vector2Ext
    {
        /// <summary>
        /// Vector2.Lerpのtの範囲を拡張したもの
        /// </summary>
        /// <param name="a">始点</param>
        /// <param name="b">終点</param>
        /// <param name="t">割合</param>
        /// <returns></returns>
        public static Vector2 OverLerp(Vector2 a, Vector2 b, float t)
        {
            float resultX = a.x + (b.x - a.x) * t;
            float resultY = a.y + (b.y - a.y) * t;
            return new Vector2(resultX, resultY);
        }
    }
}