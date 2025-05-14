using System;
using System.Collections;

using UnityEngine;

namespace LoopingBee.Shared.Utils
{
    [RequireComponent(typeof(Camera))]
    public class CameraController : MonoBehaviour
    {
        [SerializeField] RectTransform viewZone;

        Camera cam;
        Bounds? bounds;
        Coroutine encapsulateBoundsRoutine;

        void Awake()
        {
            cam = GetComponent<Camera>();
        }

        void Update()
        {
            if (viewZone != null && bounds.HasValue && viewZone.hasChanged && encapsulateBoundsRoutine == null)
                encapsulateBoundsRoutine = StartCoroutine(EncapsulateBounsdRoutine(bounds.Value));
        }

        public void EncapsulateBounds(Bounds bounds, Action onComplete = null)
        {
            encapsulateBoundsRoutine = StartCoroutine(EncapsulateBounsdRoutine(bounds, onComplete));
        }

        IEnumerator EncapsulateBounsdRoutine(Bounds bounds, Action onComplete = null)
        {
            var boundsWidth = bounds.size.x;
            var boundsHeight = bounds.size.y;
            var corners = new Vector3[4];

            if (viewZone != null)
            {
                viewZone.GetWorldCorners(corners);

                var viewZoneWidth = Vector3.Distance(corners[0], corners[3]);
                var viewZoneHeight = Vector3.Distance(corners[0], corners[1]);

                var requiredOrthographicSize = Mathf.Max(boundsWidth / viewZoneWidth * cam.orthographicSize,
                                                         boundsHeight / viewZoneHeight * cam.orthographicSize);
                cam.orthographicSize = requiredOrthographicSize;
            }
            else
            {
                var requiredOrthographicSize = Mathf.Max(boundsWidth / cam.aspect, boundsHeight) / 2f;
                cam.orthographicSize = requiredOrthographicSize;
            }

            yield return null;

            if (viewZone != null)
            {
                viewZone.GetWorldCorners(corners);
                var cornersCenter = -(corners[0] + (corners[2] - corners[0]) / 2f) + transform.position;
                transform.position = new Vector3(cornersCenter.x + bounds.center.x, cornersCenter.y + bounds.center.y, transform.position.z);
                viewZone.hasChanged = false;
            }
            else
                transform.position = new Vector3(bounds.center.x, bounds.center.y, transform.position.z);

            this.bounds = bounds;
            encapsulateBoundsRoutine = null;

            onComplete?.Invoke();
        }
    }
}
