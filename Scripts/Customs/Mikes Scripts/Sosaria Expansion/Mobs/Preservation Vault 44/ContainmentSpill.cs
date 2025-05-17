using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a containment spill residue")]
    public class ContainmentSpill : BaseCreature
    {
        // Timers for special abilities
        private DateTime m_NextCorrodeTime;
        private DateTime m_NextCloudTime;
        private DateTime m_NextSurgeTime;
        private DateTime m_NextRippleTime;
        private Point3D m_LastLocation;

        // Unique toxic-green hue
        private const int UniqueHue = 1350;

        [Constructable]
        public ContainmentSpill() : base(AIType.AI_Mage, FightMode.Closest, 12, 1, 0.1, 0.2)
        {
            Name = "a containment spill";
            Body = 51;               // Same as base slime
            BaseSoundID = 456;       // Same as base slime
            Hue = UniqueHue;

            // Greatly enhanced stats
            SetStr(300, 350);
            SetDex(100, 150);
            SetInt(500, 600);

            SetHits(1200, 1500);
            SetStam(150, 200);
            SetMana(700, 800);

            SetDamage(10, 15);       // Light physical, main damage via poison

            // Damage types
            SetDamageType(ResistanceType.Physical, 0);
            SetDamageType(ResistanceType.Poison, 100);

            // Resistances
            SetResistance(ResistanceType.Physical, 30, 40);
            SetResistance(ResistanceType.Fire, 10, 20);
            SetResistance(ResistanceType.Cold, 30, 40);
            SetResistance(ResistanceType.Poison, 80, 90);
            SetResistance(ResistanceType.Energy, 50, 60);

            // Skills
            SetSkill(SkillName.Poisoning, 120.0, 130.0);
            SetSkill(SkillName.EvalInt,   100.0, 110.0);
            SetSkill(SkillName.Magery,    100.0, 110.0);
            SetSkill(SkillName.MagicResist,115.0, 125.0);
            SetSkill(SkillName.Tactics,   90.0, 100.0);
            SetSkill(SkillName.Wrestling, 90.0, 100.0);

            Fame = 18000;
            Karma = -18000;

            VirtualArmor = 70;
            ControlSlots = 4;

            // Initialize cooldowns
            m_NextCorrodeTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(8, 12));
            m_NextCloudTime   = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
            m_NextSurgeTime   = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 25));
            m_NextRippleTime  = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 30));

            m_LastLocation = this.Location;

            // Loot: reagents and chance at Core
            PackItem(new BlackPearl(Utility.RandomMinMax(15, 20)));
            PackItem(new SulfurousAsh(Utility.RandomMinMax(15, 20)));
            PackItem(new SpidersSilk(Utility.RandomMinMax(15, 20)));
            PackItem(new Nightshade(Utility.RandomMinMax(15, 20)));

            if (Utility.RandomDouble() < 0.03) // 3% chance
                PackItem(new AlchemistsSmockOfStains());
        }

        public ContainmentSpill(Serial serial) : base(serial) { }

        public override Poison PoisonImmune => Poison.Lethal;
        public override Poison HitPoison     => Poison.Lethal;
        public override bool BleedImmune     => true;

        // Leave toxic ooze behind on movement
        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            base.OnMovement(m, oldLocation);

            if (this.Map == null || !this.Alive)
                return;

            // When it moves, spawn a lingering poison tile behind
            if (this.Location != m_LastLocation && Utility.RandomDouble() < 0.5)
            {
                Point3D p = m_LastLocation;
                int z = p.Z;
                if (!Map.CanFit(p.X, p.Y, p.Z, 16, false, false))
                    z = Map.GetAverageZ(p.X, p.Y);

                PoisonTile tile = new PoisonTile();        // Damaging poison ground
                tile.Hue = UniqueHue;
                tile.MoveToWorld(new Point3D(p.X, p.Y, z), this.Map);
            }

            m_LastLocation = this.Location;
        }

        public override void OnThink()
        {
            base.OnThink();

            if (!Alive || Combatant == null || Map == null || Map == Map.Internal)
                return;

            // Corrosive Catalyst: targeted armor/poison burst
            if (DateTime.UtcNow >= m_NextCorrodeTime && InRange(Combatant.Location, 8))
            {
                CorrosiveCatalyst();
                m_NextCorrodeTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 18));
            }
            // Toxic Cloud: AoE gas tile around random point near target
            else if (DateTime.UtcNow >= m_NextCloudTime && InRange(Combatant.Location, 12))
            {
                ToxicCloud();
                m_NextCloudTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 25));
            }
            // Acidic Surge: self-centered area burst
            else if (DateTime.UtcNow >= m_NextSurgeTime)
            {
                AcidicSurge();
                m_NextSurgeTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 30));
            }
            // Eruption Ripple: tile spill chain
            else if (DateTime.UtcNow >= m_NextRippleTime)
            {
                EruptionRipple();
                m_NextRippleTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(30, 40));
            }
        }

        private void CorrosiveCatalyst()
        {
            if (!(Combatant is Mobile target) || !CanBeHarmful(target, false))
                return;

            Say("*Containment breach!*");
            PlaySound(0x23B); // Acid sizzle

            // Direct hit poison damage
            DoHarmful(target);
            int damage = Utility.RandomMinMax(40, 60);
            AOS.Damage(target, this, damage, 0, 0, 0, damage, 0);

            // Temporary armor/magic resist reduction
            target.SendMessage(0x22, "Corrosive sludge weakens your defenses!");
            target.FixedParticles(0x374A, 10, 15, 5032, EffectLayer.Head);
            Timer.DelayCall(TimeSpan.FromSeconds(0.1), () =>
            {
                if (target.Alive)
                {
                    // <-- HERE: use the actual Resistances properties
					target.Resistances[(int)ResistanceType.Physical] = Math.Max(0, target.Resistances[(int)ResistanceType.Physical] - 10);
					target.Resistances[(int)ResistanceType.Fire]     = Math.Max(0, target.Resistances[(int)ResistanceType.Fire]     - 10);
					target.Resistances[(int)ResistanceType.Cold]     = Math.Max(0, target.Resistances[(int)ResistanceType.Cold]     - 10);
					target.Resistances[(int)ResistanceType.Energy]   = Math.Max(0, target.Resistances[(int)ResistanceType.Energy]   - 10);
					target.Resistances[(int)ResistanceType.Poison]   = Math.Max(0, target.Resistances[(int)ResistanceType.Poison]   - 10);

                }
            });
        }

        private void ToxicCloud()
        {
            var tgt = Combatant as Mobile;
            if (tgt == null || !CanBeHarmful(tgt, false))
                return;

            Say("*Releasing containment gas!*");
            PlaySound(0x21F);

            // Spawn a cloud tile at or near the target
            Point3D center = tgt.Location;
            center.X += Utility.RandomMinMax(-2, 2);
            center.Y += Utility.RandomMinMax(-2, 2);

            if (!Map.CanFit(center.X, center.Y, center.Z, 16, false, false))
                center.Z = Map.GetAverageZ(center.X, center.Y);

            ToxicGasTile gas = new ToxicGasTile();
            gas.Hue = UniqueHue;
            gas.MoveToWorld(center, this.Map);
        }

        private void AcidicSurge()
        {
            Say("*Acidic discharge!*");
            PlaySound(0x22C);
            Effects.SendLocationParticles(EffectItem.Create(Location, Map, EffectItem.DefaultDuration), 0x3709, 8, 20, UniqueHue, 0, 5052, 0);

            List<Mobile> targets = new List<Mobile>();
            foreach (Mobile m in Map.GetMobilesInRange(Location, 6))
            {
                if (m != this && CanBeHarmful(m, false))
                    targets.Add(m);
            }

            foreach (var m in targets)
            {
                DoHarmful(m);
                AOS.Damage(m, this, Utility.RandomMinMax(30, 50), 0, 0, 0, m.Hits / 10, 0);
                m.FixedParticles(0x3779, 8, 15, 5032, UniqueHue, 0, EffectLayer.Head);
            }
        }

        private void EruptionRipple()
        {
            Say("*Containment spill expanding!*");
            PlaySound(0x223);

            // Place a ring of poison/acid tiles around self
            for (int dx = -2; dx <= 2; dx++)
            {
                for (int dy = -2; dy <= 2; dy++)
                {
                    if (Math.Abs(dx) == 2 || Math.Abs(dy) == 2)
                    {
                        Point3D p = new Point3D(X + dx, Y + dy, Z);
                        int z = p.Z;
                        if (!Map.CanFit(p.X, p.Y, p.Z, 16, false, false))
                            z = Map.GetAverageZ(p.X, p.Y);

                        PoisonTile tile = new PoisonTile();
                        tile.Hue = UniqueHue;
                        tile.MoveToWorld(new Point3D(p.X, p.Y, z), Map);
                    }
                }
            }
        }

        public override void OnDeath(Container loot)
        {
            base.OnDeath(loot);

            if (Map == null) return;

            Say("*Leak... terminated!*");
            PlaySound(0x22C);
            Effects.SendLocationParticles(EffectItem.Create(Location, Map, EffectItem.DefaultDuration), 0x3709, 12, 30, UniqueHue, 0, 5052, 0);

            // Scatter hazardous tiles around corpse
            int count = Utility.RandomMinMax(5, 8);
            for (int i = 0; i < count; i++)
            {
                int dx = Utility.RandomMinMax(-3, 3);
                int dy = Utility.RandomMinMax(-3, 3);
                Point3D p = new Point3D(X + dx, Y + dy, Z);
                int z = p.Z;
                if (!Map.CanFit(p.X, p.Y, p.Z, 16, false, false))
                    z = Map.GetAverageZ(p.X, p.Y);

                PoisonTile tile = new PoisonTile();
                tile.Hue = UniqueHue;
                tile.MoveToWorld(new Point3D(p.X, p.Y, z), Map);
            }
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich);
            AddLoot(LootPack.Gems,    Utility.RandomMinMax(8, 12));
            AddLoot(LootPack.MedScrolls, Utility.RandomMinMax(1, 3));
        }

        public override int TreasureMapLevel => 5;
        public override double DispelDifficulty => 130.0;
        public override double DispelFocus     => 60.0;

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            // Re-init timers
            m_NextCorrodeTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(8, 12));
            m_NextCloudTime   = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
            m_NextSurgeTime   = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 25));
            m_NextRippleTime  = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 30));
        }
    }
}
