using System.Linq;
using UsedParts.UI.Controls;
using UsedParts.UI.Interactions;

namespace UsedParts.UI.Views
{
    public partial class MainPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void OnReloadClick(object sender, System.EventArgs e)
        {
            FixListBoxes();
        }

        private void FixListBoxes()
        {
            var listboxes = LayoutRoot.Descendants<InfiniteListBox>();
            var index = Items.SelectedIndex;
            if (listboxes.Count() <= index) 
                return;
            var listbox = listboxes.ElementAt(index);
            var first = listbox.Items.FirstOrDefault();
            if (first != null)
                listbox.ScrollIntoView(first);
        }

    }
}