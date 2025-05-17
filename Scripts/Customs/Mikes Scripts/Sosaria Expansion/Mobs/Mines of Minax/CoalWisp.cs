using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Spells;

namespace Server.Mobiles
{
    [CorpseName("a coal wisp corpse")]
    public class CoalWisp : BaseCreature
    {
        private DateTime m_NextAshCloudTime;
        private DateTime m_NextIgnitionTime;
        private DateTime m_NextShardVolleyTime;
        private const int UniqueHue = 1177; // Smoky charcoal

        [Constructable]
        public CoalWisp() : base(AIType.AI_Mage, FightMode.Closest, 12, 1, 0.2, 0.4)
        {
            Name = "a coal wisp";
            Body = 165;
            BaseSoundID = 466;
            Hue = UniqueHue;

            // Stats
            SetStr(200, 250);
            SetDex(150, 180);
            SetInt(300, 350);

            SetHits(800, 900);
            SetStam(200, 250);
            SetMana(400, 450);

            SetDamage(20, 30);

            // Damage types
            SetDamageType(ResistanceType.Physical, 20);
            SetDamageType(ResistanceType.Fire,     60);
            SetDamageType(ResistanceType.Poison,   20);

            // Resistances
            SetResistance(ResistanceType.Physical, 50, 60);
            SetResistance(ResistanceType.Fire,     70, 80);
            SetResistance(ResistanceType.Cold,     30, 40);
            SetResistance(ResistanceType.Poison,   50, 60);
            SetResistance(ResistanceType.Energy,   40, 50);

            // Skills
            SetSkill(SkillName.EvalInt,   100.0, 110.0);
            SetSkill(SkillName.Magery,    100.0, 110.0);
            SetSkill(SkillName.MagicResist, 110.0, 120.0);
            SetSkill(SkillName.Meditation,  80.0,  90.0);
            SetSkill(SkillName.Tactics,     95.0, 100.0);
            SetSkill(SkillName.Wrestling,   85.0,  95.0);

            Fame = 18000;
            Karma = -18000;
            VirtualArmor = 70;
            ControlSlots = 4;

            // Cooldowns
            m_NextAshCloudTime    = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_NextShardVolleyTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
            m_NextIgnitionTime    = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 25));

            // Loot
            PackItem(new SulfurousAsh(Utility.RandomMinMax(20, 30)));
            PackItem(new Coal(Utility.RandomMinMax(5, 10))); // assumes Coal item exists
            PackGold(1500, 2500);
        }

        // Minor on‐movement coal‐dust sting
        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            base.OnMovement(m, oldLocation);

            if (!Alive || Deleted || m == this || m.Map != Map)
                return;

            if (m.InRange(Location, 2) && CanBeHarmful(m, false))
            {
                if (m is Mobile target)
                {
                    DoHarmful(target);
                    target.SendMessage("You choke on coal dust!");
                    AOS.Damage(target, this, Utility.RandomMinMax(5, 10), 0, 0, 0, 20, 80);
                }
            }
        }

        public override void OnThink()
        {
            base.OnThink();

            if (!Alive || Map == Map.Internal || Combatant == null)
                return;

            if (DateTime.UtcNow >= m_NextAshCloudTime)
            {
                AshenBreath();
                m_NextAshCloudTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
            }

            if (DateTime.UtcNow >= m_NextShardVolleyTime && InRange(Combatant.Location, 12))
            {
                ShardVolley();
                m_NextShardVolleyTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 25));
            }

            if (DateTime.UtcNow >= m_NextIgnitionTime && InRange(Combatant.Location, 10))
            {
                VolatileIgnition();
                m_NextIgnitionTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 30));
            }
        }

        // --- Ability #1: Ashen Breath (AoE blind & poison) ---
        private void AshenBreath()
        {
            Say("*The air thickens with ash!*");
            FixedParticles(0x3709, 10, 30, 5052, UniqueHue, 0, EffectLayer.Waist);
            PlaySound(0x490);

            var targets = new List<Mobile>();
            foreach (var m in Map.GetMobilesInRange(Location, 5))
            {
                if (m != this && CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(this, m))
                    targets.Add(m);
            }

            foreach (var t in targets)
            {
                DoHarmful(t);
                if (t is Mobile tm)
                {
                    tm.SendMessage("You are blinded by coal ash!");
                    tm.FixedParticles(0x375A, 5, 15, 5026, UniqueHue, 0, EffectLayer.Head);
                    tm.ApplyPoison(this, Poison.Lesser);
                }
            }
        }

        // --- Ability #2: Shard Volley (chain ember bolts) ---
        private void ShardVolley()
        {
            if (Combatant is Mobile target && CanBeHarmful(target, false))
            {
                Say("*Shards of embers!*");
                PlaySound(0x37E);

                for (int i = 0; i < 5; i++)
                {
                    Timer.DelayCall(TimeSpan.FromSeconds(i * 0.2), () =>
                    {
                        Effects.SendMovingParticles(
                            new Entity(Serial.Zero, Location, Map),
                            new Entity(Serial.Zero, target.Location, Map),
                            0x36D4, 5, 0, false, false, UniqueHue, 0, 9502, 1, 0, EffectLayer.Waist, 0x100
                        );

                        Timer.DelayCall(TimeSpan.FromSeconds(0.05), () =>
                        {
                            if (CanBeHarmful(target, false))
                                AOS.Damage(target, this, Utility.RandomMinMax(15, 25), 100, 0, 0, 0, 0);
                        });
                    });
                }
            }
        }

        // --- Ability #3: Volatile Ignition (single‐target fire + flame tile) ---
        private void VolatileIgnition()
        {
            if (Combatant is Mobile target && CanBeHarmful(target, false))
            {
                Say("*Burn!*");
                PlaySound(0x208);
                target.FixedParticles(0x370B, 10, 30, 5050, UniqueHue, 0, EffectLayer.CenterFeet);
                AOS.Damage(target, this, Utility.RandomMinMax(30, 50), 0, 100, 0, 0, 0);

                // Spawn a FlamestrikeHazardTile under them
                if (Map.CanFit(target.X, target.Y, target.Z, 16, false, false))
                {
                    var tile = new FlamestrikeHazardTile { Hue = UniqueHue };
                    tile.MoveToWorld(target.Location, Map);
                }
            }
        }

        // --- Death Effect: Burst of molten coal ---
        public override void OnDeath(Container c)
        {
            base.OnDeath(c);

            if (Map == null)
                return;

            Say("*Coal… to ashes…*");
            PlaySound(0x2F3);

            for (int i = 0; i < 6; i++)
            {
                int xOff = Utility.RandomMinMax(-3, 3);
                int yOff = Utility.RandomMinMax(-3, 3);
                var loc = new Point3D(X + xOff, Y + yOff, Z);

                if (!Map.CanFit(loc.X, loc.Y, loc.Z, 16, false, false))
                    loc.Z = Map.GetAverageZ(loc.X, loc.Y);

                if (Map.CanFit(loc.X, loc.Y, loc.Z, 16, false, false))
                {
                    var lava = new HotLavaTile { Hue = UniqueHue };
                    lava.MoveToWorld(loc, Map);
                }
            }
        }

        // Immunities & overrides
        public override bool BleedImmune => true;
        public override Poison PoisonImmune => Poison.Lethal;
        public override int TreasureMapLevel => 5;
        public override double DispelDifficulty => 120.0;
        public override double DispelFocus    =>  50.0;

        public CoalWisp(Serial serial) : base(serial) { }
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }
        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
