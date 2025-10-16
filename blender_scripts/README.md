# Blender Scripts

This directory contains utility scripts for use within Blender.

***

## `images_to_video.py`

This Python script leverages Blender's Python API (`bpy`) to automate the process of converting a sequence of still images (like `.png` or `.jpg` files) into a single video file (like `.mp4`). It uses Blender's **Video Sequence Editor (VSE)** as the engine for this task.

Here is a step-by-step breakdown:

### 1. Setup and Image Retrieval
*   **Imports:** The script imports `bpy` for Blender operations and `os` for interacting with the operating system (specifically for listing files in a directory).
*   **`images_to_video` Function:** This is the main function that encapsulates the entire process.
*   **Get Image Files:**
    ```python
    image_files = sorted([f for f in os.listdir(image_folder) if f.lower().endswith(('.png', '.jpg', '.jpeg', '.exr'))])
    ```
    - `os.listdir(image_folder)`: Gets a list of all files and folders in the specified `image_folder`.
    - `... if f.lower().endswith(...)`: Filters this list, keeping only files with common image extensions. `.lower()` makes the check case-insensitive.
    - `sorted([...])`: **Crucially**, this sorts the filenames alphabetically. This ensures that images named `frame_001.png`, `frame_002.png`, etc., are placed in the correct order in the video.
*   **Error Handling:**
    ```python
    if not image_files:
        print(f"Error: No images found in '{image_folder}'")
        return
    ```
    If no valid images are found in the folder, the script prints an error and stops.

### 2. Video Sequence Editor (VSE) Setup
*   **Initialize the VSE:**
    ```python
    bpy.context.scene.sequence_editor_create()
    ```
    This command ensures that a Video Sequence Editor workspace exists in the current Blender scene. If one doesn't exist, it creates it. This is a necessary first step before you can add any media strips.
*   **Get Reference to Sequences:**
    ```python
    seq = bpy.context.scene.sequence_editor.sequences
    ```
    This line gets a direct reference to the collection of all media strips (video, audio, images, etc.) in the VSE timeline.

### 3. Adding the Image Strip
*   **Create the Image Strip:**
    ```python
    seq.new_image("MySequence", directory=image_folder, files=image_files, channel=1, frame_start=1)
    ```
    This is the core command that adds the images to the VSE timeline. It creates a single "Image Strip" from the list of files.
    - `"MySequence"`: A name for the new strip.
    - `directory=image_folder`: The folder where the images are located.
    - `files=image_files`: The sorted list of image filenames to include.
    - `channel=1`: Places the strip on the first track (channel) of the VSE timeline.
    - `frame_start=1`: Starts the strip at the very first frame of the timeline.
*   **Set Scene Duration:**
    ```python
    bpy.context.scene.frame_end = len(image_files)
    ```
    This sets the total length of the scene's animation to match the number of images. If there are 150 images, the scene will be 150 frames long, ensuring every image is included in the final video.

### 4. Render Configuration (Output Settings)
This section configures Blender's render engine to output a video file instead of individual images.

*   **Output Path:**
    ```python
    bpy.context.scene.render.filepath = output_path
    ```
    Sets the full path and filename for the final video (e.g., `/path/to/my_video.mp4`).

*   **File Format:**
    ```python
    bpy.context.scene.render.image_settings.file_format = 'FFMPEG'
    ```
    This tells Blender to use its built-in FFmpeg library to render a video format, not a still image format like `PNG` or `JPEG`.

*   **Video Container:**
    ```python
    bpy.context.scene.render.ffmpeg.format = 'MPEG4'
    ```
    This specifies the video container. `MPEG4` corresponds to the `.mp4` container, which is widely supported. Other options include `'QUICKTIME'` for `.mov` or `'AVI'` for `.avi`.

*   **Video Codec:**
    ```python
    bpy.context.scene.render.ffmpeg.codec = 'h264'
    ```
    This sets the video encoding algorithm (codec). `H.264` is the most common and efficient codec for `.mp4` files, offering a good balance of quality and file size.

*   **Framerate (FPS):**
    ```python
    bpy.context.scene.render.fps = fps
    ```
    Sets the video's frames per second (e.g., 24, 30, 60). This determines the playback speed.

### 5. Rendering
*   **Execute Render:**
    ```python
    bpy.ops.render.render(animation=True)
    ```
    This command starts the rendering process. The key argument is `animation=True`, which tells Blender to render the entire frame range (from `frame_start` to `frame_end`), combining all frames into the single video file specified in the output settings.
*   **Confirmation:**
    ```python
    print(f"Video created at: {output_path}")
    ```
    A simple message to confirm that the script has finished its job.

***

### How to Use the Code

This is a `bpy` script, so it **must be run from within Blender**. You cannot run it like a standard Python script (e.g., `python my_script.py`).

1.  **Modify the Paths:**
    Change these two lines at the bottom of the script to point to your actual folders:
    ```python
    # Replace with the path to the folder containing your image sequence
    image_folder = "/path/to/your/image/sequence"
    # Replace with the desired output path, including the filename and extension
    output_path = "/path/to/output/video.mp4"
    ```
    **Note:** Use absolute paths for best results (e.g., `C:/Users/YourUser/Desktop/images` on Windows or `/home/youruser/images` on Linux).

2.  **Run in Blender:**
    *   Open Blender.
    *   Go to the **Scripting** workspace tab.
    *   Click **New** to create a new text block.
    *   Copy and paste the entire modified script into the Text Editor.
    *   Click the **Run Script** button (a "play" icon) in the Text Editor's header.

Blender's render window will appear and process each image as a frame. Once complete, the final video will be saved at your specified `output_path`.