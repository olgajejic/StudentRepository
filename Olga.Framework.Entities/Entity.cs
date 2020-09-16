using System;

namespace Olga.Framework.Entities
{
    public enum State
    {
        INSERTED,
        UPDATED,
        DELETED
    }

    public abstract class Entity : IEntity
    {
        public long ID { get; set; }

        public State EntityState { get; set; }
    }
}
