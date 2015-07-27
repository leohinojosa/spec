using System;
using System.IO;
using System.Linq;

namespace spec.console
{
	/// <summary>
	/// Isolates code execution into separate AppDomain.
	/// </summary>
	/// <typeparam name="T">A class that contain the code that should run isolated.</typeparam>
	class Sandbox<T> : IDisposable where T : DomainProxy
	{
		private AppDomain domain;

		/// <summary>
		/// Initializes a new Sandbox class and loads an assembly into it.
		/// </summary>
		/// <param name="assemblyPath">An assembly to load into the new AppDomain.</param>
		public Sandbox(string assemblyPath)
		{
			var assemblyDirectory = new DirectoryInfo(Path.GetDirectoryName(assemblyPath));
			var projectDirectory = assemblyDirectory.Parent.Parent;

			var solutionDirectory = FindPackagesDirectory(projectDirectory);

			// Can only continue if valid test project
			if (string.IsNullOrEmpty(solutionDirectory)) return;

			var privateBinPath = string.Join(";",
				assemblyDirectory.FullName.Remove(0, solutionDirectory.Length + 1));

			var setup = new AppDomainSetup
			{
				ShadowCopyFiles = "true",
				LoaderOptimization = LoaderOptimization.MultiDomain,
				ApplicationBase = solutionDirectory,
				PrivateBinPath = privateBinPath
			};
			
			this.domain = AppDomain.CreateDomain("tests-sandbox", null, setup);
			
			var type = typeof(T);
			this.Content = this.domain.CreateInstanceFromAndUnwrap(type.Assembly.Location, type.FullName) as T;
			this.Content.Load(assemblyPath);
		}

		
		/// <summary>
		/// A sandboxed object.
		/// </summary>
		public T Content { get; private set; }

		public void Dispose()
		{
			this.Content = null;
			if (domain != null) AppDomain.Unload(domain);
		}

		private string FindPackagesDirectory(DirectoryInfo projectDirectory)
		{
			if (projectDirectory == null) return null;
			var anyDirectories = projectDirectory.EnumerateDirectories("packages").Any();
			return anyDirectories
				? projectDirectory.FullName
				: FindPackagesDirectory(projectDirectory.Parent);
		}
	}
}
