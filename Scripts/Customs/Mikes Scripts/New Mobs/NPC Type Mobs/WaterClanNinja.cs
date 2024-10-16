using System;
using Server.Items;
using Server.Targeting;
using Server.Spells;

namespace Server.Mobiles
{
    [CorpseName("corpse of a water clan ninja")]
    public class WaterClanNinja : BaseCreature
    {
        private TimeSpan m_SpeechDelay = TimeSpan.FromSeconds(5.0); // time between water ninja speech
        public DateTime m_NextSpeechTime;

        [Constructable]
        public WaterClanNinja() : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            // Setting the hue to a blue color to fit the water theme
            Hue = 0x5B6;

            // Setting the body to human, ninjas are typically human
            Body = 0x190; // Use 0x191 for a female ninja

            Name = NameList.RandomName("male");
            Title = "the Tsunami";

            // Dressing the ninja in a blue ninja outfit
            AddItem(new NinjaTabi() { Hue = 0x5B6 }); // Blue ninja sandals
            AddItem(new LeatherNinjaHood() { Hue = 0x5B6 }); // Blue ninja hood
            AddItem(new LeatherNinjaJacket() { Hue = 0x5B6 }); // Blue ninja jacket
            AddItem(new LeatherNinjaPants() { Hue = 0x5B6 }); // Blue ninja pants
            AddItem(new LeatherNinjaBelt() { Hue = 0x5B6 }); // Blue ninja belt

            // Defining stats for a water clan ninja, focused on agility and intelligence
            SetStr(600, 750);
            SetDex(250, 650);
            SetInt(200, 700);
            SetHits(800, 1500);

            SetDamage(110, 125);

            // Ninjas should have high stealth and poisoning skills
            SetSkill(SkillName.Anatomy, 80.0, 100.0);
			SetSkill(SkillName.Fencing, 100.0, 180.0);
            SetSkill(SkillName.Macing, 100.0, 180.0);
            SetSkill(SkillName.MagicResist, 100.0, 180.0);
            SetSkill(SkillName.Swords, 100.0, 180.0);
            SetSkill(SkillName.Tactics, 100.0, 180.0);
            SetSkill(SkillName.Wrestling, 100.0, 180.0);
            SetSkill(SkillName.Ninjitsu, 100.0, 180.0); // If your shard has the Ninjitsu skill
            SetSkill(SkillName.Stealth, 100.0, 180.0);
            SetSkill(SkillName.Hiding, 100.0, 180.0);

            // Karma for a water clan ninja could be neutral or slightly positive
            Fame = 12000;
            Karma = 1000;

            // Implementing ninja speech patterns, enigmatic and related to water
            m_NextSpeechTime = DateTime.Now + m_SpeechDelay;
        }

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
                        case 0: this.Say(true, "Flow like water, strike like the tide..."); break;
                        case 1: this.Say(true, "Drown in your fears, for they are known to me."); break;
                        case 2: this.Say(true, "The depths hold secrets, the surface only lies."); break;
                        case 3: this.Say(true, "You cannot grasp the torrent of my will."); break;
                    }

                    m_NextSpeechTime = DateTime.Now + m_SpeechDelay;				
                }

                base.OnThink();
            }			
        }
		
		public override int Damage(int amount, Mobile from)
		{
			Mobile combatant = this.Combatant as Mobile;

			if (combatant != null && combatant.Map == this.Map && combatant.InRange(this, 8))
			{
				if (Utility.RandomBool())
				{
					int phrase = Utility.Random(4);

					switch (phrase)
					{
                        case 0: this.Say(true, "Flow like water, strike like the tide..."); break;
                        case 1: this.Say(true, "Drown in your fears, for they are known to me."); break;
                        case 2: this.Say(true, "The depths hold secrets, the surface only lies."); break;
                        case 3: this.Say(true, "You cannot grasp the torrent of my will."); break;
					}

					m_NextSpeechTime = DateTime.Now + m_SpeechDelay;				
				}
			}

			return base.Damage(amount, from);
		}

        // Implementing the loot generation, water ninjas might carry specific water-themed items
        public override void GenerateLoot()
        {
            PackGold(250, 350);
			AddLoot(LootPack.UltraRich);  // Even richer loot than before
            // Add specific water clan ninja-themed loot
            // For example, add water bombs, which could act as smoke bombs but with a water effect
        }

        public WaterClanNinja(Serial serial) : base(serial)
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
