using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
public IEnumerator Shake(float duration, float magnitude)
{
Vector3 originalPos = transform.localPosition;
float elapsed = 0.0f;

while (elapsed < duration)
{
float x = Random.Range(-1f, 1f) * magnitude;
float y = Random.Range(-1f, 1f) * magnitude;

transform.localPosition = new Vector3(x, y, originalPos.z);
elapsed += Time.deltaTime;
yield return null;
}

transform.localPosition = originalPos;
}
}

// SPDX-License-Identifier: (Boost-1.0 OR MIT OR Apache-2.0)
// Copyright © 2024 The Mile-High Mythographers. All rights reserved.