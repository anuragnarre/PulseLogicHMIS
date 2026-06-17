const { app, BrowserWindow, session } = require('electron');
const path = require('path');
const url = require('url');

// The IIS backend URL — matches the port set during installation
const BACKEND_URL = 'http://localhost:8080';

function createWindow() {
  const win = new BrowserWindow({
    width: 1366,
    height: 768,
    minWidth: 1024,
    minHeight: 600,
    webPreferences: {
      nodeIntegration: false,
      contextIsolation: true,
      webSecurity: false  // Allow loading local file:// assets and cross-origin API calls to IIS
    },
    title: 'PulseLogic EMR',
    show: false  // Don't show until fully loaded to avoid white flash
  });

  // Intercept all relative API calls (/api/...) and proxy them to the IIS backend
  // This is the core fix: Electron's file:// origin can't call relative URLs
  session.defaultSession.webRequest.onBeforeRequest({ urls: ['/api/*', '/DynamicReporting/*', '/BillingReports/*', '/Reporting/*'] }, (details, callback) => {
    const requestedUrl = details.url;
    // If it's a relative-path request that went through file://, rewrite it
    if (requestedUrl.startsWith('file://')) {
      const relativePart = requestedUrl.replace(/^file:\/\/[^\/]*/i, '');
      callback({ redirectURL: BACKEND_URL + relativePart });
    } else {
      callback({});
    }
  });

  // Load the Angular production build
  const indexPath = path.join(__dirname, 'dist', 'PulseLogicApp', 'index.html');
  win.loadFile(indexPath);

  // Show window once content is ready to prevent white flash
  win.once('ready-to-show', () => {
    win.show();
  });

  win.on('closed', () => {
    app.quit();
  });

  // Open DevTools in dev mode — remove for production
  // win.webContents.openDevTools();
}

app.whenReady().then(createWindow);

app.on('window-all-closed', () => {
  if (process.platform !== 'darwin') {
    app.quit();
  }
});

app.on('activate', () => {
  if (BrowserWindow.getAllWindows().length === 0) {
    createWindow();
  }
});
