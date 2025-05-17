using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Spells;

namespace Server.Mobiles
{
    [CorpseName("a bone steed corpse")]
    public class BoneSteed : BaseMount
    {
        // Ability cooldowns
        private DateTime m_NextStampede;
        private DateTime m_NextShardBarrage;
        private DateTime m_NextSummon;
        private Point3D m_LastLocation;

        // Pale bone‑white glow
        private const int UniqueHue = 2401;

        [Constructable]
        public BoneSteed() 
            : base("a Bone Steed", 793, 0x3EBB, AIType.AI_Melee, FightMode.Closest, 12, 1, 0.2, 0.4)
        {
            Hue = UniqueHue;
            BaseSoundID = 0x78; // skeletal horse sound

            // —— Stats ——
            SetStr(350, 450);
            SetDex(200, 250);
            SetInt(100, 150);

            SetHits(2000, 2400);
            SetStam(300, 350);
            SetMana(200, 250);

            SetDamage(20, 30);

            SetDamageType(ResistanceType.Physical, 70);
            SetDamageType(ResistanceType.Cold, 30);

            SetResistance(ResistanceType.Physical, 50, 60);
            SetResistance(ResistanceType.Cold, 40, 50);
            SetResistance(ResistanceType.Poison, 60, 70);
            SetResistance(ResistanceType.Energy, 30, 40);

            SetSkill(SkillName.Tactics, 100.0, 120.0);
            SetSkill(SkillName.Wrestling, 100.0, 120.0);
            SetSkill(SkillName.MagicResist, 90.0, 110.0);

            Fame = 25000;
            Karma = -25000;
            VirtualArmor = 90;
            ControlSlots = 4; // boss slot cost
            Tamable = false;

            // Initialize cooldowns
            var now = DateTime.UtcNow;
            m_NextStampede      = now + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
            m_NextShardBarrage  = now + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_NextSummon        = now + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 35));

