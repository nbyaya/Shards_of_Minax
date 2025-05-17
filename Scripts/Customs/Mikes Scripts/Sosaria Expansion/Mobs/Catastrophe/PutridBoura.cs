using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Network; // Needed for PrivateOverheadMessage if used, though SendMessage is more common now.

namespace Server.Mobiles
{
    [CorpseName("a putrid boura corpse")]
    public class PutridBoura : BaseCreature
    {
        private DateTime m_NextVileSpewAllowed;
        private DateTime m_NextAbilityAllowed; // General cooldown for melee abilities

        // Custom Poison - Stronger than Deadly, maybe adds a DoT effect or stat drain
        public static Poison PutridPoison { get { return Poison.Lethal; } } // Using Lethal as a base, could create a truly custom Poison later

        [Constructable]
        public PutridBoura() : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.15, 0.3) // Slightly faster reaction times
        {
            Name = "a putrid boura";
            Body = 715; // Base Boura Body
            Hue = 2118; // A sickly, dark green hue

            SetStr(600, 750);
            SetDex(100, 120);
            SetInt(50, 75);

            SetHits(900, 1100);

            SetDamage(25, 35);

            // High physical and poison resistance, vulnerable to fire
            SetDamageType(ResistanceType.Physical, 70);
            SetDamageType(ResistanceType.Poison, 30);

            SetResistance(ResistanceType.Physical, 60, 70);
            SetResistance(ResistanceType.Fire, 20, 30); // Vulnerability
            SetResistance(ResistanceType.Cold, 40, 50);
            SetResistance(ResistanceType.Poison, 80, 90); // High resistance
            SetResistance(ResistanceType.Energy, 40, 50);

            SetSkill(SkillName.Anatomy, 90.0, 100.0);
            SetSkill(SkillName.MagicResist, 100.0, 110.0);
            SetSkill(SkillName.Tactics, 95.0, 105.0);
            SetSkill(SkillName.Wrestling, 100.0, 110.0);
            SetSkill(SkillName.Poisoning, 100.0, 120.0); // Important for its abilities

            Fame = 15000;
            Karma = -15000;

            VirtualArmor = 60;
            Tamable = false; // This is a monster

            // Give it a powerful melee special
            SetWeaponAbility(WeaponAbility.MortalStrike);

            m_NextVileSpewAllowed = DateTime.UtcNow;
            m_NextAbilityAllowed = DateTime.UtcNow;
        }

        public PutridBoura(Serial serial) : base(serial)
        {
        }

        // --- Resource Drops (Optional - adjust as needed) ---
        public override int Meat { get { return 2; } } // Less edible meat
        public override int Hides { get { return 10; } } // Maybe damaged hides
        public override HideType HideType { get { return HideType.Barbed; } } // Different hide type
        // Removing Fur/DragonBlood unless it makes sense lore-wise

        // --- Boura Sounds ---
        public override int GetIdleSound() { return 1507; }
        public override int GetAngerSound() { return 1504; }
        public override int GetHurtSound() { return 1506; }
        public override int GetDeathSound() { return 1505; }

        // --- Abilities ---

