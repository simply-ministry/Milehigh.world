import json

# --- 1. ALL CORE CLASSES (PASTED FROM PREVIOUS STEPS) ---
# (Includes GameObject, Character, Item, etc.)
# ... (For brevity, imagine all previously defined core classes are here) ...

class GameObject:
    """The base class for all objects in the game world."""
    def __init__(self, name="Object", symbol='?', x=0, y=0, state=None):
        self.name = name
        self.symbol = symbol
        self.x = x
        self.y = y
        self.state = state # e.g., 'normal', 'hostile', 'dead'

    def update(self, scene_manager):
        """Placeholder for object-specific logic that runs each turn."""
        pass

    def to_dict(self):
        """Serializes the object to a dictionary."""
        return {
            "class": self.__class__.__name__,
            "name": self.name,
            "symbol": self.symbol,
            "x": self.x,
            "y": self.y,
            "state": self.state,
        }

    @staticmethod
    def create_from_dict(data):
        """Factory method to deserialize a game object from a dictionary."""
        class_name = data.get("class")
        if not class_name or class_name not in globals():
            raise ValueError(f"Invalid or missing class name in data: {data}")
        target_class = globals()[class_name]
        # Dispatch to the specific class's from_dict method
        return target_class.from_dict(data)

    @classmethod
    def from_dict(cls, data):
        """Deserializes a basic GameObject from a dictionary."""
        # This version is for the base class and serves as a default.
        return cls(
            name=data.get("name"),
            symbol=data.get("symbol"),
            x=data.get("x"),
            y=data.get("y"),
            state=data.get("state"),
        )


class Item(GameObject):
    """Represents items that can be picked up or used."""
    def __init__(self, name="Item", symbol='*', x=0, y=0):
        super().__init__(name, symbol, x, y)

class Interactable(GameObject):
    """Represents objects that can be examined for a description."""
    def __init__(self, name, symbol, x, y, description):
        super().__init__(name, symbol, x, y)
        self.description = description

    def on_examine(self):
        """Returns the description of the object."""
        return self.description

    def to_dict(self):
        data = super().to_dict()
        data["description"] = self.description
        return data

    @classmethod
    def from_dict(cls, data):
        return cls(
            name=data.get("name"),
            symbol=data.get("symbol"),
            x=data.get("x"),
            y=data.get("y"),
            description=data.get("description"),
        )

# --- 2. NEW Dialogue System Classes ---

class DialogueNode:
    """Represents a single piece of dialogue and potential player choices."""
    def __init__(self, text, character_name="Narrator", options=None):
        self.text = text
        self.character_name = character_name
        self.options = options if options else {}

    def to_dict(self):
        return {
            "text": self.text,
            "character_name": self.character_name,
            "options": self.options,
        }

    @classmethod
    def from_dict(cls, data):
        return cls(
            text=data.get("text"),
            character_name=data.get("character_name", "Narrator"),
            options=data.get("options"),
        )

class DialogueManager:
    """Controls the flow of a single conversation."""
    def __init__(self, start_node_key="start"):
        self.nodes = {}
        self.current_node_key = start_node_key

    def add_node(self, key, node):
        self.nodes[key] = node

    def get_current_node(self):
        return self.nodes.get(self.current_node_key)

    def select_option(self, choice_index):
        node = self.get_current_node()
        if node and node.options:
            option_keys = list(node.options.values())
            if 0 <= choice_index < len(option_keys):
                self.current_node_key = option_keys[choice_index]
                return True
        return False

    def to_dict(self):
        return {
            "nodes": {key: node.to_dict() for key, node in self.nodes.items()},
            "current_node_key": self.current_node_key,
        }

    @classmethod
    def from_dict(cls, data):
        manager = cls(start_node_key=data.get("current_node_key", "start"))
        nodes_data = data.get("nodes", {})
        for key, node_data in nodes_data.items():
            manager.add_node(key, DialogueNode.from_dict(node_data))
        return manager

# --- 3. UPDATING THE CHARACTER AND GAME ENGINE ---

