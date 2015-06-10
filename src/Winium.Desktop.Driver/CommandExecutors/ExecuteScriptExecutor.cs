﻿namespace Winium.Desktop.Driver.CommandExecutors
{
    #region using

    using System;
    using System.Linq;
    using System.Collections.Generic;

    using WindowsInput.Native;

    using Newtonsoft.Json.Linq;

    using Winium.Cruciatus.Extensions;
    using Winium.StoreApps.Common;

    #endregion

    internal class ExecuteScriptExecutor : CommandExecutorBase
    {
        #region Methods

        protected override string DoImpl()
        {
            var script = this.ExecutedCommand.Parameters["script"].ToString();

            var prefix = string.Empty;
            string command;

            var index = script.IndexOf(':');
            if (index == -1)
            {
                command = script;
            }
            else
            {
                prefix = script.Substring(0, index);
                command = script.Substring(++index).Trim();
            }

            switch (prefix)
            {
                case "input":
                    this.ExecuteInputScript(command);
                    break;
                default:
                    var msg = string.Format("Unknown script command '{0} {1}'", prefix, command);
                    return this.JsonResponse(ResponseStatus.JavaScriptError, msg);
            }

            return this.JsonResponse();
        }

        private void ExecuteInputScript(string command)
        {
            var args = (JArray)this.ExecutedCommand.Parameters["args"];
            var elementId = args[0]["ELEMENT"].ToString();

            var element = this.Automator.Elements.GetRegisteredElement(elementId);
            
            // for backward compatibility, delete after refactoring
            if (command == "ctrl_click") {
               command = "control+click"; 
            }
            
            var commandKeys = command.Split('+');
            command = commandKeys.Last();
            var keys = new List<VirtualKeyCode>();

            // maybe there is a more concise way
            // key aliases? (ctrl => control) 
            foreach(var key in commandKeys)
            {
                VirtualKeyCode tmpKey;

                if (Enum.TryParse<VirtualKeyCode>(key, true, out tmpKey))
                {
                    keys.Add(tmpKey);
                }
            }

            switch (command)
            {
                case "click":
                    element.ClickWithPressedKeys(keys);
                    return;
                default:
                    throw new NotImplementedException(string.Format("Input-command {0} is not implemented", command));
            }
        }

        #endregion
    }
}
