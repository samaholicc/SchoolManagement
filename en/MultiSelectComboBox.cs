using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace CustomControls
{
    public class MultiSelectComboBox : ComboBox
    {
        private CheckedListBox checkedListBox;
        private bool isDropdownOpen = false;

        public MultiSelectComboBox()
        {
            // Initialize the CheckedListBox
            checkedListBox = new CheckedListBox
            {
                CheckOnClick = true,  // Enable checking/unchecking on click
                IntegralHeight = false,  // Disable auto sizing of the listbox
                SelectionMode = SelectionMode.None,  // Disable selection behavior
            };

            // Event handler for when items are checked/unchecked
            checkedListBox.ItemCheck += CheckedListBox_ItemCheck;
        }

        // This method shows the CheckedListBox as the dropdown
        protected override void OnDropDown(EventArgs e)
        {
            base.OnDropDown(e);

            // Only show the dropdown once
            if (!isDropdownOpen)
            {
                isDropdownOpen = true;

                // Add the CheckedListBox to the parent container
                this.Parent.Controls.Add(checkedListBox);

                // Populate the CheckedListBox with items from the ComboBox
                checkedListBox.Items.Clear();
                foreach (var item in this.Items)
                {
                    checkedListBox.Items.Add(item);
                }

                // Position the CheckedListBox below the ComboBox
                Point comboBoxLocation = this.PointToScreen(Point.Empty);
                checkedListBox.Location = new Point(comboBoxLocation.X, comboBoxLocation.Y + this.Height);

                // Set the size of the CheckedListBox
                checkedListBox.Width = this.Width;
                checkedListBox.Height = Math.Min(checkedListBox.Items.Count * 20, 200); // Adjust the height

                // Bring the CheckedListBox to the front
                checkedListBox.BringToFront();
            }
        }

        // Close the dropdown and update ComboBox text when dropdown closes
        protected override void OnDropDownClosed(EventArgs e)
        {
            base.OnDropDownClosed(e);

            // Remove the CheckedListBox when dropdown closes
            this.Parent.Controls.Remove(checkedListBox);
            isDropdownOpen = false;

            // Update ComboBox text based on selected items
            string selectedItems = string.Join(", ", checkedListBox.CheckedItems.Cast<object>());
            this.Text = selectedItems;  // Set ComboBox text to selected items
        }

        // Event handler when items are checked or unchecked
        private void CheckedListBox_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            // Update ComboBox text as user selects/deselects items
            string selectedItems = string.Join(", ", checkedListBox.CheckedItems.Cast<object>());
            this.Text = selectedItems;
        }
    }
}