class Character(GameObject):
    def __init__(self, name="Character", x=0, y=0, health=100, state=None):
        super().__init__(name, 'C', x, y, state)
        self.health = health
        self.max_health = health
        self.dialogue = None

    def to_dict(self):
        data = super().to_dict()
        data.update({
            "health": self.health,
            "max_health": self.max_health,
            "dialogue": self.dialogue.to_dict() if self.dialogue else None,
        })
        return data

    @classmethod
    def from_dict(cls, data):
        character = cls(
            name=data.get("name"),
            x=data.get("x"),
            y=data.get("y"),
            health=data.get("health"),
            state=data.get("state")
        )
        character.max_health = data.get("max_health", character.health)
        dialogue_data = data.get("dialogue")
        if dialogue_data:
            character.dialogue = DialogueManager.from_dict(dialogue_data)
        return character

class Anastasia(Character):
    def __init__(self, name="Anastasia", x=0, y=0, health=120, state=None):
        super().__init__(name=name, x=x, y=y, health=health, state=state)
        self.symbol = '@'

class Reverie(Character):
    def __init__(self, name="Reverie", x=0, y=0, health=100, state=None):
        super().__init__(name=name, x=x, y=y, health=health, state=state)
        self.symbol = 'R'

class Scene:
    """Holds all the data for a single game area: map, objects, etc."""
    def __init__(self, name, width=40, height=10):
        self.name = name
        self.width = width
        self.height = height
        self.game_objects = []
        self.player_character = None

    def add_object(self, obj):
        self.game_objects.append(obj)

    def set_player(self, player):
        self.player_character = player
        self.add_object(player)

    def get_object_at(self, x, y):
        for obj in self.game_objects:
            if obj.x == x and obj.y == y:
                return obj
        return None

    def to_dict(self):
        player_name = self.player_character.name if self.player_character else None
        return {
            "name": self.name,
            "width": self.width,
            "height": self.height,
            "game_objects": [obj.to_dict() for obj in self.game_objects],
            "player_character_name": player_name,
        }

    @classmethod
    def from_dict(cls, data):
        scene = cls(name=data["name"], width=data["width"], height=data["height"])
        scene.game_objects = [GameObject.create_from_dict(obj_data) for obj_data in data.get("game_objects", [])]
        player_name = data.get("player_character_name")
        if player_name:
            scene.player_character = next((obj for obj in scene.game_objects if obj.name == player_name), None)
        return scene

