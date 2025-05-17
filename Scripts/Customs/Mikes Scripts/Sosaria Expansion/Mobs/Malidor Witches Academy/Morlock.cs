using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Spells;         // For any spell effects
using Server.Network;         // For particle/sound effects
using System.Collections.Generic;
using Server.Spells.Seventh;   // For chain lightning–based effects if needed

namespace Server.Mobiles
{
    [CorpseName("a Morlock corpse")]
    public class Morlock : BaseCreature
    {
        // --- Ability Timers ---
        private DateTime m_NextRoarTime;
        private DateTime m_NextRiftTime;
        private DateTime m_NextBondTime;
        private Point3D m_LastLocation;

        // --- Unique Hue for Morlock (deep, eldritch purple) ---
        private const int UniqueHue = 1175;

        [Constructable]
        public Morlock() : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "Morlock";
            Body = 267;          // Using the troglodyte body
            BaseSoundID = 0x59F;   // Using the troglodyte sound
            Hue = UniqueHue;

            // --- Enhanced Stats ---
            SetStr(450, 550);
            SetDex(300, 350);
            SetInt(700, 800);

            SetHits(1800, 2200);
            SetStam(300, 350);
            SetMana(800, 1000);

            SetDamage(18, 24);
            // Damage type: mix of physical (20%) and mainly energy (80%)
            SetDamageType(ResistanceType.Physical, 20);
            SetDamageType(ResistanceType.Energy, 80);

            // --- Resistances ---
            SetResistance(ResistanceType.Physical, 50, 60);
            SetResistance(ResistanceType.Fire, 60, 70);
            SetResistance(ResistanceType.Cold, 60, 70);
            SetResistance(ResistanceType.Poison, 50, 60);
            SetResistance(ResistanceType.Energy, 90, 100);

            // --- Skills (Magic Focused) ---
            SetSkill(SkillName.EvalInt, 120.1, 135.0);
            SetSkill(SkillName.Magery, 120.1, 135.0);
            SetSkill(SkillName.MagicResist, 120.0, 140.0);
            SetSkill(SkillName.Meditation, 100.0, 110.0);
            SetSkill(SkillName.Tactics, 100.1, 110.0);
            SetSkill(SkillName.Wrestling, 100.1, 110.0);

            Fame = 25000;
            Karma = -25000;
            VirtualArmor = 100;
            ControlSlots = 5;

