import unittest
from game import Skyix, Anastasia, Micah, Player, Enemy, Zaia, DelilahTheDesolate, Nyxar
from game import Skyix, Anastasia, Micah, Player, Enemy, Zaia, DelilahTheDesolate

class TestNewCharacters(unittest.TestCase):

    def setUp(self):
        """Set up test fixtures before each test method."""
        self.skyix = Skyix()
        self.anastasia = Anastasia()
        self.micah = Micah()
        self.ally = Player(name="Ally")
        self.enemy = Enemy(name="Test Enemy")

    # --- Sky.ix Tests ---

    def test_skyix_initialization(self):
        """Test that Sky.ix initializes with the correct stats."""
        self.assertEqual(self.skyix.health, 120)
        self.assertEqual(self.skyix.mana, 150)
        self.assertIn("void_tech", self.skyix.spells)

    def test_skyix_deploy_drone_success(self):
        """Test that Sky.ix can deploy a drone with sufficient mana."""
        initial_mana = self.skyix.mana
        self.skyix.deploy_drone()
        self.assertEqual(self.skyix.active_drones, 1)
        self.assertLess(self.skyix.mana, initial_mana)

    def test_skyix_deploy_drone_insufficient_mana(self):
        """Test that Sky.ix cannot deploy a drone without enough mana."""
        self.skyix.mana = 10
        initial_drones = self.skyix.active_drones
        self.skyix.deploy_drone()
        self.assertEqual(self.skyix.active_drones, initial_drones)

    def test_skyix_cast_spell_void_tech(self):
        """Test that Sky.ix can cast her unique spell."""
        initial_enemy_health = self.enemy.health
        self.skyix.cast_spell("void_tech", self.enemy)
        self.assertLess(self.enemy.health, initial_enemy_health)

    # --- Anastasia Tests ---

    def test_anastasia_initialization(self):
        """Test that Anastasia initializes with the correct stats."""
        self.assertEqual(self.anastasia.health, 90)
        self.assertEqual(self.anastasia.mana, 200)

    def test_anastasia_weave_dream_heal(self):
        """Test Anastasia's healing ability."""
        self.ally.health = 50
        self.anastasia.weave_dream(self.ally, is_healing=True)
        self.assertEqual(self.ally.health, 75)

    def test_anastasia_weave_dream_harm(self):
        """Test Anastasia's harming ability."""
        initial_enemy_health = self.enemy.health
        self.anastasia.weave_dream(self.enemy, is_healing=False)
        self.assertLess(self.enemy.health, initial_enemy_health)

    def test_anastasia_glimpse_future(self):
        """Test Anastasia's utility skill."""
        initial_visions = len(self.anastasia.visions)
        self.anastasia.glimpse_future()
        self.assertGreater(len(self.anastasia.visions), initial_visions)

    # --- Micah Tests ---

    def test_micah_initialization(self):
        """Test that Micah initializes with the correct stats."""
        self.assertEqual(self.micah.health, 200)
        self.assertEqual(self.micah.fortitude, 0)
        self.assertEqual(self.micah.mana, 0)

    def test_micah_gain_fortitude_on_damage(self):
        """Test that Micah gains Fortitude when he takes damage."""
        self.micah.take_damage(50)
        self.assertEqual(self.micah.fortitude, 50)

    def test_micah_adamantine_skin_damage_reduction(self):
        """Test that Adamantine Skin reduces incoming damage."""
        self.micah.fortitude = 100
        self.micah.activate_adamantine_skin()
        initial_health = self.micah.health
        self.micah.take_damage(50)
        # Damage should be halved (25), so health should be initial_health - 25
        self.assertEqual(self.micah.health, initial_health - 25)

    def test_micah_earthen_smash(self):
        """Test that Earthen Smash deals damage and consumes Fortitude."""
        self.micah.fortitude = 50
        initial_enemy_health = self.enemy.health
        self.micah.earthen_smash(self.enemy)
        self.assertLess(self.enemy.health, initial_enemy_health)
        self.assertLess(self.micah.fortitude, 50)

    # --- Delilah Tests ---

    def test_delilah_initialization(self):
        """Test that Delilah initializes with the correct stats."""
        self.assertEqual(self.delilah.health, 160)
        self.assertEqual(self.delilah.blight, 25)
        self.assertEqual(self.delilah.max_blight, 100)

    def test_touch_of_decay_generates_blight(self):
        """Test that touch_of_decay increases blight."""
        initial_blight = self.delilah.blight
        self.delilah.touch_of_decay(self.enemy)
        self.assertEqual(self.delilah.blight, initial_blight + 5)

    def test_summon_omen_avatar_success(self):
        """Test that summon_omen_avatar can be used with sufficient blight."""
        self.delilah.blight = 70
        initial_blight = self.delilah.blight
        self.delilah.summon_omen_avatar(self.enemy)
        self.assertEqual(self.delilah.blight, initial_blight - 60)

    def test_summon_omen_avatar_insufficient_blight(self):
        """Test that summon_omen_avatar cannot be used without enough blight."""
        self.delilah.blight = 50
        initial_blight = self.delilah.blight
        self.delilah.summon_omen_avatar(self.enemy)
        self.assertEqual(self.delilah.blight, initial_blight)

    def test_voidblight_zone_success(self):
        """Test that voidblight_zone can be used with sufficient blight."""
        self.delilah.blight = 95
        initial_blight = self.delilah.blight
        self.delilah.voidblight_zone()
        self.assertEqual(self.delilah.blight, initial_blight - 90)

    def test_voidblight_zone_insufficient_blight(self):
        """Test that voidblight_zone cannot be used without enough blight."""
        self.delilah.blight = 80
        initial_blight = self.delilah.blight
        self.delilah.voidblight_zone()
        self.assertEqual(self.delilah.blight, initial_blight)