class Game:
    def __init__(self, width=40, height=10):
        self.width = width
        self.height = height
        self.message_log = []
        self.turn_taken = False
        self.game_over = False
        self.in_conversation = False
        self.dialogue_manager = None

    def to_dict(self):
        return {
            "width": self.width,
            "height": self.height,
            "message_log": self.message_log,
            "game_over": self.game_over,
            "in_conversation": self.in_conversation,
            "dialogue_manager": self.dialogue_manager.to_dict() if self.dialogue_manager else None,
        }

    @classmethod
    def from_dict(cls, data):
        game = cls(width=data["width"], height=data["height"])
        game.message_log = data.get("message_log", [])
        game.game_over = data.get("game_over", False)
        game.in_conversation = data.get("in_conversation", False)
        dialogue_data = data.get("dialogue_manager")
        if dialogue_data:
            game.dialogue_manager = DialogueManager.from_dict(dialogue_data)
        return game

    def log_message(self, message):
        self.message_log.append(message)
        if len(self.message_log) > 5:
            self.message_log.pop(0)

    def handle_input(self, scene_manager):
        """UPDATED to handle dialogue, saving, and loading."""
        if self.in_conversation:
            choice = input("Choose an option (number): ")
            if choice.isdigit() and self.dialogue_manager.select_option(int(choice) - 1):
                pass
            else:
                self.log_message("Invalid choice.")
            self.turn_taken = True
            return

        command = input("Action: ").lower().strip()
        parts = command.split()
        action = parts[0] if parts else ""

        if action == "move" and len(parts) > 1:
            direction = parts[1]
            dx, dy = 0, 0
            if direction == "up": dy = -1
            elif direction == "down": dy = 1
            elif direction == "left": dx = -1
            elif direction == "right": dx = 1

            player = scene_manager.scene.player_character
            new_x, new_y = player.x + dx, player.y + dy

            if 0 <= new_x < self.width and 0 <= new_y < self.height:
                target = scene_manager.scene.get_object_at(new_x, new_y)
                if not target:
                    player.x = new_x
                    player.y = new_y
                    self.turn_taken = True
                else:
                    self.log_message(f"You can't move there. {target.name} is in the way.")
            else:
                self.log_message("You can't move off the map.")

        elif action == "examine" and len(parts) > 1:
            target_name = " ".join(parts[1:])
            target = next((obj for obj in scene_manager.scene.game_objects if isinstance(obj, Interactable) and obj.name.lower() == target_name.lower()), None)
            if target:
                self.log_message(f"{target.name}: {target.on_examine()}")
            else:
                self.log_message(f"There is no '{target_name}' to examine.")
            self.turn_taken = True

        elif action == "talk" and len(parts) > 1:
            target_name = " ".join(parts[1:])
            target = next((obj for obj in scene_manager.scene.game_objects if obj.name.lower() == target_name.lower()), None)
            if target and isinstance(target, Character) and target.dialogue:
                distance = abs(scene_manager.scene.player_character.x - target.x) + abs(scene_manager.scene.player_character.y - target.y)
                if distance <= 2:
                    self.start_conversation(target.dialogue)
                else:
                    self.log_message(f"You are too far away to talk to {target.name}.")
            else:
                self.log_message(f"'{target_name}' has nothing to say or isn't here.")
            self.turn_taken = True

        elif action == "save":
            filename = parts[1] if len(parts) > 1 else "savegame.json"
            save_game(scene_manager, filename)
            self.log_message(f"Game saved to {filename}")
            self.turn_taken = False # Saving does not consume a turn

        elif action == "load":
            filename = parts[1] if len(parts) > 1 else "savegame.json"
            new_manager = load_game(filename)
            if new_manager:
                if scene_manager.__class__ is not new_manager.__class__:
                    self.log_message(f"Error: Save is for a different scene type ('{new_manager.__class__.__name__}').")
                else:
                    scene_manager.game = new_manager.game
                    scene_manager.scene = new_manager.scene
                    self.log_message("Game loaded successfully.")
            else:
                self.log_message("Failed to load game.")
            self.turn_taken = True

        elif action == "quit":
            self.game_over = True
        else:
            self.log_message("Unknown command. Try: move [dir], talk [name], examine [name], save/load, quit")


    def start_conversation(self, dialogue_manager):
        """Initiates a conversation."""
        self.in_conversation = True
        self.dialogue_manager = dialogue_manager
        self.log_message("A conversation begins.")

    def end_conversation(self):
        """Ends the current conversation."""
        self.in_conversation = False
        self.dialogue_manager = None
        self.log_message("The conversation ends.")

    def draw(self, scene):
        """Draws the game state to the console."""
        # Clear screen
        print("\033c", end="")

        print(f"--- {scene.name} ---")

        if self.in_conversation:
            node = self.dialogue_manager.get_current_node()
            if not node:
                self.end_conversation()
                # Fall through to draw the map on the turn the conversation ends
            else:
                print(f"\n--- Conversation with {node.character_name} ---")
                print(f"> \"{node.text}\"")
                if node.options:
                    for i, option_text in enumerate(node.options.keys()):
                        print(f"  {i+1}. {option_text}")
                else:
                    # If there are no options, the conversation ends on the next player input
                    self.end_conversation()
                # Don't draw map while in conversation
                return

        # --- Draw Map ---
        grid = [['.' for _ in range(self.width)] for _ in range(self.height)]
        for obj in sorted(scene.game_objects, key=lambda o: 0 if isinstance(o, Character) else -1):
             if 0 <= obj.x < self.width and 0 <= obj.y < self.height:
                grid[obj.y][obj.x] = obj.symbol

        for row in grid:
            print(" ".join(row))

        # --- Draw Player Status and Message Log ---
        player = scene.player_character
        print("-" * (self.width * 2 - 1))
        print(f"{player.name} | Health: {player.health}/{player.max_health}")
        print("-- Messages --")
        for msg in self.message_log:
            print(f"- {msg}")
        print("-" * (self.width * 2 - 1))


