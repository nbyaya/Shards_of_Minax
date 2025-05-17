using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Spells;            // For potential spell effects
using Server.Network;           // For visual and sound effects
using System.Collections.Generic;  // For list-based AoE targeting

namespace Server.Mobiles
{
    [CorpseName("a bound ki-rin corpse")]
    public class BoundKirin : BaseCreature
    {
        // Timers for special abilities
        private DateTime m_NextBindingTime;
        private DateTime m_NextDischargeTime;
        private DateTime m_NextChainTime;
        private Point3D m_LastLocation;

        // Unique hue for Bound Kirin (example value; adjust as needed)
        private const int UniqueHue = 1288;

        [Constructable]
        public BoundKirin()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a Bound Kirin";
            Body = 132; // Inherited body from the original Kirin
            BaseSoundID = 0x3C5; // Kirin-like sound
            Hue = UniqueHue;

            // --- Advanced Stats ---
            SetStr(400, 500);
            SetDex(250, 300);
            SetInt(550, 650);

            SetHits(1600, 1800);
            SetStam(250, 300);
            SetMana(600, 750);

            SetDamage(18, 25);
            // Damage distribution: mostly Energy damage with a small Physical component
            SetDamageType(ResistanceType.Physical, 10);
            SetDamageType(ResistanceType.Energy, 90);

            // --- Resistances ---
            SetResistance(ResistanceType.Physical, 40, 50);
            SetResistance(ResistanceType.Fire, 50, 60);
            SetResistance(ResistanceType.Cold, 50, 60);
            SetResistance(ResistanceType.Poison, 35, 45);
            SetResistance(ResistanceType.Energy, 80, 90);

            // --- Skills (emphasis on magic) ---
            SetSkill(SkillName.EvalInt, 110.1, 125.0);
            SetSkill(SkillName.Magery, 110.1, 125.0);
            SetSkill(SkillName.MagicResist, 115.2, 130.0);
            SetSkill(SkillName.Meditation, 100.0, 110.0);
            SetSkill(SkillName.Tactics, 90.1, 100.0);
            SetSkill(SkillName.Wrestling, 90.1, 100.0);

            Fame = 25000;
            Karma = -25000;

            VirtualArmor = 90;
            ControlSlots = 6;

            // This boss is not meant to be tamed.
            Tamable = false;

