using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GroundRunning.GUI.Services
{
    public interface IFolderBrowserService
    {
        void ShowDialog();
        string FolderPath { get; }
    }

    public class FolderBrowserService : IFolderBrowserService
    {
        private FolderBrowserDialog _dialog;
        private DialogResult _dialogResult;
        private string _folderPath;

        public FolderBrowserService()
        {
            _dialog = new System.Windows.Forms.FolderBrowserDialog();
        }

        public void ShowDialog()
        {
            _dialogResult = _dialog.ShowDialog();
            if(_dialogResult == DialogResult.OK)
            {
                _folderPath = _dialog.SelectedPath;
            }
        }

        public string FolderPath
        {
            get { return _folderPath;  }
        }
    }
}
