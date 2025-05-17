using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Spells;          // Needed for potential spell effects
using Server.Network;         // Needed for visual/sound effects
using System.Collections.Generic;

namespace Server.Mobiles
{
    [CorpseName("a twin head's corpse")]
    public class TwinHead : BaseCreature
    {
        // Timers for special abilities
        private DateTime m_NextRoarTime;
        private DateTime m_NextConvergenceTime;
        private DateTime m_NextDualBlastTime;
        private Point3D m_LastLocation;

        // Unique hue for Twin Head (example value; change as desired)
        private const int UniqueHue = 1289;

        [Constructable]
        public TwinHead()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.1, 0.2)
        {
            Name = "Twin Head";
            Body = 18;                 // Uses the Ettin body
            BaseSoundID = 367;         // Uses the Ettin sound
            Hue = UniqueHue;

            // --- Advanced Stats ---
            SetStr(400, 500);
            SetDex(200, 250);
            SetInt(600, 700);

            SetHits(1600, 1800);
            SetStam(300, 350);
            SetMana(700, 850);

            // Deals a mix of physical and energy damage
            SetDamage(15, 25);
            SetDamageType(ResistanceType.Physical, 40);
            SetDamageType(ResistanceType.Energy, 60);

            // --- Resistances ---
            SetResistance(ResistanceType.Physical, 50, 60);
            SetResistance(ResistanceType.Fire, 40, 50);
            SetResistance(ResistanceType.Cold, 60, 70);
            SetResistance(ResistanceType.Poison, 30, 40);
            SetResistance(ResistanceType.Energy, 80, 90);

            // --- Skills ---
            SetSkill(SkillName.EvalInt, 110.1, 125.0);
            SetSkill(SkillName.Magery, 110.1, 125.0);
            SetSkill(SkillName.MagicResist, 115.2, 130.0);
            SetSkill(SkillName.Meditation, 100.0, 110.0);
            SetSkill(SkillName.Tactics, 90.1, 100.0);
            SetSkill(SkillName.Wrestling, 90.1, 100.0);

            Fame = 22000;
            Karma = -22000;

            VirtualArmor = 85;
            ControlSlots = 5;

            // Initialize cooldown timers for abilities
            m_NextRoarTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_NextConvergenceTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
            m_NextDualBlastTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 25));

            m_LastLocation = this.Location;

            // --- Loot: Magic reagents plus potential unique drop ---
            PackItem(new BlackPearl(Utility.RandomMinMax(12, 18)));
            PackItem(new Nightshade(Utility.RandomMinMax(12, 18)));
            PackItem(new SpidersSilk(Utility.RandomMinMax(12, 18)));
            PackItem(new SulfurousAsh(Utility.RandomMinMax(12, 18)));
        }

        // --- Unique Aura: Arcane Dissonance Aura ---
        // Drains mana and applies minor energy damage to nearby mobiles
        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            if (m != null && m != this && m.Map == this.Map && m.InRange(this.Location, 2) && m.Alive && this.Alive && CanBeHarmful(m, false))
            {
                if (m is Mobile target)
                {
                    DoHarmful(target);
                    int manaDrained = Utility.RandomMinMax(10, 20);
                    if (target.Mana >= manaDrained)
                    {
                        target.Mana -= manaDrained;
                        target.SendMessage(0x22, "The twin heads' aura disrupts your concentration!");
                        target.FixedParticles(0x374A, 10, 15, 5032, EffectLayer.Head);
                        target.PlaySound(0x1F8);
                    }
                    // Inflict minor energy damage
                    AOS.Damage(target, this, Utility.RandomMinMax(5, 10), 0, 0, 0, 0, 100);
                }
            }
            base.OnMovement(m, oldLocation);
        }

        // --- Main Think Loop to Check & Trigger Abilities ---
        public override void OnThink()
        {
            base.OnThink();

            // Leave arcane residue as it moves
            if (this.Location != m_LastLocation && this.Map != null && this.Map != Map.Internal && Utility.RandomDouble() < 0.25)
            {
                Point3D oldLoc = m_LastLocation;
                m_LastLocation = this.Location;
                if (Map.CanFit(oldLoc.X, oldLoc.Y, oldLoc.Z, 16, false, false))
                {
                    VortexTile residue = new VortexTile();
                    residue.Hue = UniqueHue;
                    residue.MoveToWorld(oldLoc, this.Map);
                }
                else
                {
                    int validZ = Map.GetAverageZ(oldLoc.X, oldLoc.Y);
                    if (Map.CanFit(oldLoc.X, oldLoc.Y, validZ, 16, false, false))
                    {
                        VortexTile residue = new VortexTile();
                        residue.Hue = UniqueHue;
                        residue.MoveToWorld(new Point3D(oldLoc.X, oldLoc.Y, validZ), this.Map);
                    }
                }
            }
            else
            {
                m_LastLocation = this.Location;
            }

            if (Combatant == null || Map == null || Map == Map.Internal || !Alive)
                return;

            // Ability selection based on distance and cooldowns
            if (DateTime.UtcNow >= m_NextDualBlastTime && this.InRange(Combatant.Location, 10))
            {
                DualBlastAttack();
                m_NextDualBlastTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 35));
            }
            else if (DateTime.UtcNow >= m_NextConvergenceTime && this.InRange(Combatant.Location, 8))
            {
                ArcaneConvergenceAttack();
                m_NextConvergenceTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(18, 28));
            }
            else if (DateTime.UtcNow >= m_NextRoarTime && this.InRange(Combatant.Location, 6))
            {
                TwinRoarAttack();
                m_NextRoarTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 20));
            }
        }

        // --- Unique Ability: Twin Roar Attack ---
        // Both heads let loose a thunderous roar that deals mixed damage and disorients foes
        public void TwinRoarAttack()
        {
            this.Say("*The twin heads roar in unison!*");
            PlaySound(0x20C); // Roaring sound effect

            List<Mobile> targets = new List<Mobile>();
            IPooledEnumerable eable = Map.GetMobilesInRange(this.Location, 6);
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
                // Create a visual impact effect
                Effects.SendLocationParticles(EffectItem.Create(this.Location, this.Map, EffectItem.DefaultDuration),
                                              0x376A, 10, 60, UniqueHue, 0, 5039, 0);
                foreach (Mobile target in targets)
                {
                    DoHarmful(target);
                    int damage = Utility.RandomMinMax(25, 35);
                    // Damage split: 70% physical, 30% energy
                    AOS.Damage(target, this, damage, 70, 30, 0, 0, 0);
                    target.FixedParticles(0x379C, 10, 15, 5032, UniqueHue, 0, EffectLayer.Head);
                    target.SendMessage(0x22, "The roaring blast leaves you staggered!");
                }
            }
        }

        // --- Unique Ability: Arcane Convergence Attack ---
        // Channels raw arcane energy to drain mana and inflict energy damage in an AoE burst
        public void ArcaneConvergenceAttack()
        {
            if (Map == null)
                return;

            this.Say("*Arcane forces converge!*");
            PlaySound(0x20A); // Magical surge sound

            List<Mobile> targets = new List<Mobile>();
            IPooledEnumerable eable = Map.GetMobilesInRange(this.Location, 6);
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
                Effects.SendLocationParticles(EffectItem.Create(this.Location, this.Map, EffectItem.DefaultDuration),
                                              0x3709, 10, 30, UniqueHue, 0, 5032, 0);
                foreach (Mobile target in targets)
                {
                    DoHarmful(target);
                    int damage = Utility.RandomMinMax(30, 50);
                    // Deal 100% energy damage
                    AOS.Damage(target, this, damage, 0, 0, 0, 0, 100);
                    target.FixedParticles(0x374A, 10, 15, 5032, UniqueHue, 0, EffectLayer.Head);

                    // Drain mana with a 40% chance
                    if (Utility.RandomDouble() < 0.40)
                    {
                        if (target is Mobile targetMobile)
                        {
                            int manaDrained = Utility.RandomMinMax(20, 40);
                            if (targetMobile.Mana >= manaDrained)
                            {
                                targetMobile.Mana -= manaDrained;
                                targetMobile.SendMessage(0x22, "The converging energy drains your magical reserves!");
                                targetMobile.FixedParticles(0x374A, 10, 15, 5032, UniqueHue, 0, EffectLayer.Head);
                                targetMobile.PlaySound(0x1F8);
                            }
                        }
                    }
                }
            }
        }

        // --- Unique Ability: Dual Blast Attack ---
        // Unleashes a chain lightning–style attack that bounces between multiple foes
        public void DualBlastAttack()
        {
            if (Combatant == null || Map == null)
                return;

            this.Say("*Twin heads unleash a dual magical blast!*");
            PlaySound(0x20D); // Energy blast sound

            List<Mobile> targets = new List<Mobile>();
            Mobile initialTarget = Combatant as Mobile;
            if (initialTarget == null || !CanBeHarmful(initialTarget, false) || !SpellHelper.ValidIndirectTarget(this, initialTarget))
                return;

            targets.Add(initialTarget);
            int maxTargets = 5;
            int range = 5;

            for (int i = 0; i < maxTargets; i++)
            {
                Mobile lastTarget = targets[targets.Count - 1];
                Mobile nextTarget = null;
                double closestDistance = -1.0;
                IPooledEnumerable eable = Map.GetMobilesInRange(lastTarget.Location, range);
                foreach (Mobile m in eable)
                {
                    if (m != this && m != lastTarget && !targets.Contains(m) &&
                        CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(lastTarget, m) && lastTarget.InLOS(m))
                    {
                        double dist = lastTarget.GetDistanceToSqrt(m);
                        if (closestDistance == -1.0 || dist < closestDistance)
                        {
                            closestDistance = dist;
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

            for (int i = 0; i < targets.Count; i++)
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
                        int damage = Utility.RandomMinMax(25, 40);
                        AOS.Damage(damageTarget, this, damage, 0, 0, 0, 0, 100);
                        damageTarget.FixedParticles(0x374A, 1, 15, 9502, UniqueHue, 0, EffectLayer.Waist);
                    }
                });
            }
        }

        // --- Death Effect: Mystic Implosion ---
        public override void OnDeath(Container c)
        {
            if (Map == null)
            {
                base.OnDeath(c);
                return;
            }

            this.Say("*The twin heads collapse in a burst of arcane energy!*");
            Effects.PlaySound(this.Location, this.Map, 0x20F); // Implosion sound
            Effects.SendLocationParticles(EffectItem.Create(this.Location, this.Map, EffectItem.DefaultDuration),
                                          0x3709, 10, 60, UniqueHue, 0, 5052, 0);

            // Drop several hazardous ground tiles randomly chosen from a set
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

                // Randomly choose one of several hazardous tile types
                Item hazardTile = null;
                double randomChoice = Utility.RandomDouble();
                if (randomChoice < 0.2)
                    hazardTile = new ManaDrainTile();
                else if (randomChoice < 0.4)
                    hazardTile = new FlamestrikeHazardTile();
                else if (randomChoice < 0.6)
                    hazardTile = new IceShardTile();
                else if (randomChoice < 0.8)
                    hazardTile = new ThunderstormTile();
                else
                    hazardTile = new PoisonTile();

                if (hazardTile != null)
                {
                    hazardTile.Hue = UniqueHue;
                    hazardTile.MoveToWorld(hazardLocation, this.Map);
                    Effects.SendLocationParticles(EffectItem.Create(hazardLocation, this.Map, EffectItem.DefaultDuration),
                                                  0x376A, 10, 20, UniqueHue, 0, 5039, 0);
                }
            }

            base.OnDeath(c);
        }

        public override bool CanRummageCorpses { get { return true; } }
        public override int TreasureMapLevel { get { return 2; } }
        public override int Meat { get { return 4; } }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 2);
            AddLoot(LootPack.HighScrolls, Utility.RandomMinMax(1, 2));
            AddLoot(LootPack.Gems, Utility.RandomMinMax(8, 12));

            if (Utility.RandomDouble() < 0.02)
                PackItem(new MaxxiaScroll());

            if (Utility.RandomDouble() < 0.05)
                PackItem(new MaxxiaScroll(Utility.RandomMinMax(1, 3)));
        }

        public TwinHead(Serial serial)
            : base(serial)
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
            m_NextConvergenceTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
            m_NextDualBlastTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 25));
        }
    }
}
