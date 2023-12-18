using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Data.Managers;
using Sitecore.Diagnostics;
using Sitecore.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sitecore.Foundation.AutoArchive
{
   
    public class ArchiveTask
    {
        // constants
        private const int VersionLimit = 10;
        private const string StartPath = "/sitecore/content/Home";
        public Data.Database MasterDatabase
        {
            get
            {
                return Sitecore.Configuration.Factory.GetDatabase("master");
            }

        }
        public void Execute(Item[] items, CommandItem commandItem, ScheduleItem schedule)
        {
            if (MasterDatabase != null)
            {
                IterateThroughContentTree(MasterDatabase, StartPath);
            }
        }
        public void IterateThroughContentTree(Sitecore.Data.Database database, string path)
        {
            // Get the root item of the content tree
            Item rootItem = database.GetItem(path);

            if (rootItem != null)
            {
                // Start the recursive iteration
                IterateItem(rootItem);
            }
            else
            {
                Log.Error("Root item not found!", this);
            }
        }
        private void IterateItem(Item item)
        {
            // Process the current item
            ProcessItem(item);

            // Check if the item has children
            if (item.HasChildren)
            {
                // Iterate through the children recursively
                foreach (Item childItem in item.Children)
                {
                    IterateItem(childItem);
                }
            }
        }
        private void ProcessItem(Item item)
        {
            foreach (var language in item.Languages)
            {
                // get the item lanaguage.
                var currentLanguageItem = MasterDatabase.GetItem(item.ID, language);
                if (currentLanguageItem.Versions.Count > VersionLimit)
                {
                    // get item with version
                    var currentLanguageVersionsItems = currentLanguageItem.Versions.GetVersions();

                    for (int i = 0; i < (currentLanguageItem.Versions.Count - VersionLimit); i++)
                    {
                        var toBeArchivedItem = currentLanguageVersionsItems[i];
                        if (toBeArchivedItem !=null && toBeArchivedItem.Fields["__Archive Version date"] !=null)
                        {
                            using (new Sitecore.SecurityModel.SecurityDisabler())
                            {
                                try
                                {
                                    // set archive date to current date time.
                                    var isoDate = DateUtil.ToIsoDate(DateTime.Now);
                                    toBeArchivedItem.Editing.BeginEdit();
                                    toBeArchivedItem.Fields["__Archive Version date"].Value = isoDate;
                                    toBeArchivedItem.Editing.EndEdit();
                                }
                                catch (Exception ex)
                                {
                                    toBeArchivedItem.Editing.CancelEdit();
                                    Log.Info("Archive Job: " + ex.InnerException, this);
                                }
                                
                                
                            }
                               
                        }

                    }

                }
            }
            
        }
    }
}
