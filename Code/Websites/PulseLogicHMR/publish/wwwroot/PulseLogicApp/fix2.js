const fs = require('fs');
const p = 'd:/App and Hardware Project/App/PulseLogicHMR_v2.0/Code/Websites/PulseLogicHMR/wwwroot/PulseLogicApp/src/app/shared/pulselogic-grid/grid-columns/billing.columns.ts';
let c = fs.readFileSync(p, 'utf8');
c = c.replace(/export const BillingGridColumns = \{/, 'export class BillingGridColumns {');
c = c.replace(/(\w+):\s*\[/g, '$1 = [');
fs.writeFileSync(p, c);
