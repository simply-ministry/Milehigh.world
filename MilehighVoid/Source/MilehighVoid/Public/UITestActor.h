// Copyright Epic Games, Inc. All Rights Reserved.

#pragma once

#include "CoreMinimal.h"
#include "GameFramework/Actor.h"
#include "PlayerStatusWidget.h"
#include "UITestActor.generated.h"

/**
 * @class AUITestActor
 * @brief An actor designed for testing and demonstrating the Player Status UI.
 *
 * When placed in a level, this actor will create an instance of the specified Player Status Widget Blueprint,
 * populate it with mock data, and add it to the game viewport. This provides a simple way to verify
 * the UI's appearance and data-binding functionality without needing the full game logic.
 */
UCLASS()
class MILEHIGHVOID_API AUITestActor : public AActor
{
	GENERATED_BODY()

public:
	/** Sets default values for this actor's properties */
	AUITestActor();

protected:
	/** Called when the game starts or when spawned. This is where the widget creation and setup logic is executed. */
	virtual void BeginPlay() override;

public:
	/**
	 * @brief The Widget Blueprint to be created.
	 * This should be set in the Unreal Editor to the WBP_PlayerStatus Blueprint.
	 */
	UPROPERTY(EditAnywhere, Category = "UI")
	TSubclassOf<UPlayerStatusWidget> PlayerStatusWidgetClass;

private:
	/** A pointer to the created widget instance. */
	UPROPERTY()
	UPlayerStatusWidget* PlayerStatusWidget;
};
