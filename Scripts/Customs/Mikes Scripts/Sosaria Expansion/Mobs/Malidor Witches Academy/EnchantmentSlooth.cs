using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Spells; // For potential spell effects
using Server.Network; // For visual/sound effects
using System.Collections.Generic; // For list collections

namespace Server.Mobiles
{
    [CorpseName("an enchanted slooth corpse")]
    public class EnchantmentSlooth : BaseCreature
    {
        // Timers for special abilities
        private DateTime m_NextMysticCoilTime;
        private DateTime m_NextArcaneWebTime;
        private DateTime m_NextSpellWeaveTime;
        private Point3D m_LastLocation;

        // Unique Hue – a deep, magical color
        private const int UniqueHue = 1370;

        [Constructable]
        public EnchantmentSlooth() : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "an Enchantment Slooth";
            Body = 734; // Same as the original slith
            BaseSoundID = 0x488; // Chosen magical hiss sound (adjust if needed)
            Hue = UniqueHue;

            // --- Advanced Stats ---
            SetStr(400, 500);
            SetDex(300, 350);
            SetInt(600, 700);

            SetHits(1400, 1700);
            SetStam(300, 350);
            SetMana(800, 900);

            SetDamage(20, 30);
            // Damage Types: Mostly Energy with a bit of Physical
            SetDamageType(ResistanceType.Physical, 20);
            SetDamageType(ResistanceType.Energy, 80);

            SetResistance(ResistanceType.Physical, 45, 55);
            SetResistance(ResistanceType.Fire, 50, 60);
            SetResistance(ResistanceType.Cold, 55, 65);
            SetResistance(ResistanceType.Poison, 40, 50);
            SetResistance(ResistanceType.Energy, 90, 100);

            // --- Skills: Emphasis on magic ---
            SetSkill(SkillName.EvalInt, 110.1, 125.0);
            SetSkill(SkillName.Magery, 110.1, 125.0);
            SetSkill(SkillName.MagicResist, 115.2, 130.0);
            SetSkill(SkillName.Meditation, 100.0, 110.0);
            SetSkill(SkillName.Tactics, 95.0, 105.0);
            SetSkill(SkillName.Wrestling, 95.0, 105.0);

            Fame = 25000;
            Karma = -25000;
            VirtualArmor = 90;
            ControlSlots = 5;

            Tamable = false; // Advanced monster not intended for taming

