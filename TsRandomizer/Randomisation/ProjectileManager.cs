using System;
using System.Collections.Generic;
using System.Linq;
using Timespinner.Core;
using Timespinner.GameAbstractions.Inventory;
using TsRandomizer.IntermediateObjects;

namespace TsRandomizer.Randomisation
{
    class SlotAndType : IEquatable<SlotAndType>
    {
        public EOrbSlot Slot;
        public EInventoryOrbType Type;

        public bool Equals(SlotAndType other)
        {
            return Slot == other.Slot && Type == other.Type;
        }
    }

    class ProjectileTypeToItemMap
    {
        public Type ProjectileType;
        public EOrbSlot OrbSlot;
        public EInventoryOrbType OrbType;

        public SlotAndType SlotAndType => new SlotAndType { Slot = OrbSlot, Type = OrbType };
    }

    class SubProjectileTypeToItemMap : ProjectileTypeToItemMap
    {
        public ProjectileTypeToItemMap ParentProjectile;
        public bool ShouldShareDamageType;
    }
    class ProjectileManager
    {
        private static string LunaisProjectiles = "Timespinner.GameObjects.Heroes.LunaisProjectiles";
        private static string LunaisProjectiles2 = "Timespinner.GameObjects.Heroes.Lunais.LunaisProjectiles";
        private static string SpellProjectiles = "Timespinner.GameObjects.Heroes.Lunais.LunaisProjectiles.Spell";

