// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Design", "CA1812:Avoid uninstantiated internal classes", Justification = "Instantiated via dependency injection")]
[assembly: SuppressMessage("Performance", "CA1848:Use LoggerMessage delegates", Justification = "Pending optimization")]
[assembly: SuppressMessage("Reliability", "CA2007:Consider calling ConfigureAwait", Justification = "ASP.NET Core does not require ConfigureAwait")]
[assembly: SuppressMessage("Design", "CA1031:Modify to catch a more specific exception", Justification = "Intentional catch-all in background job")]
