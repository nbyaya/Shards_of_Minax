using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Spells;      // For spell effects
using Server.Network;     // For particle and sound effects
using System.Collections.Generic;
using Server.Spells.Seventh; // (If you need chain lightning-like effects, etc.)

namespace Server.Mobiles
{
    [CorpseName("an enchanted dragon corpse")]
    public class EnchantmentDragon : BaseCreature
    {
        // Ability cooldown timers
        private DateTime m_NextChainBlastTime;
        private DateTime m_NextOverloadTime;
        private DateTime m_NextRunicEntombmentTime;
        private Point3D m_LastLocation;

        // Unique hue for this monster (for example, a magical purple tone)
        private const int UniqueHue = 1289;

        [Constructable]
        public EnchantmentDragon() 
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "an Enchantment Dragon";
            Body = 103;             // Same as FierceDragon
            BaseSoundID = 362;        // Same as FierceDragon
            Hue = UniqueHue;

            // --- Boss-Level Stats ---
            SetStr(7000, 7100);
            SetDex(150, 200);
            SetInt(950, 1000);

            SetHits(5000, 5200);
            SetStam(300, 350);
            SetMana(800, 900);

            // --- Damage: A mix of physical and potent energy (arcane) damage ---
            SetDamage(70, 100);
            SetDamageType(ResistanceType.Physical, 10);
            SetDamageType(ResistanceType.Energy, 90);

            // --- Resistances ---
            SetResistance(ResistanceType.Physical, 70, 80);
            SetResistance(ResistanceType.Fire, 60, 70);
            SetResistance(ResistanceType.Cold, 60, 70);
            SetResistance(ResistanceType.Poison, 50, 60);
            SetResistance(ResistanceType.Energy, 95, 100);

            // --- Skills: Highly adept in magical arts ---
            SetSkill(SkillName.EvalInt, 115.0, 130.0);
            SetSkill(SkillName.Magery, 115.0, 130.0);
            SetSkill(SkillName.MagicResist, 120.0, 135.0);
            SetSkill(SkillName.Meditation, 110.0, 120.0);
            SetSkill(SkillName.Tactics, 100.0, 110.0);
            SetSkill(SkillName.Wrestling, 100.0, 110.0);

            Fame = 25000;
            Karma = -25000;
            VirtualArmor = 100;
            ControlSlots = 5;

