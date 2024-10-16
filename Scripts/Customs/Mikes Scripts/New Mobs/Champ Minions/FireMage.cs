using System;
using Server.Items;
using Server.Targeting;
using Server.Spells;

namespace Server.Mobiles
{
    [CorpseName("corpse of a fire mage")]
    public class FireMage : BaseCreature
    {
        [Constructable]
        public FireMage() : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Hue = Utility.RandomSkinHue();

            if (Female = Utility.RandomBool())
            {
                Body = 0x191;
                Name = NameList.RandomName("female");
                Title = " the Fire Mage";
            }
            else
            {
                Body = 0x190;
                Name = NameList.RandomName("male");
                Title = " the Fire Mage";
            }

            Item robe = new Robe(Utility.RandomRedHue());
            Item sandals = new Sandals(Utility.RandomYellowHue());
            Item staff = new GnarledStaff();

            AddItem(robe);
            AddItem(sandals);
            AddItem(staff);

            SetStr(400, 500);
            SetDex(150, 200);
            SetInt(300, 400);

            SetHits(300, 400);

            SetDamage(10, 15);

            SetDamageType(ResistanceType.Physical, 10);
            SetDamageType(ResistanceType.Fire, 90);

            SetResistance(ResistanceType.Physical, 40, 50);
            SetResistance(ResistanceType.Fire, 70, 90);
            SetResistance(ResistanceType.Cold, 20, 30);
            SetResistance(ResistanceType.Poison, 20, 30);
            SetResistance(ResistanceType.Energy, 20, 30);

            SetSkill(SkillName.EvalInt, 80.1, 100.0);
            SetSkill(SkillName.Magery, 80.1, 100.0);
            SetSkill(SkillName.Meditation, 80.1, 100.0);
            SetSkill(SkillName.MagicResist, 80.1, 100.0);
            SetSkill(SkillName.Tactics, 50.1, 70.0);
            SetSkill(SkillName.Wrestling, 50.1, 70.0);

            Fame = 6000;
            Karma = -6000;

            VirtualArmor = 50;
        }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null && DateTime.Now >= m_NextFireballTime)
            {
                Mobile target = Combatant as Mobile;

                if (target != null && target.Map == this.Map && target.InRange(this, 12))
                {
                    this.Say("Feel the heat of my flames!");

                    // Set next fireball time
                    m_NextFireballTime = DateTime.Now + m_FireballDelay;
                }
            }
        }

        private TimeSpan m_FireballDelay = TimeSpan.FromSeconds(5.0); // delay between fireball casts
        public DateTime m_NextFireballTime;

        public FireMage(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
