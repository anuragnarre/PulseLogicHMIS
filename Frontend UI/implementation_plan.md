# Completely Replace Legacy Metronic Theme with Modern Stitch UI Layout

The current application still relies on the legacy "Metronic" CSS theme wrappers (`.page-header-fixed`, `.page-sidebar`, `.page-content-wrapper`). To achieve the exact look of the Stitch mockups and completely abandon the old theme, we need to strip out these legacy wrappers from the root layout component and build a clean, modern layout using pure CSS Flexbox/Grid.

## Proposed Changes

### 1. Root Layout Structure Rewrite
#### [MODIFY] [AppMain.html](file:///d:/App and Hardware Project/App/PulseLogicHMR_v2.0.1/Code/Websites/PulseLogicHMR/wwwroot/PulseLogicApp/src/app/view/home-view/AppMain.html)
I will completely rewrite the HTML structure of the root layout, removing all Metronic legacy classes. The new structure will be:
- **`modern-app-layout`**: A full-height flex container.
- **`modern-header`**: A crisp, white top navigation bar featuring the PulseLogic logo on the left, a centered global search bar, and the user profile dropdown on the right.
- **`modern-body`**: A flex container holding the sidebar and main content.
- **`modern-sidebar`**: A clean, dark (`#1B263B`) sidebar. The navigation menu will be rebuilt as a simple `<ul>` without the legacy dropdown/bubble artifacts. The active states and hover effects will be completely customized.
- **`modern-main-content`**: The container for `<router-outlet>`, featuring a soft gray background (`#F4F6F9`) with appropriate padding, allowing the previously designed `.pl-card` components to shine.

### 2. Global Styles Overhaul
#### [MODIFY] [styles.css](file:///d:/App and Hardware Project/App/PulseLogicHMR_v2.0.1/Code/Websites/PulseLogicHMR/wwwroot/PulseLogicApp/src/styles.css)
I will remove all the `/* OVERRIDE METRONIC CSS */` sections. Instead, I will write fresh, explicit CSS for `.modern-header`, `.modern-sidebar`, and `.modern-main-content`. This guarantees that no lingering legacy CSS will interfere with our layout.

> [!WARNING]
> Because we are removing the legacy Metronic CSS classes from the root layout, some deep, un-redesigned child components might look slightly different if they implicitly relied on global Metronic rules. However, this is the necessary step to completely break free from the old theme and match the Stitch design 1-to-1.

## User Review Required
Please review the plan. If you approve of completely stripping the legacy layout classes in favor of a modern, custom CSS layout, I will proceed with the rewrite and execute `npm run build` so you can verify the results.
