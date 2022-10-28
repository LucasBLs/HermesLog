using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Hermes.Enums
{
  public enum LogType
  {
    [Description("LogInformation")]
    LogInformation,
    [Description("LogWarning")]
    LogWarning,
    [Description("LogError")]
    LogError,
    [Description("LogCritical")]
    LogCritical,
    [Description("LogFatal")]
    LogFatal
  }
}
