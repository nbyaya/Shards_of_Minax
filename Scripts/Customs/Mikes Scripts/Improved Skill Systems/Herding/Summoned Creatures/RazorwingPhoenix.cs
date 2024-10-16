using System;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a razorwing phoenix corpse")]
    public class RazorwingPhoenix : BaseCreature
    {
        private DateTime m_NextFeatherBlast;

        [Constructable]
        public RazorwingPhoenix()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a razorwing phoenix";
            Body = 6;
            BaseSoundID = 0x1B;
            Hue = 1161; // Fiery orange hue

            SetStr(120, 130);
            SetDex(170, 180);
            SetInt(150, 160);

            SetHits(150, 180);
            SetMana(80, 100);

            SetDamage(10, 15);

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Fire, 50);

            SetResistance(ResistanceType.Physical, 45, 55);
            SetResistance(ResistanceType.Fire, 70, 80);
            SetResistance(ResistanceType.Cold, 20, 30);
            SetResistance(ResistanceType.Poison, 30, 40);
            SetResistance(ResistanceType.Energy, 40, 50);

            SetSkill(SkillName.EvalInt, 90.0, 100.0);
            SetSkill(SkillName.Magery, 90.0, 100.0);
            SetSkill(SkillName.MagicResist, 90.0, 100.0);
            SetSkill(SkillName.Tactics, 90.0, 100.0);
            SetSkill(SkillName.Wrestling, 90.0, 100.0);

            Fame = 7000;
            Karma = 0;

            VirtualArmor = 50;

            Tamable = true;
            ControlSlots = 1;
            MinTameSkill = -10;

            m_NextFeatherBlast = DateTime.UtcNow;
        }

        public RazorwingPhoenix(Serial serial)
            : base(serial)
        {
        }

        public override int Meat { get { return 1; } }
        public override MeatType MeatType { get { return MeatType.Bird; } }
        public override int Feathers { get { return 60; } }
        public override FoodType FavoriteFood { get { return FoodType.Meat; } }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null && DateTime.UtcNow >= m_NextFeatherBlast)
            {
                FeatherBlast();
            }
        }

        private void FeatherBlast()
        {
            Mobile target = Combatant as Mobile;
            if (target != null && target.Alive && InRange(target.Location, 7))
            {
                int damage = Utility.RandomMinMax(20, 30);
                
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Feather Blast! *");
                PlaySound(0x654);
                MovingEffect(target, 0x36E4, 7, 0, false, false);

                AOS.Damage(target, this, damage, 0, 0, 0, 100, 0);
                target.FixedEffect(0x36E4, 10, 20);

                m_NextFeatherBlast = DateTime.UtcNow + TimeSpan.FromSeconds(20);
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

            m_NextFeatherBlast = DateTime.UtcNow;
        }
    }
}