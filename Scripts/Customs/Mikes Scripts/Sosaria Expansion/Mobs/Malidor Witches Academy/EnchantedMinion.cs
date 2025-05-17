using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Spells; // Needed for spell effects potentially
using Server.Network; // Needed for effects
using System.Collections.Generic; // Needed for lists in AoE targeting
using Server.Spells.First; // For Magic Arrow graphic/sound maybe
using Server.Spells.Third; // For Teleport effect/sound

namespace Server.Mobiles
{
    [CorpseName("an enchanted husk")] // Fitting corpse name
    public class EnchantedMinion : BaseCreature
    {
        // Timers for special abilities
        private DateTime m_NextManaDrainTime;
        private DateTime m_NextArcaneBurstTime;
        private DateTime m_NextPolymorphTime;
        private DateTime m_NextTeleportTime;

        // Unique Hue - Example: 1151 is a vibrant purple. Adjust as desired.
        private const int UniqueHue = 1151;

        [Constructable]
        public EnchantedMinion() : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.15, 0.3) // Slightly faster reaction
        {
            Name = "an Enchanted Minion";
            Body = 776; // Horde Minion Body
            BaseSoundID = 357; // Horde Minion Base Sound
            Hue = UniqueHue;

            // --- Significantly Boosted Stats ---
            SetStr(180, 220); // Tougher than base minion
            SetDex(130, 170); // More agile
            SetInt(380, 450); // High Intelligence for magic focus

            SetHits(900, 1200); // Much higher health pool
            SetStam(130, 170); // Increased stamina
            SetMana(1000, 1500); // Large mana pool for abilities

            SetDamage(18, 24); // Higher base damage

            // Mix of Physical and Energy damage
            SetDamageType(ResistanceType.Physical, 30);
            SetDamageType(ResistanceType.Energy, 70);

            // --- Adjusted Resistances (Magic Focused) ---
            SetResistance(ResistanceType.Physical, 50, 60);
            SetResistance(ResistanceType.Fire, 40, 50);
            SetResistance(ResistanceType.Cold, 40, 50);
            SetResistance(ResistanceType.Poison, 40, 50);
            SetResistance(ResistanceType.Energy, 75, 85); // Strong against Energy

            // --- Enhanced Skills (Magic Focused) ---
            SetSkill(SkillName.EvalInt, 105.1, 120.0);
            SetSkill(SkillName.Magery, 105.1, 120.0);
            SetSkill(SkillName.MagicResist, 115.2, 130.0); // Very high resist
            SetSkill(SkillName.Tactics, 80.1, 95.0);
            SetSkill(SkillName.Wrestling, 85.1, 100.0);
            SetSkill(SkillName.Meditation, 100.0, 110.0); // Added for mana regen flavor

            Fame = 15000;
            Karma = -15000;

            VirtualArmor = 65; // Higher passive defense
            ControlSlots = 4; // Difficult to control

            // Initialize ability cooldowns
            m_NextManaDrainTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(8, 15));
            m_NextArcaneBurstTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 18));
            m_NextPolymorphTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 25));
            m_NextTeleportTime = DateTime.UtcNow; // Can teleport reactively sooner

            // Base Horde Minion Loot + Magic Reagents
            AddItem(new LightSource()); // Keep the light source
            PackItem(new Bone(3)); // Keep the bones
            PackItem(new BlackPearl(Utility.RandomMinMax(3, 5)));
            PackItem(new Nightshade(Utility.RandomMinMax(3, 5)));
            PackItem(new SpidersSilk(Utility.RandomMinMax(3, 5)));
        }

        // --- Use Base Horde Minion Sounds ---
        public override int GetIdleSound() { return 338; }
        public override int GetAngerSound() { return 338; }
        public override int GetDeathSound() { return 338; }
        public override int GetAttackSound() { return 406; }
        public override int GetHurtSound() { return 194; }

        // --- Thinking Process for Special Attacks ---
        public override void OnThink()
        {
            base.OnThink();

            if (Combatant == null || Map == null || Map == Map.Internal || !Alive)
                return;

            // Check distance and cooldowns for abilities
            if (DateTime.UtcNow >= m_NextManaDrainTime && this.InRange(Combatant.Location, 8))
            {
                ManaDrainAura();
                m_NextManaDrainTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 25)); // Cooldown
            }
            else if (DateTime.UtcNow >= m_NextArcaneBurstTime && this.InRange(Combatant.Location, 10))
            {
                ArcaneBurstAttack();
                m_NextArcaneBurstTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 20)); // Cooldown
            }
            else if (DateTime.UtcNow >= m_NextPolymorphTime && this.InRange(Combatant.Location, 7))
            {
                 // Attempt polymorph only on Mobile targets
                if (Combatant is Mobile mobTarget)
                {
                     PolymorphJinx(mobTarget);
                     m_NextPolymorphTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 35)); // Longer cooldown for disruptive effect
                }
            }
        }



        // --- Unique Ability: Mana Drain Aura ---
        public void ManaDrainAura()
        {
            if (Map == null || !Alive) return;

            this.PlaySound(0x1F8); // Mana Drain sound
            // Subtle swirling purple particles around the minion
            Effects.SendLocationParticles(EffectItem.Create(this.Location, this.Map, EffectItem.DefaultDuration), 0x376A, 10, 20, UniqueHue, 0, 5029, 0);

            List<Mobile> targets = new List<Mobile>();
            IPooledEnumerable eable = Map.GetMobilesInRange(this.Location, 6); // 6 tile radius AoE

            foreach (Mobile m in eable)
            {
                // Check if the mobile can be harmed, is not the caster, and is a player or controllable pet
                if (m != this && CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(this, m) && (m.Player || (m is BaseCreature bc && bc.Controlled)))
                {
                    targets.Add(m);
                }
            }
            eable.Free();

            if (targets.Count > 0)
            {
                this.Say("*Your magic weakens!*"); // Flavor text

                foreach (Mobile target in targets)
                {
                    DoHarmful(target);
                    int manaDrained = Utility.RandomMinMax(25, 40);

                    // Check target is Mobile before accessing Mana
                    if (target is Mobile mobileTarget)
                    {
                        if (mobileTarget.Mana >= manaDrained)
                        {
                            mobileTarget.Mana -= manaDrained;
                            // Optional: Give some mana back to the caster?
                            // this.Mana = Math.Min(this.ManaMax, this.Mana + manaDrained / 2);
                        }
                        else
                        {
                            // Drain remaining mana and maybe deal small direct damage?
                             int remainingMana = mobileTarget.Mana;
                             mobileTarget.Mana = 0;
                             // AOS.Damage(target, this, (manaDrained - remainingMana) / 2, 0, 0, 0, 0, 100); // Damage for the difference? Optional.

                        }
                        // Visual effect on the target
                        target.FixedParticles(0x374A, 10, 15, 5032, UniqueHue, 0, EffectLayer.Head); // Purple hit/drain effect
                        target.SendMessage("You feel your magical energy being siphoned away!");
                    }
                }
            }
        }

        // --- Unique Ability: Arcane Burst ---
        public void ArcaneBurstAttack()
        {
            if (Combatant == null || Map == null || !Alive) return;

            // Ensure combatant is a Mobile before casting
            if (Combatant is Mobile target)
            {
                this.Say("*Feel the unstable magic!*");
                this.PlaySound(0x211); // Energy Bolt sound
                // Launch a particle effect towards the target
                this.MovingParticles(target, 0x3818, 7, 0, false, true, UniqueHue, 0, 9502, 4019, 0x160, 0); // Sparkle trail

                DoHarmful(target);
                int damage = Utility.RandomMinMax(50, 75); // High single-target damage
                // Deal 100% energy damage
                AOS.Damage(target, this, damage, 0, 0, 0, 0, 100);

                // Impact effect
                target.FixedParticles(0x3709, 10, 30, 5016, UniqueHue, 0, EffectLayer.CenterFeet); // Energy explosion effect
            }
        }

        // --- Unique Ability: Polymorph Jinx ---
        public void PolymorphJinx(Mobile target)
        {
             if (target == null || !target.Alive || !CanBeHarmful(target) || Map == null || !Alive)
                 return;

            // Check if the target is already polymorphed or immune
            if (target.BodyMod != 0 || target.BodyValue == 0 || !target.CanBeginAction(typeof(Spells.Seventh.PolymorphSpell))) // Basic check
            {
                 // Maybe do something else if polymorph fails? Small damage?
                 // AOS.Damage(target, this, Utility.RandomMinMax(10, 15), 0, 0, 0, 0, 100);
                return;
            }

            DoHarmful(target);
            this.Say("*Squeak!*");
            target.PlaySound(0x2F4); // Polymorph sound effect
            target.FixedParticles(0x376A, 9, 32, 5030, EffectLayer.Waist); // Polymorph graphic effect

            // Turn target into a rabbit (or chicken, etc.)
            int polymorphBody = 205; // Rabbit
            TimeSpan duration = TimeSpan.FromSeconds(Utility.RandomMinMax(6, 10));

            // Apply Polymorph - This requires a proper polymorph system or manual BodyMod
            // Simple version using BodyMod:
            target.BodyMod = polymorphBody;
            target.SendMessage("You've been turned into a helpless creature!");

             // Timer to end the polymorph
            Timer.DelayCall(duration, () =>
            {
                 // Check if target still exists and is still polymorphed by this effect
                 if (target != null && !target.Deleted && target.BodyMod == polymorphBody)
                 {
                    target.BodyMod = 0; // Revert body
                    target.SendMessage("The jinx wears off.");
                    target.FixedParticles(0x376A, 9, 32, 5030, EffectLayer.Waist); // Revert effect
                 }
            });
        }

         // --- Ability: Reactive Teleport ---
        public void ReactiveTeleport(Mobile attacker)
        {
             if (Map == null || !Alive) return;

             Point3D currentLocation = this.Location;
             Point3D newLocation = currentLocation;

             // Try to find a suitable teleport spot away from the attacker
             for (int i = 0; i < 10; ++i) // Try 10 times
             {
                 int x = X + Utility.RandomMinMax(-6, 6); // Teleport up to 6 tiles away
                 int y = Y + Utility.RandomMinMax(-6, 6);
                 int z = Z;

                 Point3D checkPoint = new Point3D(x, y, z);

                 if (Map.CanSpawnMobile(checkPoint) && !SpellHelper.CheckMulti(checkPoint, Map) && !this.InRange(attacker.Location, 2)) // Check if valid and not too close to attacker
                 {
                    // Attempt to adjust Z coordinate
                    if (!Map.CanFit(x, y, z, 16, false, false))
                    {
                        z = Map.GetAverageZ(x, y);
                        if (Map.CanFit(x, y, z, 16, false, false))
                        {
                            newLocation = new Point3D(x, y, z);
                            break;
                        }
                    }
                    else
                    {
                         newLocation = checkPoint;
                         break;
                    }
                 }
             }

             // If we found a new location different from the current one
             if (newLocation != currentLocation)
             {
                 this.PlaySound(0x1FE); // Teleport sound
                 // Teleport effects at start and end locations
                 Effects.SendLocationParticles(EffectItem.Create(currentLocation, Map, EffectItem.DefaultDuration), 0x372A, 10, 10, UniqueHue, 0, 5037, 0);
                 this.Location = newLocation;
                 Effects.SendLocationParticles(EffectItem.Create(newLocation, Map, EffectItem.DefaultDuration), 0x372A, 10, 10, UniqueHue, 0, 5037, 0);
                 m_NextTeleportTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(8, 15)); // Reset cooldown after successful teleport
             }
        }


        // --- Death Effect: Mana Vortex ---
        public override void OnDeath(Container c)
        {
            if (Map == null)
            {
                base.OnDeath(c);
                return;
            }

            this.PlaySound(0x28F); // Ethereal sound or a vortex sound
            // Large swirling purple/energy effect
            Effects.SendLocationParticles(EffectItem.Create(this.Location, this.Map, TimeSpan.FromSeconds(1.5)), 0x3709, 10, 60, UniqueHue, 0, 5052, 0); // Big central explosion effect
            Effects.SendLocationParticles(EffectItem.Create(this.Location, this.Map, TimeSpan.FromSeconds(2.0)), 0x376A, 10, 80, UniqueHue, 0, 5037, 0); // Swirling effect


            List<Mobile> targets = new List<Mobile>();
            IPooledEnumerable eable = Map.GetMobilesInRange(this.Location, 5); // 5 tile radius for mana drain

            foreach (Mobile m in eable)
            {
                 // Target players and controlled pets
                if (m != this && CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(this, m) && (m.Player || (m is BaseCreature bc && bc.Controlled)))
                {
                    targets.Add(m);
                }
            }
            eable.Free();

             if (targets.Count > 0)
             {
                 foreach (Mobile target in targets)
                 {
                     // Check if target is Mobile before accessing Mana
                     if (target is Mobile mobileTarget)
                     {
                         int manaDrained = Utility.RandomMinMax(50, 100); // Significant mana drain on death
                         int currentMana = mobileTarget.Mana;
                         mobileTarget.Mana = Math.Max(0, currentMana - manaDrained);

                         mobileTarget.SendMessage(0x22, "The minion's destruction violently rips away your mana!"); // Red warning message
                         // Visual effect on drained targets
                         mobileTarget.FixedParticles(0x374A, 10, 25, 5032, UniqueHue, 0, EffectLayer.Head);
                     }
                 }
             }

            base.OnDeath(c);
        }

        // --- Standard Properties & Loot ---
        public override bool BleedImmune { get { return true; } } // Likely magical, no blood
        public override int TreasureMapLevel { get { return 4; } } // High-level map

        // Increased Dispel difficulty
        public override double DispelDifficulty { get { return 125.0; } }
        public override double DispelFocus { get { return 50.0; } }

        public override void GenerateLoot()
        {
            // Keep the base Horde Minion mask drop logic EXACTLY as provided
            if (Utility.RandomDouble() < 0.001) // 1 in 1000 chance
            {
                this.PackItem(new HordeMastersMask());
            }

            // Add standard loot packs
            AddLoot(LootPack.FilthyRich, 1); // Good base loot
            AddLoot(LootPack.Rich, 1);
            AddLoot(LootPack.MedScrolls, 2); // Good chance for scrolls
            AddLoot(LootPack.Gems, Utility.RandomMinMax(4, 7)); // More gems

            // Chance for a unique magic-themed drop (Optional - replace or add to mask)
             if (Utility.RandomDouble() < 0.015) // 1.5% chance
             {
                 // Example: A reagent usable for high-end crafting or a decorative item
                 PackItem(new PulsatingMinionCore()); // Custom item - needs definition elsewhere
             }
        }

        // --- Serialization ---
        public EnchantedMinion(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            // Re-initialize cooldowns on load/restart to prevent instant ability use
            m_NextManaDrainTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(8, 15));
            m_NextArcaneBurstTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 18));
            m_NextPolymorphTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 25));
            m_NextTeleportTime = DateTime.UtcNow; // Can teleport reactively sooner
        }
    }

    // Define the example unique drop item (optional placeholder)
    public class PulsatingMinionCore : Item
    {
        [Constructable]
        public PulsatingMinionCore() : base(0x1F1C) // Example: Crystal Ball graphic
        {
            Name = "Pulsating Minion Core";
            Hue = 1151; // Match minion hue
            Weight = 1.0;
            LootType = LootType.Regular; // Or Blessed/Newbied if desired
        }

        public PulsatingMinionCore(Serial serial) : base(serial) { }
        public override void Serialize(GenericWriter writer) { base.Serialize(writer); writer.Write((int)0); }
        public override void Deserialize(GenericReader reader) { base.Deserialize(reader); int version = reader.ReadInt(); }
    }
}