            // --- Initialize ability cooldowns ---
            m_NextChainBlastTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
            m_NextOverloadTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_NextRunicEntombmentTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));

            m_LastLocation = this.Location;

            // --- Loot: Magic reagents for flavor ---
            PackItem(new BlackPearl(Utility.RandomMinMax(10, 15)));
            PackItem(new Nightshade(Utility.RandomMinMax(10, 15)));
            PackItem(new SpidersSilk(Utility.RandomMinMax(10, 15)));
            PackItem(new SulfurousAsh(Utility.RandomMinMax(10, 15)));
        }

        // --- OnMovement: Enchantment Aura effect ---
        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            if (m != null && m != this && m.Map == this.Map && m.InRange(this.Location, 2) && m.Alive && this.Alive && CanBeHarmful(m, false))
            {
                // Use our Mobile check before accessing properties
                if (m is Mobile targetMobile && SpellHelper.ValidIndirectTarget(this, targetMobile))
                {
                    DoHarmful(targetMobile);

                    // Drain mana and deliver a feedback message
                    int manaDrain = Utility.RandomMinMax(15, 25);
                    if (targetMobile.Mana >= manaDrain)
                    {
                        targetMobile.Mana -= manaDrain;
                        targetMobile.SendMessage(0x22, "The Enchantment Dragon's aura disrupts your magic!");
                        targetMobile.FixedParticles(0x374A, 10, 15, 5032, UniqueHue, 0, EffectLayer.Head);
                        targetMobile.PlaySound(0x1F8);
                    }

                    // Also inflict minor energy damage
                    AOS.Damage(targetMobile, this, Utility.RandomMinMax(10, 15), 0, 0, 0, 0, 100);
                }
            }

            base.OnMovement(m, oldLocation);
        }

        // --- OnThink: Ability selection and movement effects ---
        public override void OnThink()
        {
            base.OnThink();

            if (Combatant == null || Map == null || Map == Map.Internal || !Alive)
                return;

            // --- Ability: Chain Enchantment Blast (bounces among targets) ---
            if (DateTime.UtcNow >= m_NextChainBlastTime && this.InRange(Combatant.Location, 10))
            {
                ChainEnchantmentBlast();
                m_NextChainBlastTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
            }
            // --- Ability: Mystic Overload Attack (AoE mana drain + damage) ---
            else if (DateTime.UtcNow >= m_NextOverloadTime && this.InRange(Combatant.Location, 8))
            {
                MysticOverloadAttack();
                m_NextOverloadTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            }
            // --- Ability: Runic Entombment (creates ensnaring web hazards) ---
            else if (DateTime.UtcNow >= m_NextRunicEntombmentTime && this.InRange(Combatant.Location, 12))
            {
                RunicEntombment();
                m_NextRunicEntombmentTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            }

            // --- Movement Effect: Leave behind magical residue (VortexTile) ---
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

        // --- Ability: Chain Enchantment Blast ---
        public void ChainEnchantmentBlast()
        {
            if (Combatant == null || Map == null)
                return;

            this.Say("*The enchantments surge forth!*");
            PlaySound(0x20A);

            List<Mobile> targets = new List<Mobile>();
            Mobile currentTarget = Combatant as Mobile;

            if (currentTarget == null || !CanBeHarmful(currentTarget, false) || !SpellHelper.ValidIndirectTarget(this, currentTarget))
                return;

            targets.Add(currentTarget);

            int maxTargets = 5; // Maximum bounces
            int range = 5;      // Range in tiles for bounce
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

            // Deliver damage and visual effects per bounce
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
                        damageTarget.FixedParticles(0x374A, 1, 15, 9502, UniqueHue, 0, EffectLayer.Waist);
                    }
                });
            }
        }

        // --- Ability: Mystic Overload Attack (Area-of-Effect with mana drain) ---
        public void MysticOverloadAttack()
        {
            if (Map == null)
                return;

            PlaySound(0x211); // Magical explosion sound
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
                Effects.SendLocationParticles(EffectItem.Create(this.Location, this.Map, EffectItem.DefaultDuration), 0x376A, 10, 60, UniqueHue, 0, 5039, 0);

                foreach (Mobile target in targets)
                {
                    DoHarmful(target);
                    int damage = Utility.RandomMinMax(40, 60);
                    AOS.Damage(target, this, damage, 0, 0, 0, 0, 100);
                    target.FixedParticles(0x3779, 10, 25, 5032, UniqueHue, 0, EffectLayer.Head);

                    // 50% chance to drain additional mana
                    if (Utility.RandomDouble() < 0.50)
                    {
                        if (target is Mobile targetMobile)
                        {
                            int extraMana = Utility.RandomMinMax(20, 40);
                            if (targetMobile.Mana >= extraMana)
                            {
                                targetMobile.Mana -= extraMana;
                                targetMobile.SendMessage(0x22, "Your magical energy is overwhelmed by enchantment!");
                                targetMobile.FixedParticles(0x374A, 10, 15, 5032, UniqueHue, 0, EffectLayer.Head);
                                targetMobile.PlaySound(0x1F8);
                            }
                        }
                    }
                }
            }
        }

        // --- Ability: Runic Entombment (creates ensnaring TrapWeb hazards) ---
        public void RunicEntombment()
        {
            if (Combatant == null || Map == null)
                return;

            IDamageable targetDamageable = Combatant;
            Point3D targetLocation;

            if (targetDamageable is Mobile targetMobile && CanBeHarmful(targetMobile, false))
                targetLocation = targetMobile.Location;
            else
                targetLocation = targetDamageable.Location;

            this.Say("*Runic energies bind you!*");
            PlaySound(0x22F);
            Effects.SendLocationParticles(EffectItem.Create(targetLocation, this.Map, EffectItem.DefaultDuration), 0x3728, 10, 10, UniqueHue, 0, 5039, 0);

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

                // Spawn several TrapWeb hazards around the target location
                int tilesToSpawn = Utility.RandomMinMax(3, 5);
                for (int i = 0; i < tilesToSpawn; i++)
                {
                    int xOffset = Utility.RandomMinMax(-3, 3);
                    int yOffset = Utility.RandomMinMax(-3, 3);
                    Point3D tileLoc = new Point3D(spawnLoc.X + xOffset, spawnLoc.Y + yOffset, spawnLoc.Z);

                    if (!Map.CanFit(tileLoc.X, tileLoc.Y, tileLoc.Z, 16, false, false))
                    {
                        tileLoc.Z = Map.GetAverageZ(tileLoc.X, tileLoc.Y);
                        if (!Map.CanFit(tileLoc.X, tileLoc.Y, tileLoc.Z, 16, false, false))
                            continue;
                    }

                    TrapWeb trap = new TrapWeb();
                    trap.Hue = UniqueHue;
                    trap.MoveToWorld(tileLoc, this.Map);
                }
                Effects.PlaySound(spawnLoc, this.Map, 0x1F6);
            });
        }

        // --- OnDeath: Enchantment Detonation ---
        public override void OnDeath(Container c)
        {
            if (Map == null)
            {
                base.OnDeath(c);
                return;
            }

            this.Say("*Enchantment... unleashed!*");
            Effects.PlaySound(this.Location, this.Map, 0x211);
            Effects.SendLocationParticles(EffectItem.Create(this.Location, this.Map, EffectItem.DefaultDuration), 0x3709, 10, 60, UniqueHue, 0, 5052, 0);

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

                // Use a TrapWeb hazard as the enchanted detonation effect
                TrapWeb trap = new TrapWeb();
                trap.Hue = UniqueHue;
                trap.MoveToWorld(hazardLocation, this.Map);
                Effects.SendLocationParticles(EffectItem.Create(hazardLocation, this.Map, EffectItem.DefaultDuration), 0x376A, 10, 20, UniqueHue, 0, 5039, 0);
            }

            base.OnDeath(c);
        }

        // --- Sound Overrides ---
        public override int GetIdleSound() { return 0x2C4; }
        public override int GetAttackSound() { return 0x2C0; }
        public override int GetDeathSound() { return 0x2C1; }
        public override int GetAngerSound() { return 0x2C4; }
        public override int GetHurtSound() { return 0x2C3; }

        // --- Loot Generation ---
        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 2);
            AddLoot(LootPack.Average);
            AddLoot(LootPack.HighScrolls, Utility.RandomMinMax(1, 2));
            AddLoot(LootPack.Gems, Utility.RandomMinMax(8, 12));

            // Chance for unique enchanted artifact or scrolls
            if (Utility.RandomDouble() < 0.02)
            {
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

        public override bool BleedImmune { get { return true; } }
        public override int TreasureMapLevel { get { return 6; } } 
        public override double DispelDifficulty { get { return 140.0; } }
        public override double DispelFocus { get { return 70.0; } }

        // --- Serialization ---
        public EnchantmentDragon(Serial serial)
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

            // Reinitialize ability timers on load
            m_NextChainBlastTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
            m_NextOverloadTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_NextRunicEntombmentTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
        }
    }
}
