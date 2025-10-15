Technical Analysis of Underwater Physics and Interaction Implementation in Unity and Python
1. Introduction
Purpose
This document provides a detailed technical analysis and comparison of methods for implementing underwater physics and interactions within game development contexts. It examines approaches utilizing a pre-built game engine, specifically Unity with its C# scripting interface, and contrasts them with custom physics simulations coded in Python. The analysis is grounded in specific code examples and relevant documentation concerning physics principles and engine features.
Challenges
Simulating underwater environments presents unique challenges for game developers. The distinct physical properties of water, such as its density and viscosity, necessitate specialized considerations beyond typical terrestrial physics. Key phenomena like buoyancy and fluid drag must be modeled to create a believable experience. Developers face the task of balancing physical realism with computational performance, gameplay responsiveness, and the effective integration of these physics systems with complementary visual and audio cues to achieve immersion.
Scope
The scope of this analysis encompasses the provided C# scripts (UnderwaterThrust.cs, UnderwaterMovement.cs) designed for the Unity engine and the Python GameObject class snippets demonstrating custom physics logic. It delves into the implementation details of underwater thrust attacks, buoyancy, drag, and player-controlled movement within these examples. The comparison focuses on the underlying physics principles, the trade-offs between using a physics engine versus custom code, and the inherent limitations of the presented simulation models. Furthermore, it explores common techniques used to enhance the sensory experience of underwater environments, drawing upon provided documentation and examples.1
Structure Overview
The analysis begins by dissecting the Unity C# examples, examining their use of engine features like Rigidbody, Physics.Raycast, and AddForce. Subsequently, it analyzes the Python GameObject class, focusing on its custom attributes and physics calculations within the move method variations. A comparative analysis of the different Python implementations follows, highlighting variations in accuracy and approach. The underlying physics principles of buoyancy and drag are explained in detail. The report then contrasts the engine-based versus custom physics implementation strategies, discussing their respective advantages and disadvantages. An evaluation of the physical accuracy and limitations of the presented models is provided. A synthesized approach to implementing basic underwater mechanics is outlined, followed by a discussion of complementary visual and audio techniques for enhancing immersion. Finally, the document concludes with a summary of the key findings.
2. Analysis of Unity C# Underwater Mechanics
Overview
The provided Unity C# scripts, UnderwaterThrust and UnderwaterMovement, exemplify an approach that heavily leverages the built-in capabilities of the Unity engine and its integrated physics system (NVIDIA PhysX). This strategy relies on engine components, primarily the Rigidbody component, to manage the physical state and behavior of GameObjects, thereby abstracting many of the complex low-level calculations involved in physics simulation.1
Dissection of UnderwaterThrust.cs
Core Functionality
The UnderwaterThrust.cs script is designed to implement a short-range attack mechanism. When triggered by player input, it simulates a thrust originating from a specified point (attackSource) on the GameObject, projecting forward to potentially hit and affect other objects within a defined range.
Input Handling
The attack is triggered by the line if (Input.GetButtonDown("Fire1")). This utilizes Unity's legacy Input Manager system to detect the initial press of the button or key mapped to the "Fire1" virtual axis.7 The Input Manager allows developers to define named inputs and map them to various hardware controls (keyboard, mouse, joystick).7 It's worth noting that while functional, Unity now recommends using the newer Input System package for new projects due to its enhanced flexibility and capabilities.7 Input.GetButtonDown specifically returns true only during the frame the button is first pressed, suitable for triggering single actions like this attack.
Raycasting (Physics.Raycast)
To determine if the thrust hits an object, the script employs Physics.Raycast. The core logic resides in:
if (Physics.Raycast(attackSource.position, attackSource.forward, out hit, thrustRange))
This function casts an invisible ray from the attackSource's world position (attackSource.position) in the direction the attackSource is facing (attackSource.forward). The ray checks for intersections with any colliders in the scene up to a maximum distance defined by thrustRange.9
The out hit parameter is crucial; if the raycast successfully intersects a collider, this parameter is populated with a RaycastHit object. This object contains detailed information about the intersection point, the distance, and, most importantly, a reference to the Collider component of the object that was hit (hit.collider).9 Accessing hit.collider allows the script to identify the hit object (e.g., hit.collider.name for logging) and attempt to interact with its other components, such as a potential Health script or its Rigidbody.
This method provides a computationally efficient way to perform line-of-sight checks or simulate instantaneous projectile impacts without needing to instantiate and simulate a separate projectile GameObject. The parameters allow for precise control over the origin, direction, range, and potentially filtering which objects can be hit using layer masks (layerMask) or whether triggers should be detected (queryTriggerInteraction), although these optional parameters are not used in this specific snippet.9
Force Application (Rigidbody.AddForce)
Upon detecting a hit, the script attempts to apply a physical force to the struck object:
if (hit.rigidbody!= null) { hit.rigidbody.AddForce(attackSource.forward * thrustForce, ForceMode.Impulse); }
This code first checks if the hit collider (hit.collider) is attached to a GameObject that also has a Rigidbody component (hit.rigidbody). Physics forces in Unity can only be directly applied to non-kinematic Rigidbodies.1 If a Rigidbody exists, the AddForce method is called.
The force vector is calculated as attackSource.forward * thrustForce, meaning the force is applied in the same direction as the thrust originated, scaled by the public variable thrustForce.
The second argument, ForceMode.Impulse, dictates how the force is applied. ForceMode.Impulse treats the force vector parameter as an impulse (measured in Newton-seconds). It applies an instantaneous change in momentum to the Rigidbody, resulting in a velocity change equal to the impulse divided by the Rigidbody's mass (force / mass).13 This mode is independent of the physics timestep (Time.fixedDeltaTime) and is particularly well-suited for simulating sudden impacts, explosions, or instantaneous bursts of force, like the intended thrust attack.13
Contrasting this with other modes clarifies the design choice:
ForceMode.Force (the default): Applies a continuous force (Newtons) over the next physics timestep. The resulting velocity change depends on mass and deltaTime (force * deltaTime / mass). Suitable for sustained forces like thrusters or gravity.13
ForceMode.Acceleration: Applies continuous acceleration (m/s^2), ignoring mass. Velocity change depends on deltaTime (force * deltaTime). Useful for effects that should accelerate all objects equally regardless of mass.13
ForceMode.VelocityChange: Applies an instantaneous velocity change (m/s), ignoring mass (force). Useful for directly setting velocity in specific scenarios, like character jumps where mass influence might be undesired.13
The selection of ForceMode.Impulse indicates a deliberate choice to make the thrust feel like a sharp, sudden impact whose effect on the target's velocity is appropriately scaled by the target's mass, providing a physically intuitive reaction consistent across different frame rates.
Dissection of UnderwaterMovement.cs
Core Functionality
The UnderwaterMovement.cs script aims to simulate fundamental aspects of underwater physics – specifically buoyancy and fluid drag – for a GameObject. It also incorporates basic player-controlled vertical movement (thrust). Crucially, this script requires the GameObject to possess a Rigidbody component, as it directly interacts with the physics engine through this component.1
Rigidbody Requirement
The script accesses and modifies the Rigidbody via GetComponent<Rigidbody>() and subsequent calls to rb.AddForce and rb.velocity. Without a Rigidbody, these calls would fail, and the script would not produce any physical behavior.1 The Rigidbody component is the gateway for applying forces and allowing the physics engine to control the GameObject's motion.5
Initialization (Start)
In the Start method, the script obtains a reference to the Rigidbody component attached to the same GameObject and stores it in the rb variable for later use. It also caches the scene's gravity value (Physics.gravity.y).
Physics Update (FixedUpdate)
All physics calculations and AddForce calls are placed within the FixedUpdate method. This is standard practice in Unity because FixedUpdate is called at a fixed time interval (Time.fixedDeltaTime), synchronized with the physics engine's simulation cycle. Performing physics operations here ensures consistent and stable behavior, independent of fluctuations in the rendering frame rate (which governs the Update method).9
Buoyancy Calculation & Application
Buoyancy is calculated using the formula:
float buoyantForce = waterDensity * objectVolume * -gravity;
This directly implements Archimedes' Principle, which states that the buoyant force on a submerged object is equal to the weight of the fluid it displaces.16 The weight of the displaced fluid is given by mfluid​×g=(ρfluid​×Vdisplaced​)×g. In the code, waterDensity corresponds to ρfluid​, objectVolume is assumed to be the volume of displaced fluid (Vdisplaced​), and -gravity provides the magnitude of the gravitational acceleration (g, negated because Unity's gravity.y is typically negative).
The calculated force is then applied upwards using:
rb.AddForce(Vector3.up * buoyantForce);
By default, AddForce uses ForceMode.Force.13 This applies the buoyant force continuously over the physics timestep, appropriately modeling buoyancy as a persistent environmental force acting on the submerged object. The effect is dependent on the object's mass and the timestep duration.
Drag Calculation & Application
Fluid drag is calculated using the quadratic drag model:
float dragMagnitude = 0.5f * waterDensity * relativeVelocity.sqrMagnitude * dragCoefficient * frontalArea;
This corresponds to the standard formula for drag force at higher Reynolds numbers: FD​=21​ρv2Cd​A.20
0.5f is the constant factor.
waterDensity is the fluid density (ρ).
relativeVelocity.sqrMagnitude is the square of the object's velocity relative to the fluid (v2). The script calculates relativeVelocity = rb.velocity - new Vector3(0, waterVelocity, 0);, assuming static water (waterVelocity = 0).
dragCoefficient is the dimensionless drag coefficient (Cd​).
frontalArea is the cross-sectional area facing the flow (A).
The drag force vector, which opposes the direction of motion, is calculated and applied:
Vector3 dragForce = -relativeVelocity.normalized * dragMagnitude;
rb.AddForce(dragForce);
Similar to buoyancy, drag is applied using the default ForceMode.Force, treating it as a continuous resistive force that depends on the object's current velocity.13 The use of ForceMode.Force for both buoyancy and drag contrasts with the ForceMode.Impulse used for the thrust attack, reflecting the different natures of these forces – continuous environmental effects versus an instantaneous impact. Accurate simulation requires careful balancing of these forces against the object's mass and the physics timestep (Time.fixedDeltaTime).
Player Input (Input.GetAxis) & Thrust Application
Player-controlled vertical thrust is handled via:
float verticalInput = Input.GetAxis("Vertical");
Vector3 thrust = transform.up * verticalInput * 10f;
rb.AddForce(thrust);
Input.GetAxis("Vertical") retrieves a smoothed input value ranging from -1 to 1, based on the configuration in the Input Manager (typically mapped to W/S keys, up/down arrows, or a joystick axis).7 The smoothing provides gradual transitions, suitable for analog-like control.7
The thrust vector is calculated along the object's local upward direction (transform.up), scaled by the verticalInput and an arbitrary multiplier (10f). This thrust is then applied using AddForce with the default ForceMode.Force.13
Applying player-controlled thrust as a continuous force using ForceMode.Force means the input translates into an acceleration that is influenced by the object's mass and counteracted by drag and buoyancy/gravity. This results in movement with inertia, where the object gradually speeds up or slows down based on sustained input, rather than instantly changing velocity. This approach often yields a more physically plausible feel for vehicle or character movement compared to directly manipulating the Rigidbody's velocity.
3. Analysis of Python GameObject Underwater Physics
Overview
The Python GameObject class represents a contrasting approach where physics simulation is implemented manually within the class structure, without reliance on an external, dedicated physics engine like Unity's PhysX. This necessitates the explicit definition of relevant physical properties as attributes and the coding of algorithms to calculate and apply forces like buoyancy and drag, as well as integrating motion over time.
Relevant Attributes
To support underwater physics simulation, the GameObject class is augmented with several specific attributes:
underwater: A boolean flag indicating whether the object is currently submerged, used to conditionally apply underwater physics effects.
density: The density of the object itself (e.g., in kg/m3). Used in conjunction with fluid density to calculate buoyancy.16
volume: The volume of the object (e.g., in m3). Used to calculate the buoyant force, which depends on the volume of displaced fluid.16
drag_coefficient: A dimensionless factor (Cd​) representing the object's aerodynamic/hydrodynamic resistance. Used in the drag force calculation.20
frontal_area: The cross-sectional area (A) of the object perpendicular to the direction of motion. Also used in the drag force calculation.20
velocity_x, velocity_y, velocity_z: Components representing the object's current velocity vector. Essential for calculating drag and updating position.
max_underwater_speed: A gameplay constraint limiting the object's maximum speed while submerged.
underwater_agility: Potentially used to scale input forces or acceleration underwater (though not explicitly used in the provided move logic).
mass: The mass of the object (e.g., in kg). Crucial for relating applied forces to acceleration via Newton's second law (F=ma).5
These attributes directly map to the variables required by the standard physics formulas for buoyancy and drag, enabling their calculation within the custom simulation logic.
Custom Physics Logic (move Method - First Example)
The first provided implementation of the move method demonstrates a relatively detailed custom physics simulation step:

