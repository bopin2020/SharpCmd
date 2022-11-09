using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpCmd.Contract
{
    internal abstract class Command
    {
        private IList<IContract> observers = new List<IContract>();

        public void Register(IContract observer)
        {
            observers.Add(observer);
        }
        public void UnRegister(IContract observer)
        {
            observers.Remove(observer);
        }
        public void Notify(Dictionary<string, string> arguments)
        {
            if(arguments.Keys.Select(x => String.Compare(x,"help",true) == 0).FirstOrDefault())
            {
                Console.WriteLine("{0,-20} {1,-20}","Command","Description\n");
                foreach (var item in observers)
                {
                    Console.WriteLine("{0,-20} {1,-20}",item.CommandName, item.Description);
                }
            }
            foreach (var item in observers)
            {
                if (item != null && arguments.Keys.ToArray()[0] == item.CommandName)
                {
                    item.Execute(arguments);
                }
            }
        }
    }
}
