using MelonLoader;
using OldMate;
using System.Reflection;
using System.Runtime.InteropServices;

[assembly: AssemblyTitle(OldMate.BuildInfo.Name)]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany(OldMate.BuildInfo.Company)]
[assembly: AssemblyProduct(OldMate.BuildInfo.Name)]
[assembly: AssemblyCopyright("Created by " + OldMate.BuildInfo.Author)]
[assembly: AssemblyTrademark(OldMate.BuildInfo.Company)]
[assembly: AssemblyCulture("")]
[assembly: ComVisible(false)]
//[assembly: Guid("")]
[assembly: AssemblyVersion(OldMate.BuildInfo.Version)]
[assembly: AssemblyFileVersion(OldMate.BuildInfo.Version)]

[assembly: MelonInfo(typeof(OldMateMod), OldMate.BuildInfo.Name, OldMate.BuildInfo.Version, OldMate.BuildInfo.Author, OldMate.BuildInfo.DownloadLink)]
[assembly: MelonGame("VRChat", "VRChat")]