# PulseLogicHMR — UI and UX Documentation

> **Last Updated:** 2026-07-14 | **Version:** v2.0.1
> **Change Note:** Alert blocking issue fixed (replaced with non-blocking msgBox), global search UX improved, Apple HIG premium redesign elements noted.

---

## 1. Design System (Apple HIG Inspired)

PulseLogicHMR uses a custom design system heavily inspired by Apple Human Interface Guidelines (HIG), tailored for complex medical interfaces.

### Core Principles
- **Clarity:** High contrast, legible typography (`Inter` for UI, `Outfit` for branding).
- **Depth & Hierarchy:** Use of shadows, blur (`backdrop-filter`), and layering to establish visual hierarchy.
- **Glassmorphism:** Semi-transparent surfaces blurring the animated background (prominent on the Login page).

### Theming System
The application supports three dynamic themes, implemented via CSS Custom Properties on the `:root` and `[data-theme]` attributes:
1. **Light:** Clean, clinical, high contrast (default).
2. **Dark:** High-contrast dark mode for low-light environments (radiology, night shifts).
3. **Midnight:** Deep OLED black theme with neon/purple accents.

**Theme Swapping:**
- Driven by Angular `app.component.ts` (`setTheme()`).
- Images (like logos) adapt automatically via CSS `content: url(...)` overrides based on the active data-theme.

---

## 2. Layout & Navigation (AppMain)

The application shell (`AppMain.html`) consists of two main areas:

### Sidebar (Left)
- **Collapsible:** Driven by `sidebarCollapsed` state.
- **Categorised Routing:** Navigation is grouped logically (Overview, Patients, Clinical, Diagnostics, Finance, Pharmacy, People, Administration).
- **Smart Expand:** Accordion-style sub-navigation (`ChildRoutes`) with chevron indicators.
- **Admin Pinning:** Administration tools are pinned to the bottom.

### Topbar (Header)
- **Contextual Center Area:** 
  - *Default:* Displays the Spotlight "Global Search" bar.
  - *Patient Active:* Search is hidden, replaced by a "Patient Context Chip" showing Patient Name, Hospital Number, Age/Gender, and membership status.
- **Action Cluster (Right):**
  - Theme toggles (Light/Dark dots).
  - Notifications badge.
  - User Manual & Keyboard Shortcuts modal triggers.
  - User profile dropdown menu (My Profile, Date Format Preference, Sign Out).

---

## 3. UX Enhancements & Fixes (v2.0.1)

### Non-Blocking Alerts (Phase 7 Fix)
**The Problem:** The Angular initialization process (`app.component.ts`) was using native browser `alert()` calls for errors (e.g., "failed to get user permissions"). Native alerts halt the main UI thread. In a hospital where a user might have 5 tabs open, an alert on an inactive tab freezes the entire browser process, frustrating staff.

**The Solution:**
All `alert()` calls were replaced with the application's internal `msgBoxServ.showMessage()` service.
- **UX Impact:** Errors now slide in gracefully as toast/modal messages. The UI thread is never blocked. Users can continue working or refreshing without being locked out.

### Global Search UX (Phase 7 Fix)
**The Problem:** If the global search API call failed (network drop or 500 error), the spinner would stop but the user received no feedback. The error was only logged to the console.

**The Solution:**
Added a failure callback to `global-search.component.ts`.
- **UX Impact:** Users now see a clear error toast: *"Search failed. Please try again or contact support."* This eliminates confusion when results don't appear.

### Keyboard Shortcuts
A built-in modal (triggered from the topbar) allows power users to define custom keyboard shortcuts (`alt+p` -> `/Patient/Register`). This significantly speeds up workflow for data-entry staff.

### Date Format Preference
Supports switching between English (AD - Gregorian) and Nepali (BS - Bikram Sambat) calendars globally across the UI, catering to local regional requirements.

---

## 4. UI Components

### Alerts & Badges
- **Success:** Green palette (`#10b981`), used for save confirmations.
- **Error:** Red palette (`#ef4444`), used for validation failures.
- **Warning:** Amber palette (`#f59e0b`), used for alerts (e.g., drug interactions).

### Forms & Inputs
- **Floating Labels:** Inputs use placeholders and floating icons.
- **Focus States:** High-visibility focus rings (`var(--input-focus-shadow)`) ensure accessibility and clear keyboard navigation.

### Admin Banner
If the user logs in with high-level systemic rights or a specific diagnostic mode, an animated `admin-banner` drops down from the top edge (red/pink gradient) to visually warn the user they are operating in an elevated mode.
