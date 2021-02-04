using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StringFormulaCalculator
{
    public class DisplayFormulaEventArgs : EventArgs
    {
        public string Formula { get; set; }
    }
}
