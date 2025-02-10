using System.Collections.ObjectModel;
using ImageChecker_3.Models.Images;

namespace ImageChecker_3.Models
{
    public class PreviewContainerHistory
    {
        public PreviewContainerHistory()
        {
            OriginalItems = new ObservableCollection<PreviewContainer>();
            Items = new ReadOnlyObservableCollection<PreviewContainer>(OriginalItems);
        }

        public ReadOnlyObservableCollection<PreviewContainer> Items { get; set; }

        private ObservableCollection<PreviewContainer> OriginalItems { get; set; }

        public void Add(PreviewContainer item)
        {
            OriginalItems.Add(item);
        }

        public void Insert(int index, PreviewContainer item)
        {
            OriginalItems.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            OriginalItems.RemoveAt(index);
        }
    }
}