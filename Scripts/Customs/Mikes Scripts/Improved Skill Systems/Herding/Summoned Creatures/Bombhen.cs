using System;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a bombhen corpse")]
    public class Bombhen : BaseCreature
    {
        private DateTime m_NextEggBomber;

        [Constructable]
        public Bombhen()
            : base(AIType.AI_Animal, FightMode.Aggressor, 10, 1, 0.2, 0.4)
        {
            Name = "a bombhen";
            Body = 0xD0; // Chicken body
            BaseSoundID = 0x6E;
            Hue = 1161; // Fiery orange hue

            SetStr(95, 120);
            SetDex(115, 135);
            SetInt(95, 110);

            SetHits(80, 100);
            SetMana(50, 60);

            SetDamage(10, 14);

            SetDamageType(ResistanceType.Physical, 60);
            SetDamageType(ResistanceType.Cold, 40);

            SetResistance(ResistanceType.Physical, 35, 45);
            SetResistance(ResistanceType.Fire, 20, 30);
            SetResistance(ResistanceType.Cold, 50, 60);
            SetResistance(ResistanceType.Poison, 25, 35);
            SetResistance(ResistanceType.Energy, 25, 35);

            SetSkill(SkillName.MagicResist, 75.0, 85.0);
            SetSkill(SkillName.Tactics, 65.0, 75.0);
            SetSkill(SkillName.Wrestling, 65.0, 75.0);
            SetSkill(SkillName.Magery, 65.0, 75.0);
            SetSkill(SkillName.EvalInt, 65.0, 75.0);

            Fame = 2000;
            Karma = -2000;

            VirtualArmor = 38;

            Tamable = true;
            ControlSlots = 1;
            MinTameSkill = -18.9;

            m_NextEggBomber = DateTime.UtcNow;
        }

        public Bombhen(Serial serial)
            : base(serial)
        {
        }

        public override int Meat { get { return 1; } }
        public override MeatType MeatType { get { return MeatType.Bird; } }
        public override bool CanFly { get { return true; } }
        public override int Feathers { get { return 25; } }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null && DateTime.UtcNow >= m_NextEggBomber)
            {
                LayEggBomber();
            }
        }

        private void LayEggBomber()
        {
            Mobile target = Combatant as Mobile;
            if (target != null && target.Alive)
            {
                EggBomber bomb = new EggBomber();
                bomb.MoveToWorld(Location, Map);

                PublicOverheadMessage(MessageType.Emote, 0x3B2, false, "* lays an explosive egg *");
                PlaySound(0x1BF); // Explosion sound

                m_NextEggBomber = DateTime.UtcNow + TimeSpan.FromSeconds(30);
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            m_NextEggBomber = DateTime.UtcNow;
        }
    }

    public class EggBomber : Item
    {
        private Timer m_Timer;

        [Constructable]
        public EggBomber()
            : base(0x9B5) // Egg
        {
            Hue = 1161; // Fiery orange hue
            Name = "explosive egg";

            m_Timer = Timer.DelayCall(TimeSpan.FromSeconds(5), new TimerCallback(Explode));
        }

        public EggBomber(Serial serial)
            : base(serial)
        {
        }

        public override void OnDelete()
        {
            if (m_Timer != null)
                m_Timer.Stop();

            base.OnDelete();
        }

        private void Explode()
        {
            if (Deleted)
                return;

            foreach (Mobile m in GetMobilesInRange(3))
            {
                if (m.Alive && !m.IsDeadBondedPet)
                {
                    int damage = Utility.RandomMinMax(10, 20);
                    m.Damage(damage, null); // Passing null since EggBomber is not a Mobile
                    m.SendLocalizedMessage(503019); // You are damaged by the explosion!
                }
            }

            Effects.SendLocationEffect(Location, Map, 0x36BD, 20, 10);
            Effects.PlaySound(Location, Map, 0x307); // PlaySound is a method of Effects

            Delete();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            m_Timer = Timer.DelayCall(TimeSpan.FromSeconds(5), new TimerCallback(Explode));
        }
    }
}
