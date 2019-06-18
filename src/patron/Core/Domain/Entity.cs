using System;

namespace Core.Domain
{
    public class Entity
    {

        protected internal Entity()
        {
        }
        public Entity(Guid id)
        {
            Id = id;
        }
        
        public Guid Id { get; private set; }
    }
}