using ElectroneumSpace.Constants;
using System;
using Xamarin.Forms;

namespace ElectroneumSpace.Selectors
{
    public class HomeSectionSelector : DataTemplateSelector
    {

        public DataTemplate PoolTemplate { get; set; }

        public DataTemplate StatsTemplate { get; set; }

        public DataTemplate BlocksTemplate { get; set; }

        public DataTemplate SupportTemplate { get; set; }

        public DataTemplate SettingsTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            if (item is HomeSection section)
            {
                switch (section)
                {
                    case HomeSection.Home:
                        return PoolTemplate;

                    case HomeSection.Stats:
                        return StatsTemplate;

                    case HomeSection.Blocks:
                        return BlocksTemplate;

                    case HomeSection.Support:
                        return SupportTemplate;

                    case HomeSection.Settings:
                        return SettingsTemplate;
                }
            }

            return PoolTemplate;
        }
    }
}