Python


def move(self, dx, dy, dz, delta_time):
    if not self.can_move():
        #... reset velocity...
        return

    if self.underwater:
        # Calculate Net Buoyancy/Gravity Force
        buoyant_force = (self.density - WATER_DENSITY) * self.volume * GRAVITY
        # Calculate Quadratic Drag
        drag_force_x = -self.velocity_x * abs(self.velocity_x) * self.drag_coefficient * self.frontal_area * WATER_DENSITY
        drag_force_y = -self.velocity_y * abs(self.velocity_y) * self.drag_coefficient * self.frontal_area * WATER_DENSITY
        drag_force_z = -self.velocity_z * abs(self.velocity_z) * self.drag_coefficient * self.frontal_area # Missing WATER_DENSITY? Assumed typo based on X/Y
        # Calculate Acceleration (F=ma)
        acceleration_x = (drag_force_x / self.mass) + dx
        acceleration_y = (drag_force_y / self.mass) + dy
        # Note: Buoyancy acts vertically (assuming Z is up/down here)
        acceleration_z = (drag_force_z / self.mass) + dz + buoyant_force / self.mass # Buoyancy needs division by mass for acceleration
    else:
        # Apply air physics (simplified/omitted here)
        buoyant_force = 0.0
        drag_force_x = drag_force_y = drag_force_z = 0.0 # Example: No air drag
        acceleration_x = dx
        acceleration_y = dy
        acceleration_z = dz # Assuming gravity is handled elsewhere or dz includes it

    # Integrate Velocity (Euler method)
    self.velocity_x += acceleration_x * delta_time
    self.velocity_y += acceleration_y * delta_time
    self.velocity_z += acceleration_z * delta_time

    if self.underwater:
        # Apply Underwater Constraints/Damping
        # Clamp velocity
        self.velocity_x = max(min(self.velocity_x, self.max_underwater_speed), -self.max_underwater_speed)
        #... (similar for Y and Z)...
        # Apply Damping (Frame-rate independent)
        self.velocity_x *= pow(UNDERWATER_DAMPING, delta_time)
        #... (similar for Y and Z)...
    else:
        # Apply Air Damping (Example)
        self.velocity_x *= pow(AIR_DAMPING, delta_time)
        #... (similar for Y and Z)...

    # Integrate Position (Euler method)
    self.x += self.velocity_x * delta_time
    self.y += self.velocity_y * delta_time
    self.z += self.velocity_z * delta_time


