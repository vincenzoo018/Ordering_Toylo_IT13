using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace Ordering_Toylo_IT13
{
    public partial class MenuItemsForm : Form
    {
        private TextBox txtItemID;
        private TextBox txtItemName;
        private ComboBox cboCategory;
        private TextBox txtPrice;
        private CheckBox chkAvailable;
        private DataGridView dgvMenuItems;
        private Button btnAdd;
        private Button btnUpdate;
        private Button btnDelete;
        private Button btnClear;
        private Button btnSearch;
        private TextBox txtSearch;
        private Panel centerColumn;

        public MenuItemsForm()
        {
            InitializeCustomComponents();
            LoadMenuItems();
        }

        private void InitializeCustomComponents()
        {
            this.Size = new Size(980, 650);
            this.BackColor = Color.White;
            this.AutoScroll = true;
            this.Padding = new Padding(10);
            this.Resize += (s, e) => ApplyCenterLayout();

            Panel mainLayout = new Panel { Dock = DockStyle.Fill, AutoScroll = true };
            this.Controls.Add(mainLayout);

            centerColumn = new Panel { Anchor = AnchorStyles.Top, Location = new Point(0, 10), Width = 940, Height = this.ClientSize.Height - 20 };
            mainLayout.Controls.Add(centerColumn);

            Label lblTitle = new Label { Text = "MENU ITEMS MANAGEMENT", Font = new Font("Segoe UI", 16, FontStyle.Bold), ForeColor = Color.FromArgb(52, 73, 94), Dock = DockStyle.Top, Height = 40 };
            centerColumn.Controls.Add(lblTitle);

            Panel inputPanel = new Panel { Dock = DockStyle.Top, Height = 150, BorderStyle = BorderStyle.FixedSingle, BackColor = Color.FromArgb(236, 240, 241), Padding = new Padding(10) };
            centerColumn.Controls.Add(inputPanel);

            // Item ID
            Label lblItemID = new Label
            {
                Text = "Item ID:",
                Location = new Point(15, 15),
                AutoSize = true,
                Font = new Font("Segoe UI", 9)
            };
            inputPanel.Controls.Add(lblItemID);

            txtItemID = new TextBox
            {
                Location = new Point(15, 35),
                Size = new Size(80, 23),
                Font = new Font("Segoe UI", 9),
                ReadOnly = true,
                BackColor = Color.LightGray
            };
            inputPanel.Controls.Add(txtItemID);

            // Item Name
            Label lblItemName = new Label
            {
                Text = "Item Name:",
                Location = new Point(110, 15),
                AutoSize = true,
                Font = new Font("Segoe UI", 9)
            };
            inputPanel.Controls.Add(lblItemName);

            txtItemName = new TextBox
            {
                Location = new Point(110, 35),
                Size = new Size(280, 23),
                Font = new Font("Segoe UI", 9)
            };
            inputPanel.Controls.Add(txtItemName);

            // Category
            Label lblCategory = new Label
            {
                Text = "Category:",
                Location = new Point(410, 15),
                AutoSize = true,
                Font = new Font("Segoe UI", 9)
            };
            inputPanel.Controls.Add(lblCategory);

            cboCategory = new ComboBox
            {
                Location = new Point(410, 35),
                Size = new Size(180, 23),
                Font = new Font("Segoe UI", 9),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cboCategory.Items.AddRange(new string[] { "Main Course", "Appetizer", "Sides", "Beverages", "Dessert" });
            cboCategory.SelectedIndex = 0;
            inputPanel.Controls.Add(cboCategory);

            // Price
            Label lblPrice = new Label
            {
                Text = "Price (₱):",
                Location = new Point(610, 15),
                AutoSize = true,
                Font = new Font("Segoe UI", 9)
            };
            inputPanel.Controls.Add(lblPrice);

            txtPrice = new TextBox
            {
                Location = new Point(610, 35),
                Size = new Size(120, 23),
                Font = new Font("Segoe UI", 9)
            };
            inputPanel.Controls.Add(txtPrice);

            // Available Checkbox
            chkAvailable = new CheckBox
            {
                Text = "Available",
                Location = new Point(750, 35),
                AutoSize = true,
                Font = new Font("Segoe UI", 9),
                Checked = true
            };
            inputPanel.Controls.Add(chkAvailable);

            // Action Buttons Row
            btnAdd = new Button
            {
                Text = "ADD",
                Location = new Point(15, 75),
                Size = new Size(110, 32),
                BackColor = Color.FromArgb(46, 204, 113),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnAdd.FlatAppearance.BorderSize = 0;
            btnAdd.Click += BtnAdd_Click;
            inputPanel.Controls.Add(btnAdd);

            btnUpdate = new Button
            {
                Text = "UPDATE",
                Location = new Point(135, 75),
                Size = new Size(110, 32),
                BackColor = Color.FromArgb(52, 152, 219),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnUpdate.FlatAppearance.BorderSize = 0;
            btnUpdate.Click += BtnUpdate_Click;
            inputPanel.Controls.Add(btnUpdate);

            btnDelete = new Button
            {
                Text = "DELETE",
                Location = new Point(255, 75),
                Size = new Size(110, 32),
                BackColor = Color.FromArgb(231, 76, 60),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnDelete.FlatAppearance.BorderSize = 0;
            btnDelete.Click += BtnDelete_Click;
            inputPanel.Controls.Add(btnDelete);

            btnClear = new Button
            {
                Text = "CLEAR",
                Location = new Point(375, 75),
                Size = new Size(110, 32),
                BackColor = Color.FromArgb(149, 165, 166),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnClear.FlatAppearance.BorderSize = 0;
            btnClear.Click += BtnClear_Click;
            inputPanel.Controls.Add(btnClear);

            // Search Section
            Label lblSearch = new Label { Text = "Search:", Dock = DockStyle.Top, Height = 26, Font = new Font("Segoe UI", 9, FontStyle.Bold) };
            centerColumn.Controls.Add(lblSearch);

            Panel searchRow = new Panel { Dock = DockStyle.Top, Height = 35 };
            centerColumn.Controls.Add(searchRow);
            txtSearch = new TextBox { Location = new Point(0, 6), Size = new Size(250, 23), Font = new Font("Segoe UI", 9) };
            searchRow.Controls.Add(txtSearch);

            btnSearch = new Button { Text = "SEARCH", Location = new Point(260, 5), Size = new Size(90, 27), BackColor = Color.FromArgb(52, 152, 219), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 9, FontStyle.Bold), Cursor = Cursors.Hand };
            btnSearch.FlatAppearance.BorderSize = 0;
            btnSearch.Click += BtnSearch_Click;
            searchRow.Controls.Add(btnSearch);

            Button btnRefresh = new Button { Text = "SHOW ALL", Location = new Point(355, 5), Size = new Size(90, 27), BackColor = Color.FromArgb(46, 204, 113), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 9, FontStyle.Bold), Cursor = Cursors.Hand };
            btnRefresh.FlatAppearance.BorderSize = 0;
            btnRefresh.Click += (s, e) => { txtSearch.Clear(); LoadMenuItems(); };
            searchRow.Controls.Add(btnRefresh);

            dgvMenuItems = new DataGridView { Dock = DockStyle.Top, Height = 360, BackgroundColor = Color.White, BorderStyle = BorderStyle.Fixed3D, AllowUserToAddRows = false, ReadOnly = true, SelectionMode = DataGridViewSelectionMode.FullRowSelect, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill, RowHeadersWidth = 30, Font = new Font("Segoe UI", 9) };
            dgvMenuItems.CellClick += DgvMenuItems_CellClick;
            dgvMenuItems.EnableHeadersVisualStyles = false;
            dgvMenuItems.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(52, 73, 94);
            dgvMenuItems.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvMenuItems.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            dgvMenuItems.ColumnHeadersHeight = 35;
            centerColumn.Controls.Add(dgvMenuItems);

            ApplyCenterLayout();
        }

        private void LoadMenuItems(string searchTerm = "")
        {
            string query = "SELECT ItemID, ItemName, Category, Price, IsAvailable, DateAdded FROM MenuItems";

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query += " WHERE ItemName LIKE @SearchTerm OR Category LIKE @SearchTerm";
            }

            query += " ORDER BY ItemName";

            SqlParameter[] parameters = null;
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                parameters = new SqlParameter[] {
                    new SqlParameter("@SearchTerm", "%" + searchTerm + "%")
                };
            }

            DataTable dt = DatabaseHelper.ExecuteQuery(query, parameters);
            dgvMenuItems.DataSource = dt;

            if (dgvMenuItems.Columns.Count > 0)
            {
                dgvMenuItems.Columns["ItemID"].HeaderText = "ID";
                dgvMenuItems.Columns["ItemID"].Width = 50;
                dgvMenuItems.Columns["ItemName"].HeaderText = "Item Name";
                dgvMenuItems.Columns["Category"].HeaderText = "Category";
                dgvMenuItems.Columns["Price"].HeaderText = "Price";
                dgvMenuItems.Columns["Price"].DefaultCellStyle.Format = "₱#,##0.00";
                dgvMenuItems.Columns["Price"].Width = 100;
                dgvMenuItems.Columns["IsAvailable"].HeaderText = "Available";
                dgvMenuItems.Columns["IsAvailable"].Width = 80;
                dgvMenuItems.Columns["DateAdded"].HeaderText = "Date Added";
                dgvMenuItems.Columns["DateAdded"].DefaultCellStyle.Format = "MM/dd/yyyy";
                dgvMenuItems.Columns["DateAdded"].Width = 100;
            }
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            if (!ValidateInput()) return;

            string query = "INSERT INTO MenuItems (ItemName, Category, Price, IsAvailable) VALUES (@ItemName, @Category, @Price, @IsAvailable)";
            SqlParameter[] parameters = {
                new SqlParameter("@ItemName", txtItemName.Text.Trim()),
                new SqlParameter("@Category", cboCategory.Text),
                new SqlParameter("@Price", Convert.ToDecimal(txtPrice.Text)),
                new SqlParameter("@IsAvailable", chkAvailable.Checked)
            };

            if (DatabaseHelper.ExecuteNonQuery(query, parameters))
            {
                MessageBox.Show("Menu item added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadMenuItems();
                ClearForm();
            }
        }

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtItemID.Text))
            {
                MessageBox.Show("Please select a menu item to update.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!ValidateInput()) return;

            string query = "UPDATE MenuItems SET ItemName = @ItemName, Category = @Category, Price = @Price, IsAvailable = @IsAvailable WHERE ItemID = @ItemID";
            SqlParameter[] parameters = {
                new SqlParameter("@ItemID", Convert.ToInt32(txtItemID.Text)),
                new SqlParameter("@ItemName", txtItemName.Text.Trim()),
                new SqlParameter("@Category", cboCategory.Text),
                new SqlParameter("@Price", Convert.ToDecimal(txtPrice.Text)),
                new SqlParameter("@IsAvailable", chkAvailable.Checked)
            };

            if (DatabaseHelper.ExecuteNonQuery(query, parameters))
            {
                MessageBox.Show("Menu item updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadMenuItems();
                ClearForm();
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtItemID.Text))
            {
                MessageBox.Show("Please select a menu item to delete.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult result = MessageBox.Show("Are you sure you want to delete this menu item?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                string query = "DELETE FROM MenuItems WHERE ItemID = @ItemID";
                SqlParameter[] parameters = {
                    new SqlParameter("@ItemID", Convert.ToInt32(txtItemID.Text))
                };

                if (DatabaseHelper.ExecuteNonQuery(query, parameters))
                {
                    MessageBox.Show("Menu item deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadMenuItems();
                    ClearForm();
                }
            }
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            LoadMenuItems(txtSearch.Text.Trim());
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            ClearForm();
        }

        private void DgvMenuItems_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvMenuItems.Rows[e.RowIndex];
                txtItemID.Text = row.Cells["ItemID"].Value.ToString();
                txtItemName.Text = row.Cells["ItemName"].Value.ToString();
                cboCategory.Text = row.Cells["Category"].Value.ToString();
                txtPrice.Text = row.Cells["Price"].Value.ToString();
                chkAvailable.Checked = Convert.ToBoolean(row.Cells["IsAvailable"].Value);
            }
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txtItemName.Text))
            {
                MessageBox.Show("Please enter item name.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtItemName.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtPrice.Text))
            {
                MessageBox.Show("Please enter price.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPrice.Focus();
                return false;
            }

            decimal price;
            if (!decimal.TryParse(txtPrice.Text, out price) || price <= 0)
            {
                MessageBox.Show("Please enter a valid price.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPrice.Focus();
                return false;
            }

            return true;
        }

        private void ClearForm()
        {
            txtItemID.Clear();
            txtItemName.Clear();
            txtPrice.Clear();
            txtSearch.Clear();
            cboCategory.SelectedIndex = 0;
            chkAvailable.Checked = true;
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