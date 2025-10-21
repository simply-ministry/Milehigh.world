## Complete Guide: Creating a Video from an Image Sequence with Blender Python

This guide provides a comprehensive workflow for using a Blender Python script to convert a sequence of sequentially named images (e.g., `frame_0001.png`, `frame_0002.png`) into a video file (e.g., `.mp4`).

### Step 1: The Blender Python Script

Here is the complete script that automates the process. This code sets up Blender's Video Sequence Editor (VSE), imports the images, configures the output settings, and renders the video.

```python
import bpy
import os

def images_to_video(image_folder, output_path, fps=30):
    """
    Converts a sequence of images in a folder to a video file using Blender's VSE.

    Args:
        image_folder (str): Absolute path to the folder containing image files.
        output_path (str): Absolute path for the output video file (e.g., /path/to/video.mp4).
        fps (int): Frames per second for the output video.
    """
    scene = bpy.context.scene

    # --- 1. Gather Image Files ---
    image_files = [f for f in os.listdir(image_folder) if f.lower().endswith(('.png', '.jpg', '.jpeg', '.exr', '.tif'))]
    if not image_files:
        print(f"Error: No image files found in '{image_folder}'")
        return

    image_files.sort() # Ensure correct order

    # --- 2. Set up Scene and VSE ---
    # Switch to a temporary scene to avoid altering the user's default scene
    if not scene.sequence_editor:
        scene.sequence_editor_create()

    # Clear any existing strips
    scene.sequence_editor.sequences.clear()

    # Set scene properties to match the video output
    scene.render.resolution_x = 1920 # Default, will be updated by first image
    scene.render.resolution_y = 1080
    scene.render.fps = fps
    scene.frame_start = 1
    scene.frame_end = len(image_files)

    # --- 3. Add Image Strip to VSE ---
    # Create a dictionary for the file list
    file_list = [{'name': f} for f in image_files]

    # Add the image sequence strip
    strip = scene.sequence_editor.sequences.new_image(
        name="ImageSequence",
        filepath=os.path.join(image_folder, image_files[0]),
        channel=1,
        frame_start=1
    )
    # Add the rest of the images to the strip
    for i, img_file in enumerate(image_files[1:]):
        strip.elements.append(img_file)

    # Update strip and scene length
    strip.frame_final_duration = len(image_files)
    scene.frame_end = strip.frame_final_duration

    # Update resolution based on the first image
    if strip.elements:
        first_image = bpy.data.images.load(os.path.join(image_folder, strip.elements[0].filename))
        scene.render.resolution_x = first_image.size[0]
        scene.render.resolution_y = first_image.size[1]
        scene.render.resolution_percentage = 100
        bpy.data.images.remove(first_image)

    # --- 4. Configure Output Settings ---
    scene.render.image_settings.file_format = 'FFMPEG'
    scene.render.ffmpeg.format = 'MPEG4'
    scene.render.ffmpeg.codec = 'H264'
    scene.render.ffmpeg.constant_rate_factor = 'MEDIUM' # Or 'PERC_LOSSLESS', 'HIGH'
    scene.render.filepath = output_path

    # --- 5. Render the Animation ---
    print(f"Rendering {len(image_files)} frames to {output_path} at {fps} FPS...")
    bpy.ops.render.render(animation=True)
    print("Rendering complete!")

# --- EXAMPLE USAGE ---
# This part will only run when the script is executed directly.
if __name__ == "__main__":
    # ⚠️ REPLACE THESE PATHS BEFORE RUNNING!
    image_folder = "/path/to/your/image/sequence"  # <-- REPLACE THIS
    output_path = "/path/to/output/video.mp4"      # <-- REPLACE THIS

    images_to_video(image_folder, output_path, fps=30)
```

### Step 2: Prepare Your Image Sequence

The script requires a folder of sequentially named images. If you do not have this sequence, choose one of the options below.

#### Option A: Locate the Image Sequence (Recommended)

If you are working on a collaborative project or following a tutorial, the image sequence might simply be stored in a different location.

*   **Check Parent/Sibling Directories:** Look in the main `Assets/Art/` directory or a sibling directory to `Textures/` (e.g., `Assets/Art/Sequence/`). Sequences often have their own folder to avoid clutter.
*   **Search for File Names:** Search the entire project folder for files matching a sequential naming convention, such as:
    *   `frame_0001.png`
    *   `render_0001.jpg`
    *   `output0001.exr`
*   **Consult Documentation/Collaborators:** If this is part of a larger workflow, check the project's documentation, git repository history, or ask the person/team responsible for the assets.

#### Option B: Create Placeholder Images

If you cannot find the sequence, you can generate temporary placeholder images to test the script's functionality. This requires the `Pillow` library (`pip install Pillow`). Run this standard Python script (outside of Blender) to create the files.

**Placeholder Image Generation Script:**
```python
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
```

### Step 3: Configure and Run the Script

Once you have the script and the image sequence, follow these steps.

#### 1. Replace Placeholder Paths 📁

Before running the Blender script, you **must** update the example paths at the bottom:

```python
# Example usage:
image_folder = "/path/to/your/image/sequence"  # <-- REPLACE with the folder containing your images
output_path = "/path/to/output/video.mp4"      # <-- REPLACE with the desired output location and filename
images_to_video(image_folder, output_path, fps=30)
```
*   **`image_folder`**: Must be the absolute path (e.g., `C:/Users/User/Desktop/frames/` or `/home/user/frames/`).
*   **`output_path`**: Must be the absolute path for the resulting video (e.g., `C:/Users/User/Desktop/final_video.mp4`).

#### 2. Execute the Script in Blender 🚀

This script **cannot** be run with a standard Python interpreter. It must be executed by Blender.

*   **Option A: Blender's Text Editor (Easiest)**
    1.  Open **Blender**.
    2.  Switch a window to the **Text Editor** workspace.
    3.  Click **+ New** and paste the entire Blender script from Step 1.
    4.  Update the placeholder paths as described above.
    5.  Click the **Run Script** button (the triangle 'play' icon).

*   **Option B: Command Line (Best for Automation)**
    This runs Blender without a graphical interface (headless mode). Open your terminal or command prompt and use the following command structure:
    ```bash
    blender -b -P /path/to/your/script_name.py
    ```
    *   **`blender`**: The command to launch Blender (must be in your system's PATH).
    *   **`-b`**: Runs Blender in the background (headless).
    *   **`-P`**: Specifies the Python script to execute.

#### 3. Review Configuration (Optional) ⚙️

You can adjust the video settings within the `images_to_video` function if the defaults are not suitable.

| Line | Adjustment | Purpose |
| :--- | :--- | :--- |
| `images_to_video(..., fps=30)` | Change `fps=30` to `24` or `60` | Alters the video speed (Frames Per Second). |
| `scene.render.ffmpeg.format = 'MPEG4'` | Change to `'QUICKTIME'`, `'AVI_JPEG'` | Changes the video **container** type. |
| `scene.render.ffmpeg.codec = 'H264'` | Change to `'MPEG4'`, `'NONE'`, `'VP9'` | Changes the video **encoding codec** (affects quality and file size). |