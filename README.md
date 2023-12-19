# AutoArchive
Sitecore Schedule Task that Itereates through all items in the content tree as it specified in the StartPath variable e.g "/sitecore/content/Home" to check the number of versions item/language, if the number is more than the limit that was set in the VersionLimit variable, then it sets the archive date for the versions that was created in the begining, and then the build-in archive job from Sitecore will move them to the arhive folder.

For running this task on certin servers only e.g: CM, you can create your own DatabaseAgent configuration, and add this path configuration to that server only e.g:

```<agent type="Sitecore.Tasks.DatabaseAgent" method="Run" interval="01:00:00">
<param desc="database">web</param>
<param desc="schedule root">/sitecore/system/tasks/schedules/CM</param>
<LogActivity>true</LogActivity>
</agent>
