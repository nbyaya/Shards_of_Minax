using System;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("corpse of a bone shielder")]
    public class BoneShielder : BaseCreature
    {
        private TimeSpan m_SpeechDelay = TimeSpan.FromSeconds(10.0); // time between shielder's taunts
        public DateTime m_NextSpeechTime;

        [Constructable]
        public BoneShielder() : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Hue = 0x9C4; // bone color
			Team = 1;

            Body = 0x190; // using male model for simplicity
            Name = NameList.RandomName("male");
            Title = "the Bone Shielder";

            // Adding bone armor pieces as items
            AddItem(new BoneChest());
            AddItem(new BoneLegs());
            AddItem(new BoneArms());
            AddItem(new BoneGloves());
            AddItem(new BoneHelm());

            SetStr(900, 1200);
            SetDex(80, 100);
            SetInt(200, 300);

            SetHits(800, 1200);

            SetDamage(5, 15);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 75, 90);
            SetResistance(ResistanceType.Fire, 50, 65);
            SetResistance(ResistanceType.Cold, 60, 75);
            SetResistance(ResistanceType.Poison, 60, 75);
            SetResistance(ResistanceType.Energy, 60, 75);

            SetSkill(SkillName.Magery, 100.1, 110.0);
            SetSkill(SkillName.MagicResist, 80.1, 90.0);
            SetSkill(SkillName.Tactics, 50.1, 60.0);
            SetSkill(SkillName.Wrestling, 70.1, 80.0);

            Fame = 4500;
            Karma = -4500;

            VirtualArmor = 70;

            m_NextSpeechTime = DateTime.Now + m_SpeechDelay;
        }

        public override void OnThink()
        {
            if (DateTime.Now >= m_NextSpeechTime)
            {
                Mobile combatant = this.Combatant as Mobile;

                if (combatant != null && combatant.Map == this.Map && combatant.InRange(this, 8))
                {
                    Say(true, "Shields of bone, protect us!"); // shield activation speech
                    m_NextSpeechTime = DateTime.Now + m_SpeechDelay;
                }

                // Shield Effect Logic Here

                base.OnThink();
            }
        }

        public BoneShielder(Serial serial) : base(serial)
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
