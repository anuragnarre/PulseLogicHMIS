import sys
from PIL import Image, ImageDraw, ImageFont

icon_path = r"C:\Users\Anurag\.gemini\antigravity-ide\brain\b32421b1-dc00-466c-ae0c-f6b7aabcd652\media__1780784068340.png"
out_path1 = r"D:\App and Hardware Project\App\PulseLogicHMR_v2.0\Code\Websites\PulseLogicHMR\wwwroot\themes\theme-default\images\hospitals-logo\hospital-logo-landingpage.png"
out_path2 = r"D:\App and Hardware Project\App\PulseLogicHMR_v2.0\Code\Websites\PulseLogicHMR\wwwroot\themes\theme-default\images\hospitals-logo\hospital-logo-landingpage2.png"

# Canvas size (Retina for 155x36 -> 310x72)
width = 400
height = 72

# Create transparent image
img = Image.new('RGBA', (width, height), (255, 255, 255, 0))

# Load user icon
try:
    icon = Image.open(icon_path).convert("RGBA")
    # Resize icon to fit height
    icon = icon.resize((height, height), Image.Resampling.LANCZOS)
    img.paste(icon, (0, 0), icon)
except Exception as e:
    print(f"Error loading icon: {e}")
    sys.exit(1)

# Draw text
draw = ImageDraw.Draw(img)
text = "PulseLogic HMR"

# Try to load a nice font, fallback to default
try:
    font = ImageFont.truetype("arialbd.ttf", 36)
except IOError:
    try:
        font = ImageFont.truetype("segoeui.ttf", 36)
    except IOError:
        font = ImageFont.load_default()

# Get text bounding box for vertical alignment
bbox = draw.textbbox((0, 0), text, font=font)
text_w = bbox[2] - bbox[0]
text_h = bbox[3] - bbox[1]

# Draw white text so it looks good on the blue top navigation bar
draw.text((height + 15, (height - text_h) / 2 - 5), text, font=font, fill=(255, 255, 255, 255))

# Crop the image to fit the text + icon perfectly to avoid whitespace issues
img = img.crop((0, 0, height + 15 + text_w + 10, height))

# Save
img.save(out_path1)
img.save(out_path2)
print("Logos successfully generated!")
