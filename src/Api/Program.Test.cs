namespace Api;
// Require a public Program class to implement the
// fixture for the WebApplicationFactory in the
// integration tests. Using IVT is not sufficient
// in this case, because the accessibility of the
// `Program` type is checked.
#pragma warning disable S3903
#pragma warning disable S1118
// ReSharper disable once CheckNamespace
// ReSharper disable once UnusedType.Global
public partial class Program { }
#pragma warning restore S1118
#pragma warning restore S3903
