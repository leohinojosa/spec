using System;
using System.Reflection;

namespace spec.console
{
	/// <summary>
	/// A base class for any piece of code that should run in a Sandbox.
	/// All input and output in derived classes should be [Serializable] or inherit MarshalByRefObject.
	/// </summary>
	public abstract class DomainProxy : MarshalByRefObject
	{
		public string Source { get; private set; }

		protected Assembly SandboxedAssembly { get; private set; }

		public void Load(string path)
		{
			this.Source = path;
			this.SandboxedAssembly = Assembly.LoadFrom(path);
		}
	}
}