        public static ProjectileTypeToItemMap[] DamagingProjectiles =
        {
            new ProjectileTypeToItemMap {
                ProjectileType = TimeSpinnerType.Get($"{LunaisProjectiles}.BladeOrbMeleeDamageArea"),
                OrbSlot = EOrbSlot.Melee,
                OrbType = EInventoryOrbType.Blade
            },
            new ProjectileTypeToItemMap
            {
                ProjectileType = TimeSpinnerType.Get($"{LunaisProjectiles}.BloodOrbMeleeDamageDroplet"),
                OrbSlot = EOrbSlot.Melee,
                OrbType = EInventoryOrbType.Blood
            },
            new ProjectileTypeToItemMap
            {
                ProjectileType = TimeSpinnerType.Get($"{LunaisProjectiles}.BlueOrbMeleeDamageArea"),
                OrbSlot = EOrbSlot.Melee,
                OrbType = EInventoryOrbType.Blue
            },
            new ProjectileTypeToItemMap
            {
                ProjectileType = TimeSpinnerType.Get($"{LunaisProjectiles}.BlueOrbSpellBulletLarge"),
                OrbSlot = EOrbSlot.Spell,
                OrbType = EInventoryOrbType.Blue
            },
            new ProjectileTypeToItemMap
            {
                ProjectileType = TimeSpinnerType.Get($"{LunaisProjectiles}.BookOrbMeleeDamageArea"),
                OrbSlot = EOrbSlot.Melee,
                OrbType = EInventoryOrbType.Book
            },
            new ProjectileTypeToItemMap
            {
                ProjectileType = TimeSpinnerType.Get($"{LunaisProjectiles}.BookOrbSpellProjectile"),
                OrbSlot = EOrbSlot.Spell,
                OrbType = EInventoryOrbType.Book
            },
            new ProjectileTypeToItemMap
            {
                ProjectileType = TimeSpinnerType.Get($"{LunaisProjectiles}.EmpireOrbMeleeDamageArea"),
                OrbSlot = EOrbSlot.Melee,
                OrbType = EInventoryOrbType.Empire
            },
            new ProjectileTypeToItemMap
            {
                ProjectileType = TimeSpinnerType.Get($"{LunaisProjectiles}.EyeOrbMeleeDamageArea"),
                OrbSlot = EOrbSlot.Melee,
                OrbType = EInventoryOrbType.Eye
            },
            new ProjectileTypeToItemMap
            {
                ProjectileType = TimeSpinnerType.Get($"{LunaisProjectiles}.FlameOrbMeleeDamageArea"),
                OrbSlot = EOrbSlot.Melee,
                OrbType = EInventoryOrbType.Flame
            },
            new ProjectileTypeToItemMap
            {
                ProjectileType = TimeSpinnerType.Get($"{LunaisProjectiles}.GunOrbMeleeProjectile"),
                OrbSlot = EOrbSlot.Melee,
                OrbType = EInventoryOrbType.Gun
            },
            new ProjectileTypeToItemMap
            {
                ProjectileType = TimeSpinnerType.Get($"{LunaisProjectiles}.IceOrbIcicleDamageArea"),
                OrbSlot = EOrbSlot.Melee,
                OrbType = EInventoryOrbType.Ice
            },
            new ProjectileTypeToItemMap
            {
                ProjectileType = TimeSpinnerType.Get($"{LunaisProjectiles}.IceOrbPassiveProjectile"),
                OrbSlot = EOrbSlot.Passive,
                OrbType = EInventoryOrbType.Ice
            },
            new ProjectileTypeToItemMap
            {
                ProjectileType = TimeSpinnerType.Get($"{LunaisProjectiles}.IronOrbMeleeDamageArea"),
                OrbSlot = EOrbSlot.Melee,
                OrbType = EInventoryOrbType.Iron
            },
            new ProjectileTypeToItemMap
            {
                ProjectileType = TimeSpinnerType.Get($"{LunaisProjectiles}.LunaGiantHammerProjectile"),
                OrbSlot = EOrbSlot.Spell,
                OrbType = EInventoryOrbType.Iron
            },
            new ProjectileTypeToItemMap
            {
                ProjectileType = TimeSpinnerType.Get($"{LunaisProjectiles}.LunaGiantSwordProjectile"),
                OrbSlot = EOrbSlot.Spell,
                OrbType = EInventoryOrbType.Blade
            },
            new ProjectileTypeToItemMap
            {
                ProjectileType = TimeSpinnerType.Get($"{LunaisProjectiles}.LunaisFireSpellDamageArea"),
                OrbSlot = EOrbSlot.Spell,
                OrbType = EInventoryOrbType.Flame
            },
            new ProjectileTypeToItemMap
            {
                ProjectileType = TimeSpinnerType.Get($"{LunaisProjectiles}.MoonOrbMeleeDamageArea"),
                OrbSlot = EOrbSlot.Melee,
                OrbType = EInventoryOrbType.Moon
            },
            new ProjectileTypeToItemMap
            {
                ProjectileType = TimeSpinnerType.Get($"{LunaisProjectiles}.NetherOrbMeleeDamageArea"),
                OrbSlot = EOrbSlot.Melee,
                OrbType = EInventoryOrbType.Nether
            },
            new ProjectileTypeToItemMap
            {
                ProjectileType = TimeSpinnerType.Get($"{LunaisProjectiles}.PinkOrbPlasmaLazer"),
                OrbSlot = EOrbSlot.Spell,
                OrbType = EInventoryOrbType.Pink
            },
            new ProjectileTypeToItemMap
            {
                ProjectileType = TimeSpinnerType.Get($"{LunaisProjectiles}.UmbraOrbMeleeDamageArea"),
                OrbSlot = EOrbSlot.Melee,
                OrbType = EInventoryOrbType.Umbra
            },
            new ProjectileTypeToItemMap
            {
                ProjectileType = TimeSpinnerType.Get($"{LunaisProjectiles}.UmbraOrbSpellProjectile"),
                OrbSlot = EOrbSlot.Spell,
                OrbType = EInventoryOrbType.Umbra
            },
            new ProjectileTypeToItemMap
            {
                ProjectileType = TimeSpinnerType.Get($"{LunaisProjectiles}.WindOrbMeleeDamageArea"),
                OrbSlot = EOrbSlot.Melee,
                OrbType = EInventoryOrbType.Wind
            },
            new ProjectileTypeToItemMap
            {
                ProjectileType = TimeSpinnerType.Get("Timespinner.GameObjects.Heroes.Passives.GreenOrbPassiveDamageArea"),
                OrbSlot = EOrbSlot.Passive,
                OrbType = EInventoryOrbType.Blade
            },
            new ProjectileTypeToItemMap
            {
                ProjectileType = TimeSpinnerType.Get($"{LunaisProjectiles2}.BarrierOrbMeleeDamageArea"),
                OrbSlot = EOrbSlot.Melee,
                OrbType = EInventoryOrbType.Barrier
            },
            new ProjectileTypeToItemMap
            {
                ProjectileType = TimeSpinnerType.Get($"{LunaisProjectiles2}.NetherOrbSpellDamageArea"),
                OrbSlot = EOrbSlot.Spell,
                OrbType = EInventoryOrbType.Nether
            },
            new ProjectileTypeToItemMap
            {
                ProjectileType = TimeSpinnerType.Get($"{SpellProjectiles}.BarrierOrbSpellDamageArea"),
                OrbSlot = EOrbSlot.Spell,
                OrbType = EInventoryOrbType.Barrier
            },
            new ProjectileTypeToItemMap
            {
                ProjectileType = TimeSpinnerType.Get($"{SpellProjectiles}.BloodOrbSpellDamageArea"),
                OrbSlot = EOrbSlot.Spell,
                OrbType = EInventoryOrbType.Blood
            },
            new ProjectileTypeToItemMap
            {
                ProjectileType = TimeSpinnerType.Get($"{SpellProjectiles}.EmpireOrbSpellDamageArea"),
                OrbSlot = EOrbSlot.Spell,
                OrbType = EInventoryOrbType.Empire
            },
            new ProjectileTypeToItemMap
            {
                ProjectileType = TimeSpinnerType.Get($"{SpellProjectiles}.EyeOrbSpellDamageArea"),
                OrbSlot = EOrbSlot.Spell,
                OrbType = EInventoryOrbType.Eye
            },
            new ProjectileTypeToItemMap
            {
                ProjectileType = TimeSpinnerType.Get($"{SpellProjectiles}.GunOrbSpellDamageArea"),
                OrbSlot = EOrbSlot.Spell,
                OrbType = EInventoryOrbType.Gun
            },
            new ProjectileTypeToItemMap
            {
                ProjectileType = TimeSpinnerType.Get($"{SpellProjectiles}.IceOrbSpellSpikeDamageArea"),
                OrbSlot = EOrbSlot.Spell,
                OrbType = EInventoryOrbType.Ice
            },
            new ProjectileTypeToItemMap
            {
                ProjectileType = TimeSpinnerType.Get($"{SpellProjectiles}.MoonOrbSpellDamageArea"),
                OrbSlot = EOrbSlot.Spell,
                OrbType = EInventoryOrbType.Moon
            },

            new ProjectileTypeToItemMap
            {
                ProjectileType = TimeSpinnerType.Get($"{SpellProjectiles}.WindOrbSpellDamageArea"),
                OrbSlot = EOrbSlot.Spell,
                OrbType = EInventoryOrbType.Wind
            }
        };

