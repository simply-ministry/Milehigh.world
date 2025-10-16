import bpy
import os

def images_to_video(image_folder, output_path, fps=24):
    """Converts an image sequence to a video file using Blender's VSE."""

    # 1. Get a list of image files
    image_files = sorted([f for f in os.listdir(image_folder) if f.lower().endswith(('.png', '.jpg', '.jpeg', '.exr'))])
    if not image_files:
        print(f"Error: No images found in '{image_folder}'")
        return

    # 2. Set up the scene for the VSE
    bpy.context.scene.sequence_editor_create()
    seq = bpy.context.scene.sequence_editor.sequences

    # 3. Add the image sequence as a strip
    seq.new_image("MySequence", directory=image_folder, files=image_files, channel=1, frame_start=1)
    bpy.context.scene.frame_end = len(image_files)  # Set the end frame

    # 4. Configure render settings for video output
    bpy.context.scene.render.filepath = output_path
    bpy.context.scene.render.image_settings.file_format = 'FFMPEG'
    bpy.context.scene.render.ffmpeg.format = 'MPEG4'  # Or 'QUICKTIME', etc.
    bpy.context.scene.render.ffmpeg.codec = 'h264'
    bpy.context.scene.render.fps = fps

    # 5. Render the video
    bpy.ops.render.render(animation=True)
    print(f"Video created at: {output_path}")

# Example usage:
image_folder = "/path/to/your/image/sequence"  # Replace with the actual path
output_path = "/path/to/output/video.mp4"      # Replace with the desired output path
images_to_video(image_folder, output_path, fps=30)