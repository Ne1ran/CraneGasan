using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace Utils
{
    public static class MathUtils
    {
        public static float? FindNearestDistance(Vector3 origin, [CanBeNull] List<Transform> targets = null)
        {
            try {
                if (targets == null || targets.Count == 0) {
                    return null;
                }

                float bestSqr = (targets[0].position - origin).sqrMagnitude;

                for (int i = 1; i < targets.Count; i++) {
                    float sqr = (targets[i].position - origin).sqrMagnitude;
                    if (sqr < bestSqr) {
                        bestSqr = sqr;
                    }
                }

                return Mathf.Sqrt(bestSqr);
            } catch (Exception e) {
                // Обертка на случай, если удалить один из объектов, т.к. выпадет nullref
                Debug.LogException(e);
                return null;
            }
        }
    }
}