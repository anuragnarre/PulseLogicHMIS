import os
import sys
from PIL import Image, ImageDraw, ImageFont

def process_logo():
    icon_in = r"C:\Users\Anurag\.gemini\antigravity-ide\brain\372bd866-a92b-4df6-bd08-78a0aff3b07f\media__1780808672477.jpg"
    img_dir = r"D:\App and Hardware Project\App\PulseLogicHMR_v2.0\Code\Websites\PulseLogicHMR\wwwroot\themes\theme-default\images"
    
    # 1. Load and process the source icon
    try:
        src_icon = Image.open(icon_in).convert("RGBA")
    except Exception as e:
        print(f"Error loading source icon: {e}")
        return False

    width, height = src_icon.size
    
    # Crop the icon 4 pixels inwards to remove any black border compression artifacts
    mask = Image.new('L', (width, height), 0)
    draw_mask = ImageDraw.Draw(mask)
    draw_mask.ellipse((4, 4, width - 4, height - 4), fill=255)
    src_icon.putalpha(mask)

    # 2. Font configuration (scaled up to 48px)
    font_size = 48
    font = None
    fonts_to_try = ["segoeuib.ttf", "segoeui.ttf", "arialbd.ttf", "arial.ttf", "Roboto-Bold.ttf"]
    for f_name in fonts_to_try:
        try:
            font = ImageFont.truetype(f_name, font_size)
            print(f"Loaded font: {f_name}")
            break
        except IOError:
            continue
    if font is None:
        font = ImageFont.load_default()
        print("Using default fallback font")

    # 3. Helper to generate horizontal logo (Icon + Text)
    def generate_horizontal_logo(text, text_color, bg_color=(255, 255, 255, 0)):
        logo_height = 96
        # Start with a wide temporary canvas
        temp_img = Image.new('RGBA', (800, logo_height), bg_color)
        
        # Resize processed icon to match height
        icon_resized = src_icon.resize((logo_height, logo_height), Image.Resampling.LANCZOS)
        
        # Paste icon
        temp_img.paste(icon_resized, (0, 0), icon_resized)
        
        # Draw text
        draw = ImageDraw.Draw(temp_img)
        bbox = draw.textbbox((0, 0), text, font=font)
        text_w = bbox[2] - bbox[0]
        text_h = bbox[3] - bbox[1]
        
        # Vertically centered text
        text_x = logo_height + 18
        text_y = (logo_height - text_h) // 2 - 5
        
        draw.text((text_x, text_y), text, font=font, fill=text_color)
        
        # Crop to bounding box of content
        cropped_w = text_x + text_w + 18
        final_img = temp_img.crop((0, 0, cropped_w, logo_height))
        return final_img

    # 4. Generate variations
    # White Text Version (for dark navbars)
    logo_white = generate_horizontal_logo("PulseLogic EMR", (255, 255, 255, 255))
    
    # Brand Blue Version (for login pages/general use)
    logo_blue = generate_horizontal_logo("PulseLogic EMR", (14, 165, 233, 255))

    # 5. Define target file paths
    # White text destinations (Header Nav)
    white_paths = [
        os.path.join(img_dir, "hospitals-logo", "hospital-logo-landingpage.png"),
        os.path.join(img_dir, "hospitals-logo", "hospital-logo-landingpage2.png"),
    ]
    
    # Brand blue text destinations (Login & Branding)
    blue_paths = [
        os.path.join(img_dir, "pulselogic_logo_v2.png"),
        os.path.join(img_dir, "pulselogic-logo.png"),
        os.path.join(img_dir, "pulselogic.logo.PNG"),
        os.path.join(img_dir, "hospital-logo.PNG"),
        os.path.join(img_dir, "hospital-logo-phrm.PNG"),
    ]

    # Save White Text logos
    for p in white_paths:
        os.makedirs(os.path.dirname(p), exist_ok=True)
        logo_white.save(p, "PNG")
        print(f"Saved logo: {p}")

    # Save Blue Text logos
    for p in blue_paths:
        os.makedirs(os.path.dirname(p), exist_ok=True)
        logo_blue.save(p, "PNG")
        print(f"Saved logo: {p}")

    print("Logo replacement completed successfully!")
    return True

if __name__ == "__main__":
    process_logo()
