import os
from PIL import Image

dir_path = "c:/Users/Anurag/Downloads/PL logo/upscaled-transparent"
for file in os.listdir(dir_path):
    if file.endswith(".png"):
        img_path = os.path.join(dir_path, file)
        img = Image.open(img_path).convert("RGBA")
        width, height = img.size
        
        colors = {}
        for x in range(0, width, 5): # sample every 5th pixel to be fast
            for y in range(0, height, 5):
                p = img.getpixel((x, y))
                if p[3] > 0: # only non-transparent
                    colors[p] = colors.get(p, 0) + 1
        
        sorted_colors = sorted(colors.items(), key=lambda item: item[1], reverse=True)
        print(f"File: {file}")
        print("Top non-transparent colors:")
        for c, count in sorted_colors[:10]:
            print(f"  {c}: {count}")
        print("-" * 20)
