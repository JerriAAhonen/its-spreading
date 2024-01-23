using System.Collections.Generic;
using UnityEngine;

public class GameState_LevelSelection : GameState
{
	[SerializeField] private LevelSelection_LevelButton prefab;
	[SerializeField] private Transform gridView;
	[SerializeField] private MenuButton back;

	private List<LevelSelection_LevelButton> levelButtons;

	private void Awake()
	{
		back.OnClick += OnBack;
	}

	#region GameState

	public override void Enter()
	{
		gameObject.SetActive(true);

		if (levelButtons.IsNullOrEmpty())
		{
			levelButtons = new(LevelDatabase.Get().MaxLevelIndex + 1);
			for (int i = 0; i < LevelDatabase.Get().MaxLevelIndex + 1; i++)
			{
				var button = Instantiate(prefab);
				button.transform.SetParent(gridView);
				button.Init(i);
				button.LevelSelected += OnLevelSelected;
				levelButtons.Add(button);
			}
		}

		LevelButtonState state;
		int nextLevelIndex = SaveDataManager.GetLastCompletedLevel() + 1;

		for (int i = 0; i < LevelDatabase.Get().MaxLevelIndex + 1; i++)
		{
			if (i < nextLevelIndex)
				state = LevelButtonState.Completed;
			else if (i == nextLevelIndex)
				state = LevelButtonState.Current;
			else
				state = LevelButtonState.Locked;
			
			levelButtons[i].RefreshState(state);
		}
	}

	public override void Exit()
	{
		gameObject.SetActive(false);
	}

	#endregion

	private void OnLevelSelected(int index)
	{
		if (index <= SaveDataManager.GetLastCompletedLevel() + 1)
		{
			// Play
			manager.CurrentLevelIndex = index;
			manager.Transition(GameStateType.Level);
			return;
		}
		else 
		{ 
			// Level locked
		}
	}

	private void OnBack()
	{
		manager.Transition(GameStateType.MainMenu);
	}
}
