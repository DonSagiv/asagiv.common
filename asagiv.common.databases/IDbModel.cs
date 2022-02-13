using System;
using System.Collections.Generic;
using System.Text;

namespace asagiv.common.databases
{
    public interface IDbModel<TDatabseIdentifier>
    {
        public TDatabseIdentifier Id { get; set; }
    }
}
