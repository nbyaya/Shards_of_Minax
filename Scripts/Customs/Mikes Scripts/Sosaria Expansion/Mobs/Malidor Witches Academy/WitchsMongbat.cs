using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Spells;       // For spell effects, etc.
using Server.Network;      // For visual effects and sounds
using System.Collections.Generic;
using Server.Spells.Seventh; // For any spell-based utilities

namespace Server.Mobiles
{
    [CorpseName("a witch's mongbat corpse")]
    public class WitchsMongbat : BaseCreature
    {
        // Timers for special abilities
        private DateTime m_NextHexTime;
        private DateTime m_NextRiftTime;
        private DateTime m_NextChainTime;
        private Point3D m_LastLocation;

        // Unique Hue for the Witch’s Mongbat – change as desired
        private const int UniqueHue = 1175;

        [Constructable]
        public WitchsMongbat() : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.1, 0.2)
        {
            Name = "Witch’s Mongbat";
            Body = 39;            // Same as base mongbat
            BaseSoundID = 422;    // Same base sound as the mongbat
            Hue = UniqueHue;

            // --- Advanced Stats ---
            SetStr(300, 350);
            SetDex(300, 350);
            SetInt(500, 600);

            SetHits(1500, 1700);
            SetStam(300, 350);
            SetMana(800, 900);

            SetDamage(20, 25);
            // Damage types: 10% Physical, 60% Energy, 30% Poison
            SetDamageType(ResistanceType.Physical, 10);
            SetDamageType(ResistanceType.Energy, 60);
            SetDamageType(ResistanceType.Poison, 30);

            // --- Resistances ---
            SetResistance(ResistanceType.Physical, 40, 50);
            SetResistance(ResistanceType.Fire, 50, 60);
            SetResistance(ResistanceType.Cold, 55, 65);
            SetResistance(ResistanceType.Poison, 70, 80);
            SetResistance(ResistanceType.Energy, 80, 90);

            // --- Skills (magic-focused) ---
            SetSkill(SkillName.EvalInt, 110.1, 125.0);
            SetSkill(SkillName.Magery, 110.1, 125.0);
            SetSkill(SkillName.MagicResist, 115.2, 130.0);
            SetSkill(SkillName.Meditation, 100.0, 110.0);
            SetSkill(SkillName.Tactics, 90.1, 100.0);
            SetSkill(SkillName.Wrestling, 90.1, 100.0);

            Fame = 25000;
            Karma = -25000;

            VirtualArmor = 90;
            ControlSlots = 5;

            // --- Initialize ability timers ---
            m_NextHexTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_NextRiftTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
            m_NextChainTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 25));

            m_LastLocation = this.Location;

            // --- Loot: Witch-themed reagents and potential unique drops ---
            PackItem(new BlackPearl(Utility.RandomMinMax(10, 15)));
            PackItem(new Nightshade(Utility.RandomMinMax(10, 15)));
            PackItem(new SpidersSilk(Utility.RandomMinMax(10, 15)));
            PackItem(new SulfurousAsh(Utility.RandomMinMax(10, 15)));
        }

        // --- OnMovement: Leaves behind a trail of toxic gas ---
        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            if (m != null && m != this && m.Map == this.Map && m.InRange(this.Location, 2) && m.Alive && this.Alive && CanBeHarmful(m, false))
            {
                if (m is Mobile target && SpellHelper.ValidIndirectTarget(this, target))
                {
                    // 20% chance to leave a toxic gas tile at the previous location
                    if (Utility.RandomDouble() < 0.20)
                    {
                        Point3D spawnLoc = oldLocation;
                        if (Map.CanFit(spawnLoc.X, spawnLoc.Y, spawnLoc.Z, 16, false, false))
                        {
                            ToxicGasTile gasTile = new ToxicGasTile();
                            gasTile.Hue = UniqueHue;
                            gasTile.MoveToWorld(spawnLoc, this.Map);
                        }
                        else
                        {
                            int validZ = Map.GetAverageZ(spawnLoc.X, spawnLoc.Y);
                            if (Map.CanFit(spawnLoc.X, spawnLoc.Y, validZ, 16, false, false))
                            {
                                ToxicGasTile gasTile = new ToxicGasTile();
                                gasTile.Hue = UniqueHue;
                                gasTile.MoveToWorld(new Point3D(spawnLoc.X, spawnLoc.Y, validZ), this.Map);
                            }
                        }
                    }
                }
            }
            base.OnMovement(m, oldLocation);
        }

        // --- OnThink: Determines which special ability to use based on cooldowns ---
        public override void OnThink()
        {
            base.OnThink();

            if (Combatant == null || Map == null || Map == Map.Internal || !Alive)
                return;

            // Prioritize abilities based on range and cooldown
            if (DateTime.UtcNow >= m_NextChainTime && this.InRange(Combatant.Location, 10))
            {
                CursedChainAttack();
                m_NextChainTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 35));
            }
            else if (DateTime.UtcNow >= m_NextRiftTime && this.InRange(Combatant.Location, 12))
            {
                NightmareRiftAttack();
                m_NextRiftTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(18, 28));
            }
            else if (DateTime.UtcNow >= m_NextHexTime && this.InRange(Combatant.Location, 8))
            {
                HexedEchoAttack();
                m_NextHexTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 20));
            }
        }

        // --- Unique Ability 1: Hexed Echo ---
        // The Witch’s Mongbat emits a bone-chilling screech that damages foes and drains their mana.
        public void HexedEchoAttack()
        {
            if (Map == null)
                return;

            PlaySound(0x1F3); // Example screech sound
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
                // Extra visual effect to show the cursed echo spreading
                Effects.SendLocationParticles(EffectItem.Create(this.Location, this.Map, EffectItem.DefaultDuration), 0x376A, 10, 60, UniqueHue, 0, 5039, 0);

                foreach (Mobile target in targets)
                {
                    DoHarmful(target);
                    int damage = Utility.RandomMinMax(30, 50);
                    // Deal pure energy damage
                    AOS.Damage(target, this, damage, 0, 100, 0, 0, 0);

                    // Drain mana if possible – always check that the target is a Mobile
                    if (target is Mobile targetMobile)
                    {
                        int manaDrained = Utility.RandomMinMax(15, 30);
                        if (targetMobile.Mana >= manaDrained)
                        {
                            targetMobile.Mana -= manaDrained;
                            targetMobile.SendMessage(0x22, "Your magical energy is sapped by a cursed echo!");
                            targetMobile.FixedParticles(0x374A, 10, 15, 5032, EffectLayer.Head);
                            targetMobile.PlaySound(0x1F8);
                        }
                    }
                }
            }
        }

        // --- Unique Ability 2: Nightmare Rift ---
        // Tears open a dark rift at the target's location, spawning a toxic hazard.
        public void NightmareRiftAttack()
        {
            if (Combatant == null || Map == null)
                return;

            IDamageable targetDamageable = Combatant;
            Point3D targetLocation;

            if (targetDamageable is Mobile target && CanBeHarmful(target, false))
                targetLocation = target.Location;
            else
                targetLocation = targetDamageable.Location;

            Say("*A nightmare unfurls!*");
            PlaySound(0x22F); // Rift sound effect

            Effects.SendLocationParticles(EffectItem.Create(targetLocation, this.Map, EffectItem.DefaultDuration), 0x3728, 10, 10, UniqueHue, 0, 5039, 0);

            Timer.DelayCall(TimeSpan.FromSeconds(0.5), () =>
            {
                if (Map == null)
                    return;

                Point3D spawnLoc = targetLocation;
                if (!Map.CanFit(spawnLoc.X, spawnLoc.Y, spawnLoc.Z, 16, false, false))
                {
                    spawnLoc.Z = Map.GetAverageZ(spawnLoc.X, spawnLoc.Y);
                    if (!Map.CanFit(spawnLoc.X, spawnLoc.Y, spawnLoc.Z, 16, false, false))
                        return;
                }

                // Use ToxicGasTile as the hazardous effect
                ToxicGasTile toxicTile = new ToxicGasTile();
                toxicTile.Hue = UniqueHue;
                toxicTile.MoveToWorld(spawnLoc, Map);

                Effects.PlaySound(spawnLoc, Map, 0x1F6);
            });
        }

        // --- Unique Ability 3: Cursed Chain Attack ---
        // The curse leaps from one foe to another, dealing combined energy and poison damage.
        public void CursedChainAttack()
        {
            if (Combatant == null || Map == null)
                return;

            Say("*The curse leaps!*");
            PlaySound(0x20A); // Energy bolt sound effect

            List<Mobile> targets = new List<Mobile>();
            Mobile initialTarget = Combatant as Mobile;
            if (initialTarget == null || !CanBeHarmful(initialTarget, false) || !SpellHelper.ValidIndirectTarget(this, initialTarget))
                return;

            targets.Add(initialTarget);
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
                    if (m != this && m != lastTarget && !targets.Contains(m) && CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(lastTarget, m) && lastTarget.InLOS(m))
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

            // Process the chain attack on each target found
            for (int i = 0; i < targets.Count; ++i)
            {
                Mobile source = (i == 0) ? this : targets[i - 1];
                Mobile target = targets[i];

                // Send a visual projectile between the source and target
                Effects.SendMovingParticles(
                    new Entity(Serial.Zero, source.Location, source.Map),
                    new Entity(Serial.Zero, target.Location, target.Map),
                    0x3818, // Projectile graphic ID
                    7, 0, false, false, UniqueHue, 0, 9502, 1, 0, EffectLayer.Head, 0x100);

                Mobile damageTarget = target;
                Timer.DelayCall(TimeSpan.FromSeconds(i * 0.15), () =>
                {
                    if (CanBeHarmful(damageTarget, false))
                    {
                        DoHarmful(damageTarget);
                        int damage = Utility.RandomMinMax(25, 40);
                        // Apply 70% energy and 30% poison damage
                        AOS.Damage(damageTarget, this, damage, 0, 70, 0, 30, 0);
                        damageTarget.FixedParticles(0x374A, 1, 15, 9502, EffectLayer.Waist);
                    }
                });
            }
        }

        // --- Death Effect: On death, create a burst of toxic hazards ---
        public override void OnDeath(Container c)
        {
            if (Map == null)
            {
                base.OnDeath(c);
                return;
            }

            Say("*The curse dissipates...*");
            Effects.PlaySound(this.Location, this.Map, 0x211); // Death explosion sound
            Effects.SendLocationParticles(EffectItem.Create(this.Location, this.Map, EffectItem.DefaultDuration), 0x3709, 10, 60, UniqueHue, 0, 5052, 0);

            int hazardsToDrop = Utility.RandomMinMax(3, 5);
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

                ToxicGasTile toxicTile = new ToxicGasTile();
                toxicTile.Hue = UniqueHue;
                toxicTile.MoveToWorld(hazardLocation, this.Map);

                Effects.SendLocationParticles(EffectItem.Create(hazardLocation, this.Map, EffectItem.DefaultDuration), 0x376A, 10, 20, UniqueHue, 0, 5039, 0);
            }

            base.OnDeath(c);
        }

        // --- Standard Overrides for Meat, Hides, Food, etc. ---
        public override int Meat { get { return 1; } }
        public override int Hides { get { return 6; } }
        public override FoodType FavoriteFood { get { return FoodType.Meat; } }
        public override bool BleedImmune { get { return true; } }
        public override int TreasureMapLevel { get { return 6; } }
        public override double DispelDifficulty { get { return 140.0; } }
        public override double DispelFocus { get { return 70.0; } }

        // --- Generate Loot ---
        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 2);
            AddLoot(LootPack.Average);
            AddLoot(LootPack.HighScrolls, Utility.RandomMinMax(1, 2));
            AddLoot(LootPack.Gems, Utility.RandomMinMax(8, 12));

            if (Utility.RandomDouble() < 0.02)
                PackItem(new MaxxiaScroll());
            if (Utility.RandomDouble() < 0.05)
                PackItem(new MaxxiaScroll(Utility.RandomMinMax(1, 3)));
            if (Utility.RandomDouble() < 0.10)
                PackItem(new MaxxiaScroll(Utility.RandomMinMax(1, 2)));
        }

        public WitchsMongbat(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(m_NextHexTime);
            writer.Write(m_NextRiftTime);
            writer.Write(m_NextChainTime);
            writer.Write(m_LastLocation);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_NextHexTime = reader.ReadDateTime();
            m_NextRiftTime = reader.ReadDateTime();
            m_NextChainTime = reader.ReadDateTime();
            m_LastLocation = reader.ReadPoint3D();

            // Reinitialize timers if they have expired
            if (m_NextHexTime < DateTime.UtcNow)
                m_NextHexTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            if (m_NextRiftTime < DateTime.UtcNow)
                m_NextRiftTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
            if (m_NextChainTime < DateTime.UtcNow)
                m_NextChainTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 25));
        }
    }
}
