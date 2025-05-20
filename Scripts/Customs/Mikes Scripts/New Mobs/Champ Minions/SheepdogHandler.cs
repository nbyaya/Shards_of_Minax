using System;
using Server.Items;
using Server.Targeting;
using Server.Mobiles;
using System.Collections;

namespace Server.Mobiles
{
    [CorpseName("corpse of a sheepdog handler")]
    public class SheepdogHandler : BaseCreature
    {
        private TimeSpan m_SpeechDelay = TimeSpan.FromSeconds(10.0); // time between handler speech
        public DateTime m_NextSpeechTime;
        private ArrayList m_Dogs;

        [Constructable]
        public SheepdogHandler() : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Hue = Utility.RandomSkinHue();
			Team = 1;

            if (Female = Utility.RandomBool())
            {
                Body = 0x191;
                Name = NameList.RandomName("female");
                Title = " the Sheepdog Handler";
            }
            else
            {
                Body = 0x190;
                Name = NameList.RandomName("male");
                Title = " the Sheepdog Handler";
            }

            Item hat = new StrawHat();
            hat.Hue = Utility.RandomNeutralHue();
            AddItem(hat);

            Item shirt = new Shirt();
            shirt.Hue = Utility.RandomNeutralHue();
            AddItem(shirt);

            Item pants = new LongPants(Utility.RandomNeutralHue());
            AddItem(pants);

            Item boots = new Boots(Utility.RandomNeutralHue());
            AddItem(boots);

            m_Dogs = new ArrayList();

            SetStr( 250, 300 );
            SetDex( 60, 80 );
            SetInt( 61, 75 );

            SetHits( 150, 180 );

            SetDamage( 10, 15 );

            SetDamageType( ResistanceType.Physical, 100 );

            SetResistance( ResistanceType.Physical, 35, 45 );
            SetResistance( ResistanceType.Fire, 20, 30 );
            SetResistance( ResistanceType.Cold, 20, 30 );
            SetResistance( ResistanceType.Poison, 25, 35 );
            SetResistance( ResistanceType.Energy, 20, 30 );

            SetSkill( SkillName.Anatomy, 50.1, 70.0 );
            SetSkill( SkillName.MagicResist, 45.0, 60.0 );
            SetSkill( SkillName.Tactics, 70.1, 80.0 );
            SetSkill( SkillName.Wrestling, 70.1, 80.0 );

            Fame = 1500;
            Karma = -1500;

            VirtualArmor = 28;

            m_NextSpeechTime = DateTime.Now + m_SpeechDelay;
        }

        public override bool AlwaysMurderer { get { return true; } }
        public override bool CanRummageCorpses { get { return true; } }
        public override bool ShowFameTitle { get { return false; } }
        public override bool ClickTitle { get { return true; } }

		public override void OnThink()
		{
			base.OnThink();

			if (DateTime.Now >= m_NextSpeechTime)
			{
				Mobile combatant = this.Combatant as Mobile;

				if (combatant != null && combatant.Map == this.Map && combatant.InRange(this, 8))
				{
					int phrase = Utility.Random(4);

					switch (phrase)
					{
						case 0: this.Say(true, "Get them, boys!"); break;
						case 1: this.Say(true, "Herd them into position!"); break;
						case 2: this.Say(true, "Don't let them escape!"); break;
						case 3: this.Say(true, "Keep them together!"); break;
					}

					m_NextSpeechTime = DateTime.Now + m_SpeechDelay;
				}
			}

			if (Combatant != null && Combatant.Map == this.Map)
			{
				if (m_Dogs == null)
					m_Dogs = new ArrayList();

				if (m_Dogs.Count < 3)
				{
					SummonDog();
				}
			}
		}

		private void SummonDog()
		{
			if (this.Combatant == null || this.Map == null)
				return;

			Mobile dog = new Sheepdog();
			dog.Combatant = this.Combatant;

			Point3D loc = this.Location;
			Map map = this.Map;

			for (int i = 0; i < 10; ++i)
			{
				int x = loc.X + Utility.Random(3) - 1;
				int y = loc.Y + Utility.Random(3) - 1;
				int z = map.GetAverageZ(x, y);

				if (map.CanFit(x, y, loc.Z, 16, false, false))
				{
					loc = new Point3D(x, y, loc.Z);
					break;
				}
				else if (map.CanFit(x, y, z, 16, false, false))
				{
					loc = new Point3D(x, y, z);
					break;
				}
			}
		}

        public override void GenerateLoot()
        {
            PackGold(100, 150);
            PackItem(new Bandage(Utility.RandomMinMax(1, 15)));
            AddLoot(LootPack.Average);
        }

        public SheepdogHandler(Serial serial) : base(serial)
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

    public class Sheepdog : BaseCreature
    {
        private ExpireTimer m_ExpireTimer;

        [Constructable]
        public Sheepdog() : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Body = 0xD9;
            Name = "a sheepdog";
            BaseSoundID = 0xE5;

            SetStr(27, 37);
            SetDex(28, 43);
            SetInt(29, 37);

            SetHits(17, 22);
            SetMana(0);

            SetDamage(3, 7);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 20, 25);
            SetResistance(ResistanceType.Fire, 10, 15);
            SetResistance(ResistanceType.Cold, 10, 15);
            SetResistance(ResistanceType.Poison, 10, 15);
            SetResistance(ResistanceType.Energy, 10, 15);

            SetSkill(SkillName.MagicResist, 20.1, 35.0);
            SetSkill(SkillName.Tactics, 19.2, 29.0);
            SetSkill(SkillName.Wrestling, 19.2, 29.0);

            Fame = 450;
            Karma = 0;

            VirtualArmor = 12;

            Tamable = true;
            ControlSlots = 1;
            MinTameSkill = 35.1;

            // Start the expiration timer (change TimeSpan as needed)
            m_ExpireTimer = new ExpireTimer(this, TimeSpan.FromMinutes(2.0));
            m_ExpireTimer.Start();
        }

        // Clean up the timer if the dog is deleted by other means
        public override void OnDelete()
        {
            if (m_ExpireTimer != null)
                m_ExpireTimer.Stop();

            base.OnDelete();
        }

        public Sheepdog(Serial serial) : base(serial)
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

        // Timer class for self-deletion
        private class ExpireTimer : Timer
        {
            private Sheepdog m_Owner;

            public ExpireTimer(Sheepdog owner, TimeSpan delay) : base(delay)
            {
                m_Owner = owner;
                Priority = TimerPriority.OneMinute;
            }

            protected override void OnTick()
            {
                if (m_Owner != null && !m_Owner.Deleted)
                {
                    // Optionally: m_Owner.Say("The sheepdog vanishes!");
                    m_Owner.Delete();
                }
            }
		}	
    }
}
