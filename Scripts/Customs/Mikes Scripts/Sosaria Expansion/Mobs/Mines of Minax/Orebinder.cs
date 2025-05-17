using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Spells;

namespace Server.Mobiles
{
    [CorpseName("an orebinder's ore-laden corpse")]
    public class Orebinder : BaseCreature
    {
        private DateTime m_NextEruption;
        private DateTime m_NextShrapnel;
        private DateTime m_NextOverload;
        private const int UniqueHue = 2101; // Metallic gray/red tone

        [Constructable]
        public Orebinder()
            : base(AIType.AI_Mage, FightMode.Closest, 12, 1, 0.2, 0.4)
        {
            Name = "an Orebinder";
            Body = Utility.RandomList(26, 50, 56);
            BaseSoundID = 0x482;
            Hue = UniqueHue;

            // Stats
            SetStr(350, 400);
            SetDex(200, 250);
            SetInt(450, 500);

            SetHits(1200, 1400);
            SetStam(200, 250);
            SetMana(500, 600);

            SetDamage(12, 18);

            // Damage types: mostly physical shard‑piercing
            SetDamageType(ResistanceType.Physical, 80);
            SetDamageType(ResistanceType.Fire, 20);

            // Resistances
            SetResistance(ResistanceType.Physical, 60, 70);
            SetResistance(ResistanceType.Fire, 50, 60);
            SetResistance(ResistanceType.Cold, 40, 50);
            SetResistance(ResistanceType.Poison, 30, 40);
            SetResistance(ResistanceType.Energy, 30, 40);

            // Skills
            SetSkill(SkillName.EvalInt, 100.1, 110.0);
            SetSkill(SkillName.Magery, 100.1, 110.0);
            SetSkill(SkillName.MagicResist, 105.0, 120.0);
            SetSkill(SkillName.Meditation, 90.0, 100.0);
            SetSkill(SkillName.Tactics, 95.0, 105.0);
            SetSkill(SkillName.Wrestling, 95.0, 105.0);

            Fame = 18000;
            Karma = -18000;

            VirtualArmor = 75;
            ControlSlots = 4;

            // Ability cooldowns
            m_NextShrapnel = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
            m_NextEruption   = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 18));
            m_NextOverload   = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 25));

            // Starter loot: metal bars & gems
            PackItem(new IronOre(Utility.RandomMinMax(20, 30)));
            PackItem(new GoldOre(Utility.RandomMinMax(10, 20)));
            PackItem(new Diamond(Utility.RandomMinMax(10, 20)));
        }

        // Magnetic Grip Aura: drains stamina when foes move within 2 tiles
        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            base.OnMovement(m, oldLocation);

            if (m != this && m.Map == Map && Alive && m.InRange(Location, 2) && CanBeHarmful(m, false))
            {
                if (m is Mobile target)
                {
                    DoHarmful(target);
                    int drain = Utility.RandomMinMax(10, 20);
                    if (target.Stam >= drain)
                    {
                        target.Stam -= drain;
                        target.SendMessage(0x22, "The Orebinder's aura leeches your strength!");
                        target.FixedParticles(0x376A, 10, 15, 5032, UniqueHue, 0, EffectLayer.Waist);
                        target.PlaySound(0x1F7);
                        AOS.Damage(target, this, Utility.RandomMinMax(5, 10), 100, 0, 0, 0, 0);
                    }
                }
            }
        }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant == null || Map == null || !Alive) return;

            // Shrapnel Barrage: chain‑bounce ore shards
            if (DateTime.UtcNow >= m_NextShrapnel && InRange(Combatant.Location, 12))
            {
                ShrapnelBarrage();
                m_NextShrapnel = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 35));
            }
            // Molten Vein Eruption: spawn lava tiles
            else if (DateTime.UtcNow >= m_NextEruption && InRange(Combatant.Location, 10))
            {
                MoltenEruption();
                m_NextEruption = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(18, 24));
            }
            // Tectonic Overload: drop landmine hazards
            else if (DateTime.UtcNow >= m_NextOverload && InRange(Combatant.Location, 8))
            {
                TectonicOverload();
                m_NextOverload = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(30, 40));
            }
        }

        // --- Shrapnel Barrage ---
        private void ShrapnelBarrage()
        {
            Say("*Feel the fury of earth!*");
            PlaySound(0x2F3);

            if (!(Combatant is Mobile initial)) return;
            List<Mobile> hits = new List<Mobile> { initial };

            // Bounce up to 4 times
            for (int i = 0; i < 4; i++)
            {
                Mobile last = hits[hits.Count - 1];
                Mobile next = null;
                double bestDist = double.MaxValue;

                foreach (Mobile m in Map.GetMobilesInRange(last.Location, 8))
                {
                    if (m != this && m != last && !hits.Contains(m) && CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(last, m))
                    {
                        double d = last.GetDistanceToSqrt(m);
                        if (d < bestDist)
                        {
                            bestDist = d;
                            next = m;
                        }
                    }
                }

                if (next != null)
                    hits.Add(next);
                else
                    break;
            }

            // Fire the shards
            for (int i = 0; i < hits.Count; i++)
            {
                Mobile src = (i == 0 ? this : hits[i - 1]);
                Mobile tgt = hits[i];

                Effects.SendMovingParticles(
                    new Entity(Serial.Zero, src.Location, src.Map),
                    new Entity(Serial.Zero, tgt.Location, tgt.Map),
                    0x36D4, 7, 0, false, false, UniqueHue, 0, 9502, 0, 0, EffectLayer.Waist, 0);

                Timer.DelayCall(TimeSpan.FromSeconds(0.1 * i), () =>
                {
                    if (CanBeHarmful(tgt, false))
                    {
                        DoHarmful(tgt);
                        int dmg = Utility.RandomMinMax(30, 45);
                        AOS.Damage(tgt, this, dmg, 100, 0, 0, 0, 0);
                        tgt.FixedParticles(0x3779, 10, 15, 5032, UniqueHue, 0, EffectLayer.Head);
                    }
                });
            }
        }

        // --- Molten Vein Eruption ---
        private void MoltenEruption()
        {
            Say("*The veins of the earth awaken!*");
            PlaySound(0x307);

            for (int i = 0; i < 5; i++)
            {
                int dx = Utility.RandomMinMax(-4, 4), dy = Utility.RandomMinMax(-4, 4);
                Point3D loc = new Point3D(X + dx, Y + dy, Z);

                if (!Map.CanFit(loc.X, loc.Y, loc.Z, 16, false, false))
                    loc.Z = Map.GetAverageZ(loc.X, loc.Y);

                HotLavaTile lava = new HotLavaTile();
                lava.Hue = UniqueHue;
                lava.MoveToWorld(loc, Map);

                Effects.SendLocationParticles(
                    EffectItem.Create(loc, Map, EffectItem.DefaultDuration),
                    0x3709, 8, 20, UniqueHue, 0, 5032, 0);
            }
        }

        // --- Tectonic Overload ---
        private void TectonicOverload()
        {
            Say("*Earth, rend and destroy!*");
            PlaySound(0x1F7);

            for (int i = 0; i < 6; i++)
            {
                int dx = Utility.RandomMinMax(-3, 3), dy = Utility.RandomMinMax(-3, 3);
                Point3D loc = new Point3D(X + dx, Y + dy, Z);

                if (!Map.CanFit(loc.X, loc.Y, loc.Z, 16, false, false))
                    loc.Z = Map.GetAverageZ(loc.X, loc.Y);

                LandmineTile mine = new LandmineTile();
                mine.Hue = UniqueHue;
                mine.MoveToWorld(loc, Map);
            }
        }

        // --- Death Explosion ---
        public override void OnDeath(Container c)
        {
            if (Map != null)
            {
                Say("*My bonds... undone.*");
                Effects.PlaySound(Location, Map, 0x307);
                Effects.SendLocationParticles(
                    EffectItem.Create(Location, Map, EffectItem.DefaultDuration),
                    0x3709, 12, 30, UniqueHue, 0, 5032, 0);

                // Scatter a few molten hazards
                for (int i = 0; i < 4; i++)
                {
                    int dx = Utility.RandomMinMax(-2, 2), dy = Utility.RandomMinMax(-2, 2);
                    Point3D loc = new Point3D(X + dx, Y + dy, Z);

                    if (!Map.CanFit(loc.X, loc.Y, loc.Z, 16, false, false))
                        loc.Z = Map.GetAverageZ(loc.X, loc.Y);

                    HotLavaTile lava = new HotLavaTile();
                    lava.Hue = UniqueHue;
                    lava.MoveToWorld(loc, Map);
                }
            }

            base.OnDeath(c);
        }

        // Loot and immunities
        public override bool BleedImmune => true;
        public override int TreasureMapLevel => 6;
        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 2);
            AddLoot(LootPack.Gems, Utility.RandomMinMax(10, 15));
            PackItem(new IronOre(Utility.RandomMinMax(30, 50)));

            if (Utility.RandomDouble() < 0.03) // 3% chance for rare core
                PackItem(new VoidCore());
        }

        public Orebinder(Serial serial) : base(serial) { }
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }
        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_NextShrapnel = DateTime.UtcNow;
            m_NextEruption   = DateTime.UtcNow;
            m_NextOverload   = DateTime.UtcNow;
        }
    }
}