# --- Zaia Tests ---
class TestZaia(unittest.TestCase):
    def setUp(self):
        """Set up test fixtures before each test method."""
        self.zaia = Zaia()
        self.enemy = Enemy(name="Test Enemy")

    def test_zaia_initialization(self):
        """Test that Zaia initializes with the correct stats."""
        self.assertEqual(self.zaia.health, 130)
        self.assertEqual(self.zaia.momentum, 0)
        self.assertEqual(self.zaia.is_stealthed, False)

    def test_swift_strike_generates_momentum(self):
        """Test that swift_strike deals damage and generates momentum."""
        initial_enemy_health = self.enemy.health
        self.zaia.swift_strike(self.enemy)
        self.assertEqual(self.zaia.momentum, 15)
        self.assertLess(self.enemy.health, initial_enemy_health)

    def test_shadow_vanish_success_and_failure(self):
        """Test that shadow_vanish activates stealth and consumes momentum."""
        # Failure case
        self.zaia.momentum = 20
        self.zaia.shadow_vanish()
        self.assertEqual(self.zaia.is_stealthed, False)

        # Success case
        self.zaia.momentum = 40
        self.zaia.shadow_vanish()
        self.assertEqual(self.zaia.is_stealthed, True)
        self.assertEqual(self.zaia.momentum, 10)

    def test_exploit_weakness_damage_and_stealth_break(self):
        """Test that exploit_weakness deals correct damage and breaks stealth."""
        # Unstealthed case
        self.zaia.momentum = 60
        initial_enemy_health = self.enemy.health
        self.zaia.exploit_weakness(self.enemy)
        self.assertEqual(self.zaia.momentum, 10)
        self.assertEqual(self.enemy.health, initial_enemy_health - 60)
        self.assertEqual(self.zaia.is_stealthed, False)

        # Stealthed case
        self.zaia.momentum = 60
        self.zaia.is_stealthed = True
        initial_enemy_health = self.enemy.health
        self.zaia.exploit_weakness(self.enemy)
        self.assertEqual(self.zaia.momentum, 10)
        self.assertEqual(self.enemy.health, initial_enemy_health - 120)
        self.assertEqual(self.zaia.is_stealthed, False)

    def test_exploit_weakness_insufficient_momentum(self):
        """Test that exploit_weakness fails without enough momentum."""
        self.zaia.momentum = 40
        initial_enemy_health = self.enemy.health
        self.zaia.exploit_weakness(self.enemy)
        self.assertEqual(self.zaia.momentum, 40)
        self.assertEqual(self.enemy.health, initial_enemy_health)

