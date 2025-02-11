using Prism.Mvvm;

namespace ImageChecker_3.Models.Tags
{
    /// <summary>
    /// PreviewContainer の履歴をフィルタリングする際の条件を保持するためのクラスです。
    /// </summary>
    public class HistoryFilterConditions : BindableBase
    {
        private bool includeImageTag = true;
        private bool includeDrawTag = true;
        private bool includeAnimationImageTag = true;
        private bool includeAnimationDrawTag = true;

        public bool IncludeImageTag { get => includeImageTag; set => SetProperty(ref includeImageTag, value); }

        public bool IncludeDrawTag { get => includeDrawTag; set => SetProperty(ref includeDrawTag, value); }

        public bool IncludeAnimationImageTag
        {
            get => includeAnimationImageTag;
            set => SetProperty(ref includeAnimationImageTag, value);
        }

        public bool IncludeAnimationDrawTag
        {
            get => includeAnimationDrawTag;
            set => SetProperty(ref includeAnimationDrawTag, value);
        }

        public void SetAllFlags(bool flag)
        {
            IncludeImageTag = flag;
            IncludeDrawTag = flag;
            IncludeAnimationImageTag = flag;
            IncludeAnimationDrawTag = flag;
        }
    }
}