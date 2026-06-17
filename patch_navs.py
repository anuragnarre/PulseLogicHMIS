import os
import re
import glob

def process_file(filepath):
    with open(filepath, 'r', encoding='utf-8') as f:
        content = f.read()

    original = content

    # 1. Remove style blocks at the top
    content = re.sub(r'<style>.*?</style>', '', content, flags=re.DOTALL)

    # 2. Replace page-bar wrapper
    content = re.sub(r'<div class="page-bar[^"]*">', '<div class="modern-page-tabs">', content)

    # 3. Clean up ul class
    content = re.sub(r'<ul class="page-breadcrumb"\s*(\*ngIf="[^"]+")?>', 
                     lambda m: f'<ul {m.group(1)}>' if m.group(1) else '<ul>', 
                     content)
    content = content.replace('<ul class="page-breadcrumb">', '<ul>') # fallback

    # 4. Swap icon and text
    content = re.sub(r'\{\{route\.DisplayName\}\}\s*<i class="\{\{route\.Css\}\}"></i>',
                     r'<i class="{{route.Css}}"></i> {{route.DisplayName}}', content)

    # 5. Dropdown caret
    content = content.replace('<span class="caret"></span>', '<i class="fa fa-angle-down"></i>')

    # 6. Inline style removal from dropdown li
    content = content.replace('class="dropdown bil-hr-bar" style="display:inline-block;"', 'class="dropdown bil-hr-bar"')
    content = content.replace('class="dropdown bil-hr-bar"\n      style="display:inline-block;"', 'class="dropdown bil-hr-bar"')

    # 7. Router outlet wrapping
    # Remove old wrapper if exists
    content = re.sub(r'<div class="wrapper">\s*<router-outlet></router-outlet>\s*</div>', '<router-outlet></router-outlet>', content)
    
    # Check if router outlet is not already wrapped
    if '<div style="padding: 0 24px;">\n  <router-outlet></router-outlet>\n</div>' not in content:
        content = re.sub(r'<router-outlet></router-outlet>', '<div style="padding: 0 24px;">\n  <router-outlet></router-outlet>\n</div>', content)

    # Clean up empty lines left behind by <style> removal
    content = re.sub(r'^\s*$', '', content, flags=re.MULTILINE)
    # limit consecutive newlines to max 2
    content = re.sub(r'\n{3,}', '\n\n', content)

    if original != content:
        with open(filepath, 'w', encoding='utf-8') as f:
            f.write(content)
        print(f"Updated: {filepath}")
    else:
        print(f"Skipped (no changes): {filepath}")

search_dir = r"d:\App and Hardware Project\App\PulseLogicHMR_v2.0.1\Code\Websites\PulseLogicHMR\wwwroot\PulseLogicApp\src\app"
for root, dirs, files in os.walk(search_dir):
    for file in files:
        if file.endswith("-main.html") and "patients-main.html" not in file and "billing-main.html" not in file:
            process_file(os.path.join(root, file))

print("Done!")
