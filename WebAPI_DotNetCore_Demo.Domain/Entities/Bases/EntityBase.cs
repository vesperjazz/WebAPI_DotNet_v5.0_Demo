using System;

namespace WebAPI_DotNetCore_Demo.Domain.Entities.Bases
{
    public abstract class EntityBase
    {
        public Guid? ID { get; set; }
    }
}
