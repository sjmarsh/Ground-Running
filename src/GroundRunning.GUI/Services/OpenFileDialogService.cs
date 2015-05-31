using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace GroundRunning.GUI.Services
{
    public interface IOpenFileDialogService
    {

    }

    public class OpenFileDialogService
    {
        private OpenFileDialog _openFileDialog;
        private string _filePath;

        public OpenFileDialogService()
        {
            _openFileDialog = new OpenFileDialog();
        }

        public void ShowDialog(string fileTypeFilter, string initialDirectory = "")
        {
            _openFileDialog.Multiselect = false;
            _openFileDialog.InitialDirectory = initialDirectory;
            _openFileDialog.Filter = fileTypeFilter;

            var dialogResult = _openFileDialog.ShowDialog();
            
            if(dialogResult.HasValue)
            {
                _filePath = _openFileDialog.FileName;
            }
        }

        public string FilePath 
        {
            get
            {
                return _filePath;
            }
        }
    }
}
