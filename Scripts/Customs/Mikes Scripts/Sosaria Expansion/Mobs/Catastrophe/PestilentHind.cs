using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Targeting;
using Server.Mobiles; // Added for clarity

namespace Server.Mobiles
{
    [CorpseName("a diseased deer corpse")] // Changed corpse name
    public class PestilentHind : BaseCreature
    {
        private DateTime _NextDiseaseCloud;
        private DateTime _NextCorrosiveSpit;

        [Constructable]
        public PestilentHind()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.15, 0.3) // Slightly faster reaction times
        {
            Name = "a Pestilent Hind";
            Body = 0xED; // Hind body
            Hue = Utility.RandomList(0x8A5, 0x48F, 0x490, 0x495); // Sickly green/brown hues

            SetStr(180, 220);
            SetDex(90, 110);
            SetInt(110, 150); // Int for special abilities

            SetHits(350, 450);
            SetMana(200, 300); // Mana pool for abilities

            SetDamage(10, 18); // Increased base damage

            // Mostly physical damage, but with a poison component inherent in its attacks
            SetDamageType(ResistanceType.Physical, 70);
            SetDamageType(ResistanceType.Poison, 30);

            // High poison resist, decent physical, moderate others
            SetResistance(ResistanceType.Physical, 35, 45);
            SetResistance(ResistanceType.Fire, 20, 30);
            SetResistance(ResistanceType.Cold, 30, 40);
            SetResistance(ResistanceType.Poison, 70, 85); // Very high poison resist
            SetResistance(ResistanceType.Energy, 30, 40);

            // Strong combat skills + poisoning
            SetSkill(SkillName.MagicResist, 75.0, 90.0);
            SetSkill(SkillName.Tactics, 80.0, 95.0);
            SetSkill(SkillName.Wrestling, 85.0, 100.0);
            SetSkill(SkillName.Poisoning, 90.0, 100.0); // High poisoning skill
            SetSkill(SkillName.Anatomy, 60.0, 85.0);

            Fame = 6000; // Higher fame
            Karma = -6000; // Negative karma

            VirtualArmor = 40; // Increased virtual armor

            Tamable = false; // Cannot be tamed
            ControlSlots = 0; // No control slots needed

            _NextDiseaseCloud = DateTime.UtcNow;
            _NextCorrosiveSpit = DateTime.UtcNow;
        }

        public PestilentHind(Serial serial)
            : base(serial)
        {
        }

        // --- Base Hind Properties (Modified/Kept) ---
        public override int Meat { get { return 2; } } // Less edible meat
        public override int Hides { get { return 5; } } // Damaged hides
        public override FoodType FavoriteFood { get { return FoodType.None; } } // Doesn't eat normally

        public override bool BleedImmune { get { return true; } } // Immune to bleeding due to its nature
        public override Poison PoisonImmune { get { return Poison.Lethal; } } // Immune to poison

        // --- Sounds (Kept from Hind) ---
        public override int GetAttackSound() { return 0x82; }
        public override int GetHurtSound() { return 0x83; }
        public override int GetDeathSound() { return 0x84; }

        // --- Unique Abilities ---

        // Ability 1: Pestilent Touch (Melee Poison)
        public override void OnGaveMeleeAttack(Mobile defender)
        {
            base.OnGaveMeleeAttack(defender);

            // 40% chance on melee hit to apply strong poison
            if (0.4 > Utility.RandomDouble())
            {
                // *** Safety Check for Mobile ***
                if (defender is Mobile targetMobile)
                {
                    targetMobile.ApplyPoison(this, Poison.Lethal); // Apply Lethal poison
                    targetMobile.SendLocalizedMessage(1070746, this.Name); // You feel extremely ill!
                    targetMobile.PlaySound(0x22F); // Poison effect sound
                    this.PlaySound(0x1BF); // Ghoul sound for effect
                }
            }

            // Ability 2: Minor Debuff on Hit (Stamina Drain)
            // 20% chance on melee hit to drain stamina
            if (0.2 > Utility.RandomDouble())
            {
                 // *** Safety Check for Mobile ***
                if (defender is Mobile targetMobile)
                {
                    targetMobile.SendMessage("The creature's diseased touch saps your energy!");
                    targetMobile.Stam -= Utility.Random(15, 25); // Drain 15-25 Stamina
                }
            }
        }

