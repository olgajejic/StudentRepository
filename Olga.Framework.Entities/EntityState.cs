using System;
using System.Collections.Generic;
using System.Text;

namespace Olga.Framework.Entities
{
    public enum State
    {
        NEW,
        UNCHANGED,
        CHANGED,
        DELETED
    }

    public class EntityState
    {
        public EntityState(Entity entity, State state)
        {
            this.Entity = entity;
            this.EState = state;
        }
        public Entity Entity { get; set; }
        public State EState { get; set; }
        
    }
}
