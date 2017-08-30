using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace  Tesco.ClubcardProducts.MCA.Web.Common.Entities.Activation
{
    public abstract class EntityBase
    {
        private bool _isEmpty;

        public EntityBase()
        { }

        protected EntityBase(bool isEmpty)
        {
            this._isEmpty = isEmpty;
        }

        public bool IsEmpty
        {
            get { return this._isEmpty; }
            set { this._isEmpty = value; }
        }
    }
}
