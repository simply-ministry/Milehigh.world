// Copyright Epic Games, Inc. All Rights Reserved.

#pragma once

#include "CoreMinimal.h"
#include "Blueprint/UserWidget.h"
#include "CharacterStats.h"
#include "Components/TextBlock.h"
#include "Components/ProgressBar.h"
#include "PlayerStatusWidget.generated.h"

/**
 * @class UPlayerStatusWidget
 * @brief Manages the UI for displaying a character's primary status bars and information.
 *
 * This C++ class serves as the base for the WBP_PlayerStatus Widget Blueprint. It holds the character data
 * and provides properties that can be bound to visual elements like Text Blocks and Progress Bars in the UMG editor.
 */
UCLASS()
class MILEHIGHVOID_API UPlayerStatusWidget : public UUserWidget
{
	GENERATED_BODY()

protected:
	/**
	 * @brief Overridden from UUserWidget. Called when the widget is constructed.
	 * This is the ideal place to perform initial setup, such as updating the UI with initial data.
	 */
	virtual void NativeConstruct() override;

public:
	/**
	 * @brief The data structure holding all the character stats to be displayed.
	 * This property can be set from a Blueprint to change the displayed character.
	 */
	UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Player Stats")
	FCharacterStats PlayerStats;

	/**
	 * @brief Updates all UI elements with the data from the PlayerStats struct.
	 * This function should be called after modifying the PlayerStats variable to refresh the display.
	 */
	UFUNCTION(BlueprintCallable, Category = "Player Stats")
	void UpdatePlayerStatus();

	/** A Text Block widget to display the character's current and max health. Bound in the UMG editor. */
	UPROPERTY(meta = (BindWidget))
	UTextBlock* HealthText;

	/** A Progress Bar widget to visually represent the character's current health percentage. Bound in the UMG editor. */
	UPROPERTY(meta = (BindWidget))
	UProgressBar* HealthProgressBar;

	/** A Text Block widget to display the character's current and max mana. Bound in the UMG editor. */
	UPROPERTY(meta = (BindWidget))
	UTextBlock* ManaText;

	/** A Progress Bar widget to visually represent the character's current mana percentage. Bound in the UMG editor. */
	UPROPERTY(meta = (BindWidget))
	UProgressBar* ManaProgressBar;

	/** A Text Block widget to display the character's current and max rage. Bound in the UMG editor. */
	UPROPERTY(meta = (BindWidget))
	UTextBlock* RageText;

	/** A Progress Bar widget to visually represent the character's current rage percentage. Bound in the UMG editor. */
	UPROPERTY(meta = (BindWidget))
	UProgressBar* RageProgressBar;

	/** A Text Block widget to display the party's current and max Alliance power. Bound in the UMG editor. */
	UPROPERTY(meta = (BindWidget))
	UTextBlock* AllianceText;

	/** A Progress Bar widget to visually represent the party's current Alliance power percentage. Bound in the UMG editor. */
	UPROPERTY(meta = (BindWidget))
	UProgressBar* AllianceProgressBar;
};