            m_LastLocation = this.Location;
        }

        // —— Passive movement effect: Necrotic Aura + Cursed Hoofprints ——
        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            base.OnMovement(m, oldLocation);

            // Necrotic Aura: 20% chance to strike anyone stepping within 2 tiles
            if (m != this && m.Map == this.Map && m.InRange(this.Location, 2) 
                && Alive && m.Alive && Utility.RandomDouble() < 0.20)
            {
                if (m is Mobile target && CanBeHarmful(target, false))
                {
                    DoHarmful(target);

                    // Cold‑heavy strike
                    AOS.Damage(target, this, Utility.RandomMinMax(15, 25), 0, 100, 0, 0, 0);
                    target.SendMessage("You feel a chill as the Bone Steed's aura drains you!");

                    // Apply deadly poison
                    target.ApplyPoison(this, Poison.Deadly);
                    target.FixedParticles(0x374A, 10, 15, 5032, UniqueHue, 0, EffectLayer.Head);
                    target.PlaySound(0x48F);
                }
            }

            // Cursed Hoofprint: 10% chance to drop a flame‑bone hazard at previous tile
            if (this.Location != m_LastLocation && Utility.RandomDouble() < 0.10)
            {
                Point3D loc = oldLocation;
                int z = this.Map.GetAverageZ(loc.X, loc.Y);
                if (this.Map.CanFit(loc.X, loc.Y, z, 16, false, false))
                {
                    var tile = new NecromanticFlamestrikeTile();
                    tile.Hue = UniqueHue;
                    tile.MoveToWorld(new Point3D(loc.X, loc.Y, z), this.Map);
                }
            }

            m_LastLocation = this.Location;
        }

        // —— Main AI loop: triggers special attacks ——
        public override void OnThink()
        {
            base.OnThink();

            if (Combatant == null || !Alive || Map == null || Map == Map.Internal)
                return;

            var now = DateTime.UtcNow;

            // Spectral Stampede (heavy frontal AoE)
            if (now >= m_NextStampede && InRange(Combatant.Location, 12))
            {
                SpectralStampede();
                m_NextStampede = now + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            }
            // Bone Shard Barrage (projectile hail)
            else if (now >= m_NextShardBarrage && InRange(Combatant.Location, 10))
            {
                BoneShardBarrage();
                m_NextShardBarrage = now + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 18));
            }
            // Summon allied skeletons
            else if (now >= m_NextSummon && Hits < HitsMax * 0.75)
            {
                SummonSkeletalKin();
                m_NextSummon = now + TimeSpan.FromSeconds(Utility.RandomMinMax(30, 40));
            }
        }

        // —— Spectral Stampede: AoE physical + cold damage around path ——
        public void SpectralStampede()
        {
            if (Map == null) return;
            this.Say("*Feel my charge!*");
            this.PlaySound(0x11D);

            var targets = new List<Mobile>();
            var eable = Map.GetMobilesInRange(this.Location, 6);
            foreach (Mobile m in eable)
            {
                if (m != this && CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(this, m))
                    targets.Add(m);
            }
            eable.Free();

            foreach (var t in targets)
            {
                DoHarmful(t);
                AOS.Damage(t, this, Utility.RandomMinMax(40, 60), 100, 0, 0, 0, 0);
                t.SendMessage("The Bone Steed tramples you under spectral hooves!");
                t.FixedParticles(0x3779, 10, 15, 5032, UniqueHue, 0, EffectLayer.Waist);
                t.PlaySound(0x4F1);
            }
        }

        // —— Bone Shard Barrage: rapid projectiles at multiple foes ——
        public void BoneShardBarrage()
        {
            if (Map == null || !(Combatant is Mobile primary))
                return;

            this.Say("*Bone shards, fly!*");
            this.PlaySound(0x2CE);

            var targets = new List<Mobile>();
            var eable = Map.GetMobilesInRange(primary.Location, 8);
            foreach (Mobile m in eable)
            {
                if (m != this && CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(this, m))
                    targets.Add(m);
            }
            eable.Free();

            foreach (var t in targets)
            {
                DoHarmful(t);
                // Bone‑shard visual
                Effects.SendMovingParticles(
                    new Entity(Serial.Zero, this.Location, this.Map),
                    new Entity(Serial.Zero, t.Location, this.Map),
                    0x36B4, 7, 0, false, false, UniqueHue, 0, 0x100, 0, 0, 0);

                AOS.Damage(t, this, Utility.RandomMinMax(25, 35), 100, 0, 0, 0, 0);
                t.PlaySound(0x3F2);
            }
        }

        // —— Summon 2 skeletal mounts to fight alongside ——
        public void SummonSkeletalKin()
        {
            if (Map == null) return;
            this.Say("*Arise, my kin!*");

            for (int i = 0; i < 2; i++)
            {
                Point3D spawn = new Point3D(
                    this.X + Utility.RandomMinMax(-2, 2),
                    this.Y + Utility.RandomMinMax(-2, 2),
                    this.Z);

                if (!Map.CanFit(spawn.X, spawn.Y, spawn.Z, 16, false, false))
                    spawn.Z = Map.GetAverageZ(spawn.X, spawn.Y);

                var kin = new SkeletalMount("a skeletal steed");
                kin.Hue = UniqueHue;
                kin.MoveToWorld(spawn, Map);
                kin.Combatant = this.Combatant;
            }
        }

        // —— Death explosion: spawns poison & flame hazards ——
        public override void OnDeath(Container c)
        {
            if (Map != null)
            {
                this.Say("*My bones... reclaim me...*");
                this.PlaySound(0x1FB);
                Effects.SendLocationParticles(
                    EffectItem.Create(this.Location, Map, EffectItem.DefaultDuration),
                    0x3709, 10, 60, UniqueHue, 0, 5052, 0);

                for (int i = 0; i < 5; i++)
                {
                    int dx = Utility.RandomMinMax(-3, 3),
                        dy = Utility.RandomMinMax(-3, 3);

                    Point3D loc = new Point3D(this.X + dx, this.Y + dy, this.Z);
                    if (!Map.CanFit(loc.X, loc.Y, loc.Z, 16, false, false))
                        loc.Z = Map.GetAverageZ(loc.X, loc.Y);

                    var poisonTile = new PoisonTile();
                    poisonTile.Hue = UniqueHue;
                    poisonTile.MoveToWorld(loc, Map);
                }
            }

            base.OnDeath(c);
        }

        // —— Loot & misc properties ——
        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 2);
            AddLoot(LootPack.Gems, Utility.RandomMinMax(6, 10));
            if (Utility.RandomDouble() < 0.05)
                PackItem(new SkeletalReapersBardiche()); // very rare drop
        }

        public override bool BleedImmune => true;
        public override Poison PoisonImmune => Poison.Lethal;
        public override int TreasureMapLevel => 6;
        public override double DispelDifficulty => 150.0;
        public override double DispelFocus => 80.0;

        public BoneSteed(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            // Reset cooldowns on reload
            var now = DateTime.UtcNow;
            m_NextStampede     = now + TimeSpan.FromSeconds(10);
            m_NextShardBarrage = now + TimeSpan.FromSeconds(8);
            m_NextSummon       = now + TimeSpan.FromSeconds(15);

            m_LastLocation = this.Location;
        }
    }
}
