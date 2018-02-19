using GalaSoft.MvvmLight.Command;
using LiteParcer.Logik;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using static LiteParcer.Logik.Assets;

namespace LiteParcer.ViewModels
{
    class CounterModel : MainVM
    {

        public CounterModel()
        {
            URLsCount = new ObservableCollection<UrlCount>();
            URLsCount.CollectionChanged += (obj, evnt) => NotifyPropertyChanged("MaxItem");
            Progress = 0;
            TagSearch = "a";
        }

        public ObservableCollection<UrlCount> URLsCount { get; set; }
        public int MaxItem => URLsCount.Any() ? URLsCount.Max((c) => c.Count) : 0;

        private ICommand _openFileDialog;
        private ICommand _analyze;
        private ICommand _clear;

        private string Path = string.Empty;
        private string[] EAdress;

        public string Message { get; set; }
        public string TagSearch{get; set;}

        public double TotalProgressBarValue { get; set; }
        public double Progress { get; set; }



        public ICommand OpenFileDialog
        {
            get
            {
                return _openFileDialog ?? (_openFileDialog = new RelayCommand(()=>
                {
                    FileDialogSearch();
                }));
            }
        }
        public ICommand Analyze
        {
            get
            {
                return _analyze ?? (_analyze = new RelayCommand(() => 
                {
                   AnalyzeList();
                }));
            }
        }
        public ICommand Clear
        {
            get
            {
                return _clear ?? (_clear = new RelayCommand(()=> 
                {
                    ClearList();
                }));
            }
        }

        private async Task AnalyzeList()
        {
            LineCollection();
            if (EAdress != null)
            {
                Html_Analyzer analyzer = new Html_Analyzer(TagSearch);
                foreach (var url in EAdress)
                {
                    var UrlCountResultCollection = await analyzer.TagSearchAsync(url);
                    if (UrlCountResultCollection != null)
                    {
                        URLsCount.Add(new UrlCount
                        {
                            Url = UrlCountResultCollection.Url,
                            Count = UrlCountResultCollection.Count,
                            Tag = TagSearch,
                        });
                        Progress++;
                        NotifyPropertyChanged("Progress");
                    }
                }
                Progress = 0;
                NotifyPropertyChanged("Progress");
                Message = "Aналіз завершений";
            }
            else
            Message = "Файл з URL-адресою не выбран";
            NotifyPropertyChanged("Message");
        }
        private void FileDialogSearch()
        {
            Microsoft.Win32.OpenFileDialog Dialog = new Microsoft.Win32.OpenFileDialog();
            Dialog.Filter = "текстові файли (*.txt)|*.txt;";
            bool? result = Dialog.ShowDialog();
            if (result == true)
            {
                Path = Dialog.FileName;
            }
        }
        private void LineCollection()
        {
            if (!string.IsNullOrWhiteSpace(Path))
            {
                EAdress = File.ReadAllLines(Path);
                TotalProgressBarValue = EAdress.Length;
                Message = string.Empty;
            }
            else
            {
                Message = "текстовий файл не обраний";
            }
            NotifyPropertyChanged("Message");
            NotifyPropertyChanged("TotalProgressBarValue");
        }
        private void ClearList()
        {
            if (URLsCount.Count != 0)
            {
                URLsCount.Clear();
                Message = "список очищений!";
            }
            else
             Message = "нічого видаляти";
             NotifyPropertyChanged("Message");
        }
    }
}
