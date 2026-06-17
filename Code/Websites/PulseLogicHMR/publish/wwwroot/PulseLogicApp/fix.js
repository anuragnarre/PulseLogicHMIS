const fs = require('fs');
const p = 'd:/App and Hardware Project/App/PulseLogicHMR_v2.0/Code/Websites/PulseLogicHMR/wwwroot/PulseLogicApp/src/app/shared/pulselogic-grid/grid-columns/billing.columns.ts';
let c = fs.readFileSync(p, 'utf8');
c = c.replace(/(\w+)\s*=\s*\[/g, '$1: [');
c = c.replace(/ImagingRequisitionListSearch:\s*\[\s*\};/, 'ImagingRequisitionListSearch: []\n};');
fs.writeFileSync(p, c);
