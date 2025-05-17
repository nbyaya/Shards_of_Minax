using System;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("corpse of a ward caster")]
    public class WardCaster : BaseCreature
    {
        private TimeSpan m_ShieldDelay = TimeSpan.FromSeconds(30.0); // time between shield casts
        public DateTime m_NextShieldTime;

        [Constructable]
        public WardCaster() : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Hue = Utility.RandomSkinHue();
            Body = 0x190; // Using a male body
            Name = NameList.RandomName("male");
            Title = " the Ward Caster";
			Team = 2;

            // Equipments
            Item robe = new Robe(Utility.RandomBlueHue());
            Item boots = new Boots(Utility.RandomNeutralHue());
            Item staff = new GnarledStaff();

            AddItem(robe);
            AddItem(boots);
            AddItem(staff);

            SetStr(500, 700);
            SetDex(120, 150);
            SetInt(500, 700);

            SetHits(400, 600);

            SetDamage(8, 14);

            SetDamageType(ResistanceType.Physical, 0);
            SetDamageType(ResistanceType.Cold, 25);
            SetDamageType(ResistanceType.Fire, 25);
            SetDamageType(ResistanceType.Energy, 25);
            SetDamageType(ResistanceType.Poison, 25);

            SetResistance(ResistanceType.Physical, 40, 50);
            SetResistance(ResistanceType.Fire, 40, 50);
            SetResistance(ResistanceType.Cold, 40, 50);
            SetResistance(ResistanceType.Poison, 40, 50);
            SetResistance(ResistanceType.Energy, 40, 50);

            SetSkill(SkillName.EvalInt, 90.1, 100.0);
            SetSkill(SkillName.Magery, 95.5, 100.0);
            SetSkill(SkillName.Meditation, 90.1, 100.0);
            SetSkill(SkillName.MagicResist, 100.5, 150.0);
            SetSkill(SkillName.Tactics, 50.1, 70.0);
            SetSkill(SkillName.Wrestling, 50.1, 70.0);

            Fame = 6000;
            Karma = -6000;

            VirtualArmor = 40;

            m_NextShieldTime = DateTime.Now + m_ShieldDelay;
        }

        public override void OnThink()
        {
            if (DateTime.Now >= m_NextShieldTime)
            {
                this.Say(true, "By the power of magic, I shield myself!");
                BuffSelf();
                m_NextShieldTime = DateTime.Now + m_ShieldDelay;
            }

            base.OnThink();
        }

        private void BuffSelf()
        {

        }

        public override void GenerateLoot()
        {
            PackGem();
            PackGold(150, 200);
            AddLoot(LootPack.Rich);
            AddLoot(LootPack.Average);
        }

        public WardCaster(Serial serial) : base(serial)
        {
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
        }
    }
}
