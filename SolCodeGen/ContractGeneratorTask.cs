using Microsoft.Build.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SolCodeGen
{
    public class ContractGeneratorTask : Task
    {
        public string MyProperty { get; set; }

        public override bool Execute()
        {
            Log.LogWarning("Example mesage\n\n\n\n\n\nNEW LINE");
            return true;
        }

        
    }
}
