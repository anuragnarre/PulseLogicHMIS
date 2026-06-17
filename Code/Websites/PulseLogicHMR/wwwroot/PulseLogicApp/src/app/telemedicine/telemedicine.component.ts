import { Component, OnInit, OnDestroy } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-telemedicine',
  templateUrl: './telemedicine.component.html',
  styleUrls: []
})
export class TelemedicineComponent implements OnInit, OnDestroy {
  public inCall: boolean = false;
  public connectionStatus: string = "Disconnected";
  
  constructor(private router: Router) { }

  ngOnInit() {
    this.connectionStatus = "Ready to connect";
  }

  ngOnDestroy() {
    if(this.inCall) {
        this.endCall();
    }
  }

  startCall() {
    this.inCall = true;
    this.connectionStatus = "Connecting to Twilio Video / WebRTC...";
    
    // Simulate call connection
    setTimeout(() => {
        this.connectionStatus = "Connected - In Call";
    }, 2000);
  }

  endCall() {
    this.inCall = false;
    this.connectionStatus = "Call ended";
  }
}
