using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NppKate.Common
{
    public interface IDockDialog
    {
        void init(IDockableManager manager, int commandId);
    }
}
