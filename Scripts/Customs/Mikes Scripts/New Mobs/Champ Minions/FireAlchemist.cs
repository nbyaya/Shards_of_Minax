using System;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("corpse of a fire alchemist")]
    public class FireAlchemist : BaseCreature
    {
        private TimeSpan m_SpeechDelay = TimeSpan.FromSeconds(10.0); // time between alchemist's taunts
        public DateTime m_NextSpeechTime;

        [Constructable]
        public FireAlchemist() : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Hue = Utility.RandomRedHue();

            if (Female = Utility.RandomBool())
            {
                Body = 0x191;
                Name = NameList.RandomName("female");
                Title = " the Fire Alchemist";
            }
            else
            {
                Body = 0x190;
                Name = NameList.RandomName("male");
                Title = " the Fire Alchemist";
            }

            // Fire alchemist attire
            AddItem(new Robe(Utility.RandomRedHue()));
            AddItem(new Sandals());

            Item hair = new Item(Utility.RandomList(0x203B, 0x203C, 0x203D));
            hair.Hue = Utility.RandomHairHue();
            hair.Layer = Layer.Hair;
            hair.Movable = false;
            AddItem(hair);

            if (!Female)
            {
                Item beard = new Item(Utility.RandomList(0x203E, 0x2041));
                beard.Hue = hair.Hue;
                beard.Layer = Layer.FacialHair;
                beard.Movable = false;
                AddItem(beard);
            }

            SetStr(500, 800);
            SetDex(150, 250);
            SetInt(250, 400);

            SetHits(450, 750);

            SetDamage(15, 25);

            SetDamageType(ResistanceType.Physical, 25);
            SetDamageType(ResistanceType.Fire, 75);

            SetResistance(ResistanceType.Physical, 45, 60);
            SetResistance(ResistanceType.Fire, 70, 90);
            SetResistance(ResistanceType.Cold, 30, 45);
            SetResistance(ResistanceType.Poison, 40, 55);
            SetResistance(ResistanceType.Energy, 35, 50);

            SetSkill(SkillName.EvalInt, 100.1, 120.0);
            SetSkill(SkillName.Magery, 100.5, 120.0);
            SetSkill(SkillName.Meditation, 90.1, 110.0);
            SetSkill(SkillName.MagicResist, 85.0, 95.0);
            SetSkill(SkillName.Tactics, 50.1, 65.0);
            SetSkill(SkillName.Wrestling, 50.1, 65.0);

            Fame = 10000;
            Karma = -10000;

            VirtualArmor = 40;

            m_NextSpeechTime = DateTime.Now + m_SpeechDelay;
        }

		public override void OnThink()
		{
			base.OnThink();
			if (DateTime.Now >= m_NextSpeechTime)
			{
				Mobile combatant = this.Combatant as Mobile; // Explicit cast to Mobile
				if (combatant != null && combatant.Map == this.Map && combatant.InRange(this, 8))
				{
					this.Say(true, "Feel the heat of my wrath!");
					m_NextSpeechTime = DateTime.Now + m_SpeechDelay;
					// Implement area ignition ability
				}
			}
		}

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Average);
            AddLoot(LootPack.HighScrolls);
        }

        public FireAlchemist(Serial serial) : base(serial)
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