            // --- Initialize Ability Cooldowns ---
            m_NextRoarTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_NextRiftTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(18, 25));
            m_NextBondTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));

            m_LastLocation = this.Location;

            // --- Starting Loot (Magic reagents) ---
            PackItem(new BlackPearl(Utility.RandomMinMax(10, 15)));
            PackItem(new Nightshade(Utility.RandomMinMax(10, 15)));
            PackItem(new SpidersSilk(Utility.RandomMinMax(10, 15)));
            PackItem(new SulfurousAsh(Utility.RandomMinMax(10, 15)));
        }

        // --- Movement Aura: Disrupts nearby foes' mental focus ---
        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            if (m != null && m != this && m.Map == this.Map && m.InRange(this.Location, 2) && m.Alive && this.Alive && CanBeHarmful(m, false))
            {
                if (m is Mobile target)
                {
                    DoHarmful(target);
                    int drain = Utility.RandomMinMax(5, 10);
                    if (target.Mana >= drain)
                    {
                        target.Mana -= drain;
                        target.SendMessage(0x22, "Your mind feels disrupted by dark whispers!");
                        target.FixedParticles(0x374A, 10, 15, 5032, UniqueHue, 0, EffectLayer.Head);
                        target.PlaySound(0x1F8);
                    }
                }
            }
            base.OnMovement(m, oldLocation);
        }

        // --- Ability AI: Check and trigger special attacks ---
        public override void OnThink()
        {
            base.OnThink();

            if (Combatant == null || Map == null || Map == Map.Internal || !Alive)
                return;

            // Priority: Psychic Bond > Shadow Rift > Eldritch Roar
            if (DateTime.UtcNow >= m_NextBondTime && InRange(Combatant.Location, 10))
            {
                PsychicBondAttack();
                m_NextBondTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 35));
            }
            else if (DateTime.UtcNow >= m_NextRiftTime && InRange(Combatant.Location, 12))
            {
                ShadowRiftAttack();
                m_NextRiftTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(18, 28));
            }
            else if (DateTime.UtcNow >= m_NextRoarTime && InRange(Combatant.Location, 8))
            {
                EldritchRoarAttack();
                m_NextRoarTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 20));
            }

            // --- Leave a Trail of Unsettling Energy ---
            if (this.Location != m_LastLocation && this.Map != null && this.Map != Map.Internal && Utility.RandomDouble() < 0.25)
            {
                Point3D oldLoc = m_LastLocation;
                m_LastLocation = this.Location;

                if (Map.CanFit(oldLoc.X, oldLoc.Y, oldLoc.Z, 16, false, false))
                {
                    VortexTile vortex = new VortexTile();
                    vortex.Hue = UniqueHue;
                    vortex.MoveToWorld(oldLoc, this.Map);
                }
                else
                {
                    int validZ = Map.GetAverageZ(oldLoc.X, oldLoc.Y);
                    if (Map.CanFit(oldLoc.X, oldLoc.Y, validZ, 16, false, false))
                    {
                        VortexTile vortex = new VortexTile();
                        vortex.Hue = UniqueHue;
                        vortex.MoveToWorld(new Point3D(oldLoc.X, oldLoc.Y, validZ), this.Map);
                    }
                }
            }
            else
            {
                m_LastLocation = this.Location;
            }
        }

        // --- Unique Ability: Eldritch Roar ---
        // An AoE burst that deals energy damage and drains mana from nearby foes.
        public void EldritchRoarAttack()
        {
            if (Map == null)
                return;

            this.PlaySound(0x220); // Roar sound effect
            this.FixedParticles(0x3709, 10, 30, 5052, UniqueHue, 0, EffectLayer.CenterFeet);

            List<Mobile> targets = new List<Mobile>();
            IPooledEnumerable eable = Map.GetMobilesInRange(this.Location, 6);
            foreach (Mobile m in eable)
            {
                if (m != this && CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(this, m))
                    targets.Add(m);
            }
            eable.Free();

            if (targets.Count > 0)
            {
                // Outward visual burst effect
                Effects.SendLocationParticles(EffectItem.Create(this.Location, this.Map, EffectItem.DefaultDuration),
                                              0x376A, 10, 60, UniqueHue, 0, 5039, 0);

                foreach (Mobile target in targets)
                {
                    DoHarmful(target);
                    int damage = Utility.RandomMinMax(40, 60);
                    // Deal 100% energy damage
                    AOS.Damage(target, this, damage, 0, 0, 0, 0, 100);

                    // Target hit effect
                    target.FixedParticles(0x3779, 10, 25, 5032, UniqueHue, 0, EffectLayer.Head);

                    // 30% chance to drain additional mana
                    if (Utility.RandomDouble() < 0.30)
                    {
                        if (target is Mobile t)
                        {
                            int manaDrain = Utility.RandomMinMax(15, 30);
                            if (t.Mana >= manaDrain)
                            {
                                t.Mana -= manaDrain;
                                t.SendMessage(0x22, "The Eldritch Roar drains your mental fortitude!");
                                t.FixedParticles(0x374A, 10, 15, 5032, UniqueHue, 0, EffectLayer.Head);
                                t.PlaySound(0x1F8);
                            }
                        }
                    }
                }
            }
        }

        // --- Unique Ability: Shadow Rift ---
        // Creates a dark, hazardous rift at the target's location using a necromantic flamestrike tile.
        public void ShadowRiftAttack()
        {
            if (Combatant == null || Map == null)
                return;

            Point3D targetLocation;
            if (Combatant is Mobile target && SpellHelper.ValidIndirectTarget(this, target))
                targetLocation = target.Location;
            else
                targetLocation = Combatant.Location;

            this.Say("*Darkness rends the veil!*");
            PlaySound(0x22F); // Rift sound effect

            // Pre-rift flash effect
            Effects.SendLocationParticles(EffectItem.Create(targetLocation, this.Map, EffectItem.DefaultDuration),
                                          0x3728, 10, 10, UniqueHue, 0, 5039, 0);

            Timer.DelayCall(TimeSpan.FromSeconds(0.5), () =>
            {
                if (this.Map == null)
                    return;

                Point3D spawnLoc = targetLocation;
                if (!Map.CanFit(spawnLoc.X, spawnLoc.Y, spawnLoc.Z, 16, false, false))
                {
                    spawnLoc.Z = Map.GetAverageZ(spawnLoc.X, spawnLoc.Y);
                    if (!Map.CanFit(spawnLoc.X, spawnLoc.Y, spawnLoc.Z, 16, false, false))
                        return;
                }

                // Spawn a dark hazard using the NecromanticFlamestrikeTile
                NecromanticFlamestrikeTile riftTile = new NecromanticFlamestrikeTile();
                riftTile.Hue = UniqueHue;
                riftTile.MoveToWorld(spawnLoc, this.Map);

                Effects.PlaySound(spawnLoc, this.Map, 0x1F6);
            });
        }

        // --- Unique Ability: Psychic Bond ---
        // A chain attack that bounces between foes, dealing energy damage and draining mana.
        public void PsychicBondAttack()
        {
            if (Combatant == null || Map == null)
                return;

            this.Say("*Your minds are mine!*");
            PlaySound(0x20A);

            List<Mobile> targets = new List<Mobile>();
            Mobile currentTarget = Combatant as Mobile;
            if (currentTarget == null || !CanBeHarmful(currentTarget, false) || !SpellHelper.ValidIndirectTarget(this, currentTarget))
                return;

            targets.Add(currentTarget);

            int maxTargets = 5;
            int range = 5;

            for (int i = 0; i < maxTargets; ++i)
            {
                Mobile lastTarget = targets[targets.Count - 1];
                Mobile nextTarget = null;
                double closestDist = -1.0;

                IPooledEnumerable eable = Map.GetMobilesInRange(lastTarget.Location, range);
                foreach (Mobile m in eable)
                {
                    if (m != this && m != lastTarget && !targets.Contains(m) && CanBeHarmful(m, false) &&
                        SpellHelper.ValidIndirectTarget(lastTarget, m) && lastTarget.InLOS(m))
                    {
                        double dist = lastTarget.GetDistanceToSqrt(m);
                        if (closestDist == -1.0 || dist < closestDist)
                        {
                            closestDist = dist;
                            nextTarget = m;
                        }
                    }
                }
                eable.Free();

                if (nextTarget != null)
                    targets.Add(nextTarget);
                else
                    break;
            }

            // For each target, send a visual bolt and deal damage & mana drain
            for (int i = 0; i < targets.Count; ++i)
            {
                Mobile source = (i == 0) ? this : targets[i - 1];
                Mobile target = targets[i];

                Effects.SendMovingParticles(
                    new Entity(Serial.Zero, source.Location, source.Map),
                    new Entity(Serial.Zero, target.Location, target.Map),
                    0x3818, 7, 0, false, false, UniqueHue, 0, 9502, 1, 0, EffectLayer.Head, 0x100);

                Mobile damageTarget = target;
                Timer.DelayCall(TimeSpan.FromSeconds(i * 0.15), () =>
                {
                    if (CanBeHarmful(damageTarget, false))
                    {
                        DoHarmful(damageTarget);
                        int damage = Utility.RandomMinMax(30, 50);
                        AOS.Damage(damageTarget, this, damage, 0, 0, 0, 0, 100);
                        damageTarget.FixedParticles(0x374A, 1, 15, 9502, EffectLayer.Waist);

                        // Drain a bit of mana from each target
                        if (damageTarget is Mobile t)
                        {
                            int manaDrain = Utility.RandomMinMax(10, 20);
                            if (t.Mana >= manaDrain)
                            {
                                t.Mana -= manaDrain;
                                t.SendMessage(0x22, "The psychic bond drains your mental strength!");
                                t.FixedParticles(0x374A, 10, 15, 5032, UniqueHue, 0, EffectLayer.Head);
                                t.PlaySound(0x1F8);
                            }
                        }
                    }
                });
            }
        }

        // --- Death Effect: Eldritch Dissolution ---
        public override void OnDeath(Container c)
        {
            if (Map != null)
            {
                this.Say("*The mind unravels...*");
                Effects.PlaySound(this.Location, this.Map, 0x211);
                Effects.SendLocationParticles(EffectItem.Create(this.Location, this.Map, EffectItem.DefaultDuration),
                                              0x3709, 10, 60, UniqueHue, 0, 5052, 0);

                int hazardsToDrop = Utility.RandomMinMax(4, 7);
                for (int i = 0; i < hazardsToDrop; i++)
                {
                    int xOffset = Utility.RandomMinMax(-4, 4);
                    int yOffset = Utility.RandomMinMax(-4, 4);
                    Point3D hazardLocation = new Point3D(this.X + xOffset, this.Y + yOffset, this.Z);

                    if (!Map.CanFit(hazardLocation.X, hazardLocation.Y, hazardLocation.Z, 16, false, false))
                    {
                        hazardLocation.Z = Map.GetAverageZ(hazardLocation.X, hazardLocation.Y);
                        if (!Map.CanFit(hazardLocation.X, hazardLocation.Y, hazardLocation.Z, 16, false, false))
                            continue;
                    }

                    // Spawn a dark hazard using a NecromanticFlamestrikeTile
                    NecromanticFlamestrikeTile hazardTile = new NecromanticFlamestrikeTile();
                    hazardTile.Hue = UniqueHue;
                    hazardTile.MoveToWorld(hazardLocation, this.Map);
                    Effects.SendLocationParticles(EffectItem.Create(hazardLocation, this.Map, EffectItem.DefaultDuration),
                                                  0x376A, 10, 20, UniqueHue, 0, 5039, 0);
                }
            }
            base.OnDeath(c);
        }

        // --- Loot Generation ---
        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 2);
            AddLoot(LootPack.Average);
            AddLoot(LootPack.HighScrolls, Utility.RandomMinMax(1, 2));
            AddLoot(LootPack.Gems, Utility.RandomMinMax(8, 12));

            if (Utility.RandomDouble() < 0.02)
            {
                // Unique magical artifact drop (placeholder)
                PackItem(new MaxxiaScroll());
            }
            if (Utility.RandomDouble() < 0.05)
            {
                PackItem(new MaxxiaScroll(Utility.RandomMinMax(1, 3)));
            }
            if (Utility.RandomDouble() < 0.10)
            {
                PackItem(new MaxxiaScroll(Utility.RandomMinMax(1, 2)));
            }
        }

        // --- Standard Properties ---
        public override bool BleedImmune { get { return true; } }
        public override int TreasureMapLevel { get { return 6; } }
        public override double DispelDifficulty { get { return 150.0; } }
        public override double DispelFocus { get { return 75.0; } }

        public Morlock(Serial serial) : base(serial)
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
            m_NextRoarTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_NextRiftTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(18, 25));
            m_NextBondTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            m_LastLocation = this.Location;
        }
    }
}
