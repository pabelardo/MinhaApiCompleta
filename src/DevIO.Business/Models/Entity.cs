using System;

namespace DevIO.Business.Models;

public abstract class Entity
{
    protected Entity()
    {
        ID = Guid.NewGuid();
    }

    public Guid ID { get; set; }
}