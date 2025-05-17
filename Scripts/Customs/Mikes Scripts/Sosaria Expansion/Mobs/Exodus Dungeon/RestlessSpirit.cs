using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Targeting;
using Server.Network;
using Server.Spells;

namespace Server.Mobiles
{
    [CorpseName("a restless spirit's remnants")]
    public class RestlessSpirit : BaseCreature
    {
        private DateTime m_NextWail;
        private DateTime m_NextPossess;
        private DateTime m_NextPhaseShift;
        private bool m_Initialized;

        [Constructable]
        public RestlessSpirit() 
            : base(AIType.AI_Mage, FightMode.Closest, 12, 1, 0.2, 0.4)
        {
            Name = "a Restless Spirit";
            Body = 400; // Ghostly body
            Hue = 0x47E; // Ethereal blue/white hue
            BaseSoundID = 0x482;

            SetStr(500, 600);
            SetDex(150, 200);
            SetInt(300, 400);

            SetHits(800, 1000);
            SetMana(1000, 1200);

            SetDamage(20, 26);

            SetDamageType(ResistanceType.Physical, 30);
            SetDamageType(ResistanceType.Cold, 35);
            SetDamageType(ResistanceType.Energy, 35);

            SetResistance(ResistanceType.Physical, 50, 65);
            SetResistance(ResistanceType.Fire, 40, 50);
            SetResistance(ResistanceType.Cold, 70, 80);
            SetResistance(ResistanceType.Poison, 100);
            SetResistance(ResistanceType.Energy, 60, 75);

            SetSkill(SkillName.EvalInt, 100.0, 120.0);
            SetSkill(SkillName.Magery, 105.0, 120.0);
            SetSkill(SkillName.MagicResist, 110.0, 130.0);
            SetSkill(SkillName.Meditation, 100.0);
            SetSkill(SkillName.Tactics, 100.0);
            SetSkill(SkillName.Wrestling, 90.0);

            Fame = 25000;
            Karma = -25000;

            VirtualArmor = 60;

            m_Initialized = false;
        }

        public override bool BleedImmune => true;
        public override bool AutoDispel => true;
        public override Poison PoisonImmune => Poison.Lethal;
        public override bool AlwaysMurderer => true;
        public override int TreasureMapLevel => 4;
        public override bool CanRummageCorpses => true;

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null && Alive)
            {
                if (!m_Initialized)
                {
                    m_NextWail = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 30));
                    m_NextPossess = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 45));
                    m_NextPhaseShift = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 20));
                    m_Initialized = true;
                }

                if (DateTime.UtcNow >= m_NextWail)
                    WailOfTorment();

                if (DateTime.UtcNow >= m_NextPossess)
                    PossessTarget();

                if (DateTime.UtcNow >= m_NextPhaseShift)
                    PhaseShift();
            }
        }

        private void WailOfTorment()
        {
            PublicOverheadMessage(MessageType.Regular, 0x22, true, "*A soul-piercing wail echoes from the Restless Spirit!*");
            PlaySound(0x482);

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive && m is Mobile mob)
                {
                    mob.SendMessage(0x22, "A haunting wail rattles your soul!");
                    mob.Freeze(TimeSpan.FromSeconds(2));
                    mob.FixedParticles(0x3735, 10, 30, 5012, 0x480, 0, EffectLayer.Head);
                    mob.PlaySound(0x5C9);
                }
            }

            m_NextWail = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(30, 45));
        }

        private void PossessTarget()
        {
            if (Combatant is Mobile target)
            {
                PublicOverheadMessage(MessageType.Regular, 0x22, true, "*The spirit attempts to possess its target!*");
                PlaySound(0x1F7);

                target.SendMessage(0x22, "A chilling presence seeps into your mind!");
                target.Paralyze(TimeSpan.FromSeconds(3));

                Effects.SendLocationEffect(target.Location, target.Map, 0x3709, 20, 30, 0x47E, 0);
                AOS.Damage(target, this, Utility.RandomMinMax(25, 40), 0, 0, 100, 0, 0);

                m_NextPossess = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(40, 60));
            }
        }

        private void PhaseShift()
        {
            PublicOverheadMessage(MessageType.Regular, 0x22, true, "*The spirit phases into the void and reappears nearby!*");
            PlaySound(0x10B);

            Effects.SendLocationParticles(
                EffectItem.Create(Location, Map, EffectItem.DefaultDuration), 0x3728, 1, 13, 2101, 7, 5023, 0);

            Point3D to = new Point3D(
                X + Utility.RandomMinMax(-5, 5),
                Y + Utility.RandomMinMax(-5, 5),
                Z);

            if (Map.CanSpawnMobile(to))
            {
                Location = to;
                Effects.SendLocationParticles(
                    EffectItem.Create(to, Map, EffectItem.DefaultDuration), 0x3728, 1, 13, 2101, 7, 5023, 0);
            }

            m_NextPhaseShift = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 25));
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 2);
            AddLoot(LootPack.Gems, 4);

            if (Utility.RandomDouble() < 0.005) // 1 in 200 chance
                PackItem(new GhostboundRelic()); // Custom rare drop
        }

        public override bool OnBeforeDeath()
        {
            Effects.PlaySound(Location, Map, 0x10B);
            Effects.SendLocationParticles(
                EffectItem.Create(Location, Map, TimeSpan.FromSeconds(10.0)), 0x37CC, 1, 50, 0x47E, 7, 9909, 0);
            return base.OnBeforeDeath();
        }

        public RestlessSpirit(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_Initialized = false;
        }
    }

    public class GhostboundRelic : Item
    {
        [Constructable]
        public GhostboundRelic() : base(0x2B68)
        {
            Name = "Ghostbound Relic";
            Hue = 0x47E;
            Weight = 1.0;
            LootType = LootType.Blessed;
        }

        public GhostboundRelic(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
