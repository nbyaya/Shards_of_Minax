using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a mummified ostard corpse")]
    public class MummifiedOstard : BaseCreature
    {
        private DateTime m_NextSandstormTime;
        private DateTime m_NextCurseTime;
        private DateTime m_NextSummonTime;
        private Point3D  m_LastLocation;

        // A dusty amber hue
        private const int UniqueHue = 2411;

        [Constructable]
        public MummifiedOstard()
            : base(AIType.AI_Melee, FightMode.Closest, 12, 1, 0.1, 0.3)
        {
            Name        = "a mummified ostard";
            Body        = 0xD2;    // same as DesertOstard
            BaseSoundID = 0x270;   // same calls/sounds
            Hue         = UniqueHue;

            // —— Core Stats ——
            SetStr(500, 650);
            SetDex(200, 250);
            SetInt(200, 250);

            SetHits(2000, 2300);
            SetStam(250, 300);
            SetMana(100, 150);

            SetDamage(20, 30);

            // Damage types
            SetDamageType(ResistanceType.Physical, 70);
            SetDamageType(ResistanceType.Poison,   30);

            // Resistances
            SetResistance(ResistanceType.Physical, 60, 70);
            SetResistance(ResistanceType.Fire,     50, 60);
            SetResistance(ResistanceType.Cold,     40, 50);
            SetResistance(ResistanceType.Poison,   70, 80);
            SetResistance(ResistanceType.Energy,   40, 50);

            // Skills
            SetSkill(SkillName.Wrestling,   120.0, 130.0);
            SetSkill(SkillName.Tactics,     120.0, 130.0);
            SetSkill(SkillName.MagicResist, 120.0, 130.0);
            SetSkill(SkillName.Poisoning,   100.0, 110.0);

            Fame = 25000;
            Karma = -25000;

            VirtualArmor = 90;
            ControlSlots = 6;

            // Initialize cooldowns
            var now = DateTime.UtcNow;
            m_NextSandstormTime = now + TimeSpan.FromSeconds(15 + Utility.Random(5));
            m_NextCurseTime     = now + TimeSpan.FromSeconds(25 + Utility.Random(5));
            m_NextSummonTime    = now + TimeSpan.FromSeconds(20 + Utility.Random(5));

            m_LastLocation = this.Location;

            // Basic loot
            PackItem(new Bandage(Utility.RandomMinMax(5, 15)));
            PackGold(2000, 3000);
        }

        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            if ( m != null && m != this && Alive 
              && m.Map == Map && m.InRange(Location, 2) 
              && CanBeHarmful(m, false) )
            {
                var p = new PoisonTile();
                p.Hue = UniqueHue;
                p.MoveToWorld(oldLocation, Map);
            }

            base.OnMovement(m, oldLocation);
        }

        public override void OnThink()
        {
            base.OnThink();

            // pattern‐matching both declares and assigns “target”
            if ( Combatant is Mobile target 
              && Alive 
              && Map != null 
              && Map != Map.Internal      // <-- fixed here
              && CanBeHarmful(target, false) )
            {
                var now = DateTime.UtcNow;

                if ( now >= m_NextSummonTime && InRange(target.Location, 12) )
                {
                    SummonScarabSwarm(target);
                    m_NextSummonTime = now + TimeSpan.FromSeconds(25 + Utility.Random(10));
                }
                else if ( now >= m_NextCurseTime && InRange(target.Location, 10) )
                {
                    CurseOfThePharaoh(target);
                    m_NextCurseTime = now + TimeSpan.FromSeconds(30 + Utility.Random(10));
                }
                else if ( now >= m_NextSandstormTime )
                {
                    SandstormCyclone();
                    m_NextSandstormTime = now + TimeSpan.FromSeconds(20 + Utility.Random(10));
                }
            }

            // leave quicksand patches as it shuffles about
            if ( Location != m_LastLocation && Utility.RandomDouble() < 0.2 )
            {
                var qs = new QuicksandTile();
                qs.Hue = UniqueHue;
                qs.MoveToWorld(m_LastLocation, Map);
            }

            m_LastLocation = Location;
        }

        private void SummonScarabSwarm(Mobile target)
        {
            Say("*The sands bring forth my guardians!*");
            PlaySound(0x2A3);

            for (int i = 0; i < 6; i++)
            {
                var loc = new Point3D(
                    X + Utility.RandomMinMax(-3, 3),
                    Y + Utility.RandomMinMax(-3, 3),
                    Z
                );

                var scarab = new Skeleton(); // swap for your real scarab‐mob
                scarab.Hue        = UniqueHue;
                scarab.MoveToWorld(loc, Map);
                scarab.Combatant  = target;
            }
        }

        private void CurseOfThePharaoh(Mobile target)
        {
            Say("*Feel the weight of the Pharaoh's curse!*");
            PlaySound(0x212);
            target.FixedParticles(0x373A, 10, 15, 5032, UniqueHue, 0, EffectLayer.Head);

            if ( target is Mobile mTarget )
                mTarget.ApplyPoison(this, Poison.Lethal);
        }

        private void SandstormCyclone()
        {
            Say("*A storm of sands!*");
            PlaySound(0x1FE);

            var victims = new List<Mobile>();
            foreach (var m in Map.GetMobilesInRange(Location, 8))
            {
                if ( m != this && m.Alive && CanBeHarmful(m, false) )
                    victims.Add(m);
            }

            foreach (var m in victims)
            {
                DoHarmful(m);
                AOS.Damage(m, this, Utility.RandomMinMax(25, 45), 50, 0, 0, 0, 50);

                // apply a temporary dexterity debuff to simulate “slowing”
                var slowDur = TimeSpan.FromSeconds(Utility.RandomMinMax(4, 6));
                m.FixedParticles(0x3779, 10, 15, 5032, UniqueHue, 0, EffectLayer.Waist);
                m.AddStatMod(new StatMod(StatType.Dex, "MummySandSlow", -50, slowDur));
                m.SendMessage("You are battered by whipping sands and slowed!");
            }
        }

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);

			if (Map == null)
				return;

            Say("*My curse endures…*");
            PlaySound(0x213);

            // ring of flames
            for (int i = 0; i < 8; i++)
            {
                int dx = Utility.RandomMinMax(-3, 3);
                int dy = Utility.RandomMinMax(-3, 3);
                var loc = new Point3D(X + dx, Y + dy, Z);

                var flame = new NecromanticFlamestrikeTile();
                flame.Hue = UniqueHue;
                flame.MoveToWorld(loc, Map);
            }
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 2);
            AddLoot(LootPack.Gems, Utility.RandomMinMax(5, 10));
            AddLoot(LootPack.MedScrolls, Utility.RandomMinMax(2, 4));

            if (Utility.RandomDouble() < 0.05) // 5% for a big bundle
                PackItem(new Bandage(Utility.RandomMinMax(20, 40)));
        }

        public override bool BleedImmune    => true;
        public override Poison PoisonImmune => Poison.Deadly;
        public override int TreasureMapLevel => 6;
        public override double DispelDifficulty => 150.0;
        public override double DispelFocus      => 75.0;

        public MummifiedOstard(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            // reset timers on load
            var now = DateTime.UtcNow;
            m_NextSandstormTime = now + TimeSpan.FromSeconds(15 + Utility.Random(5));
            m_NextCurseTime     = now + TimeSpan.FromSeconds(25 + Utility.Random(5));
            m_NextSummonTime    = now + TimeSpan.FromSeconds(20 + Utility.Random(5));
            m_LastLocation      = this.Location;
        }
    }
}
