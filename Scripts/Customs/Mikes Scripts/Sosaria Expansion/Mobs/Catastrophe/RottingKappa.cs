using System;
using System.Collections;
using System.Collections.Generic; // Needed for List
using Server;
using Server.Items;
using Server.Network; // Needed for SendLocalizedMessage

namespace Server.Mobiles
{
    [CorpseName("a rotting kappa corpse")]
    public class RottingKappa : BaseCreature
    {
        private DateTime m_NextVileSpit; // Cooldown for Vile Spit
        private DateTime m_NextAuraPulse; // Cooldown for Rotting Aura pulse

        [Constructable]
        public RottingKappa()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.15, 0.3) // Slightly faster attack speed
        {
            Name = "a Rotting Kappa";
            Body = 240; // Kappa body
            Hue = 2212; // A sickly green/brown hue

            SetStr(450, 520);
            SetDex(110, 145);
            SetInt(85, 115);

            SetMana(250, 350);
            SetHits(680, 750);

            SetDamage(18, 26); // Higher base damage

            // High resistance to Poison/Cold, moderate Physical, lower Fire/Energy
            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Poison, 50);

            SetResistance(ResistanceType.Physical, 45, 55);
            SetResistance(ResistanceType.Fire, 25, 35);
            SetResistance(ResistanceType.Cold, 55, 65);
            SetResistance(ResistanceType.Poison, 70, 80);
            SetResistance(ResistanceType.Energy, 35, 45);

            // Higher combat skills + Poisoning
            SetSkill(SkillName.MagicResist, 90.1, 105.0);
            SetSkill(SkillName.Tactics, 95.1, 110.0);
            SetSkill(SkillName.Wrestling, 92.1, 108.0);
            SetSkill(SkillName.Poisoning, 85.1, 100.0); // Added Poisoning skill
            SetSkill(SkillName.Anatomy, 70.1, 85.0); // Added Anatomy

            Fame = 6500;
            Karma = -6500;

            VirtualArmor = 40;