class TestNyxar(unittest.TestCase):
    def setUp(self):
        """Set up test fixtures before each test method."""
        self.nyxar = Nyxar()
        self.enemy = Enemy(name="Test Enemy")

    def test_nyxar_initialization(self):
        """Test that Nyxar initializes with the correct stats."""
        self.assertEqual(self.nyxar.health, 1000)
        self.assertEqual(self.nyxar.dominion, 0)
        self.assertEqual(self.nyxar.max_dominion, 100)

    def test_shadow_tether(self):
        """Test that shadow_tether adds a target to tethered_enemies."""
        self.nyxar.shadow_tether(self.enemy)
        self.assertIn(self.enemy, self.nyxar.tethered_enemies)

    def test_update_generates_dominion(self):
        """Test that update generates dominion based on tethered enemies."""
        self.nyxar.shadow_tether(self.enemy)
        self.nyxar.update()
        self.assertEqual(self.nyxar.dominion, 5)

    def test_create_umbral_clone_success(self):
        """Test that create_umbral_clone works with enough dominion."""
        self.nyxar.dominion = 70
        initial_dominion = self.nyxar.dominion
        self.nyxar.create_umbral_clone(self.enemy)
        self.assertEqual(self.nyxar.dominion, initial_dominion - 60)

    def test_create_umbral_clone_insufficient_dominion(self):
        """Test that create_umbral_clone fails without enough dominion."""
        self.nyxar.dominion = 50
        initial_dominion = self.nyxar.dominion
        self.nyxar.create_umbral_clone(self.enemy)
        self.assertEqual(self.nyxar.dominion, initial_dominion)

    def test_worldless_chasm_success(self):
        """Test that worldless_chasm works with enough dominion."""
        self.nyxar.dominion = 100
        self.nyxar.worldless_chasm()
        self.assertEqual(self.nyxar.dominion, 0)

    def test_worldless_chasm_insufficient_dominion(self):
        """Test that worldless_chasm fails without enough dominion."""
        self.nyxar.dominion = 90
        initial_dominion = self.nyxar.dominion
        self.nyxar.worldless_chasm()
        self.assertEqual(self.nyxar.dominion, initial_dominion)

# --- Delilah Tests ---
class TestDelilah(unittest.TestCase):
    def setUp(self):
        """Set up test fixtures before each test method."""
        self.delilah = DelilahTheDesolate()
        self.enemy = Enemy(name="Test Enemy")


class TestOmegaOne(unittest.TestCase):
    def setUp(self):
        """Set up test fixtures before each test method."""
        from game import OmegaOne
        self.omega_one = OmegaOne()
        self.enemy = Enemy(name="Test Enemy")

    def test_omega_one_initialization(self):
        """Test that Omega.one initializes with the correct stats."""
        self.assertEqual(self.omega_one.health, 180)
        self.assertEqual(self.omega_one.processing_power, 100)
        self.assertEqual(self.omega_one.directive, "Guardian")

    def test_switch_directive(self):
        """Test that Omega.one can switch directives."""
        self.omega_one.switch_directive("Annihilation")
        self.assertEqual(self.omega_one.directive, "Annihilation")

    def test_energy_lance_annihilation(self):
        """Test the energy lance ability in Annihilation directive."""
        self.omega_one.switch_directive("Annihilation")
        initial_enemy_health = self.enemy.health
        self.omega_one.energy_lance(self.enemy)
        self.assertLess(self.enemy.health, initial_enemy_health)

    def test_system_overload(self):
        """Test the system overload ability."""
        initial_health = self.omega_one.health
        initial_enemy_health = self.enemy.health
        self.omega_one.system_overload([self.enemy])
        self.assertLess(self.omega_one.health, initial_health)
        self.assertLess(self.enemy.health, initial_enemy_health)


if __name__ == '__main__':
    unittest.main()