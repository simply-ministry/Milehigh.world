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
    }

    def Joint "arm"
    {
        matrix4d xformOp:transform = [...]
    }
}
```

### B. Body Mesh Definition

This defines the character's body mesh.

```usd
def Mesh "bodyMesh"
{
    int[] faceVertexCounts = [4]
    int[] faceVertexIndices = [0, 1, 2, 3]
    point3f[] points = [(-1, -1, 0), (1, -1, 0), (1, 1, 0), (-1, 1, 0)]
}
```

### C. Face Mesh Definition with BlendShapes

This defines the character's face mesh, including BlendShapes for expressions.

```usd
def Mesh "faceMesh"
{
    int[] faceVertexCounts = [4]
    int[] faceVertexIndices = [0, 1, 2, 3]
    point3f[] points = [(-0.5, -0.5, 0), (0.5, -0.5, 0), (0.5, 0.5, 0), (-0.5, 0.5, 0)]

    def BlendShape "smile"
    {
        point3f[] points = [(-0.5, -0.5, 0), (0.5, -0.5, 0), (0.5, 0.6, 0), (-0.5, 0.6, 0)]
    }

    def BlendShape "frown"
    {
        point3f[] points = [(-0.5, -0.5, 0), (0.5, -0.5, 0), (0.5, 0.4, 0), (-0.5, 0.4, 0)]
    }
}
```
}
