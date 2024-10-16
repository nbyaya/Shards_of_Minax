using System;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("corpse of a jungle naturalist")]
    public class JungleNaturalist : BaseCreature
    {
        private TimeSpan m_SpeechDelay = TimeSpan.FromSeconds(10.0); // time between naturalist speech
        public DateTime m_NextSpeechTime;

        [Constructable]
        public JungleNaturalist() : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Hue = Utility.RandomSkinHue();

            if (Female = Utility.RandomBool())
            {
                Body = 0x191;
                Name = NameList.RandomName("female");
                Title = " the Naturalist";
            }
            else
            {
                Body = 0x190;
                Name = NameList.RandomName("male");
                Title = " the Naturalist";
            }

            AddItem(new Robe(Utility.RandomNeutralHue()));
            
            Item hair = new Item(Utility.RandomList(0x203B, 0x203C, 0x203D));
            hair.Hue = Utility.RandomHairHue();
            hair.Layer = Layer.Hair;
            hair.Movable = false;

            AddItem(hair);

            if (!this.Female)
            {
                Item beard = new Item(Utility.RandomList(0x203E, 0x2041));
                beard.Hue = hair.Hue;
                beard.Layer = Layer.FacialHair;
                beard.Movable = false;
                AddItem(beard);
            }

            SetStr(800, 1200);
            SetDex(177, 255);
            SetInt(251, 350);

            SetHits(600, 1000);

            SetDamage(10, 20);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 65, 80);
            SetResistance(ResistanceType.Fire, 60, 80);
            SetResistance(ResistanceType.Cold, 50, 60);
            SetResistance(ResistanceType.Poison, 100);
            SetResistance(ResistanceType.Energy, 40, 50);

            SetSkill(SkillName.Magery, 95.5, 100.0);
            SetSkill(SkillName.EvalInt, 90.1, 100.0);
            SetSkill(SkillName.MagicResist, 100.5, 150.0);
            SetSkill(SkillName.Tactics, 90.1, 100.0);

            Tamable = false;
            ControlSlots = 1;

            Fame = 7000;
            Karma = 7000;

            VirtualArmor = 60;

            m_NextSpeechTime = DateTime.Now + m_SpeechDelay;
        }

        public override void OnThink()
        {
            base.OnThink();
            if (DateTime.Now >= m_NextSpeechTime)
            {
                SummonCreature();
                m_NextSpeechTime = DateTime.Now + m_SpeechDelay;
            }
        }

        private void SummonCreature()
        {
            if (this.Combatant != null && this.Combatant.Alive && !this.Combatant.Deleted)
            {
                Mobile creature = new TropicalCreature();
                creature.MoveToWorld(this.Location, this.Map);
                this.Say("Arise, my jungle ally!");
            }
        }

        public JungleNaturalist(Serial serial) : base(serial)
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

	public class TropicalCreature : BaseCreature
	{
		[Constructable]
		public TropicalCreature() : base(AIType.AI_Animal, FightMode.Aggressor, 10, 1, 0.2, 0.4)
		{
            this.Name = "Tropical Pig";
            this.Body = 0xCB;
            this.BaseSoundID = 0xC4;

            this.SetStr(20);
            this.SetDex(20);
            this.SetInt(5);

            this.SetHits(12);
            this.SetMana(0);

            this.SetDamage(2, 4);

            this.SetDamageType(ResistanceType.Physical, 100);

            this.SetResistance(ResistanceType.Physical, 10, 15);

            this.SetSkill(SkillName.MagicResist, 5.0);
            this.SetSkill(SkillName.Tactics, 5.0);
            this.SetSkill(SkillName.Wrestling, 5.0);

            this.Fame = 150;
            this.Karma = 0;

            this.VirtualArmor = 12;

            this.Tamable = true;
            this.ControlSlots = 1;
            this.MinTameSkill = 11.1;
		}

		public TropicalCreature(Serial serial) : base(serial)
		{
			// You can initialize additional properties or fields here if needed
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write((int)0); // Write the version number, 0 for now
			// Write additional properties here if necessary
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt(); // Read the version number
			// Read additional properties here if necessary
		}
	}

}