            // --- Initialize ability cooldown timers ---
            m_NextMysticCoilTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
            m_NextArcaneWebTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 25));
            m_NextSpellWeaveTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));

            m_LastLocation = this.Location;

            // --- Initial Loot: Magic Reagents & Gold ---
            PackItem(new BlackPearl(Utility.RandomMinMax(10, 15)));
            PackItem(new Nightshade(Utility.RandomMinMax(10, 15)));
            PackItem(new SpidersSilk(Utility.RandomMinMax(10, 15)));
            PackItem(new SulfurousAsh(Utility.RandomMinMax(10, 15)));
            PackGold(100, 200);
        }

        public EnchantmentSlooth(Serial serial) : base(serial)
        {
        }

        // --- Enchanted Trail (OnMovement): Drains mana and deals minor energy damage to near targets ---
        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            if (m != null && m != this && m.Map == this.Map && m.InRange(this.Location, 2) && m.Alive && this.Alive && CanBeHarmful(m, false))
            {
                if (m is Mobile targetMobile && SpellHelper.ValidIndirectTarget(this, targetMobile))
                {
                    DoHarmful(targetMobile);

                    int manaDrained = Utility.RandomMinMax(5, 15);
                    if (targetMobile.Mana >= manaDrained)
                    {
                        targetMobile.Mana -= manaDrained;
                        targetMobile.SendMessage(0x22, "You feel a surge of magical energy drain from you!");
                        targetMobile.FixedParticles(0x374A, 10, 15, 5032, EffectLayer.Head);
                        targetMobile.PlaySound(0x1F8);
                    }

                    // Minor energy damage (100% Energy)
                    AOS.Damage(targetMobile, this, Utility.RandomMinMax(5, 10), 0, 0, 0, 0, 100);
                }
            }
            base.OnMovement(m, oldLocation);
        }

        public override void OnThink()
        {
            base.OnThink();

            // --- Leave a magical trail ---
            if (this.Location != m_LastLocation && this.Map != null && this.Map != Map.Internal && Utility.RandomDouble() < 0.25)
            {
                Point3D oldLoc = m_LastLocation;
                m_LastLocation = this.Location;
                if (Map.CanFit(oldLoc.X, oldLoc.Y, oldLoc.Z, 16, false, false))
                {
                    // Spawn a TrapWeb tile as lingering enchanted residue
                    TrapWeb trap = new TrapWeb();
                    trap.Hue = UniqueHue;
                    trap.MoveToWorld(oldLoc, this.Map);
                }
                else
                {
                    int validZ = Map.GetAverageZ(oldLoc.X, oldLoc.Y);
                    if (Map.CanFit(oldLoc.X, oldLoc.Y, validZ, 16, false, false))
                    {
                        TrapWeb trap = new TrapWeb();
                        trap.Hue = UniqueHue;
                        trap.MoveToWorld(new Point3D(oldLoc.X, oldLoc.Y, validZ), this.Map);
                    }
                }
            }
            else
            {
                m_LastLocation = this.Location;
            }

            // --- Ability usage based on cooldowns ---
            if (Combatant == null || Map == null || Map == Map.Internal || !Alive)
                return;

            if (DateTime.UtcNow >= m_NextMysticCoilTime && this.InRange(Combatant.Location, 10))
            {
                MysticCoilAttack();
                m_NextMysticCoilTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            }
            else if (DateTime.UtcNow >= m_NextArcaneWebTime && this.InRange(Combatant.Location, 8))
            {
                ArcaneWebAttack();
                m_NextArcaneWebTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 35));
            }
            else if (DateTime.UtcNow >= m_NextSpellWeaveTime && this.InRange(Combatant.Location, 6))
            {
                SpellWeaveAttack();
                m_NextSpellWeaveTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 25));
            }
        }

        // --- Ability: Mystic Coil Attack (Chain-like attack) ---
        public void MysticCoilAttack()
        {
            this.Say("*The mystical coils lash out!*");
            PlaySound(0x20A);
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

            // Deliver damage and a slowing “flavor” to all targets in the chain
            for (int i = 0; i < targets.Count; ++i)
            {
                Mobile source = (i == 0) ? this : targets[i - 1];
                Mobile target = targets[i];

                // Visual effect: chain of arcane particles
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
                        damageTarget.SendMessage(0x22, "Mystical coils wrap around you, slowing your movement!");
                    }
                });
            }
        }

        // --- Ability: Arcane Web Attack (Spawns a TrapWeb hazard) ---
        public void ArcaneWebAttack()
        {
            IDamageable targetDamageable = Combatant;
            Point3D targetLocation;

            if (targetDamageable is Mobile targetMobile && CanBeHarmful(targetMobile, false))
            {
                targetLocation = targetMobile.Location;
            }
            else
            {
                targetLocation = targetDamageable.Location;
            }

            this.Say("*Web of arcane energy!*");
            PlaySound(0x22F);
            Effects.SendLocationParticles(
                EffectItem.Create(targetLocation, this.Map, EffectItem.DefaultDuration),
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

                TrapWeb webTile = new TrapWeb();
                webTile.Hue = UniqueHue;
                webTile.MoveToWorld(spawnLoc, this.Map);
                Effects.PlaySound(spawnLoc, this.Map, 0x1F6);
            });
        }

        // --- Ability: Spell Weave Attack (Area burst with additional mana drain) ---
        public void SpellWeaveAttack()
        {
            this.Say("*Woven spells converge!*");
            PlaySound(0x211);

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
                Effects.SendLocationParticles(
                    EffectItem.Create(this.Location, this.Map, EffectItem.DefaultDuration),
                    0x3709, 10, 60, UniqueHue, 0, 5032, 0);

                foreach (Mobile target in targets)
                {
                    DoHarmful(target);
                    int damage = Utility.RandomMinMax(30, 50);
                    AOS.Damage(target, this, damage, 0, 0, 0, 0, 100);
                    target.FixedParticles(0x3779, 10, 25, 5032, UniqueHue, 0, EffectLayer.Head);

                    // 50% chance to drain additional mana
                    if (Utility.RandomDouble() < 0.50)
                    {
                        if (target is Mobile targetMobile)
                        {
                            int manaDrained = Utility.RandomMinMax(15, 30);
                            if (targetMobile.Mana >= manaDrained)
                            {
                                targetMobile.Mana -= manaDrained;
                                targetMobile.SendMessage(0x22, "The woven magic siphons your arcane power!");
                                targetMobile.FixedParticles(0x374A, 10, 15, 5032, UniqueHue, 0, EffectLayer.Head);
                                targetMobile.PlaySound(0x1F8);
                            }
                        }
                    }
                }
            }
        }

        // --- OnDeath: Arcane Detonation with hazard tile spawning ---
        public override void OnDeath(Container c)
        {
            if (Map != null)
            {
                this.Say("*Enchantment... severed!*");
                Effects.PlaySound(this.Location, this.Map, 0x211);
                Effects.SendLocationParticles(
                    EffectItem.Create(this.Location, this.Map, EffectItem.DefaultDuration),
                    0x3709, 10, 60, UniqueHue, 0, 5052, 0);

                // Spawn multiple hazard tiles (a mix of ManaDrainTile and TrapWeb)
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

                    if (Utility.RandomDouble() < 0.5)
                    {
                        ManaDrainTile tile = new ManaDrainTile();
                        tile.Hue = UniqueHue;
                        tile.MoveToWorld(hazardLocation, this.Map);
                    }
                    else
                    {
                        TrapWeb tile = new TrapWeb();
                        tile.Hue = UniqueHue;
                        tile.MoveToWorld(hazardLocation, this.Map);
                    }
                    Effects.SendLocationParticles(
                        EffectItem.Create(hazardLocation, this.Map, EffectItem.DefaultDuration),
                        0x376A, 10, 20, UniqueHue, 0, 5039, 0);
                }
            }
            base.OnDeath(c);
        }

        public override bool BleedImmune { get { return true; } }
        public override int TreasureMapLevel { get { return 6; } }
        public override double DispelDifficulty { get { return 140.0; } }
        public override double DispelFocus { get { return 70.0; } }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 2);
            AddLoot(LootPack.Average);
            AddLoot(LootPack.HighScrolls, Utility.RandomMinMax(1, 2));
            AddLoot(LootPack.Gems, Utility.RandomMinMax(8, 12));

            // Chance for a unique magical artifact (replace MaxxiaScroll with your own item as needed)
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

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version

            writer.Write(m_NextMysticCoilTime);
            writer.Write(m_NextArcaneWebTime);
            writer.Write(m_NextSpellWeaveTime);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_NextMysticCoilTime = reader.ReadDateTime();
            m_NextArcaneWebTime = reader.ReadDateTime();
            m_NextSpellWeaveTime = reader.ReadDateTime();
        }
    }
}