        public static SubProjectileTypeToItemMap[] DamagingSubProjectiles =
        {
            new SubProjectileTypeToItemMap
            {
                ProjectileType = TimeSpinnerType.Get($"{SpellProjectiles}.NetherOrbSpellProjectile"),
                OrbSlot = EOrbSlot.Spell,
                OrbType = EInventoryOrbType.Nether,
                ParentProjectile = DamagingProjectiles.SingleOrDefault(p => p.ProjectileType == TimeSpinnerType.Get($"{LunaisProjectiles2}.NetherOrbSpellDamageArea")),
                ShouldShareDamageType = false
            },
            new SubProjectileTypeToItemMap
            {
                ProjectileType = TimeSpinnerType.Get($"{SpellProjectiles}.GunOrbSpellProjectile"),
                OrbSlot = EOrbSlot.Spell,
                OrbType = EInventoryOrbType.Gun,
                ParentProjectile = DamagingProjectiles.SingleOrDefault(p => p.ProjectileType == TimeSpinnerType.Get($"{SpellProjectiles}.GunOrbSpellDamageArea")),
                ShouldShareDamageType = false
            },
            new SubProjectileTypeToItemMap
            {
                //also the melee Plasma Orb projectile
                ProjectileType = TimeSpinnerType.Get($"{LunaisProjectiles}.ThunderBoltDamageArea"),
                OrbSlot = EOrbSlot.Melee,
                OrbType = EInventoryOrbType.Pink,
                ParentProjectile = DamagingProjectiles.SingleOrDefault(p => p.ProjectileType == TimeSpinnerType.Get($"{LunaisProjectiles}.PinkOrbPlasmaLazer")),
                ShouldShareDamageType = false
            },
            new SubProjectileTypeToItemMap
            {
                ProjectileType = TimeSpinnerType.Get($"{SpellProjectiles}.IceOrbSpellSnowProjectile"),
                OrbSlot = EOrbSlot.Spell,
                OrbType = EInventoryOrbType.Ice,
                ParentProjectile = DamagingProjectiles.SingleOrDefault(p => p.ProjectileType == TimeSpinnerType.Get($"{SpellProjectiles}.IceOrbSpellSpikeDamageArea")),
                ShouldShareDamageType = true
            },
            new SubProjectileTypeToItemMap
            {
                ProjectileType = TimeSpinnerType.Get($"{LunaisProjectiles}.NetherOrbMeleeCounterDamageArea"),
                OrbSlot = EOrbSlot.Melee,
                OrbType = EInventoryOrbType.Nether,
                ParentProjectile = DamagingProjectiles.SingleOrDefault(p => p.ProjectileType == TimeSpinnerType.Get($"{LunaisProjectiles}.NetherOrbMeleeDamageArea")),
                ShouldShareDamageType = false
            },
            new SubProjectileTypeToItemMap
            {
                ProjectileType = TimeSpinnerType.Get($"{LunaisProjectiles}.IceOrbMeleeSnowProjectile"),
                OrbSlot = EOrbSlot.Melee,
                OrbType = EInventoryOrbType.Ice,
                ParentProjectile = DamagingProjectiles.SingleOrDefault(p => p.ProjectileType == TimeSpinnerType.Get($"{LunaisProjectiles}.IceOrbIcicleDamageArea")),
                ShouldShareDamageType = true
            },
            new SubProjectileTypeToItemMap
            {
                ProjectileType = TimeSpinnerType.Get($"{LunaisProjectiles}.BlueOrbSpellBulletSmall"),
                OrbSlot = EOrbSlot.Spell,
                OrbType = EInventoryOrbType.Blue,
                ParentProjectile = DamagingProjectiles.SingleOrDefault(p => p.ProjectileType == TimeSpinnerType.Get($"{LunaisProjectiles}.BlueOrbSpellBulletLarge")),
                ShouldShareDamageType = true
            },
            new SubProjectileTypeToItemMap
            {
                ProjectileType = TimeSpinnerType.Get($"{LunaisProjectiles}.BlueOrbSpellBulletMedium"),
                OrbSlot = EOrbSlot.Spell,
                OrbType = EInventoryOrbType.Blue,
                ParentProjectile = DamagingProjectiles.SingleOrDefault(p => p.ProjectileType == TimeSpinnerType.Get($"{LunaisProjectiles}.BlueOrbSpellBulletLarge")),
                ShouldShareDamageType = true
            },
        };
    }
}
