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
        matrix4d xformOp:transform = ((1,0,0,0),(0,1,0,0),(0,0,1,0),(0,0,0,1))
    }

    def Joint "arm"
    {
        matrix4d xformOp:transform = ((1,0,0,0),(0,1,0,0),(0,0,1,0),(0,0,0,1))
    }
}
```
