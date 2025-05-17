using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Spells;      // For spell effects
using Server.Network;     // For effect sounds and visuals

namespace Server.Mobiles
{
    [CorpseName("a mystic dragon corpse")]
    public class MysticDragon : BaseCreature
    {
        // --- Timers for special abilities ---
        private DateTime m_NextMysticBreathTime;
        private DateTime m_NextArcaneRiftTime;
        private DateTime m_NextChainBlastTime;
        private Point3D m_LastLocation;

        // --- Unique Hue for the Mystic Dragon (choose any unique hue value) ---
        private const int UniqueHue = 1373;

        [Constructable]
        public MysticDragon() : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a Mystic Dragon";
            Body = Utility.RandomList(12, 59); // Same body options as FrostDragon
            BaseSoundID = 362;                // Same sound as FrostDragon
            Hue = UniqueHue;

            // --- Enhanced Stats ---
            SetStr(1500, 1600);
            SetDex(150, 180);
            SetInt(800, 900);

            SetHits(2500, 2700);
            SetDamage(30, 40);

            // Damage dealt: mix of physical and arcane energy damage
            SetDamageType(ResistanceType.Physical, 30);
            SetDamageType(ResistanceType.Energy, 70);

            // --- Resistances ---
            SetResistance(ResistanceType.Physical, 80, 90);
            SetResistance(ResistanceType.Fire, 70, 85);
            SetResistance(ResistanceType.Cold, 80, 95);
            SetResistance(ResistanceType.Poison, 70, 80);
            SetResistance(ResistanceType.Energy, 90, 100);

            // --- Skills, emphasizing magic ---
            SetSkill(SkillName.EvalInt, 120.0, 135.0);
            SetSkill(SkillName.Magery, 130.0, 145.0);
            SetSkill(SkillName.MagicResist, 125.0, 140.0);
            SetSkill(SkillName.Meditation, 100.0, 115.0);
            SetSkill(SkillName.Tactics, 110.0, 125.0);
            SetSkill(SkillName.Wrestling, 100.0, 115.0);

            Fame = 30000;
            Karma = -30000;
            VirtualArmor = 100;

