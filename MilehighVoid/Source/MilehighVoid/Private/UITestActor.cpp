// Copyright Epic Games, Inc. All Rights Reserved.

#include "UITestActor.h"
#include "Blueprint/UserWidget.h"

// Sets default values
AUITestActor::AUITestActor()
{
	// Set this actor to call Tick() every frame.  You can turn this off to improve performance if you don't need it.
	PrimaryActorTick.bCanEverTick = false;

}

// Called when the game starts or when spawned
void AUITestActor::BeginPlay()
{
	Super::BeginPlay();

	if (PlayerStatusWidgetClass)
	{
		PlayerStatusWidget = CreateWidget<UPlayerStatusWidget>(GetWorld(), PlayerStatusWidgetClass);

		if (PlayerStatusWidget)
		{
			// Populate with Ingris's stats from the reference file
			PlayerStatusWidget->PlayerStats.CharacterName = "Ingris";
			PlayerStatusWidget->PlayerStats.Title = "The Phoenix Warrior";
			PlayerStatusWidget->PlayerStats.Strength = 8;
			PlayerStatusWidget->PlayerStats.Dexterity = 6;
			PlayerStatusWidget->PlayerStats.Defense = 7;
			PlayerStatusWidget->PlayerStats.Vigor = 9;
			PlayerStatusWidget->PlayerStats.Heart = 5;
			PlayerStatusWidget->PlayerStats.VoidAffinity = 2;
			PlayerStatusWidget->PlayerStats.NexusAttunement = 1;
			PlayerStatusWidget->PlayerStats.OneiricResonance = 3;
			PlayerStatusWidget->PlayerStats.PropheticClarity = 4;

			PlayerStatusWidget->UpdatePlayerStatus();
			PlayerStatusWidget->AddToViewport();
		}
	}

}
