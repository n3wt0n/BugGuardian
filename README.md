# BugGuardian

Easily track you exceptions on VSO and TFS
------------------------------------------
**BugGuardian** is a library, written in C# like Shared Project, that allows to easily create a Bug work item on your *Visual Studio Online* account or on your on-premises *Team Foundation Server* in the case your application throws an Unhandled Exception.
It can also be invoked manually in try/catch blocks to keep track of handled exceptions.

It supports projects with .Net Framework 4.0 and above.
It supports also UWP apps (Windows 10) and .Net Core.

###Installation###

The **BugGuardian** library is available on [NuGet](https://www.nuget.org/packages/DBTek.BugGuardian).
Just search *BugGuardian* in the **Package Manager GUI** or run the following command in the **Package Manager Console**:
Install-Package DBTek.BugGuardian

**WARNING*: If you are experiencing an error like *BugGuardian already has a dependency defined for XXX*, update your NuGet client to the latest version
    
###Usage###

Refer to the [project documentation](https://github.com/n3wt0n/BugGuardian/wiki/Home) to find examples about how to use this library. You can also find some code samples in the **TestApps** folder.


###Support###

If you encounter some issues trying this library, please let me know and I'll fix the problem as soon as possible!

Please keep in mind that this library is currently in preview.
