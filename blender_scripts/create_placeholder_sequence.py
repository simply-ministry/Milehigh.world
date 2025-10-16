import os
from PIL import Image, ImageDraw, ImageFont

def create_placeholder_sequence(num_frames, output_folder, size=(1920, 1080)):
    """Generates a sequence of simple, numbered PNG images."""
    if not os.path.exists(output_folder):
        os.makedirs(output_folder)
        print(f"Created directory: {output_folder}")

    try:
        font = ImageFont.truetype("arial.ttf", 100)
    except IOError:
        font = ImageFont.load_default()

    for i in range(1, num_frames + 1):
        img = Image.new('RGB', size, color=(i * 10 % 255, 50, 200))
        draw = ImageDraw.Draw(img)
        text = f"FRAME {i:04d}"
        text_w, text_h = draw.textsize(text, font=font)
        position = ((size[0] - text_w) // 2, (size[1] - text_h) // 2)
        draw.text(position, text, fill=(255, 255, 255), font=font)

        filename = os.path.join(output_folder, f"temp_frame_{i:04d}.png")
        img.save(filename)

    print(f"\nSuccessfully generated {num_frames} frames in: {output_folder}")

# --- EXECUTE THE GENERATION ---
# Set the path to where you want the temporary images to go
temp_folder = "/path/to/your/temp/sequence_folder/"
create_placeholder_sequence(num_frames=60, output_folder=temp_folder)