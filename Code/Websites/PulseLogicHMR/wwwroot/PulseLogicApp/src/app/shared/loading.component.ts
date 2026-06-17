import { Component, Directive } from '@angular/core';
import { Input } from "@angular/core"
@Component({
    selector: "pulselogic-loading",
    template: `
   <div class="pulselogic-loading-image" [hidden]="!showLoading">
    <div class="pl-spinner-wrapper">
      <div class="pl-spinner-ring">
        <div class="pl-spinner-inner"></div>
        <div class="pl-spinner-logo">
          <svg viewBox="0 0 40 40" xmlns="http://www.w3.org/2000/svg">
            <path d="M20 8 L28 14 L28 26 L20 32 L12 26 L12 14 Z" fill="none" stroke="#4F46E5" stroke-width="2.5" opacity="0.9"/>
            <circle cx="20" cy="20" r="4" fill="#4F46E5" opacity="0.9"/>
          </svg>
        </div>
      </div>
      <p class="pl-loading-text">Loading&hellip;</p>
    </div>
   </div>`,
    styleUrls: ['../../../../themes/theme-default/loading.component.css']
})
export class LoadingComponent {
    @Input("loading")
    public showLoading: boolean = false;
    pathToImage: string = "";

    constructor() {
        this.pathToImage = "../../../../themes/theme-default/images/loading.gif";
    }
 
}

