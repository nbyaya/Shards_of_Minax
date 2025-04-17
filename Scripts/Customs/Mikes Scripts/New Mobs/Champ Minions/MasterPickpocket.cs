using System;
using Server.Items;
using Server.Targeting;
using Server.Mobiles;
using System.Collections.Generic;

namespace Server.Mobiles
{
    [CorpseName("corpse of a master pickpocket")]
    public class MasterPickpocket : BaseCreature
    {
        private TimeSpan m_SpeechDelay = TimeSpan.FromSeconds(10.0); // time between speeches
        public DateTime m_NextSpeechTime;

        [Constructable]
        public MasterPickpocket() : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Hue = Utility.RandomSkinHue();
			Team = 1;

            if (Female = Utility.RandomBool())
            {
                Body = 0x191;
                Name = NameList.RandomName("female");
                Title = "the Master Pickpocket";
            }
            else
            {
                Body = 0x190;
                Name = NameList.RandomName("male");
                Title = "the Master Pickpocket";
            }

			this.AddItem(new Backpack());
			
            Item shirt = new Shirt();
            Item pants = new LongPants();
            Item shoes = new Shoes();
            Item hat = new Cap();
            Item dagger = new Dagger();

            AddItem(shirt);
            AddItem(pants);
            AddItem(shoes);
            AddItem(hat);
            AddItem(dagger);

            SetStr(400, 500);
            SetDex(600, 700);
            SetInt(200, 300);

            SetHits(350, 450);

            SetDamage(5, 10);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 50, 60);
            SetResistance(ResistanceType.Fire, 30, 40);
            SetResistance(ResistanceType.Cold, 30, 40);
            SetResistance(ResistanceType.Poison, 20, 30);
            SetResistance(ResistanceType.Energy, 20, 30);

            SetSkill(SkillName.Fencing, 90.1, 100.0);
            SetSkill(SkillName.MagicResist, 75.0, 100.0);
            SetSkill(SkillName.Tactics, 90.1, 100.0);
            SetSkill(SkillName.Wrestling, 80.0, 100.0);
            SetSkill(SkillName.Stealing, 100.0, 120.0);
            SetSkill(SkillName.Snooping, 100.0, 120.0);

            Fame = 5000;
            Karma = -5000;

            VirtualArmor = 40;

            m_NextSpeechTime = DateTime.Now + m_SpeechDelay;
        }

        public override bool AlwaysMurderer { get { return true; } }
        public override bool CanRummageCorpses { get { return true; } }
        public override bool ShowFameTitle { get { return false; } }
        public override bool ClickTitle { get { return true; } }

        public override void OnThink()
        {
            if (DateTime.Now >= m_NextSpeechTime)
            {
                Mobile combatant = this.Combatant as Mobile;

                if (combatant != null && combatant.Map == this.Map && combatant.InRange(this, 8))
                {
                    int phrase = Utility.Random(4);

                    switch (phrase)
                    {
                        case 0: this.Say(true, "Your valuables are mine!"); break;
                        case 1: this.Say(true, "I'll take that, thank you!"); break;
                        case 2: this.Say(true, "You won't miss this, will you?"); break;
                        case 3: this.Say(true, "Easy pickings!"); break;
                    }

                    m_NextSpeechTime = DateTime.Now + m_SpeechDelay;
                }

                base.OnThink();
            }
        }

        public override void OnGotMeleeAttack(Mobile attacker)
        {
            base.OnGotMeleeAttack(attacker);

            if (Utility.RandomDouble() < 0.3) // 30% chance to steal an item or buff
            {
                TryToSteal(attacker);
            }
        }

		private void TryToSteal(Mobile target)
		{
			if (target == null || target.Backpack == null)
				return;

			List<Item> items = new List<Item>();

			// Iterate through the player's backpack to find stealable items
			foreach (Item i in target.Backpack.Items)
			{
				if (i != null && i.Movable && !i.CheckBlessed(target)) // Check if the item is blessed
				{
					items.Add(i);
				}
			}

			if (items.Count > 0)
			{
				Item item = items[Utility.Random(items.Count)];

				if (item != null)
				{
					target.Backpack.RemoveItem(item);

					if (this.Backpack == null)
					{
						this.AddItem(new Backpack());
					}

					this.Backpack.DropItem(item);
					this.Say(true, "I'll be taking this!");

					target.SendMessage("An item has been stolen from your backpack!");
				}
			}
		}



        public MasterPickpocket(Serial serial) : base(serial)
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
