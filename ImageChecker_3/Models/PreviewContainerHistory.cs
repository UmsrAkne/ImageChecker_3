using System.Collections.ObjectModel;
using System.Linq;
using ImageChecker_3.Images;
using ImageChecker_3.Tags;
using Prism.Commands;

namespace ImageChecker_3.Models
{
    public class PreviewContainerHistory
    {
        public PreviewContainerHistory()
        {
            OriginalItems = new ObservableCollection<PreviewContainer>();
            FilteredItems = new ObservableCollection<PreviewContainer>();
            Items = new ReadOnlyObservableCollection<PreviewContainer>(FilteredItems);
        }

        public ReadOnlyObservableCollection<PreviewContainer> Items { get; set; }

        public HistoryFilterConditions HistoryFilterConditions { get; set; } = new ();

        public DelegateCommand UpdateFilteredHistoryCommand => new DelegateCommand(() =>
        {
            FilteredItems.Clear();

            if (HistoryFilterConditions.IncludeImageTag)
            {
                FilteredItems.AddRange(OriginalItems.Where(h => h.TagType == TagType.Image));
            }

            if (HistoryFilterConditions.IncludeDrawTag)
            {
                FilteredItems.AddRange(OriginalItems.Where(h => h.TagType == TagType.Draw));
            }

            if (HistoryFilterConditions.IncludeAnimationImageTag)
            {
                FilteredItems.AddRange(OriginalItems.Where(h => h.TagType == TagType.AnimationImage));
            }

            if (HistoryFilterConditions.IncludeAnimationDrawTag)
            {
                FilteredItems.AddRange(OriginalItems.Where(h => h.TagType == TagType.AnimationDraw));
            }
        });

        public DelegateCommand ToggleFilterConditionsCommand => new DelegateCommand(() =>
        {
            var f1 = HistoryFilterConditions.IncludeImageTag;
            var f2 = HistoryFilterConditions.IncludeDrawTag;
            var f3 = HistoryFilterConditions.IncludeAnimationImageTag;
            var f4 = HistoryFilterConditions.IncludeAnimationDrawTag;

            if (f1 || f2 || f3 || f4)
            {
                HistoryFilterConditions.SetAllFlags(false);
            }
            else
            {
                HistoryFilterConditions.SetAllFlags(true);
            }

            UpdateFilteredHistoryCommand.Execute();
        });

        private ObservableCollection<PreviewContainer> OriginalItems { get; set; }

        private ObservableCollection<PreviewContainer> FilteredItems { get; set; }

        public void Add(PreviewContainer item)
        {
            OriginalItems.Add(item);
            UpdateFilteredHistoryCommand.Execute();
        }

        public void Insert(int index, PreviewContainer item)
        {
            OriginalItems.Insert(index, item);
            UpdateFilteredHistoryCommand.Execute();
        }

        public void RemoveAt(int index)
        {
            OriginalItems.RemoveAt(index);
            UpdateFilteredHistoryCommand.Execute();
        }
    }
}