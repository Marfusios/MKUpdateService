using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MKUpdateService.Update
{
    public class InformationEventArgs : EventArgs
    {
        public string Msg { get; set; }

        public InformationEventArgs(string msg)
        {
            Msg = msg;
        }
    }
}
