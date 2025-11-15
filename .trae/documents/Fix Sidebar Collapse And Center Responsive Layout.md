## Current State
- Sidebar built in `MainForm.cs` as `Panel sidebarPanel` (`Dock = Left`, fixed `Width = 200`), no collapse/toggle.
- Content area `contentPanel` (`Dock = Fill`) loads `OrdersForm`, `MenuItemsForm`, `OrderHistoryForm`.
- Each form lays out controls with fixed `Location`/`Size`, so content is left-aligned and not responsive.

## Goals
1. Sidebar can fully hide/expand and never overlap content.
2. Responsive behavior: content reflows and stays centered at all window sizes.
3. Apply consistent centering to Orders, Menu Items, and Order History.

## Sidebar Collapse (MainForm.cs)
1. Add a header toggle button (`btnToggleSidebar`) inside the sidebar title area.
2. Implement click toggle: set `sidebarPanel.Width = 0` when collapsed, restore to 200 when expanded; `contentPanel` remains `Dock = Fill` so it automatically occupies freed space.
3. Optional: support a “mini” mode (e.g., `Width = 60`) showing just icons; keep full collapse for maximum workspace.
4. Auto-collapse on narrow windows: in `MainForm.Resize`, collapse if `ClientSize.Width < 900`, expand back above threshold.
5. Keep `btnOrders`, `btnMenuItems`, `btnOrderHistory` functional; when collapsed, disable text padding and keep left icons.

## Centered Responsive Layout (Orders/Menu Items/Order History)
For each form (`OrdersForm.cs`, `MenuItemsForm.cs`, `OrderHistoryForm.cs`):
1. Introduce a root `Panel mainLayout` inside the form (`Dock = Fill`, `AutoScroll = true`).
2. Move existing sections (filters/info, summary, grids, action panels) into child panels with `Dock = Top` to stack vertically; remove fixed `Location`.
3. Implement `Resize` handler to center and size sections:
   - Compute `targetWidth = Math.Min(1000, ClientSize.Width - 40)`.
   - Set each top-docked section `Width = targetWidth` and `Left = (ClientSize.Width - targetWidth)/2` to keep horizontal centering.
4. Make data grids expand horizontally: set `Anchor = Top | Left | Right` and `AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill`.
5. Ensure buttons and inputs use `Anchor = Top | Left | Right` within their parent panels to stretch uniformly.
6. Keep consistent margins/padding (e.g., `Padding = new Padding(10)` and `Margin = new Padding(0, 10, 0, 0)` across sections).

## Behaviors And Polishing
- Preserve current fonts/colors; no theme changes.
- Avoid Designer churn: keep layout creation in code-behind as currently done.
- No animations; instant toggle for reliability.
- Add null-safe guards in `LoadForm` and during resize.

## Verification
1. Launch app, resize from 1280×720 down to ~800px width.
2. Toggle sidebar; confirm it fully hides and content fills, with no overlap.
3. Navigate Orders/Menu Items/Order History; confirm panels are centered and grids stretch.
4. Auto-collapse threshold works; text in mini mode remains readable.
5. Check scroll behavior with many rows; ensure vertical stacking and centering remain correct.

## Files To Update
- `MainForm.cs`: add toggle button, collapse/expand logic, Resize handling.
- `OrdersForm.cs`: convert to top-docked stacked panels, add Resize centering.
- `MenuItemsForm.cs`: same as above.
- `OrderHistoryForm.cs`: same as above.

If you approve, I will implement these changes and verify the UI across all three sections.