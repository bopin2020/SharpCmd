using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace SharpCmd.Lib.ArgsParser
{
    public static class ArgumentParser
    {
        public static ArgumentParserResult Parse(IEnumerable<string> args)
        {
            var arguments = new Dictionary<string, string>();
            try
            {
                foreach (var argument in args)
                {
                    if(argument.StartsWith("\"") && argument.EndsWith("\""))
                    {
                        var buf = argument.Trim('"');
                        arguments[buf] = string.Empty;
                        continue;
                    }

                    var idx = argument.IndexOf(':');
                    if (idx > 0)
                    {
                        arguments[argument.Substring(0, idx)] = argument.Substring(idx + 1);
                    }
                    else
                    {
                        idx = argument.IndexOf('=');
                        if (idx > 0)
                        {
                            arguments[argument.Substring(0, idx)] = argument.Substring(idx + 1);
                        }
                        else
                        {
                            arguments[argument] = string.Empty;
                        }
                    }
                }

                return ArgumentParserResult.Success(arguments);
            }
            catch (System.Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return ArgumentParserResult.Failure();
            }
        }
    }
}
