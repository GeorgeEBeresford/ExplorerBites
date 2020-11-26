using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExplorerBites.Models.ViewModels
{
    public class RootViewModel
    {
        public RootViewModel()
        {
            ObservableDrives = new ObservableCollection<DirectoryViewModel>();
            InitialiseDrives();
        }

        private void InitialiseDrives()
        {
            IEnumerable<DirectoryViewModel> initialDrives =
                Directory.GetDrives().Select(drive => new DirectoryViewModel(drive));

            foreach (DirectoryViewModel drive in initialDrives)
            {
                ObservableDrives.Add(drive);

                // The drive should have the directories ready to preview in the heirarchy
                drive.LoadDirectories();
            }
        }

        public ObservableCollection<DirectoryViewModel> ObservableDrives { get; }
    }
}
