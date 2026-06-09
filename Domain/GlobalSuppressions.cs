using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Design", "CA1040:Avoid empty interfaces", Justification = "Marker interfaces by design")]
[assembly: SuppressMessage("Design", "CA1812:Avoid uninstantiated internal classes", Justification = "Instantiated via dependency injection")]
[assembly: SuppressMessage("Performance", "CA1848:Use LoggerMessage delegates", Justification = "Pending optimization")]
[assembly: SuppressMessage("Reliability", "CA2007:Consider calling ConfigureAwait", Justification = "ASP.NET Core does not require ConfigureAwait")]
[assembly: SuppressMessage("Usage", "CA2227:Collection properties should be read only", Justification = "DTO requires settable properties for deserialization")]