        public override void OnActionCombat()
        {
            // Use IDamageable for Combatant
            IDamageable combatant = Combatant;

            // Basic null checks and range checks
            if (combatant == null || combatant.Deleted || !combatant.Alive || !InRange(combatant.Location, 12) || !CanBeHarmful(combatant) || !InLOS(combatant))
            {
                base.OnActionCombat(); // Ensure base combat logic still runs
                return;
            }

            // Vile Spew Logic
            if (DateTime.UtcNow >= m_NextVileSpewAllowed)
            {
                // Check if target is far enough away for a ranged attack
                if (GetDistanceToSqrt(combatant.Location) >= 4)
                {
                    // Check if the combatant is a Mobile before accessing Mobile properties
                    if (combatant is Mobile target)
                    {
                       VileSpew(target);
                       m_NextVileSpewAllowed = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15)); // Cooldown: 10-15 seconds
                    }
                    else // Handle non-mobile IDamageable targets if necessary (e.g., structures)
                    {
                       // Maybe a simpler effect or skip if spew only works on mobiles
                       m_NextVileSpewAllowed = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
                    }
                }
                else // Too close for spew, reset cooldown slightly
                {
                    m_NextVileSpewAllowed = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(3, 5));
                }
            }
            
            base.OnActionCombat(); // Ensure base combat logic still runs AFTER ability checks
        }


        public override void OnGaveMeleeAttack(Mobile defender)
        {
            base.OnGaveMeleeAttack(defender);

            // Check general ability cooldown
            if (DateTime.UtcNow < m_NextAbilityAllowed)
                return;

            // Check if the defender is valid (redundant safety check)
            if (defender == null || defender.Deleted || !defender.Alive)
                 return;

            double chance = 0.20; // 20% chance for either special melee effect per swing

            if (Utility.RandomDouble() < chance)
            {
                // Choose one of the melee abilities randomly
                if (Utility.RandomBool())
                {
                    // Corrosive Spittle
                    DoCorrosiveSpittle(defender);
                    m_NextAbilityAllowed = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(5, 8)); // Cooldown 5-8 seconds
                }
                else
                {
                    // Putrid Touch (Apply Poison)
                    DoPutridTouch(defender);
                    m_NextAbilityAllowed = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(5, 8)); // Cooldown 5-8 seconds
                }
            }
        }

        // Ability Implementation: Vile Spew (Ranged Poison)
        public void VileSpew(Mobile target)
        {
            this.MovingParticles(target, 0x36D4, 7, 0, false, true, 9502, 4019, 0x160); // Acid/poison particle effect
            this.PlaySound(0x108); // Poison spell sound

            DoHarmful(target);

            Timer.DelayCall(TimeSpan.FromSeconds(1.0), () =>
            {
                if (target != null && target.Alive && target.Map == this.Map && target.InRange(this.Location, 14)) // Check target validity again
                {
                    target.PlaySound(0x108); // Sound on impact

                    // Damage
                    int damage = Utility.RandomMinMax(20, 30);
                    AOS.Damage(target, this, damage, 0, 0, 0, 100, 0); // 100% Poison damage

                    // Apply Poison
                    target.ApplyPoison(this, PutridPoison);
                    target.SendLocalizedMessage(1070750); // You feel deathly sick!
                }
            });
        }

        // Ability Implementation: Corrosive Spittle (Melee Phys Resist Debuff)
        public void DoCorrosiveSpittle(Mobile target)
        {
            DoHarmful(target);
            PlaySound(0x108); // Reuse poison sound
            target.PlaySound(0x108);

            target.SendMessage(0x22, "The boura's corrosive spittle burns through your defenses!"); // Red message

            int reduction = Utility.RandomMinMax(10, 20); // Reduce Phys Resist by 10-20
            TimeSpan duration = TimeSpan.FromSeconds(Utility.RandomMinMax(8, 12)); // Duration 8-12 seconds

            // Apply the resistance mod
            ResistanceMod mod = new ResistanceMod(ResistanceType.Physical, -reduction);
            target.AddResistanceMod(mod);

            // Timer to remove the mod
            Timer.DelayCall(duration, () =>
            {
                 if (target != null && !target.Deleted)
                 {
                    target.RemoveResistanceMod(mod);
                    target.SendMessage(0x35, "The corrosive effect wears off."); // Green message
                 }
            });
        }

        // Ability Implementation: Putrid Touch (Melee Poison Application)
        public void DoPutridTouch(Mobile target)
        {
             DoHarmful(target);
             PlaySound(0x108); // Reuse poison sound
             target.PlaySound(0x108);

             target.SendMessage(0x22, "The boura's putrid touch infects you!"); // Red message
             target.ApplyPoison(this, PutridPoison);
        }

        // Ability Implementation: Death Burst (On Death AoE)
        public override void OnDeath(Container c)
        {
            // Check if controlled - shouldn't happen since Tamable=false, but good practice
            if (Controlled)
            {
                base.OnDeath(c);
                return;
            }

            // Death Burst Effect
            Map map = this.Map;
            Point3D loc = this.Location;

            if (map != null)
            {
                // Visual Effect
                Effects.SendLocationParticles(EffectItem.Create(loc, map, EffectItem.DefaultDuration), 0x3709, 10, 30, 5052); // Larger poison field effect
                Effects.PlaySound(loc, map, 0x231); // Poison explosion sound

                // Find targets in range
                List<Mobile> targets = new List<Mobile>();
                foreach (Mobile m in GetMobilesInRange(5)) // Affect targets within 5 tiles
                {
                     if (m != null && m != this && m.Alive && CanBeHarmful(m))
                         targets.Add(m);
                }

                // Apply effects to targets
                foreach (Mobile target in targets)
                {
                    DoHarmful(target);

                    // Damage
                    int damage = Utility.RandomMinMax(30, 50);
                    AOS.Damage(target, this, damage, 0, 0, 0, 100, 0); // 100% Poison

                    // Apply Poison
                    target.ApplyPoison(this, PutridPoison);
                    target.SendLocalizedMessage(1070750); // You feel deathly sick! (or custom message)
                    target.SendMessage(0x22, "You are caught in the boura's final putrid eruption!");
                }
            }

            // Drop custom loot? Example:
            if (Utility.RandomDouble() < 0.1) // 10% chance
                c.DropItem(new GrossHeart()); // Example custom item

            base.OnDeath(c); // Ensure base OnDeath runs (handles corpse creation, loot gen, etc.)
        }

        // --- Loot ---
        public override void GenerateLoot()
        {
            AddLoot(LootPack.FilthyRich, 2); // Good general loot
            AddLoot(LootPack.Average);
            AddLoot(LootPack.MedScrolls);

            if (Utility.RandomDouble() < 0.05) // 5% chance for a rare reagent/item
                 PackItem(new NoxCrystal(Utility.RandomMinMax(1,3))); // Example rare reagent
        }

        // --- Serialization ---
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version

            // No custom fields need saving currently beyond what base handles
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            // Reset cooldowns on load
            m_NextVileSpewAllowed = DateTime.UtcNow;
            m_NextAbilityAllowed = DateTime.UtcNow;
        }
    }

    // --- Example Custom Item/Reagent ---
    // You would need to create these item files separately

    public class GrossHeart : Item
    {
        [Constructable]
        public GrossHeart() : base(0x1CED) // Using a generic heart graphic
        {
            Name = "Putrid Boura Heart";
            Hue = 2118; // Match the boura's hue
            Weight = 1.0;
        }

        public GrossHeart(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer) { base.Serialize(writer); writer.Write((int)0); }
        public override void Deserialize(GenericReader reader) { base.Deserialize(reader); int version = reader.ReadInt(); }
    }

    // Example Nox Crystal - you would define its use elsewhere (Alchemy, etc.)
    // public class NoxCrystal : BaseReagent { ... }

}