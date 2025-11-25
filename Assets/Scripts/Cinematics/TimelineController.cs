using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class TimelineController : MonoBehaviour
{
public PlayableDirector director;
public TimelineAsset[] cinematics;

public void PlayCinematic(int index)
{
if (director != null && cinematics != null && index < cinematics.Length)
{
director.playableAsset = cinematics[index];
director.Play();
}
}
}

// SPDX-License-Identifier: (Boost-1.0 OR MIT OR Apache-2.0)
// Copyright © 2024 The Mile-High Mythographers. All rights reserved.