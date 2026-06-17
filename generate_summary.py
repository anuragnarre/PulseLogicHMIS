import os
import re
import glob

base_dir = r"d:\App and Hardware Project\App\PulseLogicHMR_v2.0.1"
summary_dir = os.path.join(base_dir, "Summary")

if not os.path.exists(summary_dir):
    os.makedirs(summary_dir)

backend_controllers_dir = os.path.join(base_dir, r"Code\Websites\PulseLogicHMR\Controllers")
frontend_app_dir = os.path.join(base_dir, r"Code\Websites\PulseLogicHMR\wwwroot\PulseLogicApp\src\app")

# Collect Backend Modules
backend_modules = []
if os.path.exists(backend_controllers_dir):
    backend_modules = [d for d in os.listdir(backend_controllers_dir) if os.path.isdir(os.path.join(backend_controllers_dir, d))]

# Collect Frontend Modules
frontend_modules = []
if os.path.exists(frontend_app_dir):
    frontend_modules = [d for d in os.listdir(frontend_app_dir) if os.path.isdir(os.path.join(frontend_app_dir, d))]

# Map them together based on lower case names
module_map = {}
for m in backend_modules:
    module_map[m.lower()] = {"backend": m, "frontend": None}

for m in frontend_modules:
    key = m.lower().replace("-", "")
    if key in module_map:
        module_map[key]["frontend"] = m
    else:
        # Check partial match
        matched = False
        for k in module_map.keys():
            if k in key or key in k:
                module_map[k]["frontend"] = m
                matched = True
                break
        if not matched:
            module_map[key] = {"backend": None, "frontend": m}

def extract_backend_endpoints(module_dir):
    endpoints = []
    cs_files = glob.glob(os.path.join(module_dir, "*.cs"))
    for f in cs_files:
        try:
            with open(f, 'r', encoding='utf-8-sig') as file:
                content = file.read()
                # Find methods with Http attributes
                matches = re.finditer(r'\[Http(Get|Post|Put|Delete).*?\]\s*(?:\[Route\("([^"]*)"\)\])?\s*public\s+(?:async\s+)?(?:Task<)?(?:IActionResult|ActionResult|string|object|HttpResponseMessage|[\w<>]+)(?:>)?\s+(\w+)\s*\(', content, re.MULTILINE | re.IGNORECASE)
                for match in matches:
                    method_type = "HTTP" + match.group(1).upper()
                    route = match.group(2) if match.group(2) else ""
                    method_name = match.group(3)
                    endpoints.append(f"- **{method_type}** `{route if route else method_name}` (Method: `{method_name}`)")
        except Exception as e:
            pass
    return endpoints

def extract_frontend_info(module_dir):
    info = {"components": [], "services": [], "routes": []}
    for root, dirs, files in os.walk(module_dir):
        for f in files:
            if f.endswith('.component.ts'):
                info["components"].append(f.replace('.component.ts', ''))
            elif f.endswith('.service.ts') or f.endswith('.endpoint.ts'):
                info["services"].append(f)
            elif f.endswith('-routing.module.ts') or f.endswith('.routing.ts'):
                try:
                    with open(os.path.join(root, f), 'r', encoding='utf-8') as file:
                        content = file.read()
                        routes = re.findall(r'path:\s*[\'"]([^\'"]*)[\'"]', content)
                        for r in routes:
                            if r and r not in info["routes"]:
                                info["routes"].append(r)
                except:
                    pass
    return info

for key, mod in module_map.items():
    mod_name = mod["backend"] if mod["backend"] else mod["frontend"]
    mod_name_display = mod_name.capitalize()
    
    summary_file = os.path.join(summary_dir, f"{mod_name_display}_Summary.md")
    
    with open(summary_file, 'w', encoding='utf-8') as sf:
        sf.write(f"# {mod_name_display} Module Summary\n\n")
        
        # Backend Details
        sf.write(f"## Backend Implementation\n")
        if mod["backend"]:
            b_dir = os.path.join(backend_controllers_dir, mod["backend"])
            sf.write(f"**Directory**: `Code/Websites/PulseLogicHMR/Controllers/{mod['backend']}`\n\n")
            
            endpoints = extract_backend_endpoints(b_dir)
            if endpoints:
                sf.write("### API Endpoints\n")
                for e in endpoints:
                    sf.write(f"{e}\n")
            else:
                sf.write("No direct HTTP endpoints found in controllers.\n")
                
            # Also check Services if present
            s_dir = os.path.join(base_dir, r"Code\Websites\PulseLogicHMR\Services", mod["backend"])
            if os.path.exists(s_dir):
                sf.write(f"\n### Services\n")
                sf.write(f"Backend services encapsulate business logic and data access for {mod_name_display}.\n")
                for sf_name in os.listdir(s_dir):
                    if sf_name.endswith('.cs'):
                        sf.write(f"- `{sf_name}`\n")
        else:
            sf.write("No dedicated backend controller folder found.\n")
            
        sf.write("\n---\n\n")
        
        # Frontend Details
        sf.write(f"## Frontend Implementation\n")
        if mod["frontend"]:
            f_dir = os.path.join(frontend_app_dir, mod["frontend"])
            sf.write(f"**Directory**: `wwwroot/PulseLogicApp/src/app/{mod['frontend']}`\n\n")
            
            f_info = extract_frontend_info(f_dir)
            
            if f_info["routes"]:
                sf.write("### Routing / Views\n")
                for r in f_info["routes"]:
                    sf.write(f"- `/{r}`\n")
                sf.write("\n")
                
            if f_info["components"]:
                sf.write("### Angular Components\n")
                for c in f_info["components"]:
                    sf.write(f"- `{c}`\n")
                sf.write("\n")
                
            if f_info["services"]:
                sf.write("### Angular Services & Endpoints\n")
                for s in f_info["services"]:
                    sf.write(f"- `{s}`\n")
                sf.write("\n")
        else:
            sf.write("No dedicated frontend app folder found.\n")

# Generate an overarching README
readme_file = os.path.join(summary_dir, "README.md")
with open(readme_file, 'w', encoding='utf-8') as rf:
    rf.write("# PulseLogicHMR System Architecture Summary\n\n")
    rf.write("This directory contains a full structure of the working code and app functionality for each module in the system.\n\n")
    rf.write("## Modules Overview\n\n")
    for key, mod in sorted(module_map.items()):
        mod_name = mod["backend"] if mod["backend"] else mod["frontend"]
        mod_name_display = mod_name.capitalize()
        rf.write(f"- [{mod_name_display}](./{mod_name_display}_Summary.md)\n")
    
    rf.write("\n## Architecture Overview\n")
    rf.write("The application uses an Angular 10+ frontend structured into feature modules, communicating with an ASP.NET Core Web API backend.\n")
    rf.write("- **Frontend:** Angular components organize the UI, with services handling HTTP communication via API endpoints to the backend. Routing is modularized per feature.\n")
    rf.write("- **Backend:** ASP.NET Core Controllers expose RESTful API endpoints. They delegate complex operations to the `Services` layer (BL), which orchestrates data access via Entity Framework Core (`DalLayer`) and Data Access Logic functions.\n")

print(f"Summary generated in {summary_dir}")
