using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Spells;
using Server.Spells.Necromancy; // Needed for Curse Weapon effect idea & Necro spells potentially
using Server.Spells.Second; // Needed for Curse Spell
using Server.Network;
using System.Collections.Generic;
using Server.Targeting; // Needed for custom targeting if used

namespace Server.Mobiles
{
    [CorpseName("a witchbound mummy corpse")] // Updated corpse name
    public class WitchboundMummy : BaseCreature
    {
        // Timers for special abilities
        private DateTime m_NextSpectralGraspTime;
        private DateTime m_NextNecroticPulseTime;
        private DateTime m_NextSummonTime;
        private DateTime m_NextAuraPulseTime; // Timer for aura effect application

        // Unique Hue - Example: 1266 is a deep, spectral purple.
        private const int UniqueHue = 1266;
        private const int EffectHue = 1266; // Can be same or different

        [Constructable]
        public WitchboundMummy() : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4) // Standard reaction speed
        {
            Name = "a Witchbound Mummy";
            Body = 154; // Mummy Body
            BaseSoundID = 471; // Mummy Sound
            Hue = UniqueHue;

            // --- Significantly Boosted Stats - Magic/Melee Hybrid ---
            SetStr(550, 650);    // High strength for melee
            SetDex(150, 200);    // Moderate dexterity
            SetInt(450, 550);    // High Intelligence for abilities/mana

            SetHits(2200, 2500); // Very High survivability for dungeon boss
            SetStam(200, 250);   // Good stamina pool
            SetMana(800, 1000);  // Large mana pool for abilities

            SetDamage(18, 26);   // Strong physical damage

            // Damage types: Physical and Cold base, abilities add more types
            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Cold, 50);

            // --- Adjusted Resistances - Strong vs Negative Energies ---
            SetResistance(ResistanceType.Physical, 55, 65);
            SetResistance(ResistanceType.Fire, 30, 40); // Slight weakness to fire
            SetResistance(ResistanceType.Cold, 70, 80);
            SetResistance(ResistanceType.Poison, 70, 80);
            SetResistance(ResistanceType.Energy, 50, 60); // Average energy resist

            // --- Enhanced Skills - Focus on Magic & Combat ---
            SetSkill(SkillName.EvalInt, 100.1, 115.0);
            SetSkill(SkillName.Magery, 100.1, 115.0); // Can cast some base spells too if desired
            SetSkill(SkillName.MagicResist, 110.2, 125.0); // High resist skill
            SetSkill(SkillName.Meditation, 90.0, 100.0); // Good mana regen
            SetSkill(SkillName.Tactics, 100.1, 110.0);
            SetSkill(SkillName.Wrestling, 100.1, 110.0);

            Fame = 18000; // High fame boss
            Karma = -18000; // Very evil

            VirtualArmor = 70; // High passive defense
            ControlSlots = 4; // Challenging creature, slightly below top-tier bosses

