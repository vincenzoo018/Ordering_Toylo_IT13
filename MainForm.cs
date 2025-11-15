using System;
using System.Drawing;
using System.Windows.Forms;

namespace Ordering_Toylo_IT13
{
    public partial class MainForm : Form
    {
        private Panel sidebarPanel;
        private Panel contentPanel;
        private Button btnOrders;
        private Button btnMenuItems;
        private Button btnOrderHistory;
        private Button btnToggleSidebar;
        private Label lblTitle;
        private Form activeForm;
        private bool isSidebarCollapsed;
        private const int ExpandedSidebarWidth = 200;
        private const int CollapsedSidebarWidth = 60;

        public MainForm()
        {
            InitializeComponent();
            InitializeCustomComponents();
            LoadForm(new OrdersForm());
        }

        private void InitializeCustomComponents()
        {
            // Main Form Setup
            this.Text = "Food Ordering System - Toylo";
            this.Size = new Size(1200, 700);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(240, 240, 240);
            this.Resize += (s, e) => ApplyResponsiveSidebar();

            // Sidebar Panel
            sidebarPanel = new Panel
            {
                Dock = DockStyle.Left,
                Width = ExpandedSidebarWidth,
                BackColor = Color.FromArgb(52, 73, 94)
            };
            this.Controls.Add(sidebarPanel);

            // Title Label
            lblTitle = new Label
            {
                Text = "FOOD ORDERING\nSYSTEM",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.White,
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Top,
                Height = 100,
                Padding = new Padding(10)
            };
            sidebarPanel.Controls.Add(lblTitle);

            btnToggleSidebar = new Button
            {
                Text = "⮜",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(41, 128, 185),
                FlatStyle = FlatStyle.Flat,
                Size = new Size(30, 30),
                Location = new Point(ExpandedSidebarWidth - 40, 10),
                Cursor = Cursors.Hand
            };
            btnToggleSidebar.FlatAppearance.BorderSize = 0;
            btnToggleSidebar.Click += (s, e) => ToggleSidebar();
            sidebarPanel.Controls.Add(btnToggleSidebar);

            // Menu Buttons
            btnOrders = CreateMenuButton("📋 Orders", 100);
            btnMenuItems = CreateMenuButton("🍔 Menu Items", 160);
            btnOrderHistory = CreateMenuButton("📜 Order History", 220);

            sidebarPanel.Controls.Add(btnOrders);
            sidebarPanel.Controls.Add(btnMenuItems);
            sidebarPanel.Controls.Add(btnOrderHistory);

            // Content Panel
            contentPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Padding = new Padding(10)
            };
            this.Controls.Add(contentPanel);

            // Button Events
            btnOrders.Click += (s, e) => LoadForm(new OrdersForm());
            btnMenuItems.Click += (s, e) => LoadForm(new MenuItemsForm());
            btnOrderHistory.Click += (s, e) => LoadForm(new OrderHistoryForm());

            UpdateMenuButtonsLayout();
        }

        private Button CreateMenuButton(string text, int top)
        {
            Button btn = new Button
            {
                Text = text,
                Font = new Font("Segoe UI", 11),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(52, 73, 94),
                FlatStyle = FlatStyle.Flat,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(15, 0, 0, 0),
                Size = new Size(ExpandedSidebarWidth, 50),
                Location = new Point(0, top),
                Cursor = Cursors.Hand
            };
            btn.FlatAppearance.BorderSize = 0;
            btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(41, 128, 185);
            return btn;
        }

        private void LoadForm(Form form)
        {
            if (activeForm != null)
                activeForm.Close();

            activeForm = form;
            form.TopLevel = false;
            form.FormBorderStyle = FormBorderStyle.None;
            form.Dock = DockStyle.Fill;
            contentPanel.Controls.Clear();
            contentPanel.Controls.Add(form);
            form.Show();
        }

        private void ToggleSidebar()
        {
            isSidebarCollapsed = !isSidebarCollapsed;
            sidebarPanel.Width = isSidebarCollapsed ? CollapsedSidebarWidth : ExpandedSidebarWidth;
            btnToggleSidebar.Text = isSidebarCollapsed ? "⮞" : "⮜";
            btnToggleSidebar.Location = new Point(sidebarPanel.Width - 40, 10);
            UpdateMenuButtonsLayout();
        }

        private void UpdateMenuButtonsLayout()
        {
            int width = sidebarPanel.Width;
            foreach (var btn in new[] { btnOrders, btnMenuItems, btnOrderHistory })
            {
                btn.Size = new Size(width, 50);
                if (isSidebarCollapsed)
                {
                    btn.TextAlign = ContentAlignment.MiddleCenter;
                    btn.Padding = new Padding(0);
                    if (btn == btnOrders) btn.Text = "📋";
                    else if (btn == btnMenuItems) btn.Text = "🍔";
                    else if (btn == btnOrderHistory) btn.Text = "📜";
                }
                else
                {
                    btn.TextAlign = ContentAlignment.MiddleLeft;
                    btn.Padding = new Padding(15, 0, 0, 0);
                    if (btn == btnOrders) btn.Text = "📋 Orders";
                    else if (btn == btnMenuItems) btn.Text = "🍔 Menu Items";
                    else if (btn == btnOrderHistory) btn.Text = "📜 Order History";
                }
            }
        }

        private void ApplyResponsiveSidebar()
        {
            if (this.ClientSize.Width < 900 && !isSidebarCollapsed)
            {
                isSidebarCollapsed = true;
                sidebarPanel.Width = CollapsedSidebarWidth;
                btnToggleSidebar.Text = "⮞";
            }
            else if (this.ClientSize.Width >= 900 && isSidebarCollapsed)
            {
                isSidebarCollapsed = false;
                sidebarPanel.Width = ExpandedSidebarWidth;
                btnToggleSidebar.Text = "⮜";
            }
            btnToggleSidebar.Location = new Point(sidebarPanel.Width - 40, 10);
            UpdateMenuButtonsLayout();
        }
    }
}