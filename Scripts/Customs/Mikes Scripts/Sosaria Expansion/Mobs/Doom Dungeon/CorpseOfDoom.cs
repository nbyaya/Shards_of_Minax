using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Spells;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("the Corpse of Doom")]
    public class CorpseOfDoom : BaseCreature
    {
        private DateTime m_NextAuraTime;
        private DateTime m_NextShatterTime;
        private DateTime m_NextShardsTime;
        private Point3D m_LastLocation;

        // Unique sickly green hue
        private const int UniqueHue = 2001;

        [Constructable]
        public CorpseOfDoom()
            : base(AIType.AI_Mage, FightMode.Closest, 12, 1, 0.2, 0.4)
        {
            Name           = "Corpse of Doom";
            Body           = 155;
            BaseSoundID    = 471;
            Hue            = UniqueHue;

            // Stats
            SetStr(400, 450);
            SetDex(100, 150);
            SetInt(250, 300);

            SetHits(1800, 2000);
            SetStam(200, 250);
            SetMana(400, 500);

            SetDamage(20, 25);

            // Damage types
            SetDamageType(ResistanceType.Physical, 20);
            SetDamageType(ResistanceType.Cold, 30);
            SetDamageType(ResistanceType.Poison, 50);

            // Resistances
            SetResistance(ResistanceType.Physical, 50, 60);
            SetResistance(ResistanceType.Fire, 30, 40);
            SetResistance(ResistanceType.Cold, 60, 70);
            SetResistance(ResistanceType.Poison, 80, 90);
            SetResistance(ResistanceType.Energy, 40, 50);

            // Skills
            SetSkill(SkillName.Poisoning,    120.0);
            SetSkill(SkillName.EvalInt,      100.0, 120.0);
            SetSkill(SkillName.Magery,       100.0, 120.0);
            SetSkill(SkillName.MagicResist,  110.0, 130.0);
            SetSkill(SkillName.Meditation,   100.0, 110.0);
            SetSkill(SkillName.Tactics,      100.0);
            SetSkill(SkillName.Wrestling,     95.0);

            Fame           = 25000;
            Karma         = -25000;
            VirtualArmor  = 100;
            ControlSlots  = 5;

            // Cooldowns
            var now = DateTime.UtcNow;
            m_NextAuraTime    = now + TimeSpan.FromSeconds(5);
            m_NextShatterTime = now + TimeSpan.FromSeconds(15);
            m_NextShardsTime  = now + TimeSpan.FromSeconds(25);

            m_LastLocation = this.Location;

            // Initial loot
            PackItem(new Bone(Utility.RandomMinMax(20,30)));
            PackItem(new BlackPearl(Utility.RandomMinMax(15,25)));
        }

        // --- Necrotic Aura: periodic poison/necrotic damage to nearby movers ---
        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            if (DateTime.UtcNow >= m_NextAuraTime && m != this && m.Map == this.Map && m.InRange(this, 3) && this.Alive && CanBeHarmful(m, false))
            {
                m_NextAuraTime = DateTime.UtcNow + TimeSpan.FromSeconds(8);

                if (m is Mobile target && SpellHelper.ValidIndirectTarget(this, target))
                {
                    DoHarmful(target);

                    // Direct poison hit
                    target.ApplyPoison(this, Poison.Deadly);
                    target.FixedParticles(0x374A, 10, 15, 5032, UniqueHue, 0, EffectLayer.Head);
                    target.PlaySound(0x1F8);
                }
            }

            base.OnMovement(m, oldLocation);
        }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant == null || !Alive || Map == null || Map == Map.Internal)
                return;

            var now = DateTime.UtcNow;

            // 1. Necrotic Shatter: AoE burst of PoisonTiles around itself
            if (now >= m_NextShatterTime && this.InRange(Combatant.Location, 8))
            {
                NecroticShatter();
                m_NextShatterTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            }
            // 2. Bone Shard Volley: targeted chain projectile
            else if (now >= m_NextShardsTime && this.InRange(Combatant.Location, 12))
            {
                BoneShardVolley();
                m_NextShardsTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 35));
            }
        }

        private void NecroticShatter()
        {
            Say("*Rrrrrrrr…*");
            PlaySound(0x20B);

            // Create a ring of PoisonTiles
            for (int i = 0; i < 8; i++)
            {
                double angle = i * (Math.PI * 2 / 8);
                int x = X + (int)(Math.Cos(angle) * 5);
                int y = Y + (int)(Math.Sin(angle) * 5);
                int z = Z;

                if (!Map.CanFit(x, y, z, 16, false, false))
                    z = Map.GetAverageZ(x, y);

                PoisonTile tile = new PoisonTile();
                tile.Hue = UniqueHue;
                tile.MoveToWorld(new Point3D(x, y, z), Map);
            }
        }

        private void BoneShardVolley()
        {
            if (!(Combatant is Mobile initial) || !CanBeHarmful(initial, false))
                return;

            Say("*Feel the sting of shattered bone!*");
            PlaySound(0x240);

            var targets = new List<Mobile> { initial };
            int max = 4;
            double range = 6.0;

            // find additional targets
            foreach (Mobile m in Map.GetMobilesInRange(initial.Location, (int)range))
            {
                if (targets.Count >= max) break;
                if (m != this && m != initial && CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(this, m))
                    targets.Add(m);
            }

            // fire a shard at each
            for (int i = 0; i < targets.Count; i++)
            {
                var tgt = targets[i];
                Effects.SendMovingParticles(
                    new Entity(Serial.Zero, this.Location, Map),
                    new Entity(Serial.Zero, tgt.Location, Map),
                    0x36D4, 7, 12, false, false, UniqueHue, 0, 9502, 0, 0, EffectLayer.Waist, 0);

                Timer.DelayCall(TimeSpan.FromSeconds(0.1 * i), () =>
                {
                    if (CanBeHarmful(tgt, false))
                    {
                        DoHarmful(tgt);
                        AOS.Damage(tgt, this, Utility.RandomMinMax(30, 45), 0, 0, 0, 0, 100);
                        tgt.FixedParticles(0x3709, 10, 15, 5032, UniqueHue, 0, EffectLayer.Head);
                    }
                });
            }
        }

        public override void OnDeath(Container c)
        {
            if (Map != null)
            {
                Say("*My curse… remains!*");
                PlaySound(0x212);

                // Spawn toxic gas clouds around corpse
                for (int i = 0; i < 6; i++)
                {
                    int dx = Utility.RandomMinMax(-3, 3);
                    int dy = Utility.RandomMinMax(-3, 3);
                    Point3D loc = new Point3D(X + dx, Y + dy, Z);

                    if (!Map.CanFit(loc.X, loc.Y, loc.Z, 16, false, false))
                        loc.Z = Map.GetAverageZ(loc.X, loc.Y);

                    ToxicGasTile gas = new ToxicGasTile();
                    gas.Hue = UniqueHue;
                    gas.MoveToWorld(loc, Map);
                }
            }

            base.OnDeath(c);
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 2);
            AddLoot(LootPack.Gems,         Utility.RandomMinMax(10, 15));
            AddLoot(LootPack.MedScrolls,   Utility.RandomMinMax(2, 4));

            // 5% chance for a special Doom‐touched sash
            if (Utility.RandomDouble() < 0.05)
                PackItem(new NecroticSash());
        }

        public override bool BleedImmune   => true;
        public override Poison PoisonImmune => Poison.Lethal;
        public override Poison HitPoison    => Poison.Lethal;
        public override int TreasureMapLevel => 7;
        public override TribeType Tribe     => TribeType.Undead;
        public override OppositionGroup OppositionGroup => OppositionGroup.FeyAndUndead;

        public CorpseOfDoom(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            // reset timers on reload
            var now = DateTime.UtcNow;
            m_NextAuraTime    = now + TimeSpan.FromSeconds(5);
            m_NextShatterTime = now + TimeSpan.FromSeconds(15);
            m_NextShardsTime  = now + TimeSpan.FromSeconds(25);
        }
    }
}
