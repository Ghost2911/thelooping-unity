﻿
using ProjectCard.DurakModule.EntityModule;
using ProjectCard.DurakModule.GameplayModule;

using UnityEngine;


namespace ProjectCard.DurakModule.StateModule.BattleModule
{
    public class BattleStartState : DurakState
    {
        [Header("Players")]
        [SerializeField] private PlayerStorageEntity playerStorage;
        [SerializeField] private PlayerQueueEntity playerQueue;

        [Header("Gameplay")]
        [SerializeField] private CardDealer dealer;

        [Header("Shared")]
        [SerializeField] private DeckEntity deck;

        public override async void Enter()
        {
            base.Enter();

            await dealer.DealCard();

            Debug.Log(nameof(BattleStartState));

            NextState(DurakGameState.PlayerAttacking);
        }
    }
}