            // --- Initialize ability cooldowns ---
            m_NextMysticBreathTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_NextArcaneRiftTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 25));
            m_NextChainBlastTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 30));

            m_LastLocation = this.Location;

            // --- Basic Loot ---
            PackItem(new BlackPearl(Utility.RandomMinMax(15, 20)));
            PackItem(new Nightshade(Utility.RandomMinMax(15, 20)));
            PackItem(new SpidersSilk(Utility.RandomMinMax(15, 20)));
            PackItem(new SulfurousAsh(Utility.RandomMinMax(15, 20)));
        }

        // --- Aura Effect: Arcane Resonance ---
        // Drains nearby foes’ mana and deals minor energy damage when they come too close.
        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            if (m != null && m != this && m.Map == this.Map && m.InRange(this.Location, 2) && m.Alive && this.Alive && CanBeHarmful(m, false))
            {
                if (m is Mobile target)
                {
                    DoHarmful(target);

                    int manaDrained = Utility.RandomMinMax(10, 15);
                    if (target.Mana >= manaDrained)
                    {
                        target.Mana -= manaDrained;
                        target.SendMessage(0x22, "You feel your arcane energy being siphoned by the Mystic Dragon!");
                        target.FixedParticles(0x374A, 10, 15, 5032, UniqueHue, 0, EffectLayer.Head);
                        target.PlaySound(0x1F8);
                    }

                    // Minor energy damage
                    AOS.Damage(target, this, Utility.RandomMinMax(5, 10), 0, 0, 0, 0, 100);
                }
            }

            base.OnMovement(m, oldLocation);
        }

        // --- OnThink: Check and trigger abilities ---
        public override void OnThink()
        {
            base.OnThink();

            // --- Leave Magical Residue as it moves ---
            if (this.Location != m_LastLocation && this.Map != null && this.Map != Map.Internal && Utility.RandomDouble() < 0.3)
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

            // --- Ability Checks (ensure Combatant is valid) ---
            if (Combatant == null || Map == null || Map == Map.Internal || !Alive)
                return;

            if (DateTime.UtcNow >= m_NextChainBlastTime && this.InRange(Combatant.Location, 12))
            {
                ChainMysticBlast();
                m_NextChainBlastTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 35));
            }
            else if (DateTime.UtcNow >= m_NextArcaneRiftTime && this.InRange(Combatant.Location, 14))
            {
                ArcaneRiftAttack();
                m_NextArcaneRiftTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            }
            else if (DateTime.UtcNow >= m_NextMysticBreathTime && this.InRange(Combatant.Location, 10))
            {
                MysticBreathAttack();
                m_NextMysticBreathTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
            }
        }

        // --- Ability 1: Mystic Breath Attack ---
        // Exhales a cone of arcane energy that damages foes and may spawn a random magical hazard tile.
        public void MysticBreathAttack()
        {
            if (Map == null)
                return;

            this.Say("*The Mystic Dragon exhales a torrent of arcane energy!*");
            PlaySound(0x5D0); // Example sound effect for magical breath
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
                foreach (Mobile target in targets)
                {
                    DoHarmful(target);
                    int damage = Utility.RandomMinMax(20, 30);
                    AOS.Damage(target, this, damage, 0, 0, 0, 0, 100);
                    target.FixedParticles(0x3779, 10, 25, 5032, UniqueHue, 0, EffectLayer.Head);
                    
                    // 30% chance to spawn a magical hazard at the target's location
                    if (Utility.RandomDouble() < 0.3)
                        PlaceRandomHazard(target.Location);
                }
            }
        }

        // --- Ability 2: Arcane Rift Attack ---
        // Targets the combatant's location to create a small rift that spawns a hazard tile.
        public void ArcaneRiftAttack()
        {
            if (Combatant == null || Map == null)
                return;

            Point3D targetLocation;
            if (Combatant is Mobile targetMobile && SpellHelper.ValidIndirectTarget(this, targetMobile))
                targetLocation = targetMobile.Location;
            else
                targetLocation = Combatant.Location;

            this.Say("*Reality shudders beneath my power!*");
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

                Item hazardTile = ChooseRandomHazardTile();
                if (hazardTile != null)
                {
                    hazardTile.Hue = UniqueHue;
                    hazardTile.MoveToWorld(spawnLoc, this.Map);
                }
                Effects.PlaySound(spawnLoc, this.Map, 0x1F6);
            });
        }

        // --- Ability 3: Chain Mystic Blast ---
        // Launches a chain-style attack that bounces between up to five targets with arcane energy damage.
        public void ChainMysticBlast()
        {
            if (Combatant == null || Map == null)
                return;

            this.Say("*Feel the surge of mystic energies!*");
            PlaySound(0x20A);

            List<Mobile> targets = new List<Mobile>();
            Mobile initialTarget = Combatant as Mobile;
            if (initialTarget == null || !CanBeHarmful(initialTarget, false) || !SpellHelper.ValidIndirectTarget(this, initialTarget))
                return;

            targets.Add(initialTarget);
            int maxBounces = 5, bounceRange = 5;

            for (int i = 0; i < maxBounces; ++i)
            {
                Mobile lastTarget = targets[targets.Count - 1];
                Mobile nextTarget = null;
                double closestDist = double.MaxValue;

                IPooledEnumerable eable = Map.GetMobilesInRange(lastTarget.Location, bounceRange);
                foreach (Mobile m in eable)
                {
                    if (m != this && m != lastTarget && !targets.Contains(m) && CanBeHarmful(m, false) &&
                        SpellHelper.ValidIndirectTarget(lastTarget, m) && lastTarget.InLOS(m))
                    {
                        double dist = lastTarget.GetDistanceToSqrt(m);
                        if (dist < closestDist)
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
                        int damage = Utility.RandomMinMax(25, 40);
                        AOS.Damage(damageTarget, this, damage, 0, 0, 0, 0, 100);
                        damageTarget.FixedParticles(0x374A, 1, 15, 9502, UniqueHue, 0, EffectLayer.Waist);
                    }
                });
            }
        }

        // --- Utility: Place a Random Hazard Tile ---
        public void PlaceRandomHazard(Point3D loc)
        {
            if (Map == null)
                return;

            Item hazardTile = ChooseRandomHazardTile();
            if (hazardTile == null)
                return;

            if (Map.CanFit(loc.X, loc.Y, loc.Z, 16, false, false))
            {
                hazardTile.Hue = UniqueHue;
                hazardTile.MoveToWorld(loc, this.Map);
            }
        }

        // --- Utility: Randomly select one of several magical ground hazard tiles ---
        public Item ChooseRandomHazardTile()
        {
            Type[] hazardTypes = new Type[]
            {
                typeof(ChaoticTeleportTile),
                typeof(EarthquakeTile),
                typeof(FlamestrikeHazardTile),
                typeof(HealingPulseTile),
                typeof(HotLavaTile),
                typeof(IceShardTile),
                typeof(LandmineTile),
                typeof(LightningStormTile),
                typeof(MagnetTile),
                typeof(ManaDrainTile),
                typeof(NecromanticFlamestrikeTile),
                typeof(PoisonTile),
                typeof(QuicksandTile),
                typeof(ThunderstormTile),
                typeof(ThunderstormTile2),
                typeof(ToxicGasTile),
                typeof(TrapWeb),
                typeof(VortexTile)
            };

            int index = Utility.RandomMinMax(0, hazardTypes.Length - 1);
            Type t = hazardTypes[index];
            try
            {
                return (Item)Activator.CreateInstance(t);
            }
            catch
            {
                return null;
            }
        }

        // --- Death Effect: Mystic Detonation ---
        public override void OnDeath(Container c)
        {
            if (Map == null)
            {
                base.OnDeath(c);
                return;
            }

            this.Say("*Arcane energies... unleashed!*");
            Effects.PlaySound(this.Location, this.Map, 0x211);
            Effects.SendLocationParticles(EffectItem.Create(this.Location, this.Map, EffectItem.DefaultDuration), 0x3709, 10, 60, UniqueHue, 0, 5052, 0);

            int hazards = Utility.RandomMinMax(4, 7);
            for (int i = 0; i < hazards; i++)
            {
                int xOffset = Utility.RandomMinMax(-4, 4);
                int yOffset = Utility.RandomMinMax(-4, 4);
                Point3D hazardLoc = new Point3D(this.X + xOffset, this.Y + yOffset, this.Z);

                if (!Map.CanFit(hazardLoc.X, hazardLoc.Y, hazardLoc.Z, 16, false, false))
                {
                    hazardLoc.Z = Map.GetAverageZ(hazardLoc.X, hazardLoc.Y);
                    if (!Map.CanFit(hazardLoc.X, hazardLoc.Y, hazardLoc.Z, 16, false, false))
                        continue;
                }

                Item drainTile = new ManaDrainTile();
                drainTile.Hue = UniqueHue;
                drainTile.MoveToWorld(hazardLoc, this.Map);

                Effects.SendLocationParticles(EffectItem.Create(hazardLoc, this.Map, EffectItem.DefaultDuration), 0x376A, 10, 20, UniqueHue, 0, 5039, 0);
            }

            base.OnDeath(c);
        }

        // --- Standard Properties & Loot Configuration ---
        public override bool BleedImmune { get { return true; } }
        public override int TreasureMapLevel { get { return 6; } }
        public override double DispelDifficulty { get { return 150.0; } }
        public override double DispelFocus { get { return 75.0; } }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 2);
            AddLoot(LootPack.Average);
            AddLoot(LootPack.HighScrolls, Utility.RandomMinMax(2, 3));
            AddLoot(LootPack.Gems, Utility.RandomMinMax(10, 15));

            if (Utility.RandomDouble() < 0.03)
                PackItem(new MaxxiaScroll());
            if (Utility.RandomDouble() < 0.05)
                PackItem(new MaxxiaScroll(Utility.RandomMinMax(1, 3)));
            if (Utility.RandomDouble() < 0.10)
                PackItem(new MaxxiaScroll(Utility.RandomMinMax(1, 2)));
        }

        // --- Serialization ---
        public MysticDragon(Serial serial) : base(serial)
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

            m_NextMysticBreathTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_NextArcaneRiftTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 25));
            m_NextChainBlastTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 30));
        }
    }
}
