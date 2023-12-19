# AutoArchive
Sitecore Schedule Task that itereates through all items in the content tree under the items specified in the StartPath variable e.g "/sitecore/content/Home" to check the number of versions for each language, if the number is more than the limit that was set in the VersionLimit variable, then it sets the archive date for the versions that was created in the begining, and then the build-in archive job from Sitecore will move them to the arhive folder the next time that Sitecore checks for items to archive.

For running this task on certin server only e.g: CM, you can create your own DatabaseAgent configuration, and add this path configuration to that server only e.g:

```<agent type="Sitecore.Tasks.DatabaseAgent" method="Run" interval="01:00:00">
<param desc="database">web</param>
<param desc="schedule root">/sitecore/system/tasks/schedules/CM</param>
<LogActivity>true</LogActivity>
</agent>
