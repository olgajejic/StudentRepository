using System;

namespace Olga.Framework.Entities
{

    public abstract class Entity : IEntity
    {
        public long ID { get; set; }

        public enum State
        {
            INSERTED,
            UPDATED,
            DELETED
        }
    }
}
