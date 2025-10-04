# Code & Asset Organization Document

This document contains organized code snippets related to 3D character assets and rendering shaders for Milehigh.World.

---

## 1. USD (Universal Scene Description) Asset Definitions

These sections define the 3D assets for a character using the USD format. This includes the skeleton for animation, the character's body mesh, and the face mesh with expressions.

### A. Skeleton Definition

This defines the character's animation skeleton, which is a hierarchy of joints.

```usd
def Skeleton "characterSkeleton"
{
    def Joint "root"
    {
        matrix4d xformOp:transform = [...]
        def Joint "spine_01"
        {
            matrix4d xformOp:transform = [...]
            def Joint "spine_02"
            {
                matrix4d xformOp:transform = [...]
                def Joint "neck_01"
                {
                    matrix4d xformOp:transform = [...]
                    def Joint "head"
                    {
                        matrix4d xformOp:transform = [...]
                    }
                }
                def Joint "l_clavicle"
                {
                    matrix4d xformOp:transform = [...]
                    def Joint "l_upperarm"
                    {
                        matrix4d xformOp:transform = [...]
                        def Joint "l_lowerarm"
                        {
                            matrix4d xformOp:transform = [...]
                            def Joint "l_hand"
                            {
                                matrix4d xformOp:transform = [...]
                            }
                        }
                    }
                }
                def Joint "r_clavicle"
                {
                    matrix4d xformOp:transform = [...]
                    def Joint "r_upperarm"
                    {
                        matrix4d xformOp:transform = [...]
                        def Joint "r_lowerarm"
                        {
                            matrix4d xformOp:transform = [...]
                            def Joint "r_hand"
                            {
                                matrix4d xformOp:transform = [...]
                            }
                        }
                    }
                }
            }
        }
        def Joint "l_thigh"
        {
            matrix4d xformOp:transform = [...]
            def Joint "l_calf"
            {
                matrix4d xformOp:transform = [...]
                def Joint "l_foot"
                {
                    matrix4d xformOp:transform = [...]
                }
            }
        }
        def Joint "r_thigh"
        {
            matrix4d xformOp:transform = [...]
            def Joint "r_calf"
            {
                matrix4d xformOp:transform = [...]
                def Joint "r_foot"
                {
                    matrix4d xformOp:transform = [...]
                }
            }
        }
    }
}

### B. Body Mesh Definition

This defines the character's main body geometry.

```usd
def Mesh "characterBody"
{
    int[] faceVertexCounts = [...]
    int[] faceVertexIndices = [...]
    point3f[] points = [...]
    normal3f[] normals = [...]
    texCoord2f[] uv = [...]
}
```

### C. Face Mesh and BlendShapes Definition

This defines the character's face geometry and includes blend shapes for facial expressions.

```usd
def Mesh "characterFace"
{
    int[] faceVertexCounts = [...]
    int[] faceVertexIndices = [...]
    point3f[] points = [...]
    normal3f[] normals = [...]
    texCoord2f[] uv = [...]

    def BlendShape "smile"
    {
        point3f[] offsets = [...]
    }

    def BlendShape "frown"
    {
        point3f[] offsets = [...]
    }

    def BlendShape "blink_left"
    {
        point3f[] offsets = [...]
    }

    def BlendShape "blink_right"
    {
        point3f[] offsets = [...]
    }
}
```

### D. Mascot Mesh Definition

This defines the Mascot's main body geometry.

```usd
def Mesh "mascotBody"
{
    int[] faceVertexCounts = [...]
    int[] faceVertexIndices = [...]
    point3f[] points = [...]
    normal3f[] normals = [...]
    texCoord2f[] uv = [...]
}
```