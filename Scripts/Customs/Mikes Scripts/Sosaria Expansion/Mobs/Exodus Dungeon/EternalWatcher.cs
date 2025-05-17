using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("the shattered husk of an Eternal Watcher")]
    public class EternalWatcher : BaseCreature
    {
        private DateTime m_NextMindPierce;
        private DateTime m_NextTemporalShift;
        private DateTime m_NextGazeOfNull;

        private bool m_AbilitiesInitialized;

        [Constructable]
        public EternalWatcher()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "an Eternal Watcher";
            Body = 22; // Gazer-like
            BaseSoundID = 377;
            Hue = 1150; // Ethereal blue

            SetStr(120, 150);
            SetDex(100, 120);
            SetInt(300, 350);

            SetHits(850, 1100);
            SetDamage(18, 25);

            SetDamageType(ResistanceType.Physical, 30);
            SetDamageType(ResistanceType.Energy, 40);
            SetDamageType(ResistanceType.Cold, 30);

            SetResistance(ResistanceType.Physical, 40, 55);
            SetResistance(ResistanceType.Fire, 20, 30);
            SetResistance(ResistanceType.Cold, 60, 75);
            SetResistance(ResistanceType.Poison, 40, 60);
            SetResistance(ResistanceType.Energy, 65, 80);

            SetSkill(SkillName.Magery, 100.0, 120.0);
            SetSkill(SkillName.MagicResist, 120.0, 140.0);
            SetSkill(SkillName.EvalInt, 100.0, 120.0);
            SetSkill(SkillName.Meditation, 80.0, 100.0);
            SetSkill(SkillName.Tactics, 75.0, 90.0);
            SetSkill(SkillName.Wrestling, 70.0, 85.0);

            Fame = 28000;
            Karma = -28000;
            VirtualArmor = 70;
        }

        public EternalWatcher(Serial serial)
            : base(serial)
        {
        }

        public override int TreasureMapLevel => 5;
        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 2);
        }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (!m_AbilitiesInitialized)
                {
                    m_NextMindPierce = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.Random(10, 20));
                    m_NextTemporalShift = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.Random(15, 30));
                    m_NextGazeOfNull = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.Random(25, 40));
                    m_AbilitiesInitialized = true;
                }

                if (DateTime.UtcNow >= m_NextMindPierce)
                    MindPierce();

                if (DateTime.UtcNow >= m_NextTemporalShift)
                    TemporalShift();

                if (DateTime.UtcNow >= m_NextGazeOfNull)
                    GazeOfNull();
            }
        }

        private void MindPierce()
        {
            if (Combatant is Mobile target && target.Alive)
            {
                PublicOverheadMessage(MessageType.Emote, 0x3B2, true, "*The Eternal Watcher pierces into your mind!*");
                target.PlaySound(0x1F7); // Psychic shock
                AOS.Damage(target, this, Utility.RandomMinMax(30, 50), 0, 0, 0, 0, 100);
                target.Mana -= Utility.RandomMinMax(15, 25);
                m_NextMindPierce = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.Random(20, 30));
            }
        }

        private void TemporalShift()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "*The air warps... time buckles around the Watcher!*");
            this.PlaySound(0x1E9); // Magic pulse

            Point3D newLocation = this.Location;
            Map map = this.Map;

            Effects.SendLocationEffect(newLocation, map, 0x3728, 10, 10);
            Timer.DelayCall(TimeSpan.FromSeconds(1.0), () =>
            {
                this.Location = new Point3D(newLocation.X + Utility.RandomMinMax(-3, 3), newLocation.Y + Utility.RandomMinMax(-3, 3), newLocation.Z);
                Effects.SendLocationEffect(this.Location, map, 0x3728, 10, 10);
            });

            m_NextTemporalShift = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.Random(25, 40));
        }

        private void GazeOfNull()
        {
            PublicOverheadMessage(MessageType.Emote, 0x3B2, true, "*An unblinking eye opens—everything stops.*");
            this.PlaySound(0x5C3); // Intense sound

            foreach (Mobile m in GetMobilesInRange(4))
            {
                if (m != this && m.Alive && !m.Hidden && m is Mobile mobile)
                {
                    mobile.Freeze(TimeSpan.FromSeconds(3));
                    mobile.SendMessage(0x22, "You are paralyzed by the Eternal Watcher's gaze!");
                }
            }

            m_NextGazeOfNull = DateTime.UtcNow + TimeSpan.FromSeconds(60);
        }

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);

            if (Utility.RandomDouble() < 0.01)
            {
                c.DropItem(new WatcherEye());
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_AbilitiesInitialized = false;
        }
    }

    public class WatcherEye : Item
    {
        [Constructable]
        public WatcherEye() : base(0x1B74)
        {
            Hue = 1150;
            Name = "the Eye of the Eternal Watcher";
            Weight = 1.0;
        }

        public WatcherEye(Serial serial) : base(serial)
        {
        }

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
