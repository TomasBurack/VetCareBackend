using System;
using System.Collections.Generic;
using System.Text;

namespace VetCareBackend.Domain.Entities
{
    public abstract class BaseEntity
    {
        public Guid Id { get; set; }
        public bool IsDeleted { get; set; }
        public DateTimeOffset UpdateDate { get; set; }
        public DateTimeOffset DeleteDate { get; set; }

    }
}
