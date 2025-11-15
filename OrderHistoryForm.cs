using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace Ordering_Toylo_IT13
{
    public partial class OrderHistoryForm : Form
    {
        private DataGridView dgvOrders;
        private DataGridView dgvOrderDetails;
        private ComboBox cboStatusFilter;
        private DateTimePicker dtpFromDate;
        private DateTimePicker dtpToDate;
        private Button btnFilter;
        private Button btnViewDetails;
        private Button btnUpdateStatus;
        private Button btnDeleteOrder;
        private ComboBox cboNewStatus;
        private Label lblTotalOrders;
        private Label lblTotalRevenue;
        private TextBox txtSearchCustomer;
        private Button btnSearch;
        private Panel centerColumn;

        public OrderHistoryForm()
        {
            InitializeCustomComponents();
            LoadOrders();
            UpdateSummary();
        }

        private void InitializeCustomComponents()
        {
            this.Size = new Size(1000, 650);
            this.BackColor = Color.White;
            this.Padding = new Padding(10);
            this.AutoScroll = true;
            this.Resize += (s, e) => ApplyCenterLayout();

            Panel mainLayout = new Panel { Dock = DockStyle.Fill, AutoScroll = true };
            this.Controls.Add(mainLayout);

            centerColumn = new Panel { Anchor = AnchorStyles.Top, Location = new Point(0, 10), Width = 950, Height = this.ClientSize.Height - 20 };
            mainLayout.Controls.Add(centerColumn);

            Label lblTitle = new Label { Text = "ORDER HISTORY & MANAGEMENT", Font = new Font("Segoe UI", 16, FontStyle.Bold), ForeColor = Color.FromArgb(52, 73, 94), Dock = DockStyle.Top, Height = 40 };
            centerColumn.Controls.Add(lblTitle);

            Panel filterPanel = new Panel { Dock = DockStyle.Top, Height = 100, BorderStyle = BorderStyle.FixedSingle, BackColor = Color.FromArgb(236, 240, 241), Padding = new Padding(10) };
            centerColumn.Controls.Add(filterPanel);

            Label lblFilterTitle = new Label
            {
                Text = "Filter Orders",
                Location = new Point(15, 8),
                AutoSize = true,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.FromArgb(52, 73, 94)
            };
            filterPanel.Controls.Add(lblFilterTitle);

            Label lblFrom = new Label
            {
                Text = "From:",
                Location = new Point(15, 35),
                AutoSize = true,
                Font = new Font("Segoe UI", 9)
            };
            filterPanel.Controls.Add(lblFrom);

            dtpFromDate = new DateTimePicker
            {
                Location = new Point(15, 55),
                Size = new Size(130, 25),
                Font = new Font("Segoe UI", 9),
                Format = DateTimePickerFormat.Short
            };
            dtpFromDate.Value = DateTime.Now.AddMonths(-1);
            filterPanel.Controls.Add(dtpFromDate);

            Label lblTo = new Label
            {
                Text = "To:",
                Location = new Point(160, 35),
                AutoSize = true,
                Font = new Font("Segoe UI", 9)
            };
            filterPanel.Controls.Add(lblTo);

            dtpToDate = new DateTimePicker
            {
                Location = new Point(160, 55),
                Size = new Size(130, 25),
                Font = new Font("Segoe UI", 9),
                Format = DateTimePickerFormat.Short
            };
            filterPanel.Controls.Add(dtpToDate);

            Label lblStatus = new Label
            {
                Text = "Status:",
                Location = new Point(310, 35),
                AutoSize = true,
                Font = new Font("Segoe UI", 9)
            };
            filterPanel.Controls.Add(lblStatus);

            cboStatusFilter = new ComboBox
            {
                Location = new Point(310, 55),
                Size = new Size(120, 25),
                Font = new Font("Segoe UI", 9),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cboStatusFilter.Items.AddRange(new string[] { "All", "Pending", "Completed", "Cancelled" });
            cboStatusFilter.SelectedIndex = 0;
            filterPanel.Controls.Add(cboStatusFilter);

            Label lblSearchCustomer = new Label
            {
                Text = "Customer:",
                Location = new Point(450, 35),
                AutoSize = true,
                Font = new Font("Segoe UI", 9)
            };
            filterPanel.Controls.Add(lblSearchCustomer);

            txtSearchCustomer = new TextBox
            {
                Location = new Point(450, 55),
                Size = new Size(200, 25),
                Font = new Font("Segoe UI", 9)
            };
            filterPanel.Controls.Add(txtSearchCustomer);

            btnFilter = new Button
            {
                Text = "FILTER",
                Location = new Point(670, 53),
                Size = new Size(85, 28),
                BackColor = Color.FromArgb(52, 152, 219),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnFilter.FlatAppearance.BorderSize = 0;
            btnFilter.Click += BtnFilter_Click;
            filterPanel.Controls.Add(btnFilter);

            btnSearch = new Button
            {
                Text = "SEARCH",
                Location = new Point(765, 53),
                Size = new Size(85, 28),
                BackColor = Color.FromArgb(46, 204, 113),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnSearch.FlatAppearance.BorderSize = 0;
            btnSearch.Click += BtnSearch_Click;
            filterPanel.Controls.Add(btnSearch);

            Button btnShowAll = new Button
            {
                Text = "SHOW ALL",
                Location = new Point(860, 53),
                Size = new Size(85, 28),
                BackColor = Color.FromArgb(155, 89, 182),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnShowAll.FlatAppearance.BorderSize = 0;
            btnShowAll.Click += (s, e) => { txtSearchCustomer.Clear(); LoadOrders(); };
            filterPanel.Controls.Add(btnShowAll);

            // Summary Panel
            Panel summaryPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 45,
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.FromArgb(52, 152, 219)
            };
            centerColumn.Controls.Add(summaryPanel);

            lblTotalOrders = new Label
            {
                Text = "Total Orders: 0",
                Location = new Point(20, 12),
                Size = new Size(400, 25),
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = Color.White
            };
            summaryPanel.Controls.Add(lblTotalOrders);

            lblTotalRevenue = new Label
            {
                Text = "Total Revenue: ₱0.00",
                Location = new Point(500, 12),
                Size = new Size(430, 25),
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = Color.White,
                TextAlign = ContentAlignment.MiddleRight
            };
            summaryPanel.Controls.Add(lblTotalRevenue);

            // Orders Label
            Label lblOrders = new Label
            {
                Text = "Orders List:",
                Dock = DockStyle.Top,
                Height = 26,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.FromArgb(52, 73, 94)
            };
            centerColumn.Controls.Add(lblOrders);

            // Orders DataGridView
            dgvOrders = new DataGridView
            {
                Dock = DockStyle.Top,
                Height = 150,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.Fixed3D,
                AllowUserToAddRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                Font = new Font("Segoe UI", 9)
            };
            dgvOrders.CellClick += DgvOrders_CellClick;
            dgvOrders.EnableHeadersVisualStyles = false;
            dgvOrders.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(52, 73, 94);
            dgvOrders.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvOrders.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            dgvOrders.ColumnHeadersHeight = 35;
            centerColumn.Controls.Add(dgvOrders);

            // Order Details Label
            Label lblDetails = new Label
            {
                Text = "Order Details:",
                Dock = DockStyle.Top,
                Height = 26,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.FromArgb(52, 73, 94)
            };
            centerColumn.Controls.Add(lblDetails);

            Panel detailsRow = new Panel { Dock = DockStyle.Top, Height = 180 };
            centerColumn.Controls.Add(detailsRow);

            dgvOrderDetails = new DataGridView { Location = new Point(0, 0), Size = new Size(detailsRow.Width - 340, 160), BackgroundColor = Color.White, BorderStyle = BorderStyle.Fixed3D, AllowUserToAddRows = false, ReadOnly = true, SelectionMode = DataGridViewSelectionMode.FullRowSelect, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill, Font = new Font("Segoe UI", 9) };
            dgvOrderDetails.EnableHeadersVisualStyles = false;
            dgvOrderDetails.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(52, 73, 94);
            dgvOrderDetails.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvOrderDetails.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            dgvOrderDetails.ColumnHeadersHeight = 35;
            detailsRow.Controls.Add(dgvOrderDetails);

            Panel actionPanel = new Panel { Location = new Point(detailsRow.Width - 330, 0), Size = new Size(330, 160), BorderStyle = BorderStyle.FixedSingle, BackColor = Color.FromArgb(236, 240, 241) };
            detailsRow.Controls.Add(actionPanel);
            detailsRow.Resize += (s, e) => {
                int rightWidth = 330;
                dgvOrderDetails.Size = new Size(detailsRow.Width - (rightWidth + 10), 160);
                actionPanel.Location = new Point(detailsRow.Width - rightWidth, 0);
            };

            Label lblActions = new Label
            {
                Text = "Actions:",
                Location = new Point(10, 10),
                AutoSize = true,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            actionPanel.Controls.Add(lblActions);

            Label lblNewStatus = new Label
            {
                Text = "Update Status:",
                Location = new Point(10, 40),
                AutoSize = true,
                Font = new Font("Segoe UI", 9)
            };
            actionPanel.Controls.Add(lblNewStatus);

            cboNewStatus = new ComboBox
            {
                Location = new Point(10, 60),
                Size = new Size(140, 25),
                Font = new Font("Segoe UI", 9),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cboNewStatus.Items.AddRange(new string[] { "Pending", "Completed", "Cancelled" });
            cboNewStatus.SelectedIndex = 0;
            actionPanel.Controls.Add(cboNewStatus);

            btnUpdateStatus = new Button
            {
                Text = "UPDATE",
                Location = new Point(160, 58),
                Size = new Size(160, 28),
                BackColor = Color.FromArgb(52, 152, 219),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnUpdateStatus.FlatAppearance.BorderSize = 0;
            btnUpdateStatus.Click += BtnUpdateStatus_Click;
            actionPanel.Controls.Add(btnUpdateStatus);

            btnViewDetails = new Button
            {
                Text = "VIEW DETAILS",
                Location = new Point(10, 100),
                Size = new Size(150, 35),
                BackColor = Color.FromArgb(46, 204, 113),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnViewDetails.FlatAppearance.BorderSize = 0;
            btnViewDetails.Click += BtnViewDetails_Click;
            actionPanel.Controls.Add(btnViewDetails);

            btnDeleteOrder = new Button
            {
                Text = "DELETE ORDER",
                Location = new Point(170, 100),
                Size = new Size(150, 35),
                BackColor = Color.FromArgb(231, 76, 60),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnDeleteOrder.FlatAppearance.BorderSize = 0;
            btnDeleteOrder.Click += BtnDeleteOrder_Click;
            actionPanel.Controls.Add(btnDeleteOrder);

            ApplyCenterLayout();
        }

        private void LoadOrders(string whereClause = "")
        {
            string query = "SELECT OrderID, CustomerName, TableNumber, OrderDate, TotalAmount, OrderStatus FROM Orders";

            if (!string.IsNullOrWhiteSpace(whereClause))
            {
                query += " WHERE " + whereClause;
            }

            query += " ORDER BY OrderDate DESC";

            DataTable dt = DatabaseHelper.ExecuteQuery(query);
            dgvOrders.DataSource = dt;

            if (dgvOrders.Columns.Count > 0)
            {
                dgvOrders.Columns["OrderID"].HeaderText = "Order ID";
                dgvOrders.Columns["OrderID"].Width = 80;
                dgvOrders.Columns["CustomerName"].HeaderText = "Customer";
                dgvOrders.Columns["TableNumber"].HeaderText = "Table";
                dgvOrders.Columns["TableNumber"].Width = 70;
                dgvOrders.Columns["OrderDate"].HeaderText = "Date";
                dgvOrders.Columns["OrderDate"].DefaultCellStyle.Format = "MM/dd/yyyy HH:mm";
                dgvOrders.Columns["TotalAmount"].HeaderText = "Total";
                dgvOrders.Columns["TotalAmount"].DefaultCellStyle.Format = "₱#,##0.00";
                dgvOrders.Columns["TotalAmount"].Width = 100;
                dgvOrders.Columns["OrderStatus"].HeaderText = "Status";
                dgvOrders.Columns["OrderStatus"].Width = 100;
            }

            UpdateSummary();
        }

        private void LoadOrderDetails(int orderId)
        {
            string query = "SELECT ItemName, Quantity, Price, Subtotal FROM OrderDetails WHERE OrderID = @OrderID";
            SqlParameter[] parameters = {
                new SqlParameter("@OrderID", orderId)
            };

            DataTable dt = DatabaseHelper.ExecuteQuery(query, parameters);
            dgvOrderDetails.DataSource = dt;

            if (dgvOrderDetails.Columns.Count > 0)
            {
                dgvOrderDetails.Columns["ItemName"].HeaderText = "Item";
                dgvOrderDetails.Columns["Quantity"].HeaderText = "Qty";
                dgvOrderDetails.Columns["Quantity"].Width = 60;
                dgvOrderDetails.Columns["Price"].HeaderText = "Price";
                dgvOrderDetails.Columns["Price"].DefaultCellStyle.Format = "₱#,##0.00";
                dgvOrderDetails.Columns["Price"].Width = 100;
                dgvOrderDetails.Columns["Subtotal"].HeaderText = "Subtotal";
                dgvOrderDetails.Columns["Subtotal"].DefaultCellStyle.Format = "₱#,##0.00";
                dgvOrderDetails.Columns["Subtotal"].Width = 100;
            }
        }

        private void UpdateSummary()
        {
            if (dgvOrders.DataSource != null)
            {
                DataTable dt = (DataTable)dgvOrders.DataSource;
                int totalOrders = dt.Rows.Count;
                decimal totalRevenue = 0;

                foreach (DataRow row in dt.Rows)
                {
                    totalRevenue += Convert.ToDecimal(row["TotalAmount"]);
                }

                lblTotalOrders.Text = $"Total Orders: {totalOrders}";
                lblTotalRevenue.Text = $"Total Revenue: ₱{totalRevenue:N2}";
            }
        }

        private void BtnFilter_Click(object sender, EventArgs e)
        {
            string whereClause = $"OrderDate >= '{dtpFromDate.Value:yyyy-MM-dd}' AND OrderDate <= '{dtpToDate.Value:yyyy-MM-dd 23:59:59}'";

            if (cboStatusFilter.SelectedIndex > 0)
            {
                whereClause += $" AND OrderStatus = '{cboStatusFilter.Text}'";
            }

            LoadOrders(whereClause);
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtSearchCustomer.Text))
            {
                string whereClause = $"CustomerName LIKE '%{txtSearchCustomer.Text}%'";
                LoadOrders(whereClause);
            }
            else
            {
                LoadOrders();
            }
        }

        private void BtnViewDetails_Click(object sender, EventArgs e)
        {
            if (dgvOrders.SelectedRows.Count > 0)
            {
                int orderId = Convert.ToInt32(dgvOrders.SelectedRows[0].Cells["OrderID"].Value);
                LoadOrderDetails(orderId);
            }
            else
            {
                MessageBox.Show("Please select an order to view details.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void BtnUpdateStatus_Click(object sender, EventArgs e)
        {
            if (dgvOrders.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select an order to update.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int orderId = Convert.ToInt32(dgvOrders.SelectedRows[0].Cells["OrderID"].Value);
            string newStatus = cboNewStatus.Text;

            string query = "UPDATE Orders SET OrderStatus = @OrderStatus WHERE OrderID = @OrderID";
            SqlParameter[] parameters = {
                new SqlParameter("@OrderID", orderId),
                new SqlParameter("@OrderStatus", newStatus)
            };

            if (DatabaseHelper.ExecuteNonQuery(query, parameters))
            {
                MessageBox.Show($"Order #{orderId} status updated to {newStatus}!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadOrders();
            }
        }

        private void BtnDeleteOrder_Click(object sender, EventArgs e)
        {
            if (dgvOrders.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select an order to delete.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int orderId = Convert.ToInt32(dgvOrders.SelectedRows[0].Cells["OrderID"].Value);

            DialogResult result = MessageBox.Show($"Are you sure you want to delete Order #{orderId}?\nThis will also delete all order details.",
                "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                string query = "DELETE FROM Orders WHERE OrderID = @OrderID";
                SqlParameter[] parameters = {
                    new SqlParameter("@OrderID", orderId)
                };

                if (DatabaseHelper.ExecuteNonQuery(query, parameters))
                {
                    MessageBox.Show("Order deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadOrders();
                    dgvOrderDetails.DataSource = null;
                }
            }
        }

        private void DgvOrders_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                int orderId = Convert.ToInt32(dgvOrders.Rows[e.RowIndex].Cells["OrderID"].Value);
                LoadOrderDetails(orderId);
            }
        }

        private void ApplyCenterLayout()
        {
            if (centerColumn == null) return;
            int maxWidth = 1000;
            int padding = 40;
            int targetWidth = Math.Min(maxWidth, this.ClientSize.Width - padding);
            centerColumn.Width = targetWidth;
            centerColumn.Left = (this.ClientSize.Width - centerColumn.Width) / 2;
        }
    }
}