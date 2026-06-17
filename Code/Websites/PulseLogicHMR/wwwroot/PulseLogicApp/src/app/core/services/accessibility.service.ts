import { Injectable, Inject } from '@angular/core';
import { DOCUMENT } from '@angular/common';
import { BehaviorSubject } from 'rxjs';

export interface AccessibilitySettings {
  highContrast: boolean;
  textScale: number; // 1 = 100%, 1.25 = 125%, 1.5 = 150%
  reducedMotion: boolean;
  isEasyMode: boolean;
}

@Injectable({
  providedIn: 'root'
})
export class AccessibilityService {
  private readonly STORAGE_KEY = 'pulselogic_accessibility_settings';

  private defaultSettings: AccessibilitySettings = {
    highContrast: false,
    textScale: 1,
    reducedMotion: false,
    isEasyMode: false
  };

  private settingsSubject = new BehaviorSubject<AccessibilitySettings>(this.defaultSettings);
  public settings$ = this.settingsSubject.asObservable();

  constructor(@Inject(DOCUMENT) private document: Document) {
    this.loadSettings();
    this.applyClassesToBody(this.currentSettings);
  }

  private loadSettings(): void {
    const saved = localStorage.getItem(this.STORAGE_KEY);
    if (saved) {
      try {
        const parsed = JSON.parse(saved) as AccessibilitySettings;
        this.settingsSubject.next({ ...this.defaultSettings, ...parsed });
      } catch (e) {
        console.error('Failed to parse accessibility settings', e);
      }
    }
  }

  private saveSettings(settings: AccessibilitySettings): void {
    localStorage.setItem(this.STORAGE_KEY, JSON.stringify(settings));
    this.settingsSubject.next(settings);
    this.applyClassesToBody(settings);
  }

  private applyClassesToBody(settings: AccessibilitySettings): void {
    const body = this.document.body;
    
    // High Contrast
    if (settings.highContrast) {
      body.classList.add('theme-high-contrast');
    } else {
      body.classList.remove('theme-high-contrast');
    }

    // Reduced Motion
    if (settings.reducedMotion) {
      body.classList.add('reduced-motion');
    } else {
      body.classList.remove('reduced-motion');
    }

    // Easy Mode (can be used for global overrides)
    if (settings.isEasyMode) {
      body.classList.add('easy-mode');
    } else {
      body.classList.remove('easy-mode');
    }

    // Text Scaling
    body.classList.remove('text-scale-125', 'text-scale-150');
    if (settings.textScale === 1.25) {
      body.classList.add('text-scale-125');
    } else if (settings.textScale === 1.5) {
      body.classList.add('text-scale-150');
    }
  }

  public get currentSettings(): AccessibilitySettings {
    return this.settingsSubject.getValue();
  }

  public updateSettings(partialSettings: Partial<AccessibilitySettings>): void {
    const newSettings = { ...this.currentSettings, ...partialSettings };
    this.saveSettings(newSettings);
  }

  public toggleHighContrast(): void {
    this.updateSettings({ highContrast: !this.currentSettings.highContrast });
  }

  public toggleEasyMode(): void {
    this.updateSettings({ isEasyMode: !this.currentSettings.isEasyMode });
  }

  public toggleReducedMotion(): void {
    this.updateSettings({ reducedMotion: !this.currentSettings.reducedMotion });
  }

  public setTextScale(scale: number): void {
    this.updateSettings({ textScale: scale });
  }
}

