using System;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a mystic ram corpse")]
    public class MysticRam : BaseCreature, ICarvable
    {
        private DateTime m_NextMysticAura;
        private DateTime m_NextWoolTime;

        [Constructable]
        public MysticRam()
            : base(AIType.AI_Animal, FightMode.Aggressor, 10, 1, 0.2, 0.4)
        {
            Name = "a mystic ram";
            Body = 0xCF; // Use the body type from the provided example
            BaseSoundID = 0xD6;
            Hue = 1153; // Unique hue

            this.SetStr(200);
            this.SetDex(110);
            this.SetInt(150);

            this.SetDamage(14, 21);

            this.SetDamageType(ResistanceType.Physical, 0);
            this.SetDamageType(ResistanceType.Poison, 100);

            this.SetResistance(ResistanceType.Physical, 45, 55);
            this.SetResistance(ResistanceType.Fire, 50, 60);
            this.SetResistance(ResistanceType.Cold, 20, 30);
            this.SetResistance(ResistanceType.Poison, 70, 80);
            this.SetResistance(ResistanceType.Energy, 40, 50);

            this.SetSkill(SkillName.EvalInt, 90.1, 100.0);
            this.SetSkill(SkillName.Meditation, 90.1, 100.0);
            this.SetSkill(SkillName.Magery, 90.1, 100.0);
            this.SetSkill(SkillName.MagicResist, 90.1, 100.0);
            this.SetSkill(SkillName.Tactics, 100.0);
            this.SetSkill(SkillName.Wrestling, 98.1, 99.0);

            this.VirtualArmor = 58;

            Tamable = true;
            ControlSlots = 1;
            MinTameSkill = -18.9;

            m_NextMysticAura = DateTime.UtcNow;
        }

        public MysticRam(Serial serial)
            : base(serial)
        {
        }

        public override int Meat { get { return 3; } }
        public override MeatType MeatType { get { return MeatType.LambLeg; } }
        public override FoodType FavoriteFood { get { return FoodType.FruitsAndVegies | FoodType.GrainsAndHay; } }
        public override int Wool { get { return (Body == 0xCF ? 3 : 0); } }

        public bool Carve(Mobile from, Item item)
        {
            if (DateTime.UtcNow < m_NextWoolTime)
            {
                // This sheep is not yet ready to be shorn.
                PrivateOverheadMessage(MessageType.Regular, 0x3B2, 500449, from.NetState);
                return false;
            }

            from.SendLocalizedMessage(500452); // You place the gathered wool into your backpack.
            from.AddToBackpack(new Wool(Map == Map.Felucca ? 2 : 1));

            NextWoolTime = DateTime.UtcNow + TimeSpan.FromHours(2.0); // Proper time delay

            return true;
        }

        public DateTime NextWoolTime
        {
            get { return m_NextWoolTime; }
            set
            {
                m_NextWoolTime = value;
                Body = (DateTime.UtcNow >= m_NextWoolTime) ? 0xCF : 0xDF;
            }
        }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (DateTime.UtcNow >= m_NextMysticAura)
                {
                    ActivateMysticAura();
                }
            }

            Body = (DateTime.UtcNow >= m_NextWoolTime) ? 0xCF : 0xDF;
        }

        private void ActivateMysticAura()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Mystic Aura *");
            PlaySound(0x1F1);
            FixedEffect(0x376A, 10, 16);

            SetResistance(ResistanceType.Physical, 40, 50);
            SetResistance(ResistanceType.Fire, 30, 40);
            SetResistance(ResistanceType.Cold, 30, 40);
            SetResistance(ResistanceType.Poison, 30, 40);
            SetResistance(ResistanceType.Energy, 30, 40);

            m_NextMysticAura = DateTime.UtcNow + TimeSpan.FromMinutes(5);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)1); // version

            writer.WriteDeltaTime(m_NextMysticAura);
            writer.WriteDeltaTime(m_NextWoolTime);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            switch (version)
            {
                case 1:
                    m_NextMysticAura = reader.ReadDeltaTime();
                    m_NextWoolTime = reader.ReadDeltaTime();
                    break;
            }
        }
    }
}
