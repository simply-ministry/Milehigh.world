// Copyright Epic Games, Inc. All Rights Reserved.

#pragma once

#include "CoreMinimal.h"
#include "Engine/DataTable.h"
#include "CharacterStats.generated.h"

/**
 * @struct FCharacterStats
 * @brief Defines the data structure for a character's core attributes and affinities.
 *
 * This struct inherits from FTableRowBase to be compatible with Unreal Engine's Data Table system.
 * It is marked as a BlueprintType, allowing it to be used within Blueprints.
 */
USTRUCT(BlueprintType)
struct FCharacterStats : public FTableRowBase
{
	GENERATED_BODY()

public:
	/** The display name of the character. */
	UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Character|Identity")
	FString CharacterName;

	/** The character's title or archetype. */
	UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Character|Identity")
	FString Title;

	/** Governs physical power and attack damage. */
	UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Character|Attributes")
	int32 Strength;

	/** Influences attack speed, critical hit chance, and evasion. */
	UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Character|Attributes")
	int32 Dexterity;

	/** Reduces incoming physical damage. */
	UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Character|Attributes")
	int32 Defense;

	/** Determines the character's maximum health. */
	UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Character|Attributes")
	int32 Vigor;

	/** Determines the character's maximum mana or spirit energy. */
	UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Character|Attributes")
	int32 Heart;

	/** Governs the character's proficiency with Void-based abilities. */
	UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Character|Affinities")
	int32 VoidAffinity;

	/** Governs the character's ability to interact with and stabilize Nexus energy. */
	UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Character|Affinities")
	int32 NexusAttunement;

	/** Governs the character's proficiency with Dreamscape-based abilities. */
	UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Character|Affinities")
	int32 OneiricResonance;

	/** Influences the character's resistance to mental status effects and their ability to foresee enemy actions. */
	UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Character|Affinities")
	int32 PropheticClarity;
};
