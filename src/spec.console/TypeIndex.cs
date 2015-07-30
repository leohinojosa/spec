using System;
using System.Collections.Generic;
using System.Linq;
using spec.core;
using spec.core.Model;

namespace spec.console
{
  public class TypeIndex
  {
    //TODO remove duplicate query
    public static Type[] TargetTypesToRun(IEnumerable<DefinitionSource> typesSources, Type[] availableTypes)
    {
      var targetTypesToRun = availableTypes
        .Where(t => !t.IsAbstract && t.IsSubclassOf(typeof (Spec)))
        .Where(t => typesSources.Select(x => x.ClassName).Distinct().Contains(t.FullName, StringComparer.OrdinalIgnoreCase))
        .Select(t => t)
        .ToArray();
      return targetTypesToRun;
    }

    public static Type[] TargetTypesToRun(Type[] availableTypes)
    {
      var targetTypesToRun = availableTypes
        .Where(t => !t.IsAbstract &&  t.IsSubclassOf(typeof(Spec)))
        .Select(t => t)
        .ToArray();
      return targetTypesToRun;
    }

  }
}