using MelonLoader;
using System.Reflection;
using AutomaticRematch;
// ...
[assembly: MelonInfo(typeof(AutomaticRematch.Main), AutomaticRematch.Main.BuildInfo.ModName, AutomaticRematch.Main.BuildInfo.ModVersion, AutomaticRematch.Main.BuildInfo.Author)]
[assembly: MelonGame("Buckethead Entertainment", "RUMBLE")]
[assembly: MelonColor(255, 195, 0, 255)]
[assembly: MelonAuthorColor(255, 195, 0, 255)]
[assembly: VerifyLoaderVersion(0, 6, 6, true)]
[assembly: AssemblyCopyright("Copyright ©  2025")]
