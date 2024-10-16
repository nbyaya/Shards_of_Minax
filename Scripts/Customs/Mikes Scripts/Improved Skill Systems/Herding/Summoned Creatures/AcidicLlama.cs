using System;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("an acidic llama corpse")]
    public class AcidicLlama : BaseCreature
    {
        private DateTime m_NextSpitBarrage;

        [Constructable]
        public AcidicLlama()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "an acidic llama";
            Body = 0xDC; // Using the body type from the provided example
            BaseSoundID = 0x3F3;
            Hue = 1275; // Unique hue for the Acidic Llama

            SetStr(150);
            SetDex(100);
            SetInt(75);

            SetHits(120);
            SetMana(0);

            SetDamage(10, 20);

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Poison, 50);

            SetResistance(ResistanceType.Physical, 35, 45);
            SetResistance(ResistanceType.Fire, 20, 30);
            SetResistance(ResistanceType.Cold, 30, 40);
            SetResistance(ResistanceType.Poison, 60, 70);
            SetResistance(ResistanceType.Energy, 25, 35);

            SetSkill(SkillName.MagicResist, 65.0, 75.0);
            SetSkill(SkillName.Tactics, 80.0, 90.0);
            SetSkill(SkillName.Wrestling, 80.0, 90.0);

            Fame = 4500;
            Karma = -4500;

            VirtualArmor = 40;

            Tamable = true;
            ControlSlots = 1;
            MinTameSkill = -10;

            m_NextSpitBarrage = DateTime.UtcNow;
        }

        public AcidicLlama(Serial serial)
            : base(serial)
        {
        }

        public override int Meat { get { return 1; } }
        public override int Hides { get { return 12; } }
        public override FoodType FavoriteFood { get { return FoodType.FruitsAndVegies | FoodType.GrainsAndHay; } }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null && DateTime.UtcNow >= m_NextSpitBarrage)
            {
                SpitBarrage();
            }
        }

        private void SpitBarrage()
        {
            Mobile target = Combatant as Mobile;
            if (target != null && target.Alive)
            {
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "*spits a barrage of acid*");
                PlaySound(0x23B);

                for (int i = 0; i < 3; i++) // Fires three acid spits
                {
                    Timer.DelayCall(TimeSpan.FromSeconds(i * 0.5), delegate
                    {
                        if (target.Alive)
                        {
                            int damage = Utility.RandomMinMax(5, 10);
                            target.Damage(damage, this);
                            target.SendMessage("You are hit by an acidic spit!");
                            target.FixedEffect(0x374A, 1, 15, 1166, 3); // Acidic visual effect
                        }
                    });
                }

                m_NextSpitBarrage = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Cooldown for the ability
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
            m_NextSpitBarrage = DateTime.UtcNow;
        }
    }
}
