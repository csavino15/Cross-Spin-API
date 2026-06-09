// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = "<Pending>", Scope = "member", Target = "~M:Domain.Abstractions.Result`1.op_Implicit(`0)~Domain.Abstractions.Result{`0}")]
[assembly: SuppressMessage("Design", "CA1040:Avoid empty interfaces", Justification = "Marker interface by design", Scope = "type", Target = "~T:Domain.Abstractions.IDomainEvent")]
[assembly: SuppressMessage("Design", "CA1030:Consider making 'RaiseDomainEvent' an event", Justification = "Domain event raising method by design", Scope = "member", Target = "~M:Domain.Abstractions.Entity.RaiseDomainEvent(Domain.Abstractions.IDomainEvent)")]
[assembly: SuppressMessage("Design", "CA1812:Avoid uninstantiated internal classes", Justification = "Instantiated via dependency injection")]
[assembly: SuppressMessage("Performance", "CA1848:Use LoggerMessage delegates", Justification = "Pending optimization")]
[assembly: SuppressMessage("Reliability", "CA2007:Consider calling ConfigureAwait", Justification = "ASP.NET Core does not require ConfigureAwait")]
[assembly: SuppressMessage("Usage", "CA2227:Collection properties should be read only", Justification = "DTO requires settable properties for deserialization")]