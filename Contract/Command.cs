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
                Dictionary<string, StringBuilder> dict = new Dictionary<string, StringBuilder>();
                foreach (var item in observers)
                {
                    try
                    {
                        Contractbase contractbase = item as Contractbase;
                        if(contractbase != null)
                        {
                            if(dict.ContainsKey(contractbase.ModuleName))
                            {
                                dict[contractbase.ModuleName].AppendLine(String.Format("\t{0,-20} {1,-20}", contractbase.CommandName, contractbase.Description));
                            }
                            else
                            {
                                StringBuilder sb = new StringBuilder();
                                sb.AppendLine(String.Format("\t{0,-20} {1,-20}", contractbase.CommandName, contractbase.Description));
                                dict.Add(contractbase.ModuleName,sb);
                            }
                        }
                        else
                        {
                            Console.WriteLine("{0,-20} {1,-20}", item.CommandName, item.Description);
                        }
                    }
                    catch (Exception)
                    {

                    }
                }
                Console.WriteLine("{0,-20} {1,-20} {2,-20}", "Module", "Command", "Description\n");
                foreach (var item in dict.OrderBy(i => i.Key))
                {
                    Console.WriteLine(item.Key);
                    Console.WriteLine(item.Value.ToString());
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
