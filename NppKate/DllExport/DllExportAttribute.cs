// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
using System;
using System.Runtime.InteropServices;

namespace NppPlugin.DllExport
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    sealed partial class DllExportAttribute : Attribute
    {
        public DllExportAttribute()
        {
        }
        public DllExportAttribute(string exportName)
            : this(exportName, CallingConvention.StdCall)
        {
        }
        public DllExportAttribute(string exportName, CallingConvention callingConvention)
        {
            ExportName = exportName;
            CallingConvention = callingConvention;
        }
        CallingConvention _callingConvention;
        public CallingConvention CallingConvention
        {
            get { return _callingConvention; }
            set { _callingConvention = value; }
        }
        string _exportName;
        public string ExportName
        {
            get { return _exportName; }
            set { _exportName = value; }
        }
    }
}