            // Initialize ability cooldowns
            m_NextVileSpit = DateTime.UtcNow;
            m_NextAuraPulse = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomDouble() * 5.0); // Stagger initial pulse

            // Add thematic pack items
            PackItem(new Nightshade(Utility.RandomMinMax(5, 10)));
            PackItem(new Bone(Utility.RandomMinMax(3, 6)));
            PackItem(new RawFishSteak(Utility.RandomMinMax(2, 4))); // Still a kappa... sort of


            if (Core.ML && Utility.RandomDouble() < .50) // Higher chance for peculiar seeds
                PackItem(Engines.Plants.Seed.RandomPeculiarSeed(3));

            // Inherited special ability + new ones
            SetSpecialAbility(SpecialAbility.LifeLeech);
            SetWeaponAbility(WeaponAbility.MortalStrike); // Added a weapon ability
        }

        public RottingKappa(Serial serial)
            : base(serial)
        {
        }

        // --- Unique Abilities ---

        // 1. Vile Spit (Ranged Attack)
        public void VileSpit(Mobile target)
        {
            if (target == null || !target.Alive || !CanBeHarmful(target))
                return;

            this.MovingParticles(target, 0x36D4, 7, 0, false, true, 9502, 4019, 0x163); // Acid/Poison themed particles
            this.PlaySound(0x108); // Acid sizzle sound

            DoHarmful(target);

            // Delay for travel time
            Timer.DelayCall(TimeSpan.FromSeconds(1.0), () =>
            {
                if (target != null && target.Alive)
                {
                    target.PlaySound(0x108); // Sound on impact
                    // Apply Lethal Poison + Disease
                    target.ApplyPoison(this, Poison.Lethal);
                    target.SendLocalizedMessage(1070820); // You feel a vile substance consuming your skin! (Placeholder message)
                    target.SendAsciiMessage("You feel deathly ill!"); // Disease message

                    // Apply Disease effect (temporary stat reduction)
                    target.AddStatMod(new StatMod(StatType.Str, "KappaDiseaseStr", -Utility.RandomMinMax(5, 10), TimeSpan.FromSeconds(20)));
                    target.AddStatMod(new StatMod(StatType.Dex, "KappaDiseaseDex", -Utility.RandomMinMax(5, 10), TimeSpan.FromSeconds(20)));
                    target.AddStatMod(new StatMod(StatType.Int, "KappaDiseaseInt", -Utility.RandomMinMax(5, 10), TimeSpan.FromSeconds(20)));

                    // Direct damage component
                    AOS.Damage(target, this, Utility.RandomMinMax(15, 25), 0, 0, 0, 100, 0); // Pure Poison damage
                }
            });
        }

        // 2. Festering Touch (Melee Enhancement)
        public void DoFesteringTouch(Mobile defender)
        {
             if (defender == null || !defender.Alive || !CanBeHarmful(defender))
                return;

            defender.PlaySound(0x108); // Sizzle sound
            defender.SendAsciiMessage("The kappa's touch causes your wounds to fester!");

            // Apply a festering wound (Poison DoT) - Use Timer for DoT effect
            int festerTicks = Utility.RandomMinMax(3, 5); // Number of damage ticks
            int festerDamage = Utility.RandomMinMax(5, 8); // Damage per tick

            Timer festerTimer = new FesterTimer(defender, this, festerDamage, festerTicks);
            festerTimer.Start();

            // Drain Stamina
            defender.Stam -= Utility.RandomMinMax(15, 30);
        }

        private class FesterTimer : Timer
        {
            private Mobile m_Target;
            private Mobile m_Attacker;
            private int m_Damage;
            private int m_Ticks;

            public FesterTimer(Mobile target, Mobile attacker, int damage, int ticks) : base(TimeSpan.FromSeconds(2.0), TimeSpan.FromSeconds(2.0)) // Damage every 2 seconds
            {
                m_Target = target;
                m_Attacker = attacker;
                m_Damage = damage;
                m_Ticks = ticks;
                Priority = TimerPriority.TwoFiftyMS;
            }

            protected override void OnTick()
            {
                if (m_Target != null && m_Target.Alive && m_Ticks > 0)
                {
                    m_Target.PlaySound(0x108); // Sizzle sound
                    m_Target.FixedParticles(0x374A, 10, 15, 5033, EffectLayer.Waist); // Festering effect particles
                    AOS.Damage(m_Target, m_Attacker, m_Damage, 0, 0, 0, 100, 0); // Poison damage
                    m_Ticks--;
                }
                else
                {
                    Stop();
                }
            }
        }

        // 3. Rotting Aura (Periodic AoE Debuff)
        public void DoRottingAura()
        {
             if (Deleted || Map == null)
                return;

            List<Mobile> targets = new List<Mobile>();
            foreach (Mobile m in GetMobilesInRange(3)) // Range of 3 tiles
            {
                if (m != this && CanBeHarmful(m) && m.Alive && InLOS(m))
                    targets.Add(m);
            }

            if (targets.Count > 0)
            {
                PlaySound(0x108); // Aura pulse sound
                FixedParticles(0x374A, 10, 30, 5052, EffectLayer.Waist); // Visual for aura pulse

                foreach (Mobile m in targets)
                {
                     if (m is Mobile target) // Ensure it's a Mobile
                     {
                        DoHarmful(target);
                        // Apply a weaker disease effect or minor poison damage
                        target.SendAsciiMessage("The air around the kappa feels heavy and diseased!");
                        target.ApplyPoison(this, Poison.Regular); // Apply regular poison
                        // Optionally add a short, weaker stat debuff
                         target.AddStatMod(new StatMod(StatType.Dex, "AuraDiseaseDex", -5, TimeSpan.FromSeconds(10)));
                    }
                }
            }
        }

        // 4. Death Burst (On Death AoE)
        public override bool OnBeforeDeath()
        {
            DoDeathBurst();
            return base.OnBeforeDeath();
        }

        public void DoDeathBurst()
        {
             if (Map == null)
                return;

            // Create a cloud effect
            Effects.SendLocationParticles(
                EffectItem.Create(Location, Map, EffectItem.DefaultDuration),
                0x3709, 10, 30, 5052); // Poison cloud effect
            PlaySound(0x231); // Explosion/release sound

            List<Mobile> targets = new List<Mobile>();
            foreach (Mobile m in GetMobilesInRange(5)) // Range of 5 tiles
            {
                if (CanBeHarmful(m) && m.Alive && InLOS(m))
                    targets.Add(m);
            }

            foreach (Mobile m in targets)
            {
                 if (m is Mobile target) // Ensure it's a Mobile
                 {
                    DoHarmful(target);
                    target.SendAsciiMessage("The kappa explodes in a shower of filth!");
                    // Apply Greater Poison and damage
                    target.ApplyPoison(this, Poison.Greater);
                    AOS.Damage(target, this, Utility.RandomMinMax(20, 35), 0, 0, 0, 100, 0); // Poison damage
                 }
            }
        }

        // --- Combat Logic ---

        public override void OnActionCombat()
        {
            Mobile combatant = Combatant as Mobile;

            if (combatant == null || combatant.Deleted || !combatant.Alive || combatant.Map != Map || !InRange(combatant.Location, 12) || !CanBeHarmful(combatant))
            {
                base.OnActionCombat();
                return;
            }

            // Vile Spit Logic
			if (DateTime.UtcNow >= m_NextVileSpit && InRange(combatant, 8) && !InRange(combatant, 1) && InLOS(combatant))
            {
                // Check if combatant is a mobile before accessing mobile-specific properties
                if (combatant is Mobile target)
                {
                    if (target.Mana >= 15 && target.Poison == null) // Target players with mana/not poisoned? (Optional targeting logic)
                    {
                        m_NextVileSpit = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(8, 15)); // Cooldown 8-15 sec
                        VileSpit(target);
                    }
                    else if (Utility.RandomDouble() < 0.3) // 30% chance to spit anyway
                    {
                         m_NextVileSpit = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(8, 15));
                         VileSpit(target);
                    }
                }
            }

            // Rotting Aura Logic
            if (DateTime.UtcNow >= m_NextAuraPulse)
            {
                m_NextAuraPulse = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(5, 8)); // Pulse every 5-8 sec
                DoRottingAura();
            }

            base.OnActionCombat();
        }

        public override void OnGaveMeleeAttack(Mobile defender)
        {
            base.OnGaveMeleeAttack(defender);

            // Festering Touch Logic (30% chance on hit)
            if (0.3 > Utility.RandomDouble())
            {
                 // Ensure defender is Mobile before using Mobile-specific methods
                 if (defender is Mobile target)
                 {
                    DoFesteringTouch(target);
                 }
            }
        }

        // --- Loot & Overrides ---

        public override void GenerateLoot()
        {
            AddLoot(LootPack.FilthyRich, 1); // Good base loot
            AddLoot(LootPack.MedScrolls, 1); // Chance for scrolls
            AddLoot(LootPack.HighScrolls, Utility.RandomBool() ? 1 : 0); // Occasional high scrolls
            AddLoot(LootPack.Potions, 1); // Potions

            // Chance for a specific rare drop
            if (Utility.RandomDouble() < 0.005) // 0.5% chance
            {
                // Example: A thematic unique item
                // PackItem(new RottingKappaHideGloves()); // Need to define this item elsewhere
            }
            
            // Reuse the WatercallersKimono drop from the base Kappa, but rarer
            if (Utility.RandomDouble() < 0.0005) // 1 in 2000 chance
            {
                this.PackItem(new WatercallersKimono());
            }
        }

        public override bool CanRummageCorpses { get { return true; } }
        public override Poison PoisonImmune { get { return Poison.Lethal; } } // Immune to poison
        public override Poison HitPoison { get { return Poison.Deadly; } } // Poisons on melee hit

        public override int TreasureMapLevel { get { return 4; } } // Higher level treasure map
        public override int Meat { get { return 1; } } // Less edible meat

        // --- Sound Overrides (Copied from Kappa) ---
        public override int GetAngerSound() { return 0x50B; }
        public override int GetIdleSound() { return 0x50A; }
        public override int GetAttackSound() { return 0x509; }
        public override int GetHurtSound() { return 0x50C; }
        public override int GetDeathSound() { return 0x508; }

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

            // Reset cooldowns on load
            m_NextVileSpit = DateTime.UtcNow;
            m_NextAuraPulse = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomDouble() * 5.0);
        }
    }
}