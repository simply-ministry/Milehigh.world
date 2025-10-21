"""A Blender script to render a sequence of images into a video file.

This script is designed to be run in Blender's headless (background) mode.
It manually sets up the necessary context to use Blender's Video Sequence
Editor (VSE) to import an image sequence and render it as an H.264 MP4 video.
"""

import bpy
import os

def images_to_video(image_folder, output_path, fps=30):
    """
    Renders a sequence of images into a video file using Blender's VSE.

    Args:
        image_folder (str): The absolute path to the folder containing the image sequence.
        output_path (str): The absolute path where the final video will be saved.
        fps (int): The frame rate of the output video.
    """
    # In headless mode, we need to manually find/create a VSE area
    # to provide the correct context for the operator.
    screen = bpy.data.screens.get("Layout")
    if screen is None:
        screen = bpy.data.screens[0]

    area = None
    for a in screen.areas:
        if a.type == 'VIEW_3D': # Find a suitable area to override
            area = a
            break
    if area is None:
        area = screen.areas[0]

    original_type = area.type
    area.type = 'SEQUENCE_EDITOR'

    # Create a sequence editor if one doesn't exist
    if not bpy.context.scene.sequence_editor:
        bpy.context.scene.sequence_editor_create()

    # Get a list of all image files in the folder
    images = sorted([img for img in os.listdir(image_folder) if img.endswith((".png", ".jpg", ".jpeg"))])

    if not images:
        print(f"No images found in the specified folder: {image_folder}")
        area.type = original_type # Restore area type
        return

    # Create an override context for the operator
    override_context = {
        'area': area,
        'region': area.regions[0],
        'screen': screen,
        'scene': bpy.context.scene,
        'window': bpy.context.window
    }

    # Use temp_override to safely set context for the operator
    with bpy.context.temp_override(**override_context):
        bpy.ops.sequencer.image_strip_add(
            directory=image_folder,
            files=[{"name": img} for img in images],
            frame_start=1,
            frame_end=len(images),
            channel=1
        )

    # Restore the original area type
    area.type = original_type

    # Set the scene's frame range to match the image sequence
    bpy.context.scene.frame_start = 1
    bpy.context.scene.frame_end = len(images)

    # Configure render settings
    render = bpy.context.scene.render
    render.image_settings.file_format = 'FFMPEG'
    render.ffmpeg.format = 'MPEG4'
    render.ffmpeg.codec = 'H264'
    render.resolution_x = 1920
    render.resolution_y = 1080
    render.resolution_percentage = 100
    render.fps = fps
    render.filepath = output_path

    # Render the animation
    bpy.ops.render.render(animation=True)

    print(f"Video created at: {output_path}")

# Example usage:
image_folder = os.path.abspath("blender_scripts/temp_frames/")
output_path = os.path.abspath("blender_scripts/output/video.mp4")

# Create output directory if it doesn't exist
os.makedirs(os.path.dirname(output_path), exist_ok=True)

images_to_video(image_folder, output_path, fps=30)
