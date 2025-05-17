using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using System.Collections.Generic;

namespace Server.Mobiles
{
    [CorpseName("a cursed molar corpse")]
    public class CursedMolar : BaseCreature
    {
        private DateTime m_NextShardTime;
        private DateTime m_NextDecayTime;
        private DateTime m_NextRuptureTime;
        private Point3D m_LastLocation;

        // A deep, blood‑red hue
        private const int UniqueHue = 1175;

        [Constructable]
        public CursedMolar() : base(AIType.AI_Mage, FightMode.Closest, 12, 1, 0.3, 0.5)
        {
            Name = "a cursed molar";
            Body = 0x311;              // same as Moloch
            BaseSoundID = 0x300;       // same as Moloch
            Hue = UniqueHue;

            // --- Stats ---
            SetStr(350, 400);
            SetDex(110, 130);
            SetInt(600, 700);

            SetHits(1800, 2200);
            SetStam(200, 250);
            SetMana(800, 900);

            SetDamage(20, 30);
            SetDamageType(ResistanceType.Physical, 40);
            SetDamageType(ResistanceType.Poison, 60);

            // --- Resistances ---
            SetResistance(ResistanceType.Physical, 50, 60);
            SetResistance(ResistanceType.Fire,     30, 40);
            SetResistance(ResistanceType.Cold,     30, 40);
            SetResistance(ResistanceType.Poison,   70, 80);
            SetResistance(ResistanceType.Energy,   60, 70);

            // --- Skills ---
            SetSkill(SkillName.EvalInt,      120.1, 130.0);
            SetSkill(SkillName.Magery,       120.1, 130.0);
            SetSkill(SkillName.MagicResist,  130.2, 140.0);
            SetSkill(SkillName.Meditation,   110.0, 120.0);
            SetSkill(SkillName.Tactics,      100.1, 110.0);
            SetSkill(SkillName.Wrestling,    100.1, 110.0);

            Fame = 25000;
            Karma = -25000;
            VirtualArmor = 90;
            ControlSlots = 6;

            // Schedule first casts
            m_NextDecayTime  = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(8, 12));
            m_NextShardTime  = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10,15));
            m_NextRuptureTime= DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20,25));
            m_LastLocation   = this.Location;
        }

        public override bool BleedImmune { get { return true; } }
        public override Poison PoisonImmune { get { return Poison.Lethal; } }

        // --- Decay Aura on Movement ---
        public override void OnMovement(Mobile m, Point3D oldLoc)
        {
            if (m is Mobile target && target != this && target.Map == Map && target.InRange(Location, 2)
                && target.Alive && Alive && CanBeHarmful(target, false))
            {
                DoHarmful(target);

                // Poison burst + damage
                int damage = Utility.RandomMinMax(10, 20);
                AOS.Damage(target, this, damage, 0, 0, 0, 100, 0);
                target.ApplyPoison(this, Poison.Deadly);

                target.SendMessage(0x22, "The cursed aura rots your flesh!");
                target.FixedParticles(0x377A, 10, 15, 5032, UniqueHue, 0, EffectLayer.Waist);
            }

            base.OnMovement(m, oldLoc);
        }

        public override void OnThink()
        {
            base.OnThink();

            if (!Alive || Map == Map.Internal || Combatant == null)
                return;

            var now = DateTime.UtcNow;

            if (now >= m_NextDecayTime)
            {
                SoulCorrosion();
                m_NextDecayTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 18));
            }
            else if (now >= m_NextShardTime && InRange(Combatant.Location, 10))
            {
                EnamelShardBarrage();
                m_NextShardTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
            }
            else if (now >= m_NextRuptureTime && InRange(Combatant.Location, 12))
            {
                RuptureEarth();
                m_NextRuptureTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 35));
            }

            // Update last location for OnMovement
            m_LastLocation = Location;
        }

        // --- Ability: Soul Corrosion (focused drain + self‑heal) ---
        public void SoulCorrosion()
        {
            if (Combatant is Mobile target && CanBeHarmful(target, false))
            {
                Say("*Your soul decays!*");
                PlaySound(0x20C);

                int drain = Utility.RandomMinMax(30, 50);
                int damage = Utility.RandomMinMax(25, 35);

                // Damage effect
                target.FixedParticles(0x376A, 10, 20, 5032, UniqueHue, 0, EffectLayer.Head);
                AOS.Damage(target, this, damage, 0, 0, 0, 100, 0);

                // Mana drain + partial heal
                if (target.Mana >= drain) target.Mana -= drain;
                Mana += drain/2;
                Hits += drain/2;
            }
        }

        // --- Ability: Enamel Shard Barrage (multi‐target projectile) ---
        public void EnamelShardBarrage()
        {
            if (!(Combatant is Mobile initial) || !CanBeHarmful(initial, false))
                return;

            Say("*Shatter!*");
            PlaySound(0x22E);

            var hits = new List<Mobile> { initial };
            IPooledEnumerable mobs = Map.GetMobilesInRange(Location, 8);
            foreach (Mobile m in mobs)
            {
                if (hits.Count >= 5) break;
                if (m != this && !hits.Contains(m) && CanBeHarmful(m, false) && InLOS(m))
                    hits.Add(m);
            }
            mobs.Free();

            foreach (var victim in hits)
            {
                Effects.SendMovingParticles(
                    new Entity(Serial.Zero, Location, Map),
                    new Entity(Serial.Zero, victim.Location, victim.Map),
                    0x2DD4, 7, 0, false, false, UniqueHue, 0, 9504, 1, 0, EffectLayer.Waist, 0);

                Timer.DelayCall(TimeSpan.FromSeconds(0.4), () =>
                {
                    if (victim.Alive && CanBeHarmful(victim, false))
                    {
                        DoHarmful(victim);
                        victim.FixedParticles(0x375A, 1, 20, 9504, EffectLayer.Waist);
                        AOS.Damage(victim, this, Utility.RandomMinMax(20, 30), 100, 0, 0, 0, 0);
                    }
                });
            }
        }

        // --- Ability: Rupture Earth (landmine hazard) ---
        public void RuptureEarth()
        {
            if (!(Combatant is Mobile t) || !CanBeHarmful(t, false))
                return;

            Point3D loc = t.Location;
            Say("*The earth rends!*");
            PlaySound(0x214);

            Effects.SendLocationParticles(
                EffectItem.Create(loc, Map, EffectItem.DefaultDuration),
                0x375A, 8, 20, UniqueHue, 0, 5039, 0);

            Timer.DelayCall(TimeSpan.FromSeconds(0.6), () =>
            {
                if (Map == null) return;

                if (!Map.CanFit(loc.X, loc.Y, loc.Z, 16, false, false))
                    loc.Z = Map.GetAverageZ(loc.X, loc.Y);

                var tile = new LandmineTile();
                tile.Hue = UniqueHue;
                tile.MoveToWorld(loc, Map);
                Effects.PlaySound(loc, Map, 0x11A);
            });
        }

        // --- Death Explosion + Poison Pools ---
        public override void OnDeath(Container c)
        {
            if (Map != null)
            {
                Say("*The tooth crumbles!*");
                PlaySound(0x211);
                Effects.SendLocationParticles(
                    EffectItem.Create(Location, Map, EffectItem.DefaultDuration),
                    0x3709, 10, 30, UniqueHue, 0, 5052, 0);

                for (int i = 0; i < Utility.RandomMinMax(3, 5); i++)
                {
                    int x = X + Utility.RandomMinMax(-3, 3);
                    int y = Y + Utility.RandomMinMax(-3, 3);
                    int z = Z;
                    if (!Map.CanFit(x, y, z, 16, false, false))
                        z = Map.GetAverageZ(x, y);

                    var p = new PoisonTile();
                    p.Hue = UniqueHue;
                    p.MoveToWorld(new Point3D(x, y, z), Map);
                }
            }
            base.OnDeath(c);
        }

        // --- Loot & Rewards ---
        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 2);
            AddLoot(LootPack.Gems, Utility.RandomMinMax(10, 15));

            // Very rare trophy
            if (Utility.RandomDouble() < 0.02)
                PackItem(new WindsOfClarity());
        }

        public override int TreasureMapLevel { get { return 7; } }
        public override double DispelDifficulty  { get { return 150.0; } }
        public override double DispelFocus       { get { return  80.0; } }

        public CursedMolar(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            // Reset timers after load
            m_NextDecayTime   = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(8, 12));
            m_NextShardTime   = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10,15));
            m_NextRuptureTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20,25));
            m_LastLocation    = Location;
        }
    }
}
