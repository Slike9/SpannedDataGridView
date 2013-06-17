using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Globalization;

namespace SpannedDataGridView
{
    public partial class DemoForm : Form
    {
        #region ctor
        public DemoForm()
        {
            InitializeComponent();

            dataGridView1.RowCount = 30;
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    dataGridView1[j, i].Value = string.Format(CultureInfo.CurrentCulture,
                        "( {0}, {1} )", j, i);
                }
            }
            var cell = (DataGridViewTextBoxCellEx)dataGridView1[0, 0];
            cell.ColumnSpan = 3;
            cell.RowSpan = 2;
            dataGridView1.Rows[1].DividerHeight = 2;
            ((DataGridViewTextBoxCellEx) dataGridView1[1, 5]).ColumnSpan = 3;

            clmnColumnName.DataPropertyName = "HeaderText";
            clmnColumnVisible.DataPropertyName = "Visible";
            clmnColumnFrozen.DataPropertyName = "Frozen";
            dtGrdVwColumnsProperties.AutoGenerateColumns = false;
            dtGrdVwColumnsProperties.DataSource = dataGridView1.Columns.Cast<DataGridViewColumn>()
                .ToList();
            dtGrdVwColumnsProperties.ScrollBars = ScrollBars.None;

            clmnRowName.DataPropertyName = "Index";
            clmnRowVisible.DataPropertyName = "Visible";
            clmnRowFrozen.DataPropertyName = "Frozen";
            dtGrdVwRowsProperties.AutoGenerateColumns = false;
            dtGrdVwRowsProperties.DataSource = dataGridView1.Rows.Cast<DataGridViewRow>()
                .Select(item => new RowPresenter(item))
                .ToList();

            var imgCell = (DataGridViewImageCellEx)dataGridView1[4, 0];
            imgCell.ColumnSpan = 2;
            imgCell.RowSpan = 2;
        }
        #endregion

        #region Presenters
        public class CellPresenter
        {
            #region Properties
            [Browsable(false)]
            public DataGridViewCell Cell { get; private set; }
            public bool ReadOnly
            {
                get { return Cell.ReadOnly; }
                set { Cell.ReadOnly = value; }
            }
            public string Value
            {
                get { return (string)Cell.Value; }
                set { Cell.Value = value; }
            }
            public int ColumnSpan
            {
                get
                {
                    var cell = Cell as DataGridViewTextBoxCellEx;
                    if (cell != null)
                        return cell.ColumnSpan;
                    
                    var cell2 = Cell as DataGridViewImageCellEx;
                    if (cell2 != null)
                        return cell2.ColumnSpan;

                    return 1;
                }
                set
                {
                    var cell = Cell as DataGridViewTextBoxCellEx;
                    if (cell != null)
                        cell.ColumnSpan = value;

                    var cell2 = Cell as DataGridViewImageCellEx;
                    if (cell2 != null)
                        cell2.ColumnSpan = value;

                }
            }
            public int RowSpan
            {
                get
                {
                    var cell = Cell as DataGridViewTextBoxCellEx;
                    if (cell != null)
                        return cell.RowSpan;
                    var cell2 = Cell as DataGridViewImageCellEx;
                    if (cell2 != null)
                        return cell2.RowSpan;

                    return 1;
                }
                set
                {
                    var cell = Cell as DataGridViewTextBoxCellEx;
                    if (cell != null)
                        cell.RowSpan = value;
                    var cell2 = Cell as DataGridViewImageCellEx;
                    if (cell2 != null)
                        cell2.RowSpan = value;
                }
            }
            public bool ColumnFrozen
            {
                get { return Cell.OwningColumn.Frozen; }
                set { Cell.OwningColumn.Frozen = value; }
            }
            public bool RowFrozen
            {
                get { return Cell.OwningRow.Frozen; }
                set { Cell.OwningRow.Frozen = value; }
            }
            public DataGridViewCellStyle CellStyle
            {
                get { return Cell.Style; }
                set { Cell.Style = value; }
            }
            public int ColumnDividerWidth
            {
                get { return Cell.OwningColumn.DividerWidth; }
                set { Cell.OwningColumn.DividerWidth = value; }
            }
            public int RowDividerHeight
            {
                get { return Cell.OwningRow.DividerHeight; }
                set { Cell.OwningRow.DividerHeight = value; }
            }
            #endregion

            #region ctor
            public CellPresenter(DataGridViewCell cell)
            {
                Cell = cell;
            }
            #endregion
        }
        public class RowPresenter
        {
            DataGridViewRow m_Row;
            public bool Visible
            {
                get { return m_Row.Visible; }
                set { m_Row.Visible = value; }
            }
            public bool Frozen
            {
                get { return m_Row.Frozen; }
                set { m_Row.Frozen = value; }
            }
            public int Index 
            {
                get { return m_Row.Index; } 
            }

            #region ctor
            public RowPresenter(DataGridViewRow row)
            {
                m_Row = row;
            } 
            #endregion
        }
        #endregion

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex == 0 && e.ColumnIndex == 0)
            {
                e.CellStyle.BackColor = Color.Azure;
                e.CellStyle.ForeColor = Color.Red;
            }
        }

        private void dataGridView1_CurrentCellChanged(object sender, EventArgs e)
        {
            var cell = dataGridView1.CurrentCell as DataGridViewTextBoxCellEx;
            propertyGrid1.SelectedObject = new CellPresenter( 
                cell == null || cell.OwnerCell == null
                  ? dataGridView1.CurrentCell
                  : cell);
        }

        private void dtGrdVwColumnsProperties_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dtGrdVwColumnsProperties.Height = dtGrdVwColumnsProperties.PreferredSize.Height;
            grpBxColumnsProperties.Height = grpBxColumnsProperties.PreferredSize.Height;
        }

        private void dtGrdVwColumnsProperties_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                if (e.ColumnIndex == clmnColumnFrozen.Index)
                {
                    ((DataGridView)sender).Invalidate();
                }
            }
        }

        private void dtGrdVwRowsProperties_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                if (e.ColumnIndex == clmnRowFrozen.Index)
                {
                    ((DataGridView)sender).Invalidate();
                }
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            dataGridView1.Invalidate();
        }

        private void toolStrpBtnRightToLeft_Click(object sender, EventArgs e)
        {
            toolStrpBtnRightToLeft.Checked = !toolStrpBtnRightToLeft.Checked;
            dataGridView1.RightToLeft = toolStrpBtnRightToLeft.Checked
                                            ? RightToLeft.Yes
                                            : RightToLeft.No;
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            var toolStripButton = (ToolStripButton) sender;
            toolStripButton.Checked = !toolStripButton.Checked;
            dataGridView1.RowHeadersVisible = toolStripButton.Checked;
            dataGridView1.ColumnHeadersVisible = toolStripButton.Checked;
        }
    }
}
