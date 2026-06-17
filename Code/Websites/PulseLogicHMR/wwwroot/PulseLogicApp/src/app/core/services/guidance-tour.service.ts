import { Injectable } from '@angular/core';
import * as introJs from 'intro.js';

@Injectable({
  providedIn: 'root'
})
export class GuidanceTourService {

  constructor() { }

  public startTour(): void {
    const intro = introJs();
    intro.setOptions({
      steps: [
        {
          title: 'Welcome',
          intro: 'Welcome to the PulseLogic EMR! Let us show you around.'
        },
        {
          element: document.querySelector('.modern-sidebar') || undefined,
          title: 'Navigation Menu',
          intro: 'This area allows you to access all modules and features.'
        },
        {
          element: document.querySelector('.modern-header-search') || undefined,
          title: 'Global Search',
          intro: 'Use this to quickly search for patients or settings across the app.'
        },
        {
          element: document.querySelector('.modern-header-actions') || undefined,
          title: 'Actions & Settings',
          intro: 'Here you can toggle themes, adjust accessibility settings, and view your profile.'
        }
      ],
      showProgress: true,
      showBullets: false
    });
    intro.start();
  }
}
