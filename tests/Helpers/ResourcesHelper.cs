using System.Reflection;

namespace Prometh.Tests;

internal static class ResourcesHelper
{
  internal static string GetResourcePayload(string resource)
  {
    var resourceFullName = $"{Assembly.GetExecutingAssembly().GetName().Name}.samples.{resource}";

    var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceFullName);
    using var sr = new StreamReader(stream);

    return sr.ReadToEnd();
  }

  internal static IEnumerable<string> GetResources()
  {
    var resourceNames = Assembly.GetExecutingAssembly().GetManifestResourceNames()
      .Where(r => r.EndsWith(".data"));

    return resourceNames;
  }
}