            // Initialize ability cooldowns
            m_NextSpectralGraspTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 18));
            m_NextNecroticPulseTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(8, 14));
            m_NextSummonTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 35));
            m_NextAuraPulseTime = DateTime.UtcNow + TimeSpan.FromSeconds(3); // Aura pulses every 3 seconds
        }

        // --- Unique Ability: Cursing Aura (Stamina Drain) ---
        // Passive effect on nearby mobiles
        public override void OnThink()
        {
            base.OnThink();

            // Aura Check
            if (DateTime.UtcNow >= m_NextAuraPulseTime && this.Alive && !this.Deleted && this.Map != null && this.Map != Map.Internal)
            {
                ApplyCursingAura();
                m_NextAuraPulseTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(3, 5)); // Pulse every 3-5 seconds
            }

            // --- Ability Checks ---
            if (Combatant == null || Map == null || Map == Map.Internal || !Alive)
                return;

            // Prioritize abilities based on cooldown and combat situation
            if (DateTime.UtcNow >= m_NextSummonTime)
            {
                // Only summon if not too many enemies already engaged or if health is lower
				// OLD (Line 99 - broken):
				// if (GetMobilesInRange(10).Count < 5 || Hits < (HitsMax * 0.5))

				// NEW (fixes .Count issue):
				int nearbyCount = 0;
				IPooledEnumerable nearby = GetMobilesInRange(10);
				foreach (Mobile m in nearby)
				{
					if (m != this && CanBeHarmful(m, false))
						nearbyCount++;
				}
				nearby.Free();

				if (nearbyCount < 5 || Hits < (HitsMax * 0.5))                
				{
                    DoSummonExperiment();
                    m_NextSummonTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(30, 40));
                }
            }
             else if (DateTime.UtcNow >= m_NextSpectralGraspTime && this.InRange(Combatant.Location, 8)) // Use grasp at mid-range
            {
                 DoSpectralGrasp();
                 m_NextSpectralGraspTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(18, 25));
            }
            else if (DateTime.UtcNow >= m_NextNecroticPulseTime && this.InRange(Combatant.Location, 10)) // Pulse is longer range
            {
                DoNecroticPulse();
                m_NextNecroticPulseTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 18));
            }
        }

        public void ApplyCursingAura()
        {
            List<Mobile> targets = new List<Mobile>();
            IPooledEnumerable eable = this.Map.GetMobilesInRange(this.Location, 3); // 3 tile radius aura

            foreach (Mobile m in eable)
            {
                if (m != this && CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(this, m))
                {
                   targets.Add(m);
                }
            }
            eable.Free();

            if (targets.Count > 0)
            {
                 // Subtle visual effect for the aura pulse on self
                 this.FixedParticles(0x375A, 10, 15, 5037, UniqueHue, 0, EffectLayer.Waist);

                foreach (Mobile target in targets)
                {
                    // Check if target is Mobile before accessing Stamina
                    if (target is Mobile targetMobile)
                    {
                         // Stamina Drain Effect
                        int stamDrained = Utility.RandomMinMax(8, 15);
                        if (targetMobile.Stam >= stamDrained)
                        {
                            targetMobile.Stam -= stamDrained;
                            targetMobile.SendMessage(0x22, "An unnatural chill drains your Vigor!"); // Feedback message
                            targetMobile.FixedParticles(0x374A, 10, 15, 5032, EffectHue, 0, EffectLayer.Waist); // Drain effect
                            // PlaySound(0x1F8); // Maybe too noisy for an aura
                        }
                    }
                }
            }
        }

        // --- Unique Ability: Spectral Grasp (AoE Snare/Damage) ---
        public void DoSpectralGrasp()
        {
            if (Map == null) return;

            this.Say("*Your bindings are mine!*");
            this.PlaySound(0x1EE); // Zombie or skeleton sound? Or a spectral whoosh: 0x244
            this.Animate(AnimationType.Attack, 0); // Basic attack animation

            List<Mobile> targets = new List<Mobile>();
            IPooledEnumerable eable = Map.GetMobilesInRange(this.Location, 6); // 6 tile radius AoE

            foreach (Mobile m in eable)
            {
                if (m != this && CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(this, m) && InLOS(m))
                {
                    targets.Add(m);
                }
            }
            eable.Free();

            if (targets.Count > 0)
            {
                // Visual effect: Spectral wrappings flying out
                Effects.SendLocationParticles(EffectItem.Create(this.Location, this.Map, EffectItem.DefaultDuration), 0x374A, 10, 40, UniqueHue, 0, 5021, 0); // Wraith form particles?

                foreach (Mobile target in targets)
                {
                    DoHarmful(target);
                    int damage = Utility.RandomMinMax(25, 40); // Moderate AoE damage
                    // Deal 50% physical, 50% cold damage
                    AOS.Damage(target, this, damage, 50, 0, 50, 0, 0);

                    // Add a visual effect on the target (wrapping effect)
                    target.FixedParticles(0x374A, 10, 20, 5008, UniqueHue, 0, EffectLayer.Waist); // Corpse skin effect, re-hued

                    // Apply short paralysis (Snare effect)
                    target.Paralyze(TimeSpan.FromSeconds(Utility.RandomMinMax(2, 3)));
                    target.SendMessage(0x22, "Ethereal bindings grip you!");
                }
            }
        }

        // --- Unique Ability: Necrotic Pulse (Targeted Life Drain) ---
        public void DoNecroticPulse()
        {
             if (Combatant == null || Map == null) return;

             IDamageable targetDamageable = Combatant;

            // Ensure target is a mobile before proceeding
            if (targetDamageable is Mobile targetMobile && CanBeHarmful(targetMobile, false) && SpellHelper.ValidIndirectTarget(this, targetMobile) && InLOS(targetMobile))
            {
                this.Say("*Your life force sustains me!*");
                this.PlaySound(BaseSoundID); // Make mummy sound
                this.MovingParticles(targetMobile, 0x36D4, 7, 0, false, true, EffectHue, 0, 9501, 1, 0, EffectLayer.Head, 0x100); // Mind blast effect? Or 0x3789 (Wither effect)

                int damage = Utility.RandomMinMax(50, 75); // Significant single target drain
                int lifeToDrain = 0;

                // Check target's current health
                if (targetMobile.Hits > damage)
                {
                    lifeToDrain = damage;
                }
                else
                {
                    lifeToDrain = targetMobile.Hits -1; // Drain almost all remaining life but don't kill directly with drain
                }

                 if (lifeToDrain > 0)
                 {
                    // Deal direct damage (can be resisted, but bypasses armor) - Let's use Necromancy spell damage logic
                    // AOS.Damage(targetMobile, this, lifeToDrain, 0, 0, 100, 0, 0); // Example: 100% Cold
                    // Or, let's try a direct life drain effect that's harder to resist entirely
                     targetMobile.PlaySound(0x1DF); // Wither sound on target
                     targetMobile.FixedParticles(0x374A, 10, 20, 5013, EffectHue, 0, EffectLayer.Waist); // Effect on target
                     targetMobile.SendMessage(0x22,"You feel your life force being torn away!");

                     // Apply the damage/drain
                     int actualDamage = targetMobile.Hits - lifeToDrain; // Calculate potential damage
                     targetMobile.Damage(lifeToDrain, this); // Apply damage that considers resistances etc.

                     // Calculate heal amount based on actual damage dealt if possible, or capped drain amount
                     int healAmount = Math.Max(1, lifeToDrain / 2); // Heal for 50% of intended drain

                    // Heal self
                    this.Heal(healAmount, this, false); // Non-message heal
                    this.FixedParticles(0x376A, 9, 32, 5030, EffectHue, 0, EffectLayer.Waist); // Heal effect on self
                 }
            }
        }

        // --- Unique Ability: Summon Failed Experiment ---
        public void DoSummonExperiment()
        {
             if (Map == null) return;

             this.Say("*Arise, my failed children!*");
             PlaySound(0x1FB); // Summon creature sound

             int summons = Utility.RandomMinMax(1, 2); // Summon 1 or 2

             for (int i = 0; i < summons; ++i)
             {
                 Point3D summonLoc = this.Location;
                 bool foundLoc = false;

                 // Try to find a valid spot near the mummy
                 for (int attempts = 0; attempts < 10; ++attempts)
                 {
                     int x = Location.X + Utility.RandomMinMax(-2, 2);
                     int y = Location.Y + Utility.RandomMinMax(-2, 2);
                     int z = Map.GetAverageZ(x, y);

                     if (Map.CanSpawnMobile(x, y, z))
                     {
                         summonLoc = new Point3D(x, y, z);
                         foundLoc = true;
                         break;
                     }
                 }

                 if (foundLoc)
                 {
                     // Spawn visual effect
                     Effects.SendLocationParticles(EffectItem.Create(summonLoc, this.Map, EffectItem.DefaultDuration), 0x3728, 10, 10, EffectHue, 0, 5023, 0);

                    BaseCreature minion = null;

                    // Choose a weak spectral minion type
                    if(Utility.RandomBool())
                        minion = new Spectre(); // Example: Use Spectre model
                    else
                        minion = new Wraith(); // Example: Use Wraith model

                    if (minion != null)
                    {
                        minion.Hue = EffectHue + Utility.RandomMinMax(-2, 2); // Slight hue variation
                        minion.Team = this.Team;
                        minion.Summoned = true;
                        minion.SummonMaster = this;
                        minion.ControlOrder = OrderType.Guard; // Guard the master
                        minion.ControlTarget = this;
						// OLD (Line 299 - broken):
						// minion.SummonDuration = TimeSpan.FromSeconds(Utility.RandomMinMax(25, 35));

						// NEW (commented out, or replace with optional custom handling):
						// No SummonDuration exists in BaseCreature. You can implement your own timer if needed.
						Timer.DelayCall(TimeSpan.FromSeconds(Utility.RandomMinMax(25, 35)), () =>
						{
							if (minion != null && !minion.Deleted)
								minion.Delete();
						});


                        // Optionally weaken the summoned creature slightly
                        minion.RawStr = (int)(minion.RawStr * 0.8);
                        minion.SetHits(minion.HitsMaxSeed / 2);

                        minion.MoveToWorld(summonLoc, this.Map);
                    }
                 }
             }
        }

         // --- Death Curse ---
        public override void OnDeath(Container c)
        {
            if (Map == null)
            {
                base.OnDeath(c);
                return;
            }

            this.Say("*My curse will linger!*");
            // Large central explosion effect
            Effects.PlaySound(this.Location, this.Map, 0x108); // Ghastly sound effect
            Effects.SendLocationParticles(EffectItem.Create(this.Location, this.Map, EffectItem.DefaultDuration), 0x374A, 10, 60, UniqueHue, 0, 5023, 0); // Necro/dark explosion

            List<Mobile> targets = new List<Mobile>();
            IPooledEnumerable eable = Map.GetMobilesInRange(this.Location, 5); // 5 tile radius for curse

            foreach (Mobile m in eable)
            {
                // Only target players or their bonded pets for the potent curse
                if (m != this && (m.Player || (m is BaseCreature && ((BaseCreature)m).Controlled && ((BaseCreature)m).ControlMaster != null && ((BaseCreature)m).ControlMaster.Player)) && CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(this, m))
                {
                   targets.Add(m);
                }
            }
            eable.Free();

             // Apply potent curse effect
            if (targets.Count > 0)
            {
                 foreach (Mobile target in targets)
                 {
                    target.SendMessage(0x22, "You feel the weight of the mummy's final curse!");
                    target.PlaySound(0x1ED); // Curse sound effect

                    // Apply a significant stat curse for 30 seconds
                    ResistanceMod[] mods = {
                         new ResistanceMod(ResistanceType.Physical, -15), // Example curse effect
                         new ResistanceMod(ResistanceType.Fire, -15),
                         new ResistanceMod(ResistanceType.Cold, -15),
                         new ResistanceMod(ResistanceType.Poison, -15),
                         new ResistanceMod(ResistanceType.Energy, -15)
                        };
                    StatMod[] statMods = {
                        new StatMod(StatType.Str, "DeathCurseStr", -20, TimeSpan.FromSeconds(30)),
                        new StatMod(StatType.Dex, "DeathCurseDex", -20, TimeSpan.FromSeconds(30)),
                        new StatMod(StatType.Int, "DeathCurseInt", -20, TimeSpan.FromSeconds(30))
                        };

                    // Apply the stat mods
                    foreach(StatMod sm in statMods)
                        target.AddStatMod(sm);

                    // You might need a helper function or system to apply temporary resistance mods
                    // For simplicity, we'll just apply the stat curse here.
                    // If you have a system for temporary resistance debuffs, apply `mods` here for 30 seconds.

                     target.FixedParticles(0x374A, 10, 30, 5013, EffectHue, 0, EffectLayer.Waist); // Visual curse effect
                 }
            }

            // Spawn hazardous tiles around the corpse
            int hazardsToDrop = Utility.RandomMinMax(3, 5);
            for (int i = 0; i < hazardsToDrop; i++)
            {
                Point3D hazardLocation = Point3D.Zero;
                bool foundLoc = false;

                 for (int attempts = 0; attempts < 10; ++attempts)
                 {
                     int xOffset = Utility.RandomMinMax(-4, 4);
                     int yOffset = Utility.RandomMinMax(-4, 4);
                     Point3D tryLoc = new Point3D(this.X + xOffset, this.Y + yOffset, this.Z);
                     int z = Map.GetAverageZ(tryLoc.X, tryLoc.Y);

                     if (Map.CanFit(tryLoc.X, tryLoc.Y, z, 16, false, false))
                     {
                         hazardLocation = new Point3D(tryLoc.X, tryLoc.Y, z);
                         foundLoc = true;
                         break;
                     }
                 }

                if(foundLoc)
                {
                    // Use NecromanticFlamestrikeTile or PoisonTile, re-hued
                    // NecromanticFlamestrikeTile hazard = new NecromanticFlamestrikeTile(); // Requires defining this tile
                    PoisonTile hazard = new PoisonTile(); // Use PoisonTile as fallback
                    hazard.Hue = UniqueHue; // Match mummy's hue
                    hazard.MoveToWorld(hazardLocation, this.Map);

                    // Smaller visual effect at each hazard location
                    Effects.SendLocationParticles(EffectItem.Create(hazardLocation, this.Map, EffectItem.DefaultDuration), 0x3728, 10, 20, UniqueHue, 0, 5039, 0);
                }
            }

            base.OnDeath(c);
        }

        // --- Standard Properties & Loot ---
        public override bool BleedImmune { get { return true; } }
        public override Poison PoisonImmune { get { return Poison.Deadly; } } // High poison immunity
        public override int TreasureMapLevel { get { return 5; } } // High level map drop

        // Increased Dispel difficulty (if summonable/controllable - not applicable here but good practice)
        // public override double DispelDifficulty { get { return 130.0; } }
        // public override double DispelFocus { get { return 60.0; } }

        public override TribeType Tribe { get { return TribeType.Undead; } }
        public override OppositionGroup OppositionGroup { get { return OppositionGroup.FeyAndUndead; } }


        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich);
            AddLoot(LootPack.FilthyRich); // Good base loot
            AddLoot(LootPack.MedScrolls, Utility.RandomMinMax(1, 2)); // Chance for 5th/6th scrolls
            AddLoot(LootPack.HighScrolls); // Chance for 7th/8th scrolls
            AddLoot(LootPack.Gems, Utility.RandomMinMax(6, 10)); // Lots of gems

            // Necromantic Reagents
            PackItem(new Nightshade(Utility.RandomMinMax(15, 25)));
            PackItem(new GraveDust(Utility.RandomMinMax(10, 20))); // Assuming GraveDust item exists
            PackItem(new BatWing(Utility.RandomMinMax(10, 15)));

             // Chance for Malidor Academy themed items
            if (Utility.RandomDouble() < 0.05) // 5% chance
            {
                // Placeholder - Replace with actual Malidor items if they exist
                // PackItem(new MalidorsTatteredNotes());
                PackItem(new ArcaneGem()); // Example rare resource
            }
             if (Utility.RandomDouble() < 0.01) // 1% chance for a very rare drop
            {
                // Placeholder - Replace with actual unique artifact
                // PackItem(new WitchesBindingSash());
                 PackItem(new MaxxiaScroll()); // Placeholder: Use a Power Crystal
            }
        }

        // --- Serialization ---
        public WitchboundMummy(Serial serial) : base(serial)
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

            // Re-initialize timers on load/restart
            m_NextSpectralGraspTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 18));
            m_NextNecroticPulseTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(8, 14));
            m_NextSummonTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 35));
            m_NextAuraPulseTime = DateTime.UtcNow + TimeSpan.FromSeconds(3);
        }
    }
}