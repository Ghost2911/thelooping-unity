﻿using System.Collections.Generic;

namespace ProjectCard.Shared.CollectionModule
{
    public interface IReadonlyBoard<T> : IReadOnlyCollection<T>
    {
        IReadOnlyList<T> All { get; }
        IReadOnlyList<T> Attacks { get; }
        IReadOnlyList<T> Defends { get; }

        bool IsAttacksPlace { get; }
        bool IsDefendsPlace { get; }

        bool IsEmpty { get; }
        bool IsFull { get; }
        bool IsAttacksFull { get; }
        bool IsDefendsFull { get; }
    }
}
