using UnityEngine;
using System.Collections.Generic;

public class AlliancePowerManager : MonoBehaviour
{
public static AlliancePowerManager Instance { get; private set; }

[Header("Alliance Meter")]
[SerializeField] private float maxAlliancePower = 1000f;
[SerializeField] private float currentAlliancePower;

public bool IsAllianceBreakReady => currentAlliancePower >= maxAlliancePower;

private List<CharacterAbilitiesBase> partyMembers = new List<CharacterAbilitiesBase>();

private void Awake()
{
if (Instance != null && Instance != this)
{
Destroy(gameObject);
}
else
{
Instance = this;
DontDestroyOnLoad(gameObject);
}
}

public void AddPartyMember(CharacterAbilitiesBase member)
{
if (!partyMembers.Contains(member))
{
partyMembers.Add(member);
}
}

public void AddAlliancePower(float amount)
{
currentAlliancePower = Mathf.Min(maxAlliancePower, currentAlliancePower + amount);
}

public void ExecuteAllianceBreak(GameObject primaryTarget)
{
if (!IsAllianceBreakReady) return;

Debug.Log("Executing NOVAMINAAD ALLIANCE BREAK!");

foreach (var member in partyMembers)
{
member.PerformNovaminaadFinisher(primaryTarget);
}

currentAlliancePower = 0f;
}
}

// SPDX-License-Identifier: (Boost-1.0 OR MIT OR Apache-2.0)
// Copyright © 2024 The Mile-High Mythographers. All rights reserved.