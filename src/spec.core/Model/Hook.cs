using System;

namespace spec.core.Model
{
  [Serializable]
  public class Hook : Definition
  {
  }

    public class GlobalHook : Hook
    {
        public GlobalHookKind Kind { get; set; }
    }

    public enum GlobalHookKind
    {
        BeforeAll,
        AfterAll
    }
}