using System;
using Server.Items;
using Server.Mobiles;
using Server.Targeting;
using System.Collections.Generic;

namespace Server.Mobiles
{
    [CorpseName("corpse of a field commander")]
    public class FieldCommander : BaseCreature
    {
        private TimeSpan m_CommandDelay = TimeSpan.FromSeconds(20.0); // time between issuing commands
        public DateTime m_NextCommandTime;

        [Constructable]
        public FieldCommander() : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Hue = Utility.RandomSkinHue();
            Body = Utility.RandomBool() ? 0x190 : 0x191;
            Name = NameList.RandomName(Body == 0x190 ? "male" : "female");
            Title = " the Field Commander";
			Team = 2;

            Item helmet = new PlateHelm();
            Item chest = new PlateChest();
            Item legs = new PlateLegs();
            Item gloves = new PlateGloves();
            Item boots = new PlateArms();

            helmet.Hue = 1109; // Dark color for field commander
            chest.Hue = 1109;
            legs.Hue = 1109;
            gloves.Hue = 1109;
            boots.Hue = 1109;

            AddItem(helmet);
            AddItem(chest);
            AddItem(legs);
            AddItem(gloves);
            AddItem(boots);

            AddItem(new VikingSword());
            AddItem(new HeaterShield());

            SetStr(900, 1100);
            SetDex(150, 200);
            SetInt(150, 200);

            SetHits(700, 900);

            SetDamage(15, 25);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 70, 90);
            SetResistance(ResistanceType.Fire, 60, 80);
            SetResistance(ResistanceType.Cold, 60, 80);
            SetResistance(ResistanceType.Poison, 60, 80);
            SetResistance(ResistanceType.Energy, 60, 80);

            SetSkill(SkillName.Swords, 90.1, 100.0);
            SetSkill(SkillName.Tactics, 90.1, 100.0);
            SetSkill(SkillName.MagicResist, 75.0, 100.0);

            Fame = 10000;
            Karma = -10000;

            VirtualArmor = 60;

            m_NextCommandTime = DateTime.Now + m_CommandDelay;
        }

        public override bool AlwaysMurderer { get { return true; } }

        public override void OnThink()
        {
            if (DateTime.Now >= m_NextCommandTime)
            {
                IssueCommand();
                m_NextCommandTime = DateTime.Now + m_CommandDelay;
            }

            base.OnThink();
        }

        private void IssueCommand()
        {
            List<Mobile> minions = new List<Mobile>();
            foreach (Mobile m in GetMobilesInRange(10))
            {
                if (m is BaseCreature && ((BaseCreature)m).ControlMaster == this)
                {
                    minions.Add(m);
                }
            }

            if (minions.Count > 0)
            {
                Mobile target = this.Combatant as Mobile;
                if (target != null && target.Map == this.Map && target.InRange(this, 10))
                {
                    int command = Utility.Random(3);

                    switch (command)
                    {
                        case 0: this.Say("Charge and overwhelm the enemy!"); break;
                        case 1: this.Say("Flank them from the sides!"); break;
                        case 2: this.Say("Fall back and regroup!"); break;
                    }

                                    }
            }
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich);
            AddLoot(LootPack.Gems, 5);
        }

        public FieldCommander(Serial serial) : base(serial)
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