class SceneManager:
    """Base class for controlling scenes, events, and game logic."""
    def __init__(self, scene, game, setup_scene=True):
        self.scene = scene
        self.game = game
        self.is_running = True
        if setup_scene:
            self.setup()

    def setup(self):
        """Initializes the scene with objects, characters, etc."""
        raise NotImplementedError

    def update(self):
        """Runs every game loop, checking for win/loss conditions, etc."""
        pass

    def run(self):
        """Main game loop for this scene."""
        while not self.game.game_over and self.is_running:
            self.game.draw(self.scene)
            if self.game.game_over: break

            self.game.turn_taken = False
            while not self.game.turn_taken and not self.game.game_over:
                self.game.handle_input(self)

            if self.game.turn_taken:
                # AI turn logic would go here
                for obj in self.scene.game_objects:
                    obj.update(self)

            self.update() # Check for scene-specific win/loss conditions

# --- 4. NEW Save/Load Functionality ---

def save_game(scene_manager, filename="savegame.json"):
    """Saves the current game state to a file."""
    if not scene_manager:
        print("Cannot save a null scene manager.")
        return
    try:
        # We need to save the state of the Game, the Scene, and the SceneManager's class name
        state = {
            "scene_manager_class": scene_manager.__class__.__name__,
            "game_state": scene_manager.game.to_dict(),
            "scene_state": scene_manager.scene.to_dict(),
        }
        with open(filename, "w") as f:
            json.dump(state, f, indent=4)
    except Exception as e:
        # Log to the game instance if available, otherwise print.
        if scene_manager and scene_manager.game:
            scene_manager.game.log_message(f"Error saving game: {e}")
        print(f"Error saving game: {e}") # Also print to console for debugging

def load_game(filename="savegame.json"):
    """Loads a game state from a file."""
    try:
        with open(filename, "r") as f:
            state = json.load(f)

        # Re-create the Game and Scene objects
        game_state = Game.from_dict(state["game_state"])
        scene_state = Scene.from_dict(state["scene_state"])

        # Re-create the SceneManager
        manager_class_name = state["scene_manager_class"]
        manager_class = globals().get(manager_class_name)
        if not manager_class:
            raise ValueError(f"SceneManager class '{manager_class_name}' not found.")

        # The manager needs the scene and game objects during initialization
        # We pass setup_scene=False to prevent re-populating the loaded scene
        loaded_manager = manager_class(scene_state, game_state, setup_scene=False)
        return loaded_manager
    except FileNotFoundError:
        print(f"Save file not found: {filename}")
        return None
    except Exception as e:
        print(f"Error loading game: {e}")
        return None

# --- 5. SCRIPTING THE ANASTASIA & REVERIE DIALOGUE ---

class FirstMeetingScene(SceneManager):
    """A scene where Anastasia and Reverie meet for the first time."""
    def setup(self):
        player = Anastasia(name="Anastasia", x=5, y=5)
        npc = Reverie(name="Reverie", x=7, y=5)

        # Create the dialogue tree for Reverie
        reverie_dialogue = DialogueManager()
        reverie_dialogue.add_node("start", DialogueNode(
            "Another one drawn by these old stones. You have the look of a believer. Are you one of the ten the prophecy speaks of?",
            "Reverie",
            {"I am. My name is Anastasia.": "anastasia_intro", "Who's asking?": "who_asking"}
        ))
        reverie_dialogue.add_node("anastasia_intro", DialogueNode(
            "Anastasia the Dreamer. I've heard the whispers. They say you're meant to lead us. I remain unconvinced.",
            "Reverie" # Ends conversation
        ))
        reverie_dialogue.add_node("who_asking", DialogueNode(
            "Someone who finds prophecies to be... unreliable. I am Reverie. Now, answer the question.",
            "Reverie",
            {"I am Anastasia. And we need to work together.": "anastasia_intro"}
        ))
        npc.dialogue = reverie_dialogue

        self.scene.set_player(player)
        self.scene.add_object(npc)
        self.game.log_message("You approach a skeptical-looking woman leaning against a monolith.")

# --- 6. RUNNING THE DIALOGUE SCENE ---
if __name__ == "__main__":
    # To start a new game:
    game_engine = Game()
    meeting_scene = Scene("Monolith Clearing")
    meeting_manager = FirstMeetingScene(meeting_scene, game_engine)

    # To load a game instead, you could do:
    # meeting_manager = load_game()
    # if not meeting_manager:
    #     print("Starting a new game because load failed.")
    #     game_engine = Game()
    #     meeting_scene = Scene("Monolith Clearing")
    #     meeting_manager = FirstMeetingScene(meeting_scene, game_engine)

    if meeting_manager:
        meeting_manager.run()
        print("Game over.")