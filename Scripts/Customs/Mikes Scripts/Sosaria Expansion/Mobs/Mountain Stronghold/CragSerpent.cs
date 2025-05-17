using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Spells;     // For potential spell effects
using Server.Network;    // For particle effects and sounds

namespace Server.Mobiles
{
    [CorpseName("a crag serpent's corpse")]
    public class CragSerpent : BaseCreature
    {
        // Timers for unique abilities
        private DateTime m_NextRockslideTime;
        private DateTime m_NextRiftTime;
        private DateTime m_NextChainStrikeTime;
        // To track movement for residue effects
        private Point3D m_LastLocation;

        // Unique hue for the Crag Serpent (a rocky, earthen tone)
        private const int UniqueHue = 1175;

        [Constructable]
        public CragSerpent() 
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.15, 0.3)
        {
            Name = "a Crag Serpent";
            Body = 150;             // Same as the DeepSeaSerpent base body
            BaseSoundID = 447;      // Same base sound
            Hue = UniqueHue;

            // --- Stats ---
            SetStr(500, 600);
            SetDex(300, 350);
            SetInt(250, 300);

            SetHits(2000, 2500);    // High health for a boss-level foe
            SetStam(350, 400);
            SetMana(400, 500);

            SetDamage(20, 30);      // Melee damage range

            // Damage is entirely physical
            SetDamageType(ResistanceType.Physical, 100);

            // --- Resistances ---
            SetResistance(ResistanceType.Physical, 55, 65);
            SetResistance(ResistanceType.Fire, 45, 55);
            SetResistance(ResistanceType.Cold, 45, 55);
            SetResistance(ResistanceType.Poison, 40, 50);
            SetResistance(ResistanceType.Energy, 35, 45);

            // --- Skills ---
            SetSkill(SkillName.Tactics, 100, 110);
            SetSkill(SkillName.Wrestling, 100, 110);
            SetSkill(SkillName.MagicResist, 100, 110);

            Fame = 25000;
            Karma = -25000;
            VirtualArmor = 100;
            ControlSlots = 5;

            // --- Loot ---
            PackItem(new Bloodmoss(Utility.RandomMinMax(10, 15)));
            PackItem(new MandrakeRoot(Utility.RandomMinMax(10, 15)));
            // Chance for a unique drop (e.g., CragSerpentEye defined elsewhere)
            if (Utility.RandomDouble() < 0.001)
            {
                PackItem(new MaxxiaScroll());
            }

            // --- Initialize ability cooldowns ---
            m_NextRockslideTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_NextRiftTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
            m_NextChainStrikeTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 25));

            m_LastLocation = this.Location;
        }

        // --- Rockslide Aura ---
        // As foes move close, falling rocky debris damages them.
        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            if (m != null && m != this && m.Map == this.Map && m.InRange(this.Location, 2) && m.Alive && this.Alive && CanBeHarmful(m, false))
            {
                // Ensure we're working with Mobile-specific properties…
                if (m is Mobile target)
                {
                    DoHarmful(target);
                    int damage = Utility.RandomMinMax(10, 15);
                    // Deal 100% physical damage
                    AOS.Damage(target, this, damage, 100, 0, 0, 0, 0);
                    target.SendMessage(0x22, "You are hit by falling rock debris from the Crag Serpent!");
                    target.FixedParticles(0x3709, 10, 15, 5032, UniqueHue, 0, EffectLayer.Head);
                    PlaySound(0x208); // Rock-debris sound effect
                }
            }
            base.OnMovement(m, oldLocation);
        }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant == null || Map == null || Map == Map.Internal || !Alive)
                return;

            // --- Ability Priority based on cooldowns & range ---
            if (DateTime.UtcNow >= m_NextChainStrikeTime && this.InRange(Combatant.Location, 10))
            {
                ChainStrikeAttack();
                m_NextChainStrikeTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 25));
            }
            else if (DateTime.UtcNow >= m_NextRiftTime && this.InRange(Combatant.Location, 12))
            {
                CragRiftAttack();
                m_NextRiftTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
            }
            else if (DateTime.UtcNow >= m_NextRockslideTime && this.InRange(Combatant.Location, 8))
            {
                RockslideBlastAttack();
                m_NextRockslideTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            }

            // --- Movement Effect: Leave rocky residue behind ---
            if (this.Location != m_LastLocation && this.Map != null && this.Map != Map.Internal && Utility.RandomDouble() < 0.25)
            {
                Point3D oldLoc = m_LastLocation;
                m_LastLocation = this.Location;

                if (Map.CanFit(oldLoc.X, oldLoc.Y, oldLoc.Z, 16, false, false))
                {
                    EarthquakeTile quakeTile = new EarthquakeTile();
                    quakeTile.Hue = UniqueHue;
                    quakeTile.MoveToWorld(oldLoc, this.Map);
                }
                else
                {
                    int validZ = Map.GetAverageZ(oldLoc.X, oldLoc.Y);
                    if (Map.CanFit(oldLoc.X, oldLoc.Y, validZ, 16, false, false))
                    {
                        EarthquakeTile quakeTile = new EarthquakeTile();
                        quakeTile.Hue = UniqueHue;
                        quakeTile.MoveToWorld(new Point3D(oldLoc.X, oldLoc.Y, validZ), this.Map);
                    }
                }
            }
            else
            {
                m_LastLocation = this.Location;
            }
        }

        // --- Unique Ability: Rockslide Blast ---
        // An area-of-effect burst that hurls rocky debris at nearby foes.
        public void RockslideBlastAttack()
        {
            if (Map == null)
                return;

            PlaySound(0x208); // Rockslide sound
            this.FixedParticles(0x3709, 10, 30, 5052, UniqueHue, 0, EffectLayer.CenterFeet);

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
                Effects.SendLocationParticles(EffectItem.Create(this.Location, this.Map, EffectItem.DefaultDuration), 0x376A, 10, 60, UniqueHue, 0, 5039, 0);
                foreach (Mobile target in targets)
                {
                    DoHarmful(target);
                    int damage = Utility.RandomMinMax(35, 55);
                    AOS.Damage(target, this, damage, 100, 0, 0, 0, 0);
                    target.FixedParticles(0x3779, 10, 25, 5032, UniqueHue, 0, EffectLayer.Head);
                }
            }
        }

        // --- Unique Ability: Crag Rift ---
        // A targeted ability that shreds the earth beneath an enemy and spawns a hazardous tile.
        public void CragRiftAttack()
        {
            if (Combatant == null || Map == null)
                return;

            Point3D targetLocation;
            if (Combatant is Mobile targetMobile && CanBeHarmful(targetMobile, false))
                targetLocation = targetMobile.Location;
            else
                targetLocation = Combatant.Location;

            this.Say("*The earth splits beneath you!*");
            PlaySound(0x22F); // Rift sound effect

            Effects.SendLocationParticles(EffectItem.Create(targetLocation, this.Map, EffectItem.DefaultDuration), 0x3728, 10, 10, UniqueHue, 0, 5039, 0);

            Timer.DelayCall(TimeSpan.FromSeconds(0.5), delegate
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

                // Spawn an EarthquakeTile as the hazardous rift
                EarthquakeTile quakeTile = new EarthquakeTile();
                quakeTile.Hue = UniqueHue;
                quakeTile.MoveToWorld(spawnLoc, this.Map);
                Effects.PlaySound(spawnLoc, this.Map, 0x1F6);
            });
        }

        // --- Unique Ability: Serpent Chain Strike ---
        // A bouncing physical attack that links up to several targets.
        public void ChainStrikeAttack()
        {
            if (Combatant == null || Map == null)
                return;

            this.Say("*Feel the crushing force!*");
            PlaySound(0x20A); // Impact sound

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

            for (int i = 0; i < targets.Count; ++i)
            {
                Mobile source = (i == 0) ? this : targets[i - 1];
                Mobile target = targets[i];

                Effects.SendMovingParticles(
                    new Entity(Serial.Zero, source.Location, source.Map),
                    new Entity(Serial.Zero, target.Location, target.Map),
                    0x3818, 7, 0, false, false, UniqueHue, 0, 9502, 1, 0, EffectLayer.Head, 0x100);

                Mobile damageTarget = target;
                Timer.DelayCall(TimeSpan.FromSeconds(i * 0.15), delegate
                {
                    if (CanBeHarmful(damageTarget, false))
                    {
                        DoHarmful(damageTarget);
                        int damage = Utility.RandomMinMax(25, 40);
                        AOS.Damage(damageTarget, this, damage, 100, 0, 0, 0, 0);
                        damageTarget.FixedParticles(0x374A, 1, 15, 9502, EffectLayer.Waist);
                    }
                });
            }
        }

        // --- Death Effect: Rocky Detonation ---
        public override void OnDeath(Container c)
        {
            if (Map == null)
            {
                base.OnDeath(c);
                return;
            }

            this.Say("*The Crag Serpent's fury is no more...*");
            Effects.PlaySound(this.Location, this.Map, 0x211);
            Effects.SendLocationParticles(EffectItem.Create(this.Location, this.Map, EffectItem.DefaultDuration), 0x3709, 10, 60, UniqueHue, 0, 5052, 0);

            // Spawn hazardous rocky tiles around the corpse
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

                // Alternate between EarthquakeTile and LandmineTile for a varied hazard effect
                if (Utility.RandomDouble() < 0.5)
                {
                    EarthquakeTile hazard = new EarthquakeTile();
                    hazard.Hue = UniqueHue;
                    hazard.MoveToWorld(hazardLocation, this.Map);
                }
                else
                {
                    LandmineTile hazard = new LandmineTile();
                    hazard.Hue = UniqueHue;
                    hazard.MoveToWorld(hazardLocation, this.Map);
                }
                Effects.SendLocationParticles(EffectItem.Create(hazardLocation, this.Map, EffectItem.DefaultDuration), 0x376A, 10, 20, UniqueHue, 0, 5039, 0);
            }

            base.OnDeath(c);
        }

        // --- Standard Properties & Loot ---
        public override bool BleedImmune { get { return true; } }
        public override int TreasureMapLevel { get { return 6; } }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 2);
            AddLoot(LootPack.Average);
            AddLoot(LootPack.HighScrolls, Utility.RandomMinMax(1, 2));
            AddLoot(LootPack.Gems, Utility.RandomMinMax(8, 12));

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

        public CragSerpent(Serial serial) : base(serial)
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
            m_NextRockslideTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_NextRiftTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
            m_NextChainStrikeTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 25));
        }
    }
}
