using ProjectCard.Shared.SceneModule;
using ProjectCard.Shared.ServiceModule.SceneModule;
using ProjectCard.Shared.WindowModule;

using UnityEngine;

namespace ProjectCard.DurakModule.StateModule
{
    public class GameEndState : DurakState
    {
        [Header("Window")]
        [SerializeField] private DialogWindow gameRestartDialogWindow;

        [Header("Scene")]
        [SerializeField] private SceneLoadingService sceneLoadingService;
        [SerializeField] private Transform playerDeck;

        public override async void Enter()
        {
            base.Enter();

            var result = await gameRestartDialogWindow.Show();

            if (playerDeck.childCount == 0)
                Debug.Log("Player win !");
            else
                Debug.Log("Enemy win !");
        }
    }
}