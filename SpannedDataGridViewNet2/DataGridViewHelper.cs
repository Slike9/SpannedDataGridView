using System.Windows.Forms;

namespace SpannedDataGridViewNet2
{
    static class DataGridViewHelper
    {
        public static bool SingleHorizontalBorderAdded(DataGridView dataGridView)
        {
            return !dataGridView.ColumnHeadersVisible &&
                (dataGridView.AdvancedCellBorderStyle.All == DataGridViewAdvancedCellBorderStyle.Single ||
                 dataGridView.CellBorderStyle == DataGridViewCellBorderStyle.SingleHorizontal);
        }

        public static bool SingleVerticalBorderAdded(DataGridView dataGridView)
        {
            return !dataGridView.RowHeadersVisible &&
                (dataGridView.AdvancedCellBorderStyle.All == DataGridViewAdvancedCellBorderStyle.Single ||
                 dataGridView.CellBorderStyle == DataGridViewCellBorderStyle.SingleVertical);
        }
    }
}