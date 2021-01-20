using System;

namespace spec.console
{
	/// <summary>
	/// Isolates code execution into separate AppDomain.
	/// </summary>
	/// <typeparam name="T">A class that contain the code that should run isolated.</typeparam>
	public class Sandbox<T> : IDisposable where T : DomainProxy
	{
		/// <summary>
		/// Initializes a new Sandbox class and loads an assembly into it.
		/// </summary>
		/// <param name="assemblyPath">An assembly to load into the new AppDomain.</param>
		public Sandbox(string assemblyPath)
		{
			var xxx = System.Runtime.Loader.AssemblyLoadContext.Default;
			var type = typeof(T);
			
			var assembly = xxx.LoadFromAssemblyPath(type.Assembly.Location);
			Content =  assembly.CreateInstance(type.FullName) as T;
			Content.Load(assemblyPath);
		}

		/// <summary>
		/// A sandboxed object.
		/// </summary>
		public T Content { get; private set; }

		public void Dispose()
		{
			this.Content = null;
		}
	}
}
