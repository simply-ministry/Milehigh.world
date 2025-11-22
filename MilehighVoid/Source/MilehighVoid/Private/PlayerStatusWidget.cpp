// Copyright Epic Games, Inc. All Rights Reserved.


#include "PlayerStatusWidget.h"

void UPlayerStatusWidget::NativeConstruct()
{
	Super::NativeConstruct();
	UpdatePlayerStatus();
}

void UPlayerStatusWidget::UpdatePlayerStatus()
{
	if (HealthText)
	{
		HealthText->SetText(FText::FromString(FString::Printf(TEXT("%d / 100"), PlayerStats.Vigor * 10)));
	}

	if (HealthProgressBar)
	{
		HealthProgressBar->SetPercent((float)(PlayerStats.Vigor * 10) / 100.0f);
	}

	if (ManaText)
	{
		ManaText->SetText(FText::FromString(FString::Printf(TEXT("%d / 100"), PlayerStats.Heart * 10)));
	}

	if (ManaProgressBar)
	{
		ManaProgressBar->SetPercent((float)(PlayerStats.Heart * 10) / 100.0f);
	}
}
