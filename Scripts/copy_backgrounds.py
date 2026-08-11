import os
from PIL import Image

def copy_and_convert_backgrounds():
    light_in = r"C:\Users\Anurag\.gemini\antigravity-ide\brain\372bd866-a92b-4df6-bd08-78a0aff3b07f\login_left_light_1780809523961.png"
    dark_in = r"C:\Users\Anurag\.gemini\antigravity-ide\brain\372bd866-a92b-4df6-bd08-78a0aff3b07f\login_left_dark_1780809546332.png"
    dest_dir = r"D:\App and Hardware Project\App\PulseLogicHMR_v2.0\Code\Websites\PulseLogicHMR\wwwroot\themes\theme-default\images"

    os.makedirs(dest_dir, exist_ok=True)

    # 1. Process Light Background
    if os.path.exists(light_in):
        img_light = Image.open(light_in)
        # Convert to RGB (required for JPEG format)
        img_light_rgb = img_light.convert("RGB")
        
        # Save as login-left-light.jpg
        p_light = os.path.join(dest_dir, "login-left-light.jpg")
        img_light_rgb.save(p_light, "JPEG", quality=90)
        print(f"Saved: {p_light}")

        # Save as legacy login-left.jpg
        p_legacy_jpg = os.path.join(dest_dir, "login-left.jpg")
        img_light_rgb.save(p_legacy_jpg, "JPEG", quality=90)
        print(f"Saved: {p_legacy_jpg}")

        # Save as legacy login-left.png
        p_legacy_png = os.path.join(dest_dir, "login-left.png")
        img_light.save(p_legacy_png, "PNG")
        print(f"Saved: {p_legacy_png}")
    else:
        print(f"Error: {light_in} not found")

    # 2. Process Dark Background
    if os.path.exists(dark_in):
        img_dark = Image.open(dark_in)
        img_dark_rgb = img_dark.convert("RGB")
        
        # Save as login-left-dark.jpg
        p_dark = os.path.join(dest_dir, "login-left-dark.jpg")
        img_dark_rgb.save(p_dark, "JPEG", quality=90)
        print(f"Saved: {p_dark}")
    else:
        print(f"Error: {dark_in} not found")

    print("Background image replacements completed successfully!")

if __name__ == "__main__":
    copy_and_convert_backgrounds()