State Check: The method first checks self.can_move(), demonstrating the importance of integrating game state logic with physics updates.
Conditional Physics: The if self.underwater: block ensures that buoyancy and water drag are only applied when the object is submerged.
Buoyancy Calculation: buoyant_force = (self.density - WATER_DENSITY) * self.volume * GRAVITY calculates the net force resulting from the object's weight (implicitly m×g=ρobj​×V×g) and the upward buoyant force (ρfluid​×V×g). This differs from the Unity example, where buoyancy was calculated separately, and gravity was handled implicitly by the Rigidbody (unless useGravity is disabled 1). Correction: For this to be acceleration, buoyant_force needs division by mass.
Drag Calculation: The drag calculation (drag_force_x = -self.velocity_x * abs(self.velocity_x) *...) correctly implements the quadratic drag model (FD​∝v2) opposing velocity on each axis.20 It includes the object's drag_coefficient, frontal_area, and crucially, the WATER_DENSITY (ρfluid​), aligning with the standard formula.20 (Note: The provided snippet for drag_force_z seems to omit WATER_DENSITY, which is likely a typo given its presence in the X and Y calculations).
Force Integration (Euler Integration): The code calculates acceleration (a=Fnet​/m) by summing the calculated drag forces, the net buoyancy/gravity force (applied vertically, assumed Z here), and any external input forces (dx, dy, dz, which are assumed to be accelerations or forces pre-scaled by mass/timestep), and dividing by mass. It then updates velocity using vnew​=vold​+a×Δt. This process is a basic numerical integration technique known as the Forward Euler method.
Velocity Clamping & Damping: After updating velocity, the code clamps it to max_underwater_speed and applies exponential damping using pow(UNDERWATER_DAMPING, delta_time). This form of damping provides a frame-rate independent way to simulate energy loss, helping to stabilize the simulation and prevent uncontrolled velocity increases. This contrasts with Unity's built-in Rigidbody.drag and angularDrag properties, which achieve a similar effect but are managed by the engine's solver.1
Position Update: Finally, the object's position is updated using pnew​=pold​+vnew​×Δt, completing the Euler integration step.
This custom implementation demonstrates the level of detail required when bypassing a physics engine. Every aspect, from checking game state to calculating forces based on physical principles, performing numerical integration, and adding stability measures like clamping and damping, must be handled explicitly in code. This offers granular control but significantly increases complexity and the potential for numerical instability or inaccuracies compared to using a robust, pre-built physics engine solver.
4. Comparative Analysis of Python move Method Implementations
Overview
The provided materials include three distinct versions of the Python GameObject.move method. Comparing these implementations reveals different strategies and trade-offs in simulating underwater movement, particularly concerning physical accuracy, complexity, and stability.
Implementation 1 (Detailed)
As analyzed previously, this version aims for higher fidelity:
Calculates net buoyancy/gravity force based on object and fluid densities, volume, and gravity.16
Implements per-axis quadratic drag including fluid density (ρ), velocity squared (v2), drag coefficient (Cd​), and area (A).20
Uses a standard Euler integration scheme: Forces → Acceleration → Velocity → Position.
Includes velocity clamping and frame-rate independent exponential damping (pow(DAMPING, delta_time)).
Implementation 2 (Simplified Buoyancy/Drag)
This version simplifies several aspects:
Buoyancy: Uses a simplified buoyant_force = self.buoyancy, implying a constant or pre-calculated value, rather than deriving it from density and volume. It applies this directly to velocity (self.velocity_z += buoyant_force * delta_time), which implicitly treats it as an acceleration (assuming mass=1 or the force is pre-scaled).
Drag: Calculates quadratic drag similarly (drag_force_x = -self.velocity_x * abs(self.velocity_x) * self.drag_coefficient * self.frontal_area), but critically omits the fluid density (ρ). This deviates from the standard physical formula FD​=0.5ρv2Cd​A and makes the drag force independent of the fluid medium, which is physically inaccurate.20
Integration: The velocity update self.velocity_x += (acceleration_x + dx) * delta_time appears to mix acceleration derived from drag (acceleration_x = drag_force_x / self.mass) with the input dx, which is added before scaling by delta_time. This suggests dx might be intended as an instantaneous velocity change or an impulse, rather than a continuous force or acceleration, leading to potentially inconsistent behavior depending on how dx is generated.
Damping: Uses a fixed damping factor per frame (self.velocity_x *= 0.90). This method is simpler to implement but results in damping strength being dependent on the frame rate (delta_time); lower frame rates lead to stronger damping per second.
Constraints: Includes a basic check to prevent movement below z=0 (if self.z < 0:).
Implementation 3 (Drag Only)
This is the most simplified version:
Buoyancy: Completely omitted. The object's vertical movement would be solely determined by input dz and drag/damping (and potentially gravity if handled elsewhere).
Drag: Uses the same simplified drag formula as Implementation 2, lacking fluid density (ρ) and thus being physically inaccurate.20
Integration: Applies drag directly to velocity scaled by time (self.velocity_x += drag_force_x * delta_time), again implying acceleration assuming mass=1. The position update self.x += (dx + self.velocity_x) * delta_time adds the input dx directly to the displacement calculation for the frame, alongside the velocity contribution. This treats dx as an intended velocity for the duration of the frame, differing significantly from the force-based acceleration model of Implementation 1.
Damping: Uses a fixed damping factor per frame (self.velocity_x *= 0.95), subject to frame-rate dependency.
Discussion of Differences & Impact
Physical Accuracy: Implementation 1 offers the most physically plausible simulation by incorporating fluid density in drag and calculating buoyancy based on fundamental properties. Implementations 2 and 3 sacrifice accuracy through the omission of fluid density in the drag calculation and simplified or absent buoyancy logic. This directly impacts how realistically objects interact with the simulated water.
Frame-Rate Dependency: Implementation 1's use of pow(DAMPING, delta_time) for damping ensures consistent behavior regardless of frame rate fluctuations. Implementations 2 and 3, using fixed multiplicative factors per frame, will exhibit stronger damping effects at lower frame rates and weaker effects at higher frame rates, potentially leading to inconsistent gameplay feel.
Integration & Force Application: Implementation 1 adheres to a clearer physics-based integration (Force → Acceleration → Velocity → Position). Implementations 2 and 3 employ more ad-hoc or simplified integration methods, particularly in how input (dx, dy, dz) is applied, which might lead to less predictable or stable results, especially when object mass varies significantly.
Complexity vs. Simplicity: The three versions illustrate a clear spectrum. Implementation 1 prioritizes accuracy and robustness at the cost of complexity. Implementation 3 offers maximum simplicity by omitting buoyancy and using inaccurate drag, suitable perhaps only for very specific, non-realistic gameplay. Implementation 2 sits in between.
These variations underscore a critical point about custom physics: seemingly minor implementation details significantly affect the simulation's outcome. Factors like correctly applying physical formulas (including all relevant variables like fluid density), ensuring frame-rate independence for time-based effects like damping, and using consistent numerical integration methods are crucial for achieving stable, predictable, and physically plausible results. The challenges inherent in getting these details right often motivate the use of pre-built, validated physics engines.
5. Underlying Physics Principles Explained
A clear understanding of the fundamental physics principles governing underwater motion is essential for implementing or evaluating any simulation, whether engine-based or custom. The primary forces at play in the provided examples are buoyancy and fluid drag.
Buoyancy (Archimedes' Principle)
Concept: Buoyancy is the upward force exerted by a fluid that counteracts the weight of an object partially or fully immersed in it. This force arises because fluid pressure increases with depth, resulting in greater upward pressure on the bottom surface of an object compared to the downward pressure on its top surface.16
Formula: Archimedes' Principle quantifies this force: the buoyant force (FB​) is equal in magnitude to the weight of the fluid displaced by the object (wfl​).16 Mathematically: FB​=wfl​=mfl​×g=(ρfluid​×Vdisplaced​)×g where ρfluid​ is the density of the fluid, Vdisplaced​ is the volume of the fluid displaced by the submerged portion of the object, and g is the acceleration due to gravity.26
Floating/Sinking Behavior: The net vertical force on a submerged object determines whether it floats, sinks, or remains suspended. This depends on the comparison between the buoyant force and the object's own weight (Wobj​=mobj​×g=ρobj​×Vobj​×g).
Floats: FB​>Wobj​ (occurs if ρobj​<ρfluid​).17
Sinks: FB​<Wobj​ (occurs if ρobj​>ρfluid​).17
Suspended (Neutrally Buoyant): FB​=Wobj​ (occurs if ρobj​=ρfluid​).17
Code Implementation: The Unity UnderwaterMovement.cs script calculates FB​=waterDensity×objectVolume×(−gravity), directly applying the formula assuming objectVolume represents Vdisplaced​. The Python Implementation 1 calculates the net force Fnet​=FB​−Wobj​=(ρfluid​−ρobj​)×V×g (represented as (WATER_DENSITY - self.density) * self.volume * GRAVITY), effectively combining buoyancy and weight.
Fluid Drag (Quadratic Drag Model)
Concept: Fluid drag is a type of friction, a force that opposes an object's motion through a fluid (liquid or gas). Unlike simple kinetic friction between solids, fluid drag is highly dependent on the object's velocity relative to the fluid.20
Quadratic Drag: For objects of macroscopic size moving at speeds typically encountered in games (i.e., not extremely slow or microscopic), the dominant form of drag is turbulent or quadratic drag. The magnitude of this drag force (FD​) is proportional to the square of the relative speed (v).20
Formula: The standard equation for quadratic drag is: FD​=21​ρfluid​v2Cd​A where:
ρfluid​ is the density of the fluid.
v is the speed of the object relative to the fluid.
Cd​ is the drag coefficient, a dimensionless number that depends on the object's shape, surface roughness, and the flow conditions (characterized by the Reynolds number, Re).20
A is the reference area, typically the cross-sectional area of the object perpendicular to the direction of flow.20
Code Implementation: The Unity UnderwaterMovement.cs script implements this formula as 0.5f * waterDensity * relativeVelocity.sqrMagnitude * dragCoefficient * frontalArea. Python Implementation 1 uses an equivalent per-axis calculation drag_force_x = -self.velocity_x * abs(self.velocity_x) * self.drag_coefficient * self.frontal_area * WATER_DENSITY. Python Implementations 2 and 3 use a similar structure but omit the crucial WATER_DENSITY (ρfluid​) term.
Simplifications in Games: In game development, for performance and simplicity, Cd​ and A are often treated as constant values for a given object, ignoring their potential dependence on orientation or the complex relationship between Cd​ and the Reynolds number (which itself depends on velocity, size, and fluid viscosity).20 While Stokes' Law (Fs​=6πrηv) describes drag at very low speeds (linear dependence on v), it's less commonly the primary model used for typical game character/vehicle movement.21
Grasping these principles allows developers to implement buoyancy and drag forces more accurately, even when making necessary simplifications. It also provides the basis for tuning the relevant parameters (waterDensity, objectVolume, dragCoefficient, frontalArea, mass) to achieve specific underwater movement characteristics and gameplay feel. Applying the formulas incorrectly, such as omitting fluid density from the drag calculation as seen in some Python examples, leads to behavior that deviates significantly from physical reality.
6. Comparison: Unity Physics Engine vs. Custom Python Implementation
Overview
The choice between utilizing a comprehensive, pre-built physics engine like Unity's integrated PhysX and developing custom physics logic from scratch, as demonstrated by the Python examples, represents a fundamental decision in game development with significant implications for effort, performance, control, and features.
Development Effort & Time
Unity Engine: Leveraging Rigidbody and associated components generally accelerates the implementation of standard physics behaviors. Collision detection, response, gravity, and force application are handled largely by the engine, reducing the need for developers to write extensive boilerplate physics code. Setting up basic interactions can be relatively quick.28
Custom Python: Implementing physics from the ground up requires substantial development effort. This includes coding core algorithms for numerical integration (updating position and velocity based on forces), potentially complex collision detection and response systems, and ensuring numerical stability. Debugging custom physics can also be time-consuming.28
Performance
Unity Engine: Benefits from using a highly optimized physics engine (PhysX), typically written in C++ and potentially leveraging hardware acceleration or multi-threading. These engines are designed to handle large numbers of interacting objects and complex collision scenarios efficiently.5
Custom Python: Performance is heavily contingent on the quality and optimization of the custom code and the inherent execution speed of Python. While potentially faster for extremely simple scenarios with very few objects, custom Python physics is unlikely to scale as effectively as a dedicated engine for complex simulations without significant optimization efforts, such as using numerical libraries (e.g., NumPy) or writing performance-critical parts in C/C++.29
Control & Flexibility
Unity Engine: Provides control through the parameters and methods exposed by components like Rigidbody (e.g., mass, drag, constraints, AddForce modes).1 While flexible for many game types, achieving highly unconventional physics behavior or gaining fine-grained control over the internal solver steps can be challenging and may require workarounds or advanced techniques.30 Kinematic rigidbodies offer a way to control transforms directly while still benefiting from collision/trigger events.5
Custom Python: Offers potentially absolute control over every aspect of the simulation. Developers define the exact formulas, integration methods, collision rules, and data structures. This is advantageous for games requiring unique physics mechanics not easily modeled by standard engines, specific determinism guarantees (important for some networking approaches), or when avoiding engine abstractions is desired.28
Features & Robustness
Unity Engine: Comes with a rich, mature feature set including various collider shapes (Box, Sphere, Capsule, Mesh), sophisticated collision detection modes (Discrete, Continuous, Continuous Speculative) 6, different types of joints (Hinge, Fixed, Spring, Configurable) 5, character controllers, ragdoll physics support, physics materials for defining friction and bounciness, stable numerical solvers, and interpolation/extrapolation options to smooth visual movement.1 These systems are battle-tested across thousands of shipped games.5
Custom Python: The feature set is limited entirely to what the developer explicitly implements. Replicating the robustness, stability (avoiding issues like tunneling or energy gain), and extensive feature set of a mature physics engine is a monumental task.29
Realism vs. Gameplay
Unity Engine: The underlying physics engine (PhysX) aims for physically plausible simulation but is ultimately optimized and designed for real-time game performance and stability. Parameters are exposed to allow developers to tune the behavior, often deviating from strict realism to achieve better gameplay feel.30
Custom Python: The level of realism is entirely determined by the implementation choices. It can be tailored for high-fidelity simulation if desired, or it can implement completely non-physical rules ("game physics") specifically designed to serve gameplay mechanics.29
Learning Curve
Unity Engine: Requires learning the specific API of the engine's physics system – understanding Rigidbody, Collider, ForceMode, Raycast, physics layers, etc..1
Custom Python: Demands a strong foundational understanding of classical mechanics (forces, energy, momentum), numerical methods for solving differential equations (integration techniques like Euler, Verlet, Runge-Kutta), potential instability issues, and potentially complex algorithms for collision detection and resolution.30
Feature/Capability Comparison Table

Feature/Aspect
Unity Engine Physics (via Rigidbody)
Custom Python Physics Implementation
Basic Force Application
Built-in (AddForce with ForceMode) 1
Manual calculation and integration required
Collision Detection
Built-in, various modes (Discrete, Continuous) 6
Requires manual implementation (e.g., SAT, GJK) or external library
Collision Response
Built-in (impulse-based resolution, materials) 5
Requires manual implementation (e.g., impulses, constraints)
Joints & Constraints
Built-in (Hinge, Fixed, Spring, Configurable, etc.) 1
Requires manual implementation
Stability
Generally robust, handled by engine solver 30
Highly dependent on implementation quality; prone to instability
Performance Scaling
Optimized C++ engine, generally scales well 5
Dependent on Python speed and optimization; may scale poorly
Development Time (Simple)
Relatively fast setup for standard physics 28
Can be slower due to boilerplate physics code
Development Time (Complex)
Faster due to pre-built features (joints, complex collisions) 28
Very time-consuming to replicate engine features
Control Granularity
High-level control; deep solver access limited 30
Absolute control over all calculations and logic 28
Determinism Control
Can be challenging across platforms/versions; engine managed
Full control, easier to achieve if designed for
Learning Curve
Learn engine API and physics concepts 1
Requires deep physics and numerical methods knowledge 30
Built-in Visual Debugging
Yes (e.g., collider outlines, physics debug visualization)
Requires manual implementation

Ultimately, the decision hinges on project specifics. For most games requiring standard or moderately complex physics interactions, leveraging a mature engine like Unity provides significant advantages in development speed, robustness, and feature set.28 Custom physics is typically reserved for projects with highly specialized or unconventional physics requirements, where absolute control is paramount, or potentially for educational purposes, fully acknowledging the substantial development and maintenance burden involved.29
7. Evaluation of Simulation Models: Accuracy and Limitations
Overview
It is crucial to recognize that physics simulations in games, including the underwater models presented in the Unity and Python examples, are almost always approximations of reality. They prioritize computational efficiency, stability, and controllable gameplay feel over strict physical accuracy.30 Evaluating these models involves identifying the simplifications made and understanding their impact.
Common Simplifications and Their Impact
Constant Water Properties: Both the Unity and Python examples assume a constant waterDensity. In reality, water density varies slightly with temperature, pressure (depth), and salinity.2 Ignoring these variations simplifies calculations but means buoyancy effects won't subtly change in different water conditions as they would in the real world.
Static Fluid Assumption: The drag calculations, particularly in the Unity example's relativeVelocity calculation, assume the water itself is stationary (waterVelocity = 0). This neglects the effects of currents or flows, which can significantly influence an object's trajectory and the forces acting upon it in real underwater environments. Adding currents would require a more complex system to define and sample water velocity throughout the game world.
Simplified Object Properties:
objectVolume is used for buoyancy, implicitly assuming it equals the displaced volume. This might not hold true for partially submerged objects unless specifically calculated.
frontalArea (A) and dragCoefficient (Cd​) are treated as constants. In reality, the effective frontal area changes as an object rotates. The drag coefficient (Cd​) is not truly constant; it depends significantly on the object's shape, orientation relative to the flow, surface roughness, and the Reynolds number (Re), which itself is a function of size, velocity, and fluid viscosity.20 Treating them as constants simplifies the drag calculation immensely but means the drag force won't adapt realistically to changes in orientation or flow conditions (e.g., the transition from laminar to turbulent flow, known as the "drag crisis" for spheres 22).
Basic Numerical Integration: The custom Python examples likely employ the simple Forward Euler integration method. While easy to implement, Euler integration is known to be less stable and accurate than more sophisticated methods (like Verlet integration, Runge-Kutta methods) often used within commercial physics engines. Euler integration can lead to energy drift (either gaining or losing energy over time) and inaccuracies, especially with larger time steps.30 Unity's PhysX engine uses more advanced and stable integration techniques.
Simplified Collision Handling: While Unity provides robust collision detection and response 6, the Python examples shown lack any explicit collision handling beyond a rudimentary boundary check in Implementation 2 (if self.z < 0:). A full custom physics system would need complex algorithms to detect and resolve collisions between arbitrarily shaped objects.
Ignoring Viscosity (Stokes' Drag): The models focus exclusively on quadratic drag (FD​∝v2), which dominates at higher speeds. They neglect viscous drag (Stokes' Law, FD​∝v), which is more significant at very low speeds or for very small objects.21 While often a reasonable simplification for game objects, omitting it means the simulation might not accurately represent behavior during slow movements or settling.
Impact on Realism and Gameplay
These simplifications inevitably mean the simulated behavior will deviate from real-world underwater physics. Objects might feel consistently buoyant regardless of depth, drag forces might seem less nuanced than expected, and interactions might lack the complexity of real fluid dynamics.
However, these simplifications are often deliberate design choices. Constant parameters are easier for designers to tune and predict. Less complex calculations lead to better performance, crucial for real-time applications. The goal is frequently not perfect realism, but rather believable or stylized physics that supports the intended gameplay.30 An overly complex and accurate simulation might be computationally prohibitive, difficult to control, and potentially less fun for the player.29
Specific Limitations Noted
Python Implementations 2 & 3: The omission of fluid density (ρ) in the drag formula is a significant physical inaccuracy.20 The use of fixed damping factors introduces frame-rate dependency.
Unity UnderwaterMovement: Relies on the accurate configuration of Rigidbody properties (mass, engine drag settings 1) and assumes the provided objectVolume and frontalArea are appropriate representations for the simulation. The behavior is also tied to the global physics settings of the Unity project (e.g., gravity, solver iteration counts 1).
In summary, game physics simulations are engineered approximations. Understanding the specific simplifications made—such as assuming constant fluid/object properties, using basic integration, and neglecting certain forces or complexities—is essential for interpreting the simulation's behavior, tuning it effectively, and recognizing its inherent limitations compared to reality. These trade-offs are typically accepted in favor of performance, stability, and design control.
8. Synthesized Approach to Basic Underwater Mechanics
Goal
By analyzing the common patterns present in both the Unity C# examples and the various Python GameObject implementations, a generalized, step-by-step process for creating basic underwater movement and interaction mechanics can be distilled. This process outlines the core components required, regardless of whether a physics engine is used or the logic is implemented manually.
Key Steps
Setup Physics Representation:
Engine-Based (e.g., Unity): Attach a Rigidbody component to the GameObject. Configure its fundamental properties like mass, useGravity, and potentially initial drag and angularDrag values.1 Add appropriate Collider components (e.g., BoxCollider, SphereCollider, CapsuleCollider) to define the object's physical shape for collisions and raycasts.5
Custom (e.g., Python): Define attributes within the object's class to store its physical state and properties: mass, volume, density, drag_coefficient, frontal_area, current position, and current velocity (likely as vector components).
Determine Underwater State:
Implement a mechanism to detect whether the object is currently submerged in the "water" volume. This could involve:
Using trigger colliders representing the water volume and detecting OnTriggerEnter/OnTriggerExit events (common in engines).
Checking the object's position (e.g., Y-coordinate) against a defined water level.
Performing raycasts downwards or using shape casts to detect the water surface.
Store this state, typically as a boolean flag (e.g., isUnderwater).
Handle Input:
Read input from the player intended to control movement or trigger actions.
Engine-Based: Use the engine's input system (e.g., Input.GetAxis, Input.GetButtonDown in Unity's legacy system 7) to get values for thrust, direction changes, or action triggers.
Custom: Read input through appropriate libraries or frameworks and translate it into desired forces, accelerations, or velocity changes.
Calculate & Apply Forces (Perform in Physics Update Step):
This step is typically executed within a fixed-timestep update function (FixedUpdate in Unity 13) for stability.
Buoyancy: If the object isUnderwater, calculate the buoyant force based on the fluid density (ρfluid​) and the volume of the object submerged (Vdisplaced​), using FB​=ρfluid​Vdisplaced​g.16 Apply this force upwards.
Engine: rigidbody.AddForce(Vector3.up * buoyantForce, ForceMode.Force);.13
Custom: Add the force to a net force accumulator for the frame, or directly calculate the resulting acceleration.
Drag: If the object isUnderwater, calculate the drag force opposing the object's velocity. Typically use the quadratic drag model FD​=0.5ρfluid​v2Cd​A.20 Apply this force in the direction opposite to the velocity vector.
Engine: rigidbody.AddForce(-velocity.normalized * dragMagnitude, ForceMode.Force);.13
Custom: Add the force to the net force accumulator or calculate the resulting acceleration.
Propulsion/Thrust: Calculate the intended propulsion force based on player input and the object's orientation. Apply this force.
Engine: rigidbody.AddForce(thrustDirection * thrustMagnitude, ForceMode.Force); (or other ForceMode as appropriate).13
Custom: Add the force to the net force accumulator or calculate acceleration.
Gravity: Ensure gravitational force is accounted for.
Engine: Usually handled automatically by the Rigidbody if useGravity is true.1
Custom: Explicitly add the gravitational force (m×g) downwards to the net force accumulator.
Integrate Motion:
Engine-Based: The physics engine automatically handles the integration step. It takes all forces applied via AddForce during the FixedUpdate step, computes the net force and torque, solves constraints, and updates the Rigidbody's position and rotation based on its internal solver and integration method.
Custom: Explicitly perform numerical integration. Calculate the net force accumulated in step 4. Compute acceleration using a=Fnet​/m. Update the object's velocity using an integration formula (e.g., vnew​=vold​+a×Δt for Forward Euler). Update the object's position (e.g., pnew​=pold​+vnew​×Δt). Implement necessary damping or velocity clamping for stability and control.
Handle Interactions (e.g., Thrust Attack):
Implement detection logic for interactions.
Engine: Use Physics.Raycast 9 for line-of-sight checks, or rely on collision events (OnCollisionEnter) or trigger events (OnTriggerEnter) generated by the engine based on Collider interactions.
Custom: Implement custom raycasting or collision detection algorithms (e.g., separating axis theorem).
Apply the consequences of the interaction. This could involve dealing damage to a health component, applying an impulse force to the target object (rigidbody.AddForce(force, ForceMode.Impulse) in Unity 13), triggering animations, or playing sound effects.
This synthesized process highlights the core loop common to simulating underwater physics: determining state, gathering input, calculating relevant forces based on physics principles, applying these forces to update the object's motion (either via engine calls or manual integration), and handling specific interactions. The fundamental difference lies in the degree of abstraction provided by a physics engine versus the explicit manual implementation required for custom physics.
9. Enhancing Underwater Immersion: Complementary Techniques
Overview
While plausible physics simulation provides the mechanical foundation for underwater movement and interaction, achieving a truly immersive and believable underwater environment requires a synergistic approach that incorporates carefully crafted visual and audio elements. These sensory layers work in concert with the physics to convince the player they are truly submerged.4
Visual Effects (VFX)
Visual effects play a critical role in establishing the look and feel of the underwater world.3 Key techniques include:
Water Surface Rendering: Creating a convincing transition point often involves realistic rendering of the water surface from both above and below. This includes:
Reflections: Mirroring the skybox and nearby above-water scenery.3
Refraction: Bending light that passes through the surface, distorting the view of submerged objects when seen from above, and vice-versa.3 Complex refraction can be computationally expensive.3
Waves: Simulating surface movement using techniques ranging from scrolling normal maps (for low-cost ripples) to dynamic vertex displacement driven by mathematical functions (like Gerstner waves) or physics simulations.3
Performance Optimization: Often, game water surfaces are rendered opaque when viewed from above until the camera goes underwater, avoiding the cost of rendering both environments simultaneously.3
Underwater Fog and Murkiness: Simulating the reduced visibility characteristic of water due to suspended particles. This is typically achieved using depth fog, where visibility decreases with distance from the camera. The fog is usually tinted blue or green to match common water coloration.3
Caustics: These are the shimmering, projected patterns of light seen on underwater surfaces (like the seabed or submerged objects) caused by sunlight refracting through the moving waves on the surface. Implementing caustics, often via projected animated textures or shader calculations, adds significant dynamism and realism to underwater lighting.3
Light Shafts (God Rays): Volumetric lighting effects simulating beams of sunlight penetrating the water from the surface, often enhanced by the underwater fog.
Screen Distortion: Applying subtle post-processing effects that warp or distort the rendered image can simulate the refractive effect of looking through water or a diving mask. Techniques like chromatic aberration (slight color separation at edges) can also be used, sometimes achieved by displacing color channels differently.3
Particle Effects: Adding dynamic elements like:
Bubbles emanating from the player character (breathing), moving objects, or environmental sources.3
Suspended particulate matter (plankton, sediment) drifting in the water to enhance the sense of density and currents.
Sediment kicked up from the seabed by movement.3
Foam and Splashes: Visual effects generated at the water surface where objects enter or exit, or where waves break against objects or shorelines. These are often implemented using particle systems and specialized shaders.3
Implementation: These effects are typically realized using a combination of advanced shaders (for surface rendering, refraction, caustics), post-processing effects (fog, screen distortion), particle systems (bubbles, splashes, ambient particles), and carefully crafted textures (normal maps, caustic patterns).3
Audio Design
Sound design is equally critical for underwater immersion, often employing specific techniques to modify sounds and create a unique soundscape distinct from an above-water environment.2
Filtering (Equalization - EQ): Manipulating the frequency content of sounds is fundamental.
Low-Pass Filtering (Muffling): A common cinematic trope is to heavily filter out high frequencies, creating a muffled, bass-heavy sound.2 While not entirely realistic (high frequencies can travel well in water, though perhaps not perceived the same way by human ears submerged without equipment 2), it's effective psychoacoustically for conveying submersion.
Frequency Emphasis: More nuanced approaches might involve emphasizing specific resonant frequencies or using less aggressive filtering to maintain some clarity, potentially mimicking hydrophone recordings which can sound surprisingly bright.2
Low-Frequency Propagation: Low frequencies tend to travel far and sound less directional underwater, which can be simulated in the mix.2
Reverberation and Delay: Using reverb and delay effects to simulate the way sound reflects and propagates over long distances in water. This can create a sense of vastness and depth, particularly effective for distant sounds like whale calls or sonar pings.2
Source Material Selection: Choosing or creating appropriate source sounds is key.
Hydrophone Recordings: Capturing real underwater sounds provides authentic source material, though it may need processing to fit the desired aesthetic.2
Contact Microphones: Recording vibrations through solids can yield sounds closer in quality to liquid transmission than air transmission.2
Processed Water Sounds: Recording above-water sounds (splashes, pours, streams) at high sample rates and drastically slowing them down (varispeeding) can create interesting underwater textures while preserving some high-frequency content.2 Recording through tubes can simulate resonance.2
Synthesis: Techniques like granular synthesis can be used to create bubble textures or other unique underwater sounds.37
Characteristic Underwater Sounds: Incorporating sounds strongly associated with underwater environments:
Bubbles (player breathing apparatus, character movement, geothermal vents).
Muffled versions of impacts, explosions, or mechanical sounds.
Adapted creature vocalizations (pitched down, filtered, reverberated).
Sonar pings (if appropriate for the setting).4
Ambient drones and tonal beds to create atmosphere.37
Mixing Perspectives: If the player is inside a vehicle (like a submarine), blending the internal sounds (machinery, alarms, interface sounds) with the external underwater ambience heard through the hull or via sensors creates a layered and immersive experience.4
Transitions: Carefully managing the audio shift when the player or camera moves between above-water and underwater environments, often involving crossfading soundscapes and applying/removing filters smoothly.2
Gameplay Cues: Using sound directionality (or the deliberate lack thereof for low frequencies) to provide players with information about their surroundings, such as the location of threats or points of interest.36
Achieving a convincing underwater world is therefore a multidisciplinary challenge. While physics governs movement and interaction, it is the thoughtful application of visual effects simulating light's behavior in water and audio design manipulating sound propagation and incorporating characteristic noises that truly sells the experience to the player's senses.33 The most successful examples often blend accurate simulation with artistic license and clever technical tricks to create a compelling and immersive whole.
10. Conclusion
Summary of Findings
The analysis reveals two distinct methodologies for implementing underwater physics and interactions in game development. The engine-based approach, exemplified by the Unity C# scripts, leverages built-in components like Rigidbody and functions like Physics.Raycast and AddForce (with specific ForceMode options) to handle complex physics calculations and interactions.1 This significantly reduces development time for standard behaviors and benefits from the optimized performance and robustness of the underlying physics engine.28 Conversely, the custom physics approach, demonstrated by the Python GameObject class, requires manual implementation of all physical laws (buoyancy, drag), numerical integration, and potentially collision systems.16 While offering maximum control and flexibility for unique mechanics, this path entails substantially greater development effort, potential performance bottlenecks, and challenges in achieving stability and accuracy comparable to mature engines.30 The comparison highlights a clear trade-off between the convenience, feature-richness, and performance of an engine versus the granular control afforded by custom implementation.
Evaluation Synthesis
Both approaches necessitate simplifications compared to real-world fluid dynamics. Common approximations include assuming constant water properties, neglecting currents, treating object characteristics like drag coefficients and frontal areas as constants despite their real-world variability, and often using basic numerical integration in custom systems.2 These simplifications are generally accepted trade-offs made to ensure real-time performance, stability, and design tractability. The goal in game development is typically plausible or engaging physics rather than strict scientific accuracy.30 Awareness of these limitations is crucial for understanding simulation behavior and tuning parameters effectively. The Python examples, particularly Implementations 2 and 3, demonstrated further deviations through inaccurate drag formulas (omitting fluid density) and frame-rate dependent damping, underscoring the pitfalls of custom implementation if not carefully executed based on sound physics principles.
Importance of Integration
Ultimately, creating compelling underwater experiences transcends the physics simulation itself. Success hinges on the cohesive integration of multiple elements. Plausible physics provides the foundation for how objects move and interact, but it must be complemented by evocative visual effects—simulating the unique behavior of light underwater, the presence of particles, and the dynamics of the water surface—and immersive audio design that captures the characteristic soundscape of submersion through filtering, reverberation, and specific sound sources.2 None of these components alone is sufficient; their synergy is what creates a truly believable and engaging underwater world for the player.
Final Thoughts
The decision between employing a physics engine or pursuing custom physics development for underwater mechanics should be driven by a careful assessment of project goals, team expertise, required features, and performance targets. For projects needing robust, complex, and standard physics interactions with reasonable development time, leveraging an engine like Unity is often the most pragmatic choice.28 Custom solutions may be warranted for highly specialized or unconventional mechanics, research purposes, or when absolute control over the simulation is non-negotiable, provided the significant development challenges are understood and accounted for.29 Regardless of the chosen path, a solid understanding of the underlying physics principles, the capabilities and limitations of the tools being used, and the importance of integrating physics with compelling visuals and audio is essential for crafting successful underwater gameplay.
Works cited
Scripting API: Rigidbody - Unity - Manual, accessed April 20, 2025, https://docs.unity3d.com/6000.0/Documentation/ScriptReference/Rigidbody.html
recording - "Underwater" sound tricks!? How do you make it sound ..., accessed April 20, 2025, https://sound.stackexchange.com/questions/4154/underwater-sound-tricks-how-do-you-make-it-sound-underwater-y
Everything to Know About the Properties of Water FX in Games, accessed April 20, 2025, https://www.vfxapprentice.com/blog/everything-know-about-water-fx
Realism & Sound Design in subROV – Underwater Discoveries, accessed April 20, 2025, https://80.lv/articles/realism-sound-design-in-subrov-underwater-discoveries/
Rigidbody - Unity - Manual, accessed April 20, 2025, https://docs.unity3d.com/550/Documentation/Manual/class-Rigidbody.html
Rigidbody component reference - Unity - Manual, accessed April 20, 2025, https://docs.unity.cn/Manual//class-Rigidbody.html
Scripting API: Input.GetAxis - Unity - Manual, accessed April 20, 2025, https://docs.unity3d.com/6000.0/Documentation/ScriptReference/Input.GetAxis.html
Input - Scripting API - Unity User Manual 2021.3 (LTS), accessed April 20, 2025, https://docs.unity.cn/560/Documentation/ScriptReference/Input.html
Scripting API: Physics.Raycast - Unity - Manual, accessed April 20, 2025, https://docs.unity3d.com/6000.0/Documentation/ScriptReference/Physics.Raycast.html
Scripting API: Physics - Unity User Manual 2021.3 (LTS), accessed April 20, 2025, https://docs.unity.cn/2021.1/Documentation/ScriptReference//Physics.html
Raycast Unity 3d - unity raycast tutorial - YouTube, accessed April 20, 2025, https://m.youtube.com/watch?v=cUf7FnNqv7U&pp=ygUPI3BoeXNpY3NyYXljYXN0
Raycasts in Unity (made easy) - YouTube, accessed April 20, 2025, https://www.youtube.com/watch?v=B34iq4O5ZYI&pp=0gcJCdgAo7VqN5tD
Scripting API: Rigidbody.AddForce - Unity - Manual, accessed April 20, 2025, https://docs.unity3d.com/6000.0/Documentation/ScriptReference/Rigidbody.AddForce.html
ForceMode - Scripting API - Unity - Manual, accessed April 20, 2025, https://docs.unity3d.com/6000.0/Documentation/ScriptReference/ForceMode.html
RigidbodySynchronizable.AddForce(Single, Single, Single, ForceMode) Method, accessed April 20, 2025, https://alteruna.github.io/au-multiplayer-api-docs/html/M_Alteruna_RigidbodySynchronizable_AddForce.htm
courses.lumenlearning.com, accessed April 20, 2025, https://courses.lumenlearning.com/suny-physics/chapter/11-7-archimedes-principle/#:~:text=That%20is%2C%20FB%20%3D%20w,a%20statement%20of%20Archimedes'%20principle.&text=Archimedes'%20principle%20is-,FB%20%3D%20wfl%2C,whether%20partially%20or%20totally%20submerged.
Archimedes' Principle and Buoyancy – University Physics Volume 1, accessed April 20, 2025, https://pressbooks.bccampus.ca/universityphysicssandbox/chapter/archimedes-principle-and-buoyancy/
Archimedes' Principle | Physics - Lumen Learning, accessed April 20, 2025, https://courses.lumenlearning.com/suny-physics/chapter/11-7-archimedes-principle/
14.6: Archimedes' Principle and Buoyancy - Physics LibreTexts, accessed April 20, 2025, https://phys.libretexts.org/Bookshelves/University_Physics/University_Physics_(OpenStax)/Book%3A_University_Physics_I_-_Mechanics_Sound_Oscillations_and_Waves_(OpenStax)/14%3A_Fluid_Mechanics/14.06%3A_Archimedes_Principle_and_Buoyancy
6.7: Drag Force and Terminal Speed - Physics LibreTexts, accessed April 20, 2025, https://phys.libretexts.org/Bookshelves/University_Physics/University_Physics_(OpenStax)/Book%3A_University_Physics_I_-_Mechanics_Sound_Oscillations_and_Waves_(OpenStax)/06%3A_Applications_of_Newton's_Laws/6.07%3A_Drag_Force_and_Terminal_Speed
Drag Forces | Physics - Lumen Learning, accessed April 20, 2025, https://courses.lumenlearning.com/suny-physics/chapter/5-2-drag-forces/
Drag (physics) - Wikipedia, accessed April 20, 2025, https://en.wikipedia.org/wiki/Drag_(physics)
Drag coefficient - Wikipedia, accessed April 20, 2025, https://en.wikipedia.org/wiki/Drag_coefficient
Input.GetAxis - Unity Script Reference - Huihoo, accessed April 20, 2025, https://docs.huihoo.com/unity/3.3/Documentation/ScriptReference/Input.GetAxis.html
C# GetAxis in Unity! - Beginner Scripting Tutorial - YouTube, accessed April 20, 2025, https://www.youtube.com/watch?v=MK4OmsViqMA
What Is Archimedes Principle - BYJU'S, accessed April 20, 2025, https://byjus.com/physics/archimedes-principle/
I have made a physics simulator that replicates projectile motion with quadratic drag! Please feel free to download and compile it. Let me know of any bugs! - Reddit, accessed April 20, 2025, https://www.reddit.com/r/Physics/comments/105wc6y/i_have_made_a_physics_simulator_that_replicates/
Custom Game Engine vs Ready-Made Solutions: Which Path Should You Take?, accessed April 20, 2025, https://meliorgames.com/game-development/custom-game-engine-vs-ready-made-solutions-which-path-should-you-take/
When should I use a physics engine? [closed] - Game Development Stack Exchange, accessed April 20, 2025, https://gamedev.stackexchange.com/questions/6120/when-should-i-use-a-physics-engine
How Does Video Game Physics Work - Game Design Skills, accessed April 20, 2025, https://gamedesignskills.com/game-development/video-game-physics/
Should I write my physics engine or use a made one instead? : r/gamedev - Reddit, accessed April 20, 2025, https://www.reddit.com/r/gamedev/comments/g1m8ue/should_i_write_my_physics_engine_or_use_a_made/
Rigid Bodies | coherence Documentation, accessed April 20, 2025, https://docs.coherence.io/manual/networking-state-changes/rigid-bodies
How Water Works (in Video Games) - YouTube, accessed April 20, 2025, https://www.youtube.com/watch?v=BqJm3B8cubo
Underwater VFX Tutorial in After Effects - YouTube, accessed April 20, 2025, https://m.youtube.com/watch?v=11Ath8JoJJQ&pp=0gcJCfcAhR29_xXO
The Evolution of Water Effects In Video Games - YouTube, accessed April 20, 2025, https://www.youtube.com/watch?v=JW9UZeTnVhk
How to Make Underwater Sound Effects - Video Game Sound Design - YouTube, accessed April 20, 2025, https://www.youtube.com/watch?v=gVAnyAJfqSc
How To Create An Underwater Ambience - YouTube, accessed April 20, 2025, https://www.youtube.com/watch?v=S9ReTaU5EU0