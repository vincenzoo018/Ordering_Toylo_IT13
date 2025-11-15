using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace Ordering_Toylo_IT13
{
    public partial class OrdersForm : Form
    {
        private TextBox txtCustomerName;
        private TextBox txtTableNumber;
        private ComboBox cboMenuItem;
        private NumericUpDown numQuantity;
        private TextBox txtPrice;
        private DataGridView dgvOrderItems;
        private Label lblTotal;
        private Button btnAddItem;
        private Button btnRemoveItem;
        private Button btnPlaceOrder;
        private Button btnClear;
        private decimal totalAmount = 0;
        private Panel centerColumn;

        public OrdersForm()
        {
            InitializeCustomComponents();
            LoadMenuItems();
        }

        private void InitializeCustomComponents()
        {
            this.Size = new Size(1000, 650);
            this.BackColor = Color.White;
            this.Padding = new Padding(10);
            this.AutoScroll = true;
            this.Resize += (s, e) => ApplyCenterLayout();

            Panel mainLayout = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true
            };
            this.Controls.Add(mainLayout);

            centerColumn = new Panel
            {
                Anchor = AnchorStyles.Top,
                Location = new Point(0, 10),
                Width = 950,
                Height = this.ClientSize.Height - 20
            };
            mainLayout.Controls.Add(centerColumn);

            Label lblTitle = new Label
            {
                Text = "CREATE NEW ORDER",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.FromArgb(52, 73, 94),
                Dock = DockStyle.Top,
                Height = 40
            };
            centerColumn.Controls.Add(lblTitle);

            // Customer Information Panel
            Panel customerPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 90,
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.FromArgb(236, 240, 241)
            };
            customerPanel.Padding = new Padding(10);
            centerColumn.Controls.Add(customerPanel);

            Label lblPanelTitle = new Label
            {
                Text = "Customer Information",
                Location = new Point(10, 8),
                AutoSize = true,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.FromArgb(52, 73, 94)
            };
            customerPanel.Controls.Add(lblPanelTitle);

            Label lblCustomer = new Label
            {
                Text = "Customer Name:",
                Location = new Point(15, 35),
                AutoSize = true,
                Font = new Font("Segoe UI", 9)
            };
            customerPanel.Controls.Add(lblCustomer);

            txtCustomerName = new TextBox
            {
                Location = new Point(10, 55),
                Size = new Size(350, 25),
                Font = new Font("Segoe UI", 10)
            };
            customerPanel.Controls.Add(txtCustomerName);

            Label lblTable = new Label
            {
                Text = "Table Number:",
                Location = new Point(400, 35),
                AutoSize = true,
                Font = new Font("Segoe UI", 9)
            };
            customerPanel.Controls.Add(lblTable);

            txtTableNumber = new TextBox
            {
                Location = new Point(400, 55),
                Size = new Size(150, 25),
                Font = new Font("Segoe UI", 10)
            };
            customerPanel.Controls.Add(txtTableNumber);

            // Order Items Panel
            Panel orderPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 120,
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.FromArgb(236, 240, 241)
            };
            orderPanel.Padding = new Padding(10);
            centerColumn.Controls.Add(orderPanel);

            Label lblOrderTitle = new Label
            {
                Text = "Add Items to Order",
                Location = new Point(15, 8),
                AutoSize = true,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.FromArgb(52, 73, 94)
            };
            orderPanel.Controls.Add(lblOrderTitle);

            Label lblMenuItem = new Label
            {
                Text = "Select Menu Item:",
                Location = new Point(15, 35),
                AutoSize = true,
                Font = new Font("Segoe UI", 9)
            };
            orderPanel.Controls.Add(lblMenuItem);

            cboMenuItem = new ComboBox
            {
                Location = new Point(10, 55),
                Size = new Size(300, 25),
                Font = new Font("Segoe UI", 9),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cboMenuItem.SelectedIndexChanged += CboMenuItem_SelectedIndexChanged;
            orderPanel.Controls.Add(cboMenuItem);

            Label lblQuantity = new Label
            {
                Text = "Quantity:",
                Location = new Point(340, 35),
                AutoSize = true,
                Font = new Font("Segoe UI", 9)
            };
            orderPanel.Controls.Add(lblQuantity);

            numQuantity = new NumericUpDown
            {
                Location = new Point(340, 55),
                Size = new Size(100, 25),
                Font = new Font("Segoe UI", 9),
                Minimum = 1,
                Maximum = 100,
                Value = 1
            };
            orderPanel.Controls.Add(numQuantity);

            Label lblPrice = new Label
            {
                Text = "Price:",
                Location = new Point(460, 35),
                AutoSize = true,
                Font = new Font("Segoe UI", 9)
            };
            orderPanel.Controls.Add(lblPrice);

            txtPrice = new TextBox
            {
                Location = new Point(460, 55),
                Size = new Size(120, 25),
                Font = new Font("Segoe UI", 9),
                ReadOnly = true,
                BackColor = Color.White
            };
            orderPanel.Controls.Add(txtPrice);

            btnAddItem = new Button
            {
                Text = "Add to Order",
                Location = new Point(10, 85),
                Size = new Size(140, 28),
                BackColor = Color.FromArgb(46, 204, 113),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnAddItem.FlatAppearance.BorderSize = 0;
            btnAddItem.Click += BtnAddItem_Click;
            orderPanel.Controls.Add(btnAddItem);

            btnRemoveItem = new Button
            {
                Text = "Remove Item",
                Location = new Point(165, 85),
                Size = new Size(140, 28),
                BackColor = Color.FromArgb(231, 76, 60),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnRemoveItem.FlatAppearance.BorderSize = 0;
            btnRemoveItem.Click += BtnRemoveItem_Click;
            orderPanel.Controls.Add(btnRemoveItem);

            // Label for Order Items
            Label lblOrderItems = new Label
            {
                Text = "Order Items:",
                Dock = DockStyle.Top,
                Height = 26,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.FromArgb(52, 73, 94)
            };
            centerColumn.Controls.Add(lblOrderItems);

            // DataGridView for Order Items
            dgvOrderItems = new DataGridView
            {
                Dock = DockStyle.Top,
                Height = 200,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.Fixed3D,
                AllowUserToAddRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                Font = new Font("Segoe UI", 9)
            };
            dgvOrderItems.EnableHeadersVisualStyles = false;
            dgvOrderItems.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(52, 73, 94);
            dgvOrderItems.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvOrderItems.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            dgvOrderItems.ColumnHeadersHeight = 35;
            centerColumn.Controls.Add(dgvOrderItems);

            // Setup DataGridView Columns
            dgvOrderItems.Columns.Add("ItemID", "Item ID");
            dgvOrderItems.Columns.Add("ItemName", "Item Name");
            dgvOrderItems.Columns.Add("Quantity", "Quantity");
            dgvOrderItems.Columns.Add("Price", "Price");
            dgvOrderItems.Columns.Add("Subtotal", "Subtotal");
            dgvOrderItems.Columns["ItemID"].Visible = false;

            // Total Label
            lblTotal = new Label
            {
                Text = "TOTAL: ₱0.00",
                Dock = DockStyle.Top,
                Height = 30,
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.FromArgb(231, 76, 60),
                TextAlign = ContentAlignment.MiddleRight
            };
            centerColumn.Controls.Add(lblTotal);

            // Action Buttons
            FlowLayoutPanel actionsRow = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                Height = 50,
                FlowDirection = FlowDirection.RightToLeft,
                Padding = new Padding(0),
                WrapContents = false
            };
            centerColumn.Controls.Add(actionsRow);

            btnPlaceOrder = new Button
            {
                Text = "PLACE ORDER",
                Size = new Size(145, 40),
                BackColor = Color.FromArgb(52, 152, 219),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Cursor = Cursors.Hand,
                Margin = new Padding(10, 5, 0, 5)
            };
            btnPlaceOrder.FlatAppearance.BorderSize = 0;
            btnPlaceOrder.Click += BtnPlaceOrder_Click;
            actionsRow.Controls.Add(btnPlaceOrder);

            btnClear = new Button
            {
                Text = "CLEAR",
                Size = new Size(145, 40),
                BackColor = Color.FromArgb(149, 165, 166),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Cursor = Cursors.Hand,
                Margin = new Padding(10, 5, 0, 5)
            };
            btnClear.FlatAppearance.BorderSize = 0;
            btnClear.Click += BtnClear_Click;
            actionsRow.Controls.Add(btnClear);

            ApplyCenterLayout();
        }

        private void LoadMenuItems()
        {
            string query = "SELECT ItemID, ItemName, Price FROM MenuItems WHERE IsAvailable = 1 ORDER BY ItemName";
            DataTable dt = DatabaseHelper.ExecuteQuery(query);

            cboMenuItem.DataSource = dt;
            cboMenuItem.DisplayMember = "ItemName";
            cboMenuItem.ValueMember = "ItemID";
        }

        private void CboMenuItem_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboMenuItem.SelectedValue != null)
            {
                DataRowView row = cboMenuItem.SelectedItem as DataRowView;
                if (row != null)
                {
                    txtPrice.Text = Convert.ToDecimal(row["Price"]).ToString("0.00");
                }
            }
        }

        private void BtnAddItem_Click(object sender, EventArgs e)
        {
            if (cboMenuItem.SelectedValue == null)
            {
                MessageBox.Show("Please select a menu item.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int itemId = Convert.ToInt32(cboMenuItem.SelectedValue);
            string itemName = cboMenuItem.Text;
            int quantity = Convert.ToInt32(numQuantity.Value);
            decimal price = Convert.ToDecimal(txtPrice.Text);
            decimal subtotal = quantity * price;

            dgvOrderItems.Rows.Add(itemId, itemName, quantity, price.ToString("0.00"), subtotal.ToString("0.00"));

            totalAmount += subtotal;
            lblTotal.Text = $"TOTAL: ₱{totalAmount:0.00}";
        }

        private void BtnRemoveItem_Click(object sender, EventArgs e)
        {
            if (dgvOrderItems.SelectedRows.Count > 0)
            {
                decimal subtotal = Convert.ToDecimal(dgvOrderItems.SelectedRows[0].Cells["Subtotal"].Value);
                totalAmount -= subtotal;
                lblTotal.Text = $"TOTAL: ₱{totalAmount:0.00}";

                dgvOrderItems.Rows.RemoveAt(dgvOrderItems.SelectedRows[0].Index);
            }
            else
            {
                MessageBox.Show("Please select an item to remove.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void BtnPlaceOrder_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCustomerName.Text))
            {
                MessageBox.Show("Please enter customer name.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtCustomerName.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtTableNumber.Text))
            {
                MessageBox.Show("Please enter table number.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTableNumber.Focus();
                return;
            }

            if (dgvOrderItems.Rows.Count == 0)
            {
                MessageBox.Show("Please add at least one item to the order.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Insert Order
                string orderQuery = "INSERT INTO Orders (CustomerName, TableNumber, TotalAmount, OrderStatus) OUTPUT INSERTED.OrderID VALUES (@CustomerName, @TableNumber, @TotalAmount, 'Pending')";
                SqlParameter[] orderParams = {
                    new SqlParameter("@CustomerName", txtCustomerName.Text),
                    new SqlParameter("@TableNumber", Convert.ToInt32(txtTableNumber.Text)),
                    new SqlParameter("@TotalAmount", totalAmount)
                };

                object orderIdObj = DatabaseHelper.ExecuteScalar(orderQuery, orderParams);
                int orderId = Convert.ToInt32(orderIdObj);

                // Insert Order Details
                foreach (DataGridViewRow row in dgvOrderItems.Rows)
                {
                    string detailQuery = "INSERT INTO OrderDetails (OrderID, ItemID, ItemName, Quantity, Price, Subtotal) VALUES (@OrderID, @ItemID, @ItemName, @Quantity, @Price, @Subtotal)";
                    SqlParameter[] detailParams = {
                        new SqlParameter("@OrderID", orderId),
                        new SqlParameter("@ItemID", Convert.ToInt32(row.Cells["ItemID"].Value)),
                        new SqlParameter("@ItemName", row.Cells["ItemName"].Value.ToString()),
                        new SqlParameter("@Quantity", Convert.ToInt32(row.Cells["Quantity"].Value)),
                        new SqlParameter("@Price", Convert.ToDecimal(row.Cells["Price"].Value)),
                        new SqlParameter("@Subtotal", Convert.ToDecimal(row.Cells["Subtotal"].Value))
                    };
                    DatabaseHelper.ExecuteNonQuery(detailQuery, detailParams);
                }

                MessageBox.Show($"Order #{orderId} placed successfully!\nTotal: ₱{totalAmount:0.00}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error placing order: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            ClearForm();
        }

        private void ClearForm()
        {
            txtCustomerName.Clear();
            txtTableNumber.Clear();
            txtPrice.Clear();
            numQuantity.Value = 1;
            dgvOrderItems.Rows.Clear();
            totalAmount = 0;
            lblTotal.Text = "TOTAL: ₱0.00";
            if (cboMenuItem.Items.Count > 0)
                cboMenuItem.SelectedIndex = 0;
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