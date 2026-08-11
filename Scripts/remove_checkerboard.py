import os
from rembg import remove

input_dir = "c:/Users/Anurag/Downloads/PL logo/upscaled-transparent"
output_dir = "c:/Users/Anurag/Downloads/PL logo/cleaned-transparent"

if not os.path.exists(output_dir):
    os.makedirs(output_dir)

for file in os.listdir(input_dir):
    if file.endswith(".png"):
        input_path = os.path.join(input_dir, file)
        output_path = os.path.join(output_dir, file)
        
        print(f"Processing {file}...")
        with open(input_path, 'rb') as i:
            with open(output_path, 'wb') as o:
                input_data = i.read()
                output_data = remove(input_data)
                o.write(output_data)
        print(f"Saved to {output_path}")

print("All done!")
