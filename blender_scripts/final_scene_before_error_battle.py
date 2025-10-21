"""A Blender script to generate the animation for the final scene before the battle.

This script uses the Blender Python API (bpy) to create a short animated
sequence, setting up the positions, rotations, and subtle movements of the
characters Skye and Error, as well as animating the camera and lighting.
"""

import bpy

def final_scene_before_error_battle(skye_object_name="Skye", error_object_name="Error", start_frame=1, end_frame=100):
    """
    Generates a Blender animation script for the final scene before Skye battles Error.

    Args:
        skye_object_name (str): The name of Skye's character object in Blender.
        error_object_name (str): The name of Error's character object in Blender.
        start_frame (int): The starting frame number for this scene.
        end_frame (int): The ending frame number for this scene.
    """

    # Ensure the scene is set to the start frame
    bpy.context.scene.frame_set(start_frame)
    bpy.context.scene.frame_start = start_frame
    bpy.context.scene.frame_end = end_frame

    # --- Skye's Animation ---
    skye = bpy.data.objects.get(skye_object_name)
    if skye:
        # Example: Set Skye to a determined pose
        skye.location = (0, -2, 1)  # Adjust position as needed
        skye.rotation_euler = (0, 0, 0) # Adjust rotation as needed
        skye.keyframe_insert(data_path="location", frame=start_frame)
        skye.keyframe_insert(data_path="rotation_euler", frame=start_frame)

        # Example: Add a subtle breathing animation (assuming a rigged character)
        if skye.type == 'ARMATURE':
            for frame in range(start_frame, end_frame + 1, 10):
                bpy.context.scene.frame_set(frame)
                # Adjust a chest bone's scale slightly
                bone = skye.pose.bones.get("chest") # Replace "chest" with your bone name
                if bone:
                    bone.scale = (1.01, 1.01, 1.01)
                    bone.keyframe_insert(data_path="scale", frame=frame)
                else:
                    print(f"Warning: Bone 'chest' not found in {skye_object_name}")

            for frame in range(start_frame + 5, end_frame + 1, 10):
                bpy.context.scene.frame_set(frame)
                bone = skye.pose.bones.get("chest") # Replace "chest" with your bone name
                if bone:
                    bone.scale = (1.0, 1.0, 1.0)
                    bone.keyframe_insert(data_path="scale", frame=frame)
                else:
                    print(f"Warning: Bone 'chest' not found in {skye_object_name}")
        else:
            print(f"Warning: {skye_object_name} is not an armature, cannot animate bones.")

        # Example: Animate Skye's gaze towards where Error might appear
        # Assuming you have a head bone or can rotate the object
        for frame in range(start_frame, end_frame + 1, 20):
            bpy.context.scene.frame_set(frame)
            if skye.type == 'ARMATURE':
                head_bone = skye.pose.bones.get("head") # Replace "head" with your bone name
                if head_bone:
                    head_bone.rotation_euler = (0.2, 0, 0.1) # Adjust rotation values
                    head_bone.keyframe_insert(data_path="rotation_euler", frame=frame)
                else:
                    print(f"Warning: Bone 'head' not found in {skye_object_name}")
            else:
                skye.rotation_euler = (0, 0.1, 0) # Adjust rotation values
                skye.keyframe_insert(data_path="rotation_euler", frame=frame)

        # Reset gaze at the end
        bpy.context.scene.frame_set(end_frame)
        if skye.type == 'ARMATURE':
            head_bone = skye.pose.bones.get("head") # Replace "head" with your bone name
            if head_bone:
                head_bone.rotation_euler = (0, 0, 0)
                head_bone.keyframe_insert(data_path="rotation_euler", frame=end_frame)
        else:
            skye.rotation_euler = (0, 0, 0)
            skye.keyframe_insert(data_path="rotation_euler", frame=end_frame)

    else:
        print(f"Error: Object '{skye_object_name}' not found in the scene.")

    # --- Error's Presence (Subtle Cue) ---
    error = bpy.data.objects.get(error_object_name)
    if error:
        # Example: Make Error subtly appear or a shadow grow
        error.location = (5, -2, 1) # Adjust position as needed
        error.scale = (0.1, 0.1, 0.1)
        error.keyframe_insert(data_path="location", frame=end_frame - 20)
        error.keyframe_insert(data_path="scale", frame=end_frame - 20)

        bpy.context.scene.frame_set(end_frame)
        error.scale = (1, 1, 1)
        error.keyframe_insert(data_path="scale", frame=end_frame)
        error.keyframe_insert(data_path="location", frame=end_frame)
        error.hide_render = False # Ensure Error is visible at the end
        error.keyframe_insert(data_path="hide_render", frame=end_frame)
    else:
        print(f"Warning: Object '{error_object_name}' not found in the scene.")

    # --- Camera Animation ---
    camera = bpy.data.objects.get("Camera") # Assuming your camera object is named "Camera"
    if camera:
        # Example: Slow zoom in on Skye
        camera.location = (5, -5, 3) # Initial camera position
        camera.keyframe_insert(data_path="location", frame=start_frame)

        bpy.context.scene.frame_set(end_frame)
        camera.location = (1, -3, 2) # Closer camera position
        camera.keyframe_insert(data_path="location", frame=end_frame)
    else:
        print("Warning: Camera object 'Camera' not found in the scene.")

    # --- Lighting ---
    # Example: Intensify the lighting slightly towards the end
    for light in bpy.data.lights:
        if light.type == 'SUN': # Or 'POINT', 'SPOT', etc.
            light.energy = 2.0
            light.keyframe_insert(data_path="energy", frame=start_frame)
            bpy.context.scene.frame_set(end_frame)
            light.energy = 2.5
            light.keyframe_insert(data_path="energy", frame=end_frame)

    print("Final scene animation script generated.")

if __name__ == "__main__":
    # Replace with your actual object names and frame range
    final_scene_before_error_battle(skye_object_name="Skye_Character", error_object_name="Error_Entity", start_frame=150, end_frame=250)