        // Ability 3: Disease Cloud (AoE Damage/Debuff)
        // Ability 4: Corrosive Spit (Ranged Damage/Debuff)
        public override void OnActionCombat()
        {
            Mobile combatant = Combatant as Mobile;

            // *** Safety Check for Mobile ***
            if (combatant == null || combatant.Deleted || !combatant.Alive || !CanBeHarmful(combatant))
            {
                base.OnActionCombat(); // Fallback to base behavior if target isn't valid
                return;
            }

            // Check Disease Cloud cooldown and range (prefer closer targets for AoE)
            if (DateTime.UtcNow >= _NextDiseaseCloud && IsValidTarget(combatant) && InRange(combatant, 6) && InLOS(combatant))
            {
                ReleaseDiseaseCloud(combatant);
                _NextDiseaseCloud = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(18, 25)); // Cooldown 18-25 seconds
            }
            // Check Corrosive Spit cooldown and range (prefer farther targets for ranged)
            else if (DateTime.UtcNow >= _NextCorrosiveSpit && IsValidTarget(combatant) && InRange(combatant, 10) && !InRange(combatant, 2) && InLOS(combatant)) // Range 3-10
            {
                SpitCorrosiveGlob(combatant);
                _NextCorrosiveSpit = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 18)); // Cooldown 12-18 seconds
            }
            else
            {
                base.OnActionCombat(); // Default combat action if no special abilities are ready/valid
            }
        }

        // Helper to check if combatant is a valid mobile target
        private bool IsValidTarget(IDamageable target)
        {
            return target is Mobile && target.Map == this.Map && target.Alive;
        }

        // Implementation of Disease Cloud
        private void ReleaseDiseaseCloud(Mobile target)
        {
            this.PlaySound(0x103); // Ghoul sound or similar noxious sound
            this.Say("*Wheeze*"); // Flavor text

            Map map = this.Map;
            if (map == null)
                return;

            List<Mobile> targets = new List<Mobile>();
            foreach (Mobile m in this.GetMobilesInRange(4)) // Affect mobiles within 4 tiles
            {
                if (m != this && CanBeHarmful(m) && IsValidTarget(m))
                    targets.Add(m);
            }

            Effects.SendLocationParticles(
                EffectItem.Create(this.Location, map, EffectItem.DefaultDuration),
                0x3709, 10, 30, 5052); // Noxious cloud effect

            // Apply effects to targets in the cloud
            foreach (Mobile m in targets)
            {
                DoHarmful(m);

                // Apply poison damage over time effect
                AOS.Damage(m, this, Utility.RandomMinMax(15, 25), 0, 0, 0, 100, 0); // Pure Poison damage

                // Apply a temporary debuff (e.g., reduce Dexterity)
                m.AddStatMod(new StatMod(StatType.Dex, "DiseaseCloudDexDebuff", -Utility.RandomMinMax(10, 20), TimeSpan.FromSeconds(10)));
                m.SendMessage("You inhale the noxious fumes, feeling weak and clumsy!");

                // Chance to apply disease (if your server supports it) or a stronger poison
                if (0.5 > Utility.RandomDouble())
                {
                     m.ApplyPoison(this, Poison.Greater); // Apply Greater poison as part of the cloud
                }

                 m.PlaySound(0x231); // Cough sound
            }
        }

        // Implementation of Corrosive Spit
        private void SpitCorrosiveGlob(Mobile target)
        {
            this.Animate(AnimationType.Attack, 7); // Attack animation
            this.PlaySound(0x56); // Spit/Splash sound
            this.MovingParticles(target, 0x36D4, 7, 0, false, true, 0x8A5, 0, 9502, 6014, 0x11D, EffectLayer.Waist, 0); // Sickly green particle effect

            DoHarmful(target);

            // Delay for travel time
            Timer.DelayCall(TimeSpan.FromSeconds(0.8), () =>
            {
                if (target != null && target.Alive && target.Map == this.Map && GetDistanceToSqrt(target) <= 12) // Recheck range/validity
                {
                    target.PlaySound(0x1C); // Acid sizzle sound on impact

                    // Deal direct poison damage
                    int damage = Utility.RandomMinMax(20, 30);
                    AOS.Damage(target, this, damage, 0, 0, 0, 100, 0); // Pure Poison damage

                    // Chance to apply a mana drain effect
                    if (0.3 > Utility.RandomDouble())
                    {
                        target.Mana -= Utility.RandomMinMax(20, 35);
                        target.SendMessage("The corrosive spit burns and disrupts your concentration!");
                    }
                    // Apply poison
                     target.ApplyPoison(this, Poison.Deadly); // Apply Deadly poison
                }
            });
        }


        // --- Loot ---
        public override void GenerateLoot()
        {
            // Use richer loot packs appropriate for a tougher monster
            AddLoot(LootPack.Rich, 1); // Add 1 pull from Rich loot pack
            AddLoot(LootPack.Average, 2); // Add 2 pulls from Average loot pack
            AddLoot(LootPack.MedScrolls); // Chance for medium-level scrolls

            // Add thematic loot
            if (0.2 > Utility.RandomDouble()) // 20% chance
                PackItem(new Nightshade(Utility.RandomMinMax(3, 6)));
            if (0.1 > Utility.RandomDouble()) // 10% chance
                PackItem(new DeadlyPoisonPotion());
            if (0.05 > Utility.RandomDouble()) // 5% chance for a unique item (placeholder)
                 PackItem(new DaemonBone(10)); // Example: Pack some daemon bone as a rare drop

            // Remove default Hind loot if any was inherited implicitly
            // Example: Remove the SylvanWardenBoots if it came from base
        }

        // --- Serialization ---
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            // Initialize cooldowns on load to prevent instant firing
            _NextDiseaseCloud = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(5, 10));
             _NextCorrosiveSpit = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(5, 10));
        }
    }
}