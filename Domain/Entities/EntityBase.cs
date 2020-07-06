using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public enum RowState
    {
        Normal, Add, Edit, Delete
    }
    public abstract class EntityBase
    {
        public string Guid { get; private set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedProgram { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedProgram { get; set; }
        public uint? RowVersion { get; set; }
        public RowState? RowState { get; set; }

        public EntityBase()
        {
            this.Guid = System.Guid.NewGuid().ToString();
            this.RowState = Entities.RowState.Normal;
        }
    }
}
