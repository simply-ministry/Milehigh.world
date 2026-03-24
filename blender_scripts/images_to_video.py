"""A Blender script to convert a sequence of images into a video file.

This script uses Blender's Video Sequence Editor (VSE) to import an
image sequence from a specified folder, configure the output settings,
and render it as an H.264 MP4 video.
"""

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
