﻿using SharpCmd.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpCmd.ConcreteCommand.Recon
{
    internal class ping : IContract
    {
        public string CommandName => "ping";

        public void Execute(Dictionary<string, string> arguments)
        {

        }
    }
}