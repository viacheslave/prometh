using System.Reflection;

namespace Prometh.Tests;

public class AssemblyResources
{
  public string GetResourcePayload(string resource)
  {
    var resourceFullName = $"{Assembly.GetExecutingAssembly().GetName().Name}.samples.{resource}";

    var stream = Assembly.GetExecutingAssembly()
      .GetManifestResourceStream(resourceFullName);
    
    using var sr = new StreamReader(stream);

    return sr.ReadToEnd();
  }
}
