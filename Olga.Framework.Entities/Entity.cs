using MessagePack;
using System;

namespace Olga.Framework.Entities
{
   [MessagePackObject(keyAsPropertyName: true)]
    public abstract class Entity : IEntity
    {
        
       // [Key(0)]
        public long ID { get; set; }
       // [Key(1)]
        public int VersionNumber { get; set; }
    }
}