            // Initialize ability cooldowns
            m_NextDischargeTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(8, 12));
            m_NextBindingTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 18));
            m_NextChainTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));

            // Record starting location for movement-based effects
            m_LastLocation = this.Location;
        }

        // --- Movement Effect: Leave behind a magical snare ---
        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            // If Bound Kirin moved and is on a valid map, occasionally drop a snaring trap
            if (this.Location != m_LastLocation && this.Map != null && this.Map != Map.Internal && Utility.RandomDouble() < 0.25)
            {
                Point3D dropLoc = m_LastLocation;
                m_LastLocation = this.Location;

                if (Map.CanFit(dropLoc.X, dropLoc.Y, dropLoc.Z, 16, false, false))
                {
                    // Use the provided TrapWeb tile to represent magical binding energy
                    TrapWeb web = new TrapWeb();
                    web.Hue = UniqueHue;
                    web.MoveToWorld(dropLoc, this.Map);
                }
                else
                {
                    int validZ = Map.GetAverageZ(dropLoc.X, dropLoc.Y);
                    if (Map.CanFit(dropLoc.X, dropLoc.Y, validZ, 16, false, false))
                    {
                        TrapWeb web = new TrapWeb();
                        web.Hue = UniqueHue;
                        web.MoveToWorld(new Point3D(dropLoc.X, dropLoc.Y, validZ), this.Map);
                    }
                }
            }
            else
            {
                m_LastLocation = this.Location;
            }

            base.OnMovement(m, oldLocation);
        }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant == null || Map == null || Map == Map.Internal || !Alive)
                return;

            // Ability prioritization based on cooldowns and distance
            if (DateTime.UtcNow >= m_NextDischargeTime && this.InRange(Combatant.Location, 6))
            {
                ArcaneDischargeAttack();
                m_NextDischargeTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
            }
            else if (DateTime.UtcNow >= m_NextBindingTime && this.InRange(Combatant.Location, 8))
            {
                BindingChainsAttack();
                m_NextBindingTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(18, 25));
            }
            else if (DateTime.UtcNow >= m_NextChainTime && this.InRange(Combatant.Location, 12))
            {
                ChainedArcaneTempest();
                m_NextChainTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 35));
            }
        }

        // --- Ability 1: Binding Chains Attack ---
        // A targeted attack that lashes magical chains at an enemy, dealing energy damage and draining mana.
        public void BindingChainsAttack()
        {
            if (Combatant == null || Map == null)
                return;

            IDamageable targetDamageable = Combatant;
            if (targetDamageable is Mobile target && CanBeHarmful(target, false))
            {
                this.Say("*Chains bind thee!*");
                PlaySound(0x20B); // Use a distinct sound for the binding effect
                // Visual effect: Magical chains lash from the Bound Kirin to the target
                Effects.SendLocationParticles(EffectItem.Create(target.Location, this.Map, EffectItem.DefaultDuration),
                                              0x379B, 10, 15, UniqueHue, 0, 5039, 0);
                DoHarmful(target);

                int damage = Utility.RandomMinMax(30, 50);
                // Deal pure energy damage
                AOS.Damage(target, this, damage, 0, 0, 0, 0, 100);

                // Drain mana if available – check target is Mobile before accessing Mana
                int manaDrain = Utility.RandomMinMax(20, 40);
                if (target.Mana >= manaDrain)
                {
                    target.Mana -= manaDrain;
                    target.SendMessage(0x22, "Magical chains drain your energy!");
                    target.FixedParticles(0x374A, 10, 15, 5032, EffectLayer.Head);
                    target.PlaySound(0x1F8);
                }
            }
        }

        // --- Ability 2: Arcane Discharge Attack ---
        // An area-of-effect burst that deals energy damage and drains mana from nearby foes.
        public void ArcaneDischargeAttack()
        {
            if (Map == null)
                return;

            PlaySound(0x211); // Magic explosion sound
            FixedParticles(0x3709, 10, 30, 5052, UniqueHue, 0, EffectLayer.CenterFeet);

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
                // Outward visual effect: an explosion of arcane energy
                Effects.SendLocationParticles(EffectItem.Create(this.Location, this.Map, EffectItem.DefaultDuration),
                                              0x376A, 10, 60, UniqueHue, 0, 5039, 0);
                foreach (Mobile target in targets)
                {
                    DoHarmful(target);
                    int damage = Utility.RandomMinMax(35, 55);
                    AOS.Damage(target, this, damage, 0, 0, 0, 0, 100);

                    // Visual particle on target
                    target.FixedParticles(0x3779, 10, 25, 5032, UniqueHue, 0, EffectLayer.Head);

                    // Chance to drain additional mana
                    if (Utility.RandomDouble() < 0.40)
                    {
                        if (target is Mobile targetMobile)
                        {
                            int additionalDrain = Utility.RandomMinMax(20, 40);
                            if (targetMobile.Mana >= additionalDrain)
                            {
                                targetMobile.Mana -= additionalDrain;
                                targetMobile.SendMessage(0x22, "The arcane burst saps your focus!");
                                targetMobile.FixedParticles(0x374A, 10, 15, 5032, EffectLayer.Head);
                                targetMobile.PlaySound(0x1F8);
                            }
                        }
                    }
                }
            }
        }

        // --- Ability 3: Chained Arcane Tempest ---
        // A chain lightning–style attack that bounces between multiple targets.
        public void ChainedArcaneTempest()
        {
            if (Combatant == null || Map == null)
                return;

            this.Say("*Feel the tempest of binding magic!*");
            PlaySound(0x20A); // Energy bolt sound

            List<Mobile> targets = new List<Mobile>();
            Mobile currentTarget = Combatant as Mobile;
            if (currentTarget == null || !CanBeHarmful(currentTarget, false) || !SpellHelper.ValidIndirectTarget(this, currentTarget))
                return;

            targets.Add(currentTarget);
            int maxBounces = 5;
            int bounceRange = 5;
            for (int i = 0; i < maxBounces; i++)
            {
                Mobile lastTarget = targets[targets.Count - 1];
                Mobile nextTarget = null;
                double closestDist = -1.0;
                IPooledEnumerable eable = Map.GetMobilesInRange(lastTarget.Location, bounceRange);
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

            for (int i = 0; i < targets.Count; i++)
            {
                Mobile source = (i == 0) ? this : targets[i - 1];
                Mobile target = targets[i];

                // Visual effect: bolt traveling from source to target
                Effects.SendMovingParticles(
                    new Entity(Serial.Zero, source.Location, source.Map),
                    new Entity(Serial.Zero, target.Location, target.Map),
                    0x3818, 7, 0, false, false, UniqueHue, 0, 9502, 1, 0, EffectLayer.Head, 0x100);

                // Apply damage with a slight delay per bounce for visual timing
                Mobile damageTarget = target;
                Timer.DelayCall(TimeSpan.FromSeconds(i * 0.15), () =>
                {
                    if (CanBeHarmful(damageTarget, false))
                    {
                        DoHarmful(damageTarget);
                        int damage = Utility.RandomMinMax(25, 40);
                        AOS.Damage(damageTarget, this, damage, 0, 0, 0, 0, 100);
                        damageTarget.FixedParticles(0x374A, 1, 15, 9502, EffectLayer.Waist);
                    }
                });
            }
        }

        // --- Death Effect: Arcane Shattering ---
        public override void OnDeath(Container c)
        {
            if (Map == null)
            {
                base.OnDeath(c);
                return;
            }

            this.Say("*The binding shatters!*");
            PlaySound(0x211);
            Effects.SendLocationParticles(EffectItem.Create(this.Location, this.Map, EffectItem.DefaultDuration),
                                          0x3709, 10, 60, UniqueHue, 0, 5052, 0);

            // Spawn hazardous ManaDrainTile effects around the corpse
            int hazardCount = Utility.RandomMinMax(4, 7);
            for (int i = 0; i < hazardCount; i++)
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

                ManaDrainTile drainTile = new ManaDrainTile();
                drainTile.Hue = UniqueHue;
                drainTile.MoveToWorld(hazardLocation, this.Map);

                Effects.SendLocationParticles(EffectItem.Create(hazardLocation, this.Map, EffectItem.DefaultDuration),
                                              0x376A, 10, 20, UniqueHue, 0, 5039, 0);
            }

            base.OnDeath(c);
        }

        // --- Loot Generation ---
        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 2);
            AddLoot(LootPack.HighScrolls, Utility.RandomMinMax(1, 2));
            AddLoot(LootPack.Gems, Utility.RandomMinMax(8, 12));
            // Chance for a unique magical artifact (replace with your custom item)
            if (Utility.RandomDouble() < 0.02)
                PackItem(new MaxxiaScroll());
        }

        public override bool BleedImmune { get { return true; } }
        public override int TreasureMapLevel { get { return 6; } }
        public override double DispelDifficulty { get { return 140.0; } }
        public override double DispelFocus { get { return 70.0; } }

        // --- Serialization ---
        public BoundKirin(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)1); // version
            writer.Write(m_NextBindingTime);
            writer.Write(m_NextDischargeTime);
            writer.Write(m_NextChainTime);
            writer.Write(m_LastLocation);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_NextBindingTime = reader.ReadDateTime();
            m_NextDischargeTime = reader.ReadDateTime();
            m_NextChainTime = reader.ReadDateTime();
            m_LastLocation = reader.ReadPoint3D();

            // Reinitialize cooldowns if needed
            if (DateTime.UtcNow > m_NextBindingTime)
                m_NextBindingTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 18));
            if (DateTime.UtcNow > m_NextDischargeTime)
                m_NextDischargeTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(8, 12));
            if (DateTime.UtcNow > m_NextChainTime)
                m_NextChainTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
        }
    }
}
