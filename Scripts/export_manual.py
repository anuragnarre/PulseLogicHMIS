import markdown
import os
import shutil

md_path = r"C:\Users\Anurag\.gemini\antigravity-ide\brain\9d3a5e67-5aa7-4be4-ad1d-5afdfdf73159\PulseLogicHMR_User_Manual.md"
web_dir = r"d:\App and Hardware Project\App\PulseLogicHMR_v2.0.1\Code\Websites\PulseLogicHMR\wwwroot\PulseLogicManual"
assets_dir = os.path.join(web_dir, "assets")
os.makedirs(assets_dir, exist_ok=True)

image_map = {
    r"C:/Users/Anurag/.gemini/antigravity-ide/brain/9d3a5e67-5aa7-4be4-ad1d-5afdfdf73159/final_dashboard_light_1781627097129.png": "dashboard_light.png",
    r"C:/Users/Anurag/.gemini/antigravity-ide/brain/9d3a5e67-5aa7-4be4-ad1d-5afdfdf73159/final_dashboard_dark_1781627122952.png": "dashboard_dark.png",
    r"C:/Users/Anurag/.gemini/antigravity-ide/brain/9d3a5e67-5aa7-4be4-ad1d-5afdfdf73159/shortcuts_modal_1781627143175.png": "shortcuts_modal.png",
    r"C:/Users/Anurag/.gemini/antigravity-ide/brain/9d3a5e67-5aa7-4be4-ad1d-5afdfdf73159/phase1_patients_1781627670221.png": "phase1_patients.png",
    r"C:/Users/Anurag/.gemini/antigravity-ide/brain/9d3a5e67-5aa7-4be4-ad1d-5afdfdf73159/phase1_clinical_1781627814398.png": "phase1_clinical.png",
    r"C:/Users/Anurag/.gemini/antigravity-ide/brain/9d3a5e67-5aa7-4be4-ad1d-5afdfdf73159/phase1_laboratory_1781627930848.png": "phase1_laboratory.png",
    r"C:/Users/Anurag/.gemini/antigravity-ide/brain/9d3a5e67-5aa7-4be4-ad1d-5afdfdf73159/phase1_dispensary_1781627983901.png": "phase1_dispensary.png",
    r"C:/Users/Anurag/.gemini/antigravity-ide/brain/9d3a5e67-5aa7-4be4-ad1d-5afdfdf73159/phase1_billing_1781628001066.png": "phase1_billing.png",
}

for src, fname in image_map.items():
    real_src = src.replace('/', '\\')
    if os.path.exists(real_src):
        shutil.copy(real_src, os.path.join(assets_dir, fname))

with open(md_path, 'r', encoding='utf-8') as f:
    text = f.read()

for src, fname in image_map.items():
    text = text.replace(src, f"./assets/{fname}")

text = text.replace('````carousel\n', '').replace('\n<!-- slide -->\n', '\n\n').replace('````\n', '')

html = markdown.markdown(text)
full_html = f'''<!DOCTYPE html>
<html><head>
<meta charset="utf-8">
<title>PulseLogicHMR User Manual</title>
<style>
body {{ font-family: -apple-system, BlinkMacSystemFont, "Segoe UI", Roboto, Helvetica, Arial, sans-serif; line-height: 1.6; padding: 40px; max-width: 900px; margin: 0 auto; color: #333; }}
img {{ max-width: 100%; border: 1px solid #ddd; border-radius: 4px; box-shadow: 0 2px 4px rgba(0,0,0,0.1); margin: 20px 0; }}
h1, h2, h3 {{ color: #111; border-bottom: 1px solid #eee; padding-bottom: 5px; }}
code {{ background: #f4f4f4; padding: 2px 5px; border-radius: 3px; font-family: monospace; }}
</style>
</head><body>{html}</body></html>
'''

with open(os.path.join(web_dir, "index.html"), "w", encoding="utf-8") as f:
    f.write(full_html)
    
with open(os.path.join(web_dir, "PulseLogicHMR_User_Manual.md"), "w", encoding="utf-8") as f:
    f.write(text)

with open(os.path.join(web_dir, "PulseLogicHMR_User_Manual.doc"), "w", encoding="utf-8") as f:
    f.write(full_html)

try:
    from weasyprint import HTML
    HTML(string=full_html, base_url=web_dir).write_pdf(os.path.join(web_dir, "PulseLogicHMR_User_Manual.pdf"))
    print("PDF generated successfully.")
except Exception as e:
    print(f"PDF generation failed (Weasyprint needs GTK): {e}")

print("Export complete.")
