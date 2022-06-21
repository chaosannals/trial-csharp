﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Interop;
using Microsoft.Tools.WindowsInstallerXml.Bootstrapper;

namespace Wix3BootstrapperNF40.UI
{
    public class BootstrapperApplicationModel
    {
        private IntPtr hwnd;
        public BootstrapperApplicationModel(BootstrapperApplication bootstrapperApplication)
        {
            BootstrapperApplication = bootstrapperApplication;

            hwnd = IntPtr.Zero;
        }

        public BootstrapperApplication BootstrapperApplication
        {
            get;
            private set;
        }

        public int FinalResult { get; set; }

        public void SetWindowHandle(Window view)
        {
            hwnd = new WindowInteropHelper(view).Handle;
        }

        public void PlanAction(LaunchAction action)
        {
            BootstrapperApplication.Engine.Plan(action);
        }

        public void ApplyAction()
        {
            BootstrapperApplication.Engine.Apply(hwnd);
        }

        public void LogMessage(string message)
        {
            BootstrapperApplication.Engine.Log(LogLevel.Standard, message);
        }

        public void SetBurnVariable(string variableName, string value)
        {
            BootstrapperApplication.Engine.StringVariables[variableName] = value;
        }
    }
}
