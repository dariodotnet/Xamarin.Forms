using System.Reflection;

#if DEBUG
[assembly: AssemblyConfiguration("DEBUG")]
#endif
#if RELEASE
[assembly: AssemblyConfiguration ("RELEASE")]
#endif

[assembly: AssemblyCompany("Xamarin Inc.")]
[assembly: AssemblyProduct("Xamarin.Forms")]
[assembly: AssemblyCopyright("Copyright © Xamarin Inc. 2013-2019")]
[assembly: AssemblyTrademark("")]
#pragma warning disable CS0436  // Type conflicts with imported type

[assembly: AssemblyVersion(
	  ThisAssembly.Git.BaseVersion.Major + "."
	+ ThisAssembly.Git.BaseVersion.Minor + "."
	+ "0" + "."
	+ "0")]

[assembly: AssemblyFileVersion(
	  ThisAssembly.Git.BaseVersion.Major + "."
	+ ThisAssembly.Git.BaseVersion.Minor + "."
	+ ThisAssembly.Git.BaseVersion.Patch + "."
	+ ThisAssembly.Git.Commits)]

[assembly: AssemblyInformationalVersion(
	ThisAssembly.Git.BaseVersion.Major + "."
	+ ThisAssembly.Git.BaseVersion.Minor + "."
	+ ThisAssembly.Git.BaseVersion.Patch + "."
	+ ThisAssembly.Git.Commits + "-"
	+ ThisAssembly.Git.Branch + "+"
	+ ThisAssembly.Git.Commit)]
#pragma warning restore CS0436 // Type conflicts